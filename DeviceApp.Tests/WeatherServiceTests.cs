using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DeviceApp.Services;
using DeviceApp.Models;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace DeviceApp.Tests
{
    public class WeatherServiceTests
    {

        [Fact]
        public async void ConnectToAPI_ShouldReturnSuccessStatusCode()
        {
            var connection = await WeatherService.ConnectToAPI();
            Assert.True(connection.IsSuccessStatusCode);
        }

        [Fact]
        public async void FetchWeatherData_ShouldReturnATemperatureModel()
        {
            var data = await WeatherService.FetchWeatherData();
            Assert.IsType<TemperatureModel>(data);
        }

        [Fact]
        public async void FetchWeatherData_ReturnShouldContainDataFromAPI()
        {
            var data = await WeatherService.FetchWeatherData();
            Assert.NotNull(data?.Temperature);
            Assert.NotNull(data?.Humidity);
        }

        [Fact]
        public void APIModel_ShouldImplicitlyConvertToTemperatureModel()
        {
            var data = new { main = new { temp = 1.01, humidity = 1.01 } };
            TemperatureModel temperatureModel 
                = JsonConvert.DeserializeObject<APIModel>(JsonConvert.SerializeObject(data));

            Assert.Equal(1.01, temperatureModel.Temperature);
            Assert.Equal(1.01, temperatureModel.Humidity);
        }
    }
}
