﻿using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xamarin_iot_app.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace xamarin_iot_app.Services
{
    class APIService
    {
        const string API_URL_SENSOR_LIST = "http://homeiot.aspifyhost.cz/api/SensorList";
        const string API_URL_SENSOR_DATA = "http://homeiot.aspifyhost.cz/api/SensorData?sensorid=";
        const string API_URL_FAVORITED_SENSORS_DATA = "http://homeiot.aspifyhost.cz/api/favoritedsensorsdata";

        public async Task<IEnumerable<Sensor>> SensorListAsync()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = await wc.DownloadStringTaskAsync(new Uri(API_URL_SENSOR_LIST));
                    var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
                    return data.OrderByDescending(x => x.isFavorited).Select(x => new Sensor() { Id = x.id, Name = x.name, Units = x.units });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IEnumerable<SensorValue>> SensorDataAsync(int id)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = await wc.DownloadStringTaskAsync(new Uri(API_URL_SENSOR_DATA + id));
                    var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
                    return data.Select(x => new SensorValue() { Timestamp = x.t, Value = x.v });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IEnumerable<Sensor>> FavoritedSensorsDataAsync()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = await wc.DownloadStringTaskAsync(new Uri(API_URL_FAVORITED_SENSORS_DATA));
                    var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
                    var aaa = data.First().data as JContainer;
                    return data.Select(x => 
                        new Sensor()
                        {
                            Id = x.id, Name = x.name, Units = x.units,
                            Values = (x.data as JContainer).OfType<dynamic>().Select(d => new SensorValue() { Timestamp = d.t, Value = d.v })
                        });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
