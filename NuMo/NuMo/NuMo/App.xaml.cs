﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NuMo

{
    public partial class App :Application
    {
        public App()
        {
            InitializeComponent();
            //This app will be based off of navigation pages
            NavigationPage navPage = new NavigationPage(new NuMo.MainPage());
            navPage.BarTextColor = Color.Black;
            MainPage = navPage;
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
