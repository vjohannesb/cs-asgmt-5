using MAD = Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DeviceApp.Services;

namespace DeviceApp.Tests
{
    public class SetTelemetryIntervalTests
    {
        [Theory]
        [InlineData("10", true)]
        [InlineData("\"1\"", true)]
        [InlineData("0.99", false)]
        [InlineData("five", false)]
        public static void ParseRequestData_ShouldReturnAppropriateBool(string data, bool expectedBool)
        {
            var methodRequest = new MethodRequest("", Encoding.UTF8.GetBytes(data));
            Assert.Equal(expectedBool, DeviceService.ParseRequestData(methodRequest));
        }

        [Theory]
        [InlineData("10", 200)]
        [InlineData("\"1\"", 200)]
        [InlineData("0.99", 400)]
        [InlineData("five", 400)]
        public async void SetTelemetryInterval_ShouldReturnAppropriateStatusCode(string data, int expectedStatusCode)
        {
            var methodRequest = new MethodRequest("", Encoding.UTF8.GetBytes(data));
            var response = await DeviceService.SetTelemetryInterval(methodRequest, new object());

            Assert.Equal(expectedStatusCode, response.Status);
        }

        [Theory]
        [InlineData("10", 10)]
        [InlineData("\"1\"", 1)]
        [InlineData("100", 100)]
        public async void SetTelemetryInterval_ShouldChangeTelemetryInterval(string data, int expectedValue)
        {
            await DeviceService.SetTelemetryInterval(new MethodRequest("", Encoding.UTF8.GetBytes(data)), new object());

            Assert.Equal(expectedValue, DeviceService.telemetryInterval);
        }
    }
}
