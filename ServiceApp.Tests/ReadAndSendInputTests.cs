using ServiceApp.Interfaces;
using ServiceApp.Services;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace ServiceApp.Tests
{
    // Dependency injection för att kunna testa/simulera input
    class SimulatedInput : IUserInput
    {
        public SimulatedInput(string str)
        {
            SimInput = str;
        }
        public string SimInput { get; set; }
        public string ReadLine() => SimInput;
    }

    public class ReadAndSendInputTests
    {

        [Theory]
        [InlineData("10", true)]
        [InlineData("\"1\"", false)]
        [InlineData("0.99", false)]
        [InlineData("five", false)]
        public void ParseInput_ShouldReturnAppropriateBool(string data, bool expectedBool)
        {
            Assert.Equal(expectedBool, DeviceService.ParseInput(data));
        }

        /* Deprecated
        [Theory]
        [InlineData("10", 10)]
        [InlineData("\"1\"", 1)]
        [InlineData("0.99", 0)]
        [InlineData("five", 0)]
        public void ReadAndSendInput_ShouldProcessInput(string data, int expectedValue)
        {
            DeviceService.ReadAndSendInput(new SimulatedInput(data)).GetAwaiter();
            Assert.Equal(expectedValue, DeviceService.telemetryInterval);
        }
        */
    }
}
