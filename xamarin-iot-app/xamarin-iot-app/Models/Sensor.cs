using System.Collections.Generic;
using System.Text;
using System;

namespace xamarin_iot_app.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Units { get; set; }
        public bool IsOk { get; set; }
        public IEnumerable<SensorValue> Values { get; set; }
    }
}