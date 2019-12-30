using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using xamarin_iot_app.Models;
using xamarin_iot_app.Services;
using OxyPlot;
using Xamarin.Forms;
using System.Threading.Tasks;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;

namespace xamarin_iot_app.ViewModels
{
    public abstract class ChartPageViewModel : BaseViewModel
    {
        #region Fields

        protected APIService apiService = DependencyService.Get<APIService>();
        protected int intervalHours = 6;
        private PlotModel chartModel;

        #endregion

        #region Properties

        public PlotModel ChartModel
        {
            get { return chartModel; }
            set { SetProperty(ref chartModel, value); }
        }

        public Command IncreaseIntervalCommand { get; set; }
        public Command LoadDataCommand { get; set; }

        #endregion

        #region Constructors

        public ChartPageViewModel()
        {
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
            IncreaseIntervalCommand = new Command(async () => await ExecuteIncreaseIntervalCommand());
        }

        #endregion

        #region Methods

        protected abstract Task<IEnumerable<Sensor>> GetDataAsync();

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
                var data = await GetDataAsync();
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

        #endregion
    }
}