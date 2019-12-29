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
    public partial class GroupDetailPage : ContentPage
    {
        public GroupDetailPage(SensorGroup group)
        {
            InitializeComponent();

            this.BindingContext = new GroupDetailPageViewModel(group);
            (this.BindingContext as GroupDetailPageViewModel).OnError += SensorDetailPage_OnError;
        }

        private async void SensorDetailPage_OnError(object sender, MsgEventArgs e)
        {
            await DisplayAlert("Error", e.Msg, "Refresh");
            (this.BindingContext as GroupDetailPageViewModel).LoadDataCommand.Execute(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as BaseViewModel).Initialize();
        }
    }
}