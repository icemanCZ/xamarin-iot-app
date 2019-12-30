using System.Linq;
using System;
using xamarin_iot_app.Models;
using xamarin_iot_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamarin_iot_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorsPage : ContentPage
    {
        public SensorsPage()
        {
            InitializeComponent();
            this.BindingContext = new SensorsPageViewModel();
            (this.BindingContext as SensorsPageViewModel).OnError += SensorsPage_OnError;
        }

        private async void SensorsPage_OnError(object sender, MsgEventArgs e)
        {
            await DisplayAlert("Error", e.Msg, "Refresh");
            (this.BindingContext as SensorsPageViewModel).LoadDataCommand.Execute(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as BaseViewModel).Initialize();
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var item = e.Item as Sensor;

            await Navigation.PushModalAsync(new SensorDetailPage(item));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}