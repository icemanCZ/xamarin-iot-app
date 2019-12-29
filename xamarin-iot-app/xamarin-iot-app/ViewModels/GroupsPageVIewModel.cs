using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin_iot_app.Models;
using xamarin_iot_app.Services;

namespace xamarin_iot_app.ViewModels
{
    class GroupsPageViewModel : BaseViewModel
    {
        private APIService apiService = DependencyService.Get<APIService>();

        public ObservableCollection<SensorGroup> Groups { get; set; } = new ObservableCollection<SensorGroup>();
        public Command LoadDataCommand { get; set; }

        public GroupsPageViewModel()
        {
            Title = "Groups";
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        protected override void InitializeInternal()
        {
            LoadDataCommand.Execute(null);
        }

        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Groups.Clear();
                var items = await apiService.GroupListAsync();
                if (items != null)
                {
                    foreach (var item in items)
                        Groups.Add(item);
                }
                else
                {
                    RaiseError("Failed loading data");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
