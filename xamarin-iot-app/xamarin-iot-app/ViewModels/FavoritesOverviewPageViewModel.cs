using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin_iot_app.Services;
using System.Diagnostics;
using xamarin_iot_app.Models;

namespace xamarin_iot_app.ViewModels
{
    class FavoritesOverviewPageViewModel : ChartPageViewModel
    {
        public FavoritesOverviewPageViewModel()
        {

        }

        protected override async Task<IEnumerable<Sensor>> GetDataAsync()
        {
            return await apiService.FavoritedSensorsDataAsync(intervalHours);
        }
    }
}
