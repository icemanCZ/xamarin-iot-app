using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_iot_app.Models;
using xamarin_iot_app.ViewModels;

namespace xamarin_iot_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupsPage : ContentPage
    {
        public GroupsPage()
        {
            InitializeComponent();
            this.BindingContext = new GroupsPageViewModel();
            (this.BindingContext as GroupsPageViewModel).OnError += GroupsPage_OnError;
        }

        private async void GroupsPage_OnError(object sender, MsgEventArgs e)
        {
            await DisplayAlert("Error", e.Msg, "Refresh");
            (this.BindingContext as GroupsPageViewModel).LoadDataCommand.Execute(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as BaseViewModel).Initialize();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var item = e.Item as SensorGroup;

            await Navigation.PushModalAsync(new GroupDetailPage(item));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
