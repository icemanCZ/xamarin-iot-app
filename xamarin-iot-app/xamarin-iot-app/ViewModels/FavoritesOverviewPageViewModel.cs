using xamarin_iot_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;

namespace xamarin_iot_app.ViewModels
{
    internal class FavoritesOverviewPageViewModel : ChartPageViewModel
    {
        #region Constructors

        public FavoritesOverviewPageViewModel()
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<Sensor>> GetDataAsync()
        {
            return await apiService.FavoritedSensorsDataAsync(intervalHours);
        }

        #endregion
    }
}