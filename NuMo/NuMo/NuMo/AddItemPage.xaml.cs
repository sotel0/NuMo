using NuMo.DatabaseItems;
using System;
using System.Diagnostics;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NuMo
{
    //Core add item page for creating a foodhistory item to go the the database.
    public partial class AddItemPage : ContentPage
    {
        public NumoNameSearch selectedResult;
		public Entry searchbar;
        public string Quantity 
        {   
            get
            {
                return quantity.Text;
            }
            set
            {
                quantity.Text = value;
            }
        }
        public string UnitPickerText
        {
            get
            {
                return UnitsPicker.Title;
            }
            set
            {
                UnitsPicker.Title = value;
            }
        }

        public AddItemPage()
        {
            InitializeComponent();
            

			searchbar = new Entry
			{
				Placeholder = "Search item",
			};

            //Get search results for every key entry into the search bar and display them
			searchbar.TextChanged += (sender, e) =>
			{
				var searchItem = e.NewTextValue;
				var db = DataAccessor.getDataAccessor();
				var searchResults = db.searchName(searchItem);
				searchList.ItemsSource = searchResults;
			};

            
			mainStack.Children.Insert(0, searchbar);

            setBaseUnitPickerChoices();
        }

        //Adds the static units to the unitPicker, ie grams, pounds, kilograms, things nonspecific to the user selection
        private void setBaseUnitPickerChoices()
        {
            UnitsPicker.Items.Clear();
            foreach(var item in UnitConverter.standardUnits)
            {
                UnitsPicker.Items.Add(item);
            }
            //add other base items here.
        }

        //update search results.
        public void searchForMatches(object sender, TextChangedEventArgs e)
        {
            var searchItem = e.NewTextValue;
            var db = DataAccessor.getDataAccessor();
            var searchResults = db.searchName(searchItem);
            searchList.ItemsSource = searchResults;
        }

        //When an item is selected, make it clear to the user and append additional unit choices unique to that item to the unit picker
        /*
        public void itemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            searchbar.Text = e.SelectedItem.ToString();

            //navigate to NutrFacts page, passing in 
            //await Navigation.PushAsync(new NutrFacts(e.SelectedItem.ToString()));

            selectedResult = (NumoNameSearch)e.SelectedItem;
            setBaseUnitPickerChoices();
            updateUnitPickerWithCustomOptions();
        }
        */


        public async void itemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            //searchbar.Text = e.SelectedItem.ToString();

            //navigate to NutrFacts page, passing in 
            await Navigation.PushAsync(new NutrFacts(e.Item.ToString()));

            selectedResult = (NumoNameSearch)e.Item;
            setBaseUnitPickerChoices();
            updateUnitPickerWithCustomOptions();
        }

        public void updateUnitPickerWithCustomOptions()
        {
            if(selectedResult != null)
            {
                var db = DataAccessor.getDataAccessor();
                db.addCustomQuantifiers(selectedResult);
                foreach(var converter in selectedResult.convertions)
                {
                    if(converter.name != null)
                        UnitsPicker.Items.Add(converter.name);
                }
            }
        }

        //Clear all fields to make it obvious the button press had an impact.
        void saveButtonClicked(object sender, EventArgs args)
        {
            clearAllFields();
        }

        public String getQuantifier()
        {
            if (UnitsPicker.SelectedIndex >= 0)
                return UnitsPicker.Items[UnitsPicker.SelectedIndex];
            else if (UnitsPicker.Title != null)
                return UnitsPicker.Title;
            else
                return null;
        }

        // Resets all fields to be empty so the user can add another item
        public void clearAllFields()
        {
            searchbar.Text = "";
            searchbar.Placeholder = "Search";
            searchList.ItemsSource = null;
            quantity.Text = "";
            quantity.Placeholder = "Number of";
            UnitsPicker.SelectedIndex = -1;
            UnitsPicker.Title = " Units                           ";
        }
    }
}
