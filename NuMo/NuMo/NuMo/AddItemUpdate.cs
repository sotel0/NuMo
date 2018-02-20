﻿using NuMo.DatabaseItems;
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
        NumoNameSearch search;

        public AddItemUpdate(MyDayFoodItem item)
        {
            myDayItem = item;

            //get food item from database
            var db = DataAccessor.getDataAccessor();
            FoodHistoryItem foodHistoryItem = db.getFoodHistoryItem(item.id);

            //store food info in NumoNameSearch var
            var search = new NumoNameSearch();
            this.search = search;
            search.food_no = foodHistoryItem.food_no;
            search.name = foodHistoryItem.DisplayName;

            //create new instance to display food info
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

            if (search != null && nutrQuantity != null && !nutrQuantity.Equals("0") && nutrQuantifier != null)
            {
                var db = DataAccessor.getDataAccessor();

                FoodHistoryItem item = new FoodHistoryItem();
                //need to add date, quantity, quantifiers, and food_no to this item
                item.food_no = search.food_no;
                item.Quantity = Convert.ToDouble(nutrQuantity);
                item.Quantifier = nutrQuantifier;


                //Add to our database
                db.updateFoodHistory(item, myDayItem.id);
            }
            MyDayFoodItem.sendRefresh();
        }
    }
}
