using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NuMo
{
    public partial class NutrFacts : ContentPage
    {
        //System.Diagnostics.Debug.WriteLine("cewl");

        public AddItemPage aip;
             
        public NumoNameSearch selectedResult;

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
<<<<<<< HEAD
=======
                //UnitsPicker.SelectedItem = UnitsPicker.Items.IndexOf(value);
                UnitsPicker.SelectedItem = UnitsPicker.Items.IndexOf(value);

>>>>>>> c04ca7142a0673c21d56d87954396de3e7642d73
            }
            //UnitsPicker.SelectedItem = UnitsPicker.Items.IndexOf(value);
        }
        public string DescriptView
        {
            get
            {
                return descrView.Text;
            }
            set
            {
                descrView.Text = value;
            }
        }

        //empty constructor needed for visual studios preview
        public NutrFacts(){ 
            InitializeComponent();
        }

        public NutrFacts(AddItemPage aip, NumoNameSearch search){
            InitializeComponent();

            //save reference to food item
            selectedResult = search;

            //so the saveButtonClicked method can be used by the classes inheritting from AddItemPage
            this.aip = aip;

            //establish the save button on the top right
            ToolbarItem save = new ToolbarItem();
            save.Text = "Save";
            save.Clicked += saveButtonClicked;
            ToolbarItems.Add(save);

            //establish description view
            descrView.Text = search.ToString();
            descrView.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            descrView.VerticalOptions = LayoutOptions.Start;
            descrView.HorizontalOptions = LayoutOptions.Start;

            //set default units in picker
            //add any custom picker units from selected item
            updateUnitPickerWithCustomOptions();
        }

        private void setBaseUnitPickerChoices()
        {
            foreach (var item in UnitConverter.standardUnits)
            {
                if (item != null)
                {
                    UnitsPicker.Items.Add(item);
                }
            }
            //add other base items here.
        }

        public void updateUnitPickerWithCustomOptions()
        {
            //reset entire picker
            UnitsPicker.Items.Clear();

            //to reset duplicate units appearing
            setBaseUnitPickerChoices();

            if (selectedResult != null)
            {
                var db = DataAccessor.getDataAccessor();
                db.addCustomQuantifiers(selectedResult);
                foreach (var converter in selectedResult.convertions)
                {
                    if ( converter.name != null && !UnitsPicker.Items.Contains(converter.name) && converter.name != "")
                    {
                        UnitsPicker.Items.Insert(0,converter.name);
                    }
                }
            }
        }

        //return item from the picker if there is one selected
        public String getQuantifier()
        {
            if (UnitsPicker.SelectedIndex >= 0)
                return UnitsPicker.Items[UnitsPicker.SelectedIndex];
            else if(UnitsPicker.Title != null){
                return UnitsPicker.Title;                
            }
            else
                return null;
        }


        //clear all the fields when saved
        async void saveButtonClicked(object sender, EventArgs args)
        {
            //do not save unless input values are there
            if (Quantity == null || Quantity.Equals("") || Quantity.Equals("0"))
            {
                await DisplayAlert("Please input NUMBER OF value", "example: 1.5 or 3", "OK");
<<<<<<< HEAD

=======
            //if (Quantity == null || Quantity.Equals(""))
            //{
            //    await DisplayAlert("Please enter NUMBER OF amount to save", "", "OK");
>>>>>>> c04ca7142a0673c21d56d87954396de3e7642d73

            } else if(UnitsPicker.SelectedIndex < 0 && UnitsPicker.Title.Equals("Units")){
                await DisplayAlert("Please select UNITS value", "", "OK");
            }
            else
            {
                aip.saveButtonClicked(sender, args);
                await Navigation.PopAsync();
            }
        }

    }
}
