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
    class SensorsPageViewModel : BaseViewModel
    {
        private APIService apiService = DependencyService.Get<APIService>();

        public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();
        public Command LoadDataCommand { get; set; }

        public SensorsPageViewModel()
        {
            Title = "Sensors";
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
                Sensors.Clear();
                var items = await apiService.SensorListAsync();
                if (items != null)
                {
                    foreach (var item in items)
                        Sensors.Add(item);
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
