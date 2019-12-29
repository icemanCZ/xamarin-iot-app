using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin_iot_app.Models;
using xamarin_iot_app.Services;

namespace xamarin_iot_app.ViewModels
{
    class GroupDetailPageViewModel : ChartPageViewModel
    {
        private string header;

        public SensorGroup Group { get; set; }
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        public GroupDetailPageViewModel(SensorGroup group)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
            Header = group.Name;
        }

        protected override async Task<IEnumerable<Sensor>> GetDataAsync()
        {
            return await apiService.GroupSensorsDataAsync(Group.Id, intervalHours);
        }
    }
}
