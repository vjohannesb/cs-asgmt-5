using Newtonsoft.Json;
using ServiceApp.Models;
using ServiceApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceApp.Tests
{
    public class InvokeMethodTests
    {
        // Kräver två instanser, en som kör Device och en för Test
        [Theory]
        [InlineData("SetTelemetryInterval", "10", 200)]
        [InlineData("SetTelemetryInterval", "\"1\"", 200)]
        [InlineData("SetTelemetryInterval", "0.99", 400)]
        [InlineData("SetInterval", "10", 501)]
        public async void InvokeMethod_ShouldReturnAppropriateStatusCode(string methodName, string payload, int expectedStatus)
        {
            var response = await DeviceService.InvokeMethod("DeviceApp", methodName, payload);
            Assert.Equal(expectedStatus, response.Status);
        }

        [Theory]
        [InlineData("SetTelemetryInterval", "10", "Executed")]
        [InlineData("SetTelemetryInterval", "\"1\"", "Executed")]
        [InlineData("SetTelemetryInterval", "", "Bad request")]
        [InlineData("SetInterval", "10", "not implemented")]
        public async void InvokeMethod_ReturnShouldContainPayload(string methodName, string payload, string expectedValue)
        {
            var response = await DeviceService.InvokeMethod("DeviceApp", methodName, payload);
            Assert.Contains(expectedValue, response.GetPayloadAsJson());
        }
    }
}
