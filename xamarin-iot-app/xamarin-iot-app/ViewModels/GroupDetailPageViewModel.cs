using xamarin_iot_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;

namespace xamarin_iot_app.ViewModels
{
    internal class GroupDetailPageViewModel : ChartPageViewModel
    {
        #region Fields

        private string header;

        #endregion

        #region Properties

        public SensorGroup Group { get; set; }

        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        #endregion

        #region Constructors

        public GroupDetailPageViewModel(SensorGroup group)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
            Header = group.Name;
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<Sensor>> GetDataAsync()
        {
            return await apiService.GroupSensorsDataAsync(Group.Id, intervalHours);
        }

        #endregion
    }
}