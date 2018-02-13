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
            }
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

        //empty constructor needed for preview
        public NutrFacts(){ 
            InitializeComponent();
        }

        public NutrFacts(AddItemPage aip){
            InitializeComponent();

            //so the saveButtonClicked method can be used by the classes inheritting from AddItemPage
            this.aip = aip;

            //establish description view
            descrView.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            descrView.VerticalOptions = LayoutOptions.Start;
            descrView.HorizontalOptions = LayoutOptions.Start;

            //set default units in picker
            setBaseUnitPickerChoices();
        }
        
        public NutrFacts(AddItemPage aip, ItemTappedEventArgs e){
            //for associated xaml file
            InitializeComponent();



            //so the saveButtonClicked method can be used by the classes inheritting from AddItemPage
            this.aip = aip;

            //give the description label values
            descrView.Text = e.Item.ToString();
            descrView.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            descrView.VerticalOptions = LayoutOptions.Start;
            descrView.HorizontalOptions = LayoutOptions.Start;

            //set default units in picker
            setBaseUnitPickerChoices();

            //get selected result from search
            selectedResult = (NumoNameSearch)e.Item;

            //add any custom picker units from selected item
            updateUnitPickerWithCustomOptions();

        }

        private void setBaseUnitPickerChoices()
        {
            UnitsPicker.Items.Clear();
            foreach (var item in UnitConverter.standardUnits)
            {
                UnitsPicker.Items.Add(item);
            }
            //add other base items here.
        }

        public void updateUnitPickerWithCustomOptions()
        {
            if (selectedResult != null)
            {
                var db = DataAccessor.getDataAccessor();
                db.addCustomQuantifiers(selectedResult);
                foreach (var converter in selectedResult.convertions)
                {
                    if (converter.name != null)
                        UnitsPicker.Items.Add(converter.name);
                }
            }
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


        //clear all the fields when saved
        void saveButtonClicked(object sender, EventArgs args)
        {
            aip.saveButtonClicked(sender, args);
            Navigation.PopAsync();
        }

    }
}
