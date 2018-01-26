using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NuMo
{
    public partial class NutrFacts : ContentPage
    {
        public NutrFacts(){ //empty constructor needed for preview
            InitializeComponent();
        }
        
        public NutrFacts(String description){
            //for associated xaml file
            InitializeComponent();

            //give the description label values
            descrView.Text = description;
            descrView.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            descrView.VerticalOptions = LayoutOptions.Start;
            descrView.HorizontalOptions = LayoutOptions.Start;

        }

        //clear all the fields when saved
        void saveButtonClicked(object sender, EventArgs args)
        {
            //clearAllFields();
        }


    }
}
