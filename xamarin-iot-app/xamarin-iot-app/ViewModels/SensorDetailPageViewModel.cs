﻿using OxyPlot;
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
    class SensorDetailPageViewModel : ChartPageViewModel
    {
        private string header;

        public Sensor Sensor { get; set; }
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        public SensorDetailPageViewModel(Sensor sensor)
        {
            Sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
            Header = Sensor.Name;
        }

        protected override async Task<IEnumerable<Sensor>> GetDataAsync()
        {
            Sensor.Values = await apiService.SensorDataAsync(Sensor.Id, intervalHours);
            return new[] { Sensor };
        }
    }
}
