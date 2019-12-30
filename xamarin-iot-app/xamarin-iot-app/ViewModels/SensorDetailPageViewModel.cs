using xamarin_iot_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;

namespace xamarin_iot_app.ViewModels
{
    internal class SensorDetailPageViewModel : ChartPageViewModel
    {
        #region Fields

        private string header;

        #endregion

        #region Properties

        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        public Sensor Sensor { get; set; }

        #endregion

        #region Constructors

        public SensorDetailPageViewModel(Sensor sensor)
        {
            Sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
            Header = Sensor.Name;
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<Sensor>> GetDataAsync()
        {
            Sensor.Values = await apiService.SensorDataAsync(Sensor.Id, intervalHours);
            return new[] { Sensor };
        }

        #endregion
    }
}