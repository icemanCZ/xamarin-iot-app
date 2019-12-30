using System.Collections.Generic;
using System.Text;
using System;

namespace xamarin_iot_app.Models
{
    public class SensorValue
    {
        public DateTime Timestamp { get; set; }
        public float Value { get; set; }
    }
}