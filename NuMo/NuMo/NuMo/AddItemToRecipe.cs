using NuMo.DatabaseItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

// Add item to recipe page. This page is simply an add on to the add recipe page, where the recipe can intake more ingredients

namespace NuMo
{
    public class AddItemToRecipe : AddItemPage
    {
        List<FoodHistoryItem> recipeList;

        public AddItemToRecipe(List<FoodHistoryItem> temp) : base()
        {
            recipeList = temp;
        }

        //Append created item to the recipeList.
        public override void saveButtonClicked(object sender, EventArgs args)
        {
            var result = new FoodHistoryItem();
            result.food_no = selectedResult.food_no;

            //nutrFacts is from parenting class
            result.Quantity = Convert.ToDouble(nutrFacts.Quantity);
            result.Quantifier = nutrFacts.getQuantifier();
            recipeList.Add(result);
            Navigation.RemovePage(this);
            base.OnBackButtonPressed();
        }
    }
}
