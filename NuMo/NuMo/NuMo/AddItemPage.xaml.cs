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

        public NutrFacts nutrFacts;

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
                String searchItem = e.NewTextValue;;
                var db = DataAccessor.getDataAccessor();
                var searchResults = db.searchName(searchItem);
                searchList.ItemsSource = searchResults;
            };


            mainStack.Children.Insert(0, searchbar);
        }

        //update search results.
        public void searchForMatches(object sender, TextChangedEventArgs e)
        {
            var searchItem = e.NewTextValue;
            var db = DataAccessor.getDataAccessor();
            var searchResults = db.searchName(searchItem);
            searchList.ItemsSource = searchResults;
        }

        //Opens a new window when a search item is clicked.
        public async void itemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            //navigate to NutrFacts page, passing in the event arguments
            nutrFacts = new NutrFacts(this, (NumoNameSearch)e.Item);
            await Navigation.PushAsync(nutrFacts);

            selectedResult = (NumoNameSearch)e.Item;
        }

        //Clear all fields to make it obvious the button press had an impact.
        public virtual void saveButtonClicked(object sender, EventArgs args)
        {

        }

    }
}
