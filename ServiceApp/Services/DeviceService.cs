using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common;
using ServiceApp.Interfaces;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Services
{
    public class DeviceService
    {
        private static ServiceClient serviceClient =
            ServiceClient.CreateFromConnectionString(Config.IotHubConnectionString);
        public static int telemetryInterval = 5;

        public static bool ParseInput(string input) 
            => int.TryParse(input, out telemetryInterval);

        // Testad med omvänd if-sats (och return) istället för while-sats (interface för input-simulering)
        public static async Task<CloudToDeviceMethodResult> ReadAndSendInput(IUserInput input)
        {
            while (true)
            {
                Console.WriteLine("Set telemetry interval: ");

                while (!ParseInput(input.ReadLine()) || telemetryInterval < 1)
                    Console.WriteLine($"Please enter a numerical value between 1-{int.MaxValue}.");

                InvokeMethod("DeviceApp", "SetTelemetryInterval", telemetryInterval.ToString()).GetAwaiter();
                await Task.Delay(1000);
            }
        }

        public static async Task<CloudToDeviceMethodResult> InvokeMethod(string deviceId, string methodName, string payload)
        {
            // För att trigga "bad request" från SetTelemetryInterval om payload är tom
            if (string.IsNullOrEmpty(payload))
                payload = "0";

            var methodInvocation = new CloudToDeviceMethod(methodName, TimeSpan.FromSeconds(10));
            methodInvocation.SetPayloadJson(payload);

            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);

            Console.WriteLine("-----");
            Console.WriteLine($"Response status: {response.Status}");
            Console.WriteLine(response.GetPayloadAsJson());
            Console.WriteLine("-----");

            return response;
        }
    }
}
