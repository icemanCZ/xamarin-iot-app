using Newtonsoft.Json;
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
    public class APIService
    {
        const string API_URL_SENSOR_LIST = "http://homeiot.aspifyhost.cz/api/SensorList";
        const string API_URL_GROUP_LIST = "http://homeiot.aspifyhost.cz/api/GroupList";
        const string API_URL_SENSOR_DATA = "http://homeiot.aspifyhost.cz/api/SensorData?sensorid={ID}&from={FROM}";
        const string API_URL_FAVORITED_SENSORS_DATA = "http://homeiot.aspifyhost.cz/api/favoritedsensorsdata?from={FROM}";
        const string API_URL_GROUP_SENSORS_DATA = "http://homeiot.aspifyhost.cz/api/groupsensorsdata?groupid={ID}&from={FROM}";

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

        public async Task<IEnumerable<SensorGroup>> GroupListAsync()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = await wc.DownloadStringTaskAsync(new Uri(API_URL_GROUP_LIST));
                    var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
                    return data.Select(x => new SensorGroup() { Id = x.id, Name = x.name });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IEnumerable<SensorValue>> SensorDataAsync(int id, int intervalHours)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = await wc.DownloadStringTaskAsync(
                        new Uri(API_URL_SENSOR_DATA
                            .Replace("{ID}", id.ToString())
                            .Replace("{FROM}", DateTime.Now.AddHours(-intervalHours).ToFileTimeUtc().ToString())));
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

        public async Task<IEnumerable<Sensor>> FavoritedSensorsDataAsync(int intervalHours)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = await wc.DownloadStringTaskAsync(
                        new Uri(API_URL_FAVORITED_SENSORS_DATA
                        .Replace("{FROM}", DateTime.Now.AddHours(-intervalHours).ToFileTimeUtc().ToString())));
                    var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
                    return data.Select(x =>
                        new Sensor()
                        {
                            Id = x.id,
                            Name = x.name,
                            Units = x.units,
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

        public async Task<IEnumerable<Sensor>> GroupSensorsDataAsync(int groupId, int intervalHours)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = await wc.DownloadStringTaskAsync(
                        new Uri(API_URL_GROUP_SENSORS_DATA
                        .Replace("{ID}", groupId.ToString())
                        .Replace("{FROM}", DateTime.Now.AddHours(-intervalHours).ToFileTimeUtc().ToString())));
                    var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
                    return data.Select(x =>
                        new Sensor()
                        {
                            Id = x.id,
                            Name = x.name,
                            Units = x.units,
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
