using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceApp.Models
{
    public class TemperatureModel
    {
        // Egen klass för "snyggare" meddelanden till IoTD
        public TemperatureModel(double? temp, double? hum)
        {
            Temperature = temp;
            Humidity = hum;
        }

        public double? Temperature { get; set; } = null;
        public double? Humidity { get; set; } = null;

        // Implicit konvertering från APIModel -> TemperatureModel
        public static implicit operator TemperatureModel(APIModel am)
            => new TemperatureModel(am.main.temp, am.main.humidity);
    }
}
