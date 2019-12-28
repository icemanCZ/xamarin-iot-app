using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_iot_app.Models;
using xamarin_iot_app.ViewModels;

namespace xamarin_iot_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorDetailPage : ContentPage
    {
        public SensorDetailPage(Sensor sensor)
        {
            InitializeComponent();

            this.BindingContext = new SensorDetailPageViewModel(sensor);
            (this.BindingContext as SensorDetailPageViewModel).OnError += SensorDetailPage_OnError;
        }

        private async void SensorDetailPage_OnError(object sender, MsgEventArgs e)
        {
            await DisplayAlert("Error", e.Msg, "Refresh");
            (this.BindingContext as SensorDetailPageViewModel).LoadDataCommand.Execute(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as BaseViewModel).Initialize();
        }
    }
}