using NuMo.DatabaseItems;
using NuMo.ItemViews;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static NuMo.NumoNameSearch;

namespace NuMo
{
    //Impliments Singleton design pattern: This class is the focus point of all database queries
    class DataAccessor
    {
        private static DataAccessor accessor;
        SQLiteConnection dbConn;

        private DataAccessor()
        {
            dbConn = DependencyService.Get<ISQLite>().GetConnection();

        }

        public static DataAccessor getDataAccessor()
        {
            if (accessor == null)
            {
                accessor = new NuMo.DataAccessor();
            }
            return accessor;
        }

        //For use in addItem pages.
        public List<NumoNameSearch> searchName(String name)
        {
            
            if (name == null || name.Length <= 0 || name.Trim().Length == 0)
                return new List<NumoNameSearch>();
            //ignore single quotes
            name = name.Replace("’", "");//iPhone specific
            name = name.Replace("'", "");//Android Specific
            name = name.ToUpper();
            String where = "WHERE ";
            String whereOR = "WHERE ";
            var names = name.Split(' ');

            //build where clause.
            foreach (var word in names)
            {
                if (word.Length > 0)
                {
                    var temp = word;
                    //Removes plural form, but still catches correct form using wild cards
                    if (word[word.Length - 1] == 'S')
                    {
                        temp = word.Substring(0, word.Length - 1);
                    }
                    System.Diagnostics.Debug.WriteLine(temp);
                    where += String.Format("UPPER(Long_Desc) LIKE '%{0}%' AND ", temp);
                    whereOR += String.Format("UPPER(Long_Desc) LIKE '%{0}%' OR ", temp);
                }
            }
            where = where.Remove(where.Length - 4);//remove last 4 as we don't want the final 'AND '
            whereOR = whereOR.Remove(whereOR.Length - 3);//remove last 3 as we don't want the final 'OR '
            var query = String.Format("SELECT NDB_No as food_no, Long_Desc as name FROM FOOD_DES {0}order by Times_Searched DESC", where);
            var queryOR = String.Format("SELECT NDB_No as food_no, Long_Desc as name FROM FOOD_DES {0}order by Times_Searched DESC", whereOR);

            //query = query.Replace("'%'", "%'");
            //queryOR = queryOR.Replace("'%'", "%'");

            System.Diagnostics.Debug.WriteLine(query);
            System.Diagnostics.Debug.WriteLine(queryOR);

            var resultList = dbConn.Query<NumoNameSearch>(query);
            var resultsOR = dbConn.Query<NumoNameSearch>(queryOR);
            foreach (var item in resultsOR)
                resultList.Add(item);
            return resultList;
        }

        //Retrieve custom quantifiers from database and add to searchItem
        public void addCustomQuantifiers(NumoNameSearch item)
        {
            string data_num = item.food_no.ToString();
            if (data_num.Length == 4)
                data_num = "0" + data_num;

            var convertItems = getCustomQuantifiers(item.food_no);
            item.convertions.AddRange(convertItems);
        }

        //Retrives custom quantifiers from the database for a food_no
        public List<ConvertItem> getCustomQuantifiers(int food_no)
        {
            string data_num = food_no.ToString();
            if (data_num.Length == 4)
                data_num = "0" + data_num;
            var quantifier1 = dbConn.Query<ConvertItem>(String.Format("SELECT GmWt_1 as gramsMultiplier, GmWt_Desc1 as name FROM ABBREV WHERE NDB_No = '{0}'", data_num));
            var quantifier2 = dbConn.Query<ConvertItem>(String.Format("SELECT GmWt_2 as gramsMultiplier, GmWt_Desc2 as name FROM ABBREV WHERE NDB_No = '{0}'", data_num));
            quantifier1.AddRange(quantifier2);
            return quantifier1;
        }

        //Retrives macronutrients from the database for a food_no
        //NOT WORKING
        public List<ConvertItem> getFoodInfo(int food_no)
        {
            string data_num = food_no.ToString();
            if (data_num.Length == 4)
                data_num = "0" + data_num;
            var foodInfo = dbConn.Query<ConvertItem>(String.Format("SELECT Carbohydrt_(g) as carbs, Sugar_Tot_(g) as sugarTotal FROM ABBREV WHERE NDB_No = '{0}'", data_num));
            return foodInfo;
        }


        //Retrieves one clean list of all the nutrients related to a list of historyItems
        public List<Nutrient> getNutrientsFromHistoryList(List<FoodHistoryItem> historyItems)
        {
            var list = new List<FoodItem>();
            foreach(var historyItem in historyItems)
            {
                var foodItem = getFoodItem(historyItem.food_no, UnitConverter.getMultiplier(historyItem.Quantifier, historyItem.food_no)*historyItem.Quantity);
                list.Add(foodItem);
            }
            var combinedFoodItem = new FoodItem(list);
            combinedFoodItem.stripExtraOmegs();
            return combinedFoodItem.nutrients;
        }

        //Retrive a foodItem with nutrients calculated for the input quantityInGrams
        public FoodItem getFoodItem(int food_no, double quantityInGrams)
        {
            string data_num = food_no.ToString();
            if (data_num.Length == 4)
                data_num = "0" + data_num;
            var values = dbConn.Query<NutData>(String.Format("Select NDB_No as food_no, Nutr_No as nutr_no, Nutr_Val as nutr_value FROM NUT_DATA WHERE NDB_No = '{0}'", data_num));

            FoodItem foodItem = new NuMo.FoodItem(values, quantityInGrams);
            return foodItem;
        }

        //Used to save and read Settings and User Info from the database
        public void saveSettingsItem(String settingName, String settingValue)
        {
            var values = dbConn.Query<MyDayRemainderItem>(String.Format("SELECT Setting_Name from SETTINGS WHERE Setting_Name = '{0}'", settingName));
            if (values.Any())
            {
                dbConn.Execute(String.Format("UPDATE SETTINGS set Setting_Val = '{0}' WHERE Setting_Name = '{1}'", settingValue, settingName));
            }
            else
            {
                dbConn.Execute(String.Format("INSERT INTO SETTINGS (Setting_Name, Setting_Val) VALUES ('{0}', '{1}')", settingName, settingValue));
            }
        }

        public string getSettingsItem(String settingName)
        {
            var values = dbConn.Query<MyDayRemainderItem>(String.Format("SELECT Setting_Val as imageString from SETTINGS WHERE Setting_Name = '{0}'", settingName));
            if (values.Any())
                return values.First().imageString;
            else
                return "";
        }

        public void saveDRIValue(String DRIName, String DRIValue)
        {
            var values = dbConn.Query<MyDayRemainderItem>(String.Format("SELECT DRI_Name from DRI_VALUES WHERE DRI_Name = '{0}'", DRIName));
            if (values.Any())
            {
                dbConn.Execute(String.Format("UPDATE DRI_VALUES set DRI_Val = '{0}' WHERE DRI_Name = '{1}'", DRIValue, DRIName));
            }
            else
            {
                dbConn.Execute(String.Format("INSERT INTO DRI_VALUES (DRI_Name, DRI_Val) VALUES ('{0}', '{1}')", DRIName, DRIValue));
            }
        }

        public string getDRIValue(String DRIName)
        {
            var values = dbConn.Query<MyDayRemainderItem>(String.Format("SELECT DRI_Val as imageString from DRI_VALUES WHERE DRI_Name = '{0}'", DRIName));
            if (values.Any())
                return values.First().imageString;
            else
                return "";
        }
        
        //Inserts a new reminder for a specific date. Reminders are held in the database as a base64 string.
        public void insertRemainder(MyDayRemainderItem item)
        {
            if(item.imageString == null)
            {
                StreamImageSource streamImageSource = (StreamImageSource)item.RemainderImage.Source;
                System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
                Task<Stream> task = streamImageSource.Stream(cancellationToken);
                Stream stream = task.Result;
                var memStream = new MemoryStream();
                stream.CopyTo(memStream);
                item.imageString = Convert.ToBase64String(memStream.ToArray());
            }
            dbConn.Execute(String.Format("INSERT INTO FoodHistory (Date, Image, Food_Id, Quantity, Quantifier) VALUES ('{0}', '{1}', 0, 0, '')", item.Date, item.imageString));
            dbConn.Commit();
        }
        
        //retrieves reminder from database and converts it back to an image from base64.
        public List<MyDayRemainderItem> getRemainders(String date)
        {
            var baseList = dbConn.Query<MyDayRemainderItem>(String.Format("SELECT History_Id as id, date as Date, Image as imageString FROM FoodHistory WHERE date='{0}' AND Image NOT NULL", date));
            //convert the image string to actual image in the MyDayRemainderItem TODO
            foreach(var item in baseList)
            {
                var imageString = item.imageString;
                var byteArray = Convert.FromBase64String(imageString);
                Image image = new Image();
                image.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                item.RemainderImage = image;
            }
            return baseList;

        }

        
        public String getNameFromID(int food_no)
        {
            var values = dbConn.Query<NumoNameSearch>(String.Format("SELECT Long_Desc as name FROM FOOD_DES where NDB_No = {0}", food_no));
            return values.First<NumoNameSearch>().name;
        }

        //quantity is the number of servings...
        public void createFoodItem(List<Nutrient> values, String name, double multiplier, String quantifier)
        {
            // var keys = dbConn.Query<FoodItem>("SELECT MAX(NDB_No) as food_no FROM FOOD_DES");
            //var food_no = keys.First<FoodItem>.
            name.Replace("'", "''");
    		dbConn.Execute(String.Format("INSERT INTO FOOD_DES (Long_Desc, Times_Searched) VALUES ('{0}', 10)", name));
            dbConn.Commit();
            var food_no_data = dbConn.Query<NumoNameSearch>(String.Format("SELECT NDB_No as food_no, Long_Desc as name FROM FOOD_DES where UPPER(Long_Desc) LIKE '{0}' order by Times_Searched DESC", name));
            var food_no = food_no_data.FirstOrDefault().food_no;
            String data_num = food_no.ToString();
            if (data_num.Length == 4)
                data_num = "0" + data_num;
            dbConn.Execute(String.Format("INSERT INTO ABBREV (NDB_No, GmWt_1, GmWt_Desc1) VALUES ('{0}', {1}, '{2}')", data_num, multiplier, quantifier));
            foreach (var value in values)
            {
                dbConn.Execute(String.Format("INSERT INTO NUT_DATA (NDB_No, Nutr_No, Nutr_Val) VALUES ('{0}', '{1}', {2})", food_no, value.dbNo, value.quantity*100));//database stores based on 100 grams, we treat entries as 1 gram
            }
        }

        //Adds the foodHistory entry to the database
        public void addFoodHistory(FoodHistoryItem item)
        {
            dbConn.Execute(String.Format("INSERT INTO FoodHistory (Date, Food_Id, Quantity, Quantifier) VALUES ('{0}', {1}, {2}, '{3}')", item.Date, item.food_no, item.Quantity, item.Quantifier));
        }

        //retrieves the foodhistory associated to a certain date
        public List<FoodHistoryItem> getFoodHistoryList(String date)
        {
            return dbConn.Query<FoodHistoryItem>(String.Format("SELECT History_Id, Date as Date, Food_Id as food_no, Quantity as Quantity, Quantifier as Quantifier FROM FoodHistory WHERE Date LIKE '%{0}%'", date));
        }

        //retrieves the foodhistory associated to a certain date and prepares it for display as a MyDayFoodItem
        public List<MyDayFoodItem> getFoodHistory(String date)
        {
            var historyList = dbConn.Query<FoodHistoryItem>(String.Format("SELECT History_Id, Date as Date, Food_Id as food_no, Quantity as Quantity, Quantifier as Quantifier FROM FoodHistory WHERE Date LIKE '%{0}%'", date));
            var resultList = new List<MyDayFoodItem>();
            foreach(var result in historyList)
            {
                if (result.food_no > 0)
                {
                    var newItem = new MyDayFoodItem();
                    newItem.id = result.History_Id;
                    newItem.DisplayName = getNameFromID(result.food_no);
                    newItem.Quantity = "(" + result.Quantity.ToString() + ") " + result.Quantifier;
                    resultList.Add(newItem);
                }
            }
            return resultList;
        }


        //Update an entry in the foodhistory table.
        public void updateFoodHistory(FoodHistoryItem item, int id)
        {
            dbConn.Execute(String.Format("UPDATE FoodHistory SET Quantity = {0}, Quantifier = '{1}', Food_Id = {2} WHERE History_id = {3}", item.Quantity, item.Quantifier, item.food_no, id));
        }

        //Retreives a foodhistory item by id
        public FoodHistoryItem getFoodHistoryItem(int foodHistoryId)
        {
            var historyList = dbConn.Query<FoodHistoryItem>(String.Format("SELECT History_Id, Date as Date, Food_Id as food_no, Quantity as Quantity, Quantifier as Quantifier FROM FoodHistory WHERE History_Id = {0}", foodHistoryId));
            return historyList.First<FoodHistoryItem>();
        }

        //Delete an entry in the foodHistory table
        public void deleteFoodHistoryItem(int id)
        {
            if(id >= 0)
            {
                dbConn.Execute(String.Format("DELETE FROM FoodHistory WHERE History_Id={0}", id));
            }
        }

        //increment the timessearched field so that this entry gets preference in future searches
        public void incrementTimesSearched(int food_no)
        {
            int changes = dbConn.Execute(String.Format("UPDATE FOOD_DES SET Times_Searched = Times_Searched + 1 WHERE NDB_No = {0}", food_no));
            dbConn.Commit();
        }

		//Methods below here are for testing purposes only
        public void checkTimesSearched(int food_no)
        {
            var result = dbConn.Query<testTimesSearched>(String.Format("SELECT Times_Searched as times_searched FROM FOOD_DES WHERE NDB_NO = {0}", food_no));
            int times_searched = result.First<testTimesSearched>().times_searched;
            times_searched--;
            times_searched++;
        }

        public class testTimesSearched
        {
            public int times_searched { get; set; }
        }
    }
}
