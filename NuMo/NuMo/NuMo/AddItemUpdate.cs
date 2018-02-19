using NuMo.DatabaseItems;
using NuMo.ItemViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuMo
{
    //Update a foodhistory item from MyDayPage(date cannot be changed)
    class AddItemUpdate : AddItemPage
    {
        MyDayFoodItem myDayItem;

        public AddItemUpdate(MyDayFoodItem item)
        {
            myDayItem = item;

            var db = DataAccessor.getDataAccessor();
            FoodHistoryItem foodHistoryItem = db.getFoodHistoryItem(item.id);

            var search = new NumoNameSearch();
            search.food_no = foodHistoryItem.food_no;
            search.name = foodHistoryItem.DisplayName;

            nutrFacts = new NutrFacts(this,search);

            //update the values being displayed
            nutrFacts.DescriptView = foodHistoryItem.DisplayName;
            nutrFacts.Quantity = foodHistoryItem.Quantity.ToString();
            nutrFacts.UnitPickerText = foodHistoryItem.Quantifier;

            nutrFacts.selectedResult = search;
            nutrFacts.updateUnitPickerWithCustomOptions();


        }

        public override void saveButtonClicked(object sender, EventArgs e)
        {
            var nutrQuantifier = nutrFacts.getQuantifier();
            var nutrQuantity = nutrFacts.Quantity;

            if (selectedResult != null && nutrQuantity != null && !nutrQuantity.Equals("0") && nutrQuantifier != null)
            {
                var db = DataAccessor.getDataAccessor();
                //Increment the times this item has been selected so it will get priority in the future
                db.incrementTimesSearched(selectedResult.food_no);
                FoodHistoryItem item = new FoodHistoryItem();
                //need to add date, quantity, quantifiers, and food_no to this item
                item.food_no = selectedResult.food_no;
                item.Quantity = Convert.ToDouble(nutrQuantity);
                item.Quantifier = nutrQuantifier;


                //Add to our database
                db.updateFoodHistory(item, myDayItem.id);
            }
            MyDayFoodItem.sendRefresh();
        }
    }
}
