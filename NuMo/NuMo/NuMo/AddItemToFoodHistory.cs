﻿using NuMo.DatabaseItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuMo
{
    //Add item page accessed from myday page
    class AddItemToFoodHistory : AddItemPage
    {
        DateTime date;

        public AddItemToFoodHistory(DateTime date) : base()
        {
            //display date at top of page
            this.Title += " " + date.Month + "/" + date.Day + "/" + date.Year;
            this.date = date;
        }

        //When saved, data needs to be send to the database.
        public override void saveButtonClicked(object sender, EventArgs args)
        {
            if (nutrFacts != null)
            {
                var nutrQuantifier = nutrFacts.getQuantifier();
                var nutrQuantity = nutrFacts.Quantity;
                //save info to database here
                if (selectedResult != null && nutrQuantity != null && !nutrQuantity.Equals("0") && nutrQuantifier != null && date != null)
                {
                    var db = DataAccessor.getDataAccessor();

                    //Increment the times this item has been selected so it will get priority in the future
                    db.incrementTimesSearched(selectedResult.food_no);

                    FoodHistoryItem item = new FoodHistoryItem();
                    //need to add date, quantity, quantifiers, and food_no to this item
                    item.food_no = selectedResult.food_no;
                    item.Date = date.ToString();
                    item.Quantity = Convert.ToDouble(nutrQuantity);
                    item.Quantifier = nutrQuantifier;


                    //Add to our database
                    db.addFoodHistory(item);
                }
            }
        }
    }
}
