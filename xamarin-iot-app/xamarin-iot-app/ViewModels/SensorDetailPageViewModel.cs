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
    class SensorDetailPageViewModel : BaseViewModel
    {
        private APIService apiService = DependencyService.Get<APIService>();
        private PlotModel chartModel;
        private string header;
        private int intervalHours = 6;

        public Sensor Sensor { get; set; }
        public PlotModel ChartModel
        {
            get { return chartModel; }
            set { SetProperty(ref chartModel, value); }
        }
        public Command LoadDataCommand { get; set; }
        public Command IncreaseIntervalCommand { get; set; }
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        public SensorDetailPageViewModel(Sensor sensor)
        {
            Sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
            Header = Sensor.Name;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
            IncreaseIntervalCommand = new Command(async () => await ExecuteIncreaseIntervalCommand());
        }

        protected override void InitializeInternal()
        {
            LoadDataCommand.Execute(null);
        }

        private async Task ExecuteIncreaseIntervalCommand()
        {
            if (IsBusy)
                return;

            intervalHours += 6;
            await ExecuteLoadDataCommand();
        }

        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Sensor.Values = await apiService.SensorDataAsync(Sensor.Id, intervalHours);
                if (Sensor.Values != null)
                {
                    var cm = new PlotModel();
                    cm.IsLegendVisible = false;
                    cm.PlotAreaBorderColor = OxyColors.LightGray;
                    cm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm", MajorGridlineStyle = LineStyle.Dot, MajorGridlineColor = OxyColors.LightGray });
                    cm.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Dot, MajorGridlineColor = OxyColors.LightGray });
                    var series = new LineSeries { MarkerType = MarkerType.None };
                    series.Points.AddRange(Sensor.Values.Select(x => new DataPoint(DateTimeAxis.ToDouble(x.Timestamp), x.Value)));
                    cm.Series.Add(series);
                    ChartModel = cm;
                    Header = $"{Sensor.Name} {(Sensor.Values.LastOrDefault()?.Value.ToString() ?? "?")}{Sensor.Units}";
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
