using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NuMo
{
    public partial class NutrFacts : ContentPage
    {
        public NumoNameSearch selectedResult;

        //empty constructor needed for preview
        public NutrFacts(){ 
            InitializeComponent();
        }
        
        public NutrFacts(ItemTappedEventArgs e){
            //for associated xaml file
            InitializeComponent();

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



        //clear all the fields when saved
        void saveButtonClicked(object sender, EventArgs args)
        {
            //clearAllFields();
        }

        //Adds the static units to the unitPicker, ie grams, pounds, kilograms, things nonspecific to the user selection
        private void setBaseUnitPickerChoices()
        {
            UnitsPicker.Items.Clear();
            foreach (var item in UnitConverter.standardUnits)
            {
                UnitsPicker.Items.Add(item);
            }
            //add other base items here.
        }


    }
}
