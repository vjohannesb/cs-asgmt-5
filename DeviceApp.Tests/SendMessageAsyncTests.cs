using DeviceApp.Models;
using DeviceApp.Services;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharedLibrary;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeviceApp.Tests
{
    public class SendMessageAsyncTests
    {
        private static readonly TemperatureModel _weatherData = WeatherService.FetchWeatherData().Result;

        [Fact]
        public void ConstructMessage_ShouldReturnAMessageType() 
            => Assert.IsType<Message>(DeviceService.ConstructMessage(_weatherData));

        [Fact]
        public void AddProperties_ShouldNotCopyMessageParameter()
        {
            var message = DeviceService.ConstructMessage(_weatherData);
            var messageWithProperties = DeviceService.AddProperties(message, "keyAlert", "valueAlert");
            Assert.Same(message, messageWithProperties);
        }

        [Fact]
        public void AddProperties_ShouldAddPropertyToMessage()
        {
            var message = DeviceService.ConstructMessage(_weatherData);
            DeviceService.AddProperties(message, "keyAlert", "valueAlert");
            Assert.Equal("valueAlert", message.Properties["keyAlert"]);

            /*
            Assert.Contains("keyAlert", message.Properties);
            Assert.Contains("keyAlert", message.Properties.Keys);
            Assert.Contains("valueAlert", message.Properties.Values);
            */
        }



    }
}
