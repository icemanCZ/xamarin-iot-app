using System.Collections.Generic;
using System.Text;
using System;
using xamarin_iot_app.Models;
using xamarin_iot_app.Services;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace xamarin_iot_app.ViewModels
{
    internal class SensorsPageViewModel : BaseViewModel
    {
        #region Fields

        private APIService apiService = DependencyService.Get<APIService>();

        #endregion

        #region Properties

        public Command LoadDataCommand { get; set; }
        public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();

        #endregion

        #region Constructors

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

        #endregion

        #region Methods

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
                    RaiseError("Failed loading data. No internet access?");
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

        #endregion
    }
}