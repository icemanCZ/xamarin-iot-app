using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_iot_app.Services;
using xamarin_iot_app.Views;

namespace xamarin_iot_app
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<APIService>();
            MainPage = new MainPage();
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
