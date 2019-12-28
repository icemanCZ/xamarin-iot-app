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

namespace xamarin_iot_app.ViewModels
{
    class FavoritesOverviewPageViewModel : BaseViewModel
    {
        private APIService apiService = DependencyService.Get<APIService>();
        private PlotModel chartModel;
        private int intervalHours = 6;

        public PlotModel ChartModel
        {
            get { return chartModel; }
            set { SetProperty(ref chartModel, value); }
        }
        public Command LoadDataCommand { get; set; }
        public Command IncreaseIntervalCommand { get; set; }

        public FavoritesOverviewPageViewModel()
        {
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
            IncreaseIntervalCommand = new Command(async () => await ExecuteIncreaseIntervalCommand());
        }

        private async Task ExecuteIncreaseIntervalCommand()
        {
            if (IsBusy)
                return;

            intervalHours += 6;
            await ExecuteLoadDataCommand();
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
                var data = await apiService.FavoritedSensorsDataAsync(intervalHours);
                if (data != null)
                {
                    var cm = new PlotModel();
                    cm.PlotAreaBorderColor = OxyColors.LightGray;
                    cm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm", MajorGridlineStyle = LineStyle.Dot, MajorGridlineColor = OxyColors.LightGray });
                    cm.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Dot, MajorGridlineColor = OxyColors.LightGray });
                    foreach (var s in data)
                    {

                        var series = new LineSeries { Title = $"{s.Name} {s.Values.LastOrDefault()?.Value} {s.Units}", MarkerType = MarkerType.None };
                        series.Points.AddRange(s.Values.Select(x => new DataPoint(DateTimeAxis.ToDouble(x.Timestamp), x.Value)));
                        cm.Series.Add(series);
                    }
                    ChartModel = cm;
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
