using NuMo.DatabaseItems;
using NuMo.ItemViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Xamarin.Forms;

namespace NuMo
{
	public partial class CreateRecipePage : ContentPage
	{
        List<FoodHistoryItem> recipeList = new List<FoodHistoryItem>();
        String Quantity { get; set; }

		public CreateRecipePage()
		{
			InitializeComponent();
            saveButton.Clicked += onSaveClicked;
		}

        //Save the user input data into the database
        private async void onSaveClicked(object sender, EventArgs e)
        {
            //check if inputs are valid
            if (recipeName.Text == null || recipeName.Text == "")
            {
                await DisplayAlert("Please input RECIPE NAME", "", "OK");
            }
            else if (quantity.Text == null || quantity.Text == "" || quantity.Text == "0")
            {
                await DisplayAlert("Please input QUANTITY", "example: 1.5 or 3", "OK");
            }
            else if (quantifier.Text == null || quantifier.Text == "")
            {
                await DisplayAlert("Please input QUANTIFIER NAME", "", "OK");
            }
            else if (recipeList.Count == 0)
            {
                await DisplayAlert("Please add an INGREDIENT", "", "OK");
            }
            else //perform save
            {

                //access database
                var db = DataAccessor.getDataAccessor();
                var nutrientList = db.getNutrientsFromHistoryList(recipeList);
                double totalQuantity = 0;

                double servingQuantity = Convert.ToDouble(quantity.Text);

                foreach (var item in recipeList)
                {
                    totalQuantity += UnitConverter.getMultiplier(item.Quantifier, item.food_no) * item.Quantity;
                }
                foreach (var item in nutrientList)
                {
                    item.quantity /= (totalQuantity);
                    item.quantity *= 100;//database is in 100g standard.
                }
                var servingMultiplier = (totalQuantity / servingQuantity) / 100;
                db.createFoodItem(nutrientList, recipeName.Text, servingMultiplier, quantifier.Text.ToString());

                //clear variables
                recipeList.Clear();
                recipeName.Text = "";
                ingredientList.ItemsSource = "";
                quantity.Text = "";
                quantifier.Text = "";

                await DisplayAlert("Recipe Saved", "", "OK");
            }
        }

        //Open page to select a new ingrediant and input quantity
        async void OnAddIngredient(object sender, EventArgs args)
		{
            await Navigation.PushAsync(new AddItemToRecipe(recipeList));
        }

        //refresh everytime we return from the addItemPage
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ingredientList.BeginRefresh();
            ingredientList.ItemsSource = null;
            
            List<MyDayFoodItem> foodItems = new List<MyDayFoodItem>();
            foreach(var item in recipeList)
            {
                var foodItem = new MyDayFoodItem();
                foodItem.DisplayName = item.DisplayName;
                foodItem.Quantity = "("+ item.Quantity +")"+ " " + item.Quantifier;
                foodItems.Add(foodItem);
            }
            ingredientList.ItemsSource = foodItems;
            ingredientList.EndRefresh();
        }
    }
}
