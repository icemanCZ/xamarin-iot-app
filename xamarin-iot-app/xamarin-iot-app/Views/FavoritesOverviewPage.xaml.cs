using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_iot_app.ViewModels;

namespace xamarin_iot_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesOverviewPage : ContentPage
    {
        public FavoritesOverviewPage()
        {
            InitializeComponent();

            this.BindingContext = new FavoritesOverviewPageViewModel();
            (this.BindingContext as FavoritesOverviewPageViewModel).OnError += FavoritesOverviewPage_OnError;
        }

        private async void FavoritesOverviewPage_OnError(object sender, MsgEventArgs e)
        {
            await DisplayAlert("Error", e.Msg, "Refresh");
            (this.BindingContext as FavoritesOverviewPageViewModel).LoadDataCommand.Execute(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as BaseViewModel).Initialize();
        }
    }
}