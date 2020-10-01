using DeviceApp.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceApp.Services
{
    public class DeviceService
    {
        // Uppstyckade funktioner för att kunna testa hela vägen

        // _requestResult för att använda TryParse men förhindra nollställning av telemetryInterval
        public static int telemetryInterval = 5;
        private static int _requestResult;
        static string infoString = string.Empty;

        public static Message ConstructMessage(TemperatureModel data)
            => new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));

        public static Message AddProperties(Message message, string key, string value)
        {
            message.Properties.Add(key, value);
            return message;
        }

        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {
            while (true)
            {
                var data = await WeatherService.FetchWeatherData();
                var message = ConstructMessage(data);
                var payload = AddProperties(message, "autumnAlert", (data.Temperature < 15).ToString());

                await deviceClient.SendEventAsync(payload);
                Console.WriteLine($"Message sent: {JsonConvert.SerializeObject(data)}");

                await Task.Delay(telemetryInterval * 1000);
            }
        }

        public static bool ParseRequestData(MethodRequest request) 
            => int.TryParse(Encoding.UTF8.GetString(request.Data).Replace("\"", ""), out _requestResult);

        public static Task<MethodResponse> SetTelemetryInterval(MethodRequest request, object userContext)
        {
            if (ParseRequestData(request) && _requestResult >= 1)
            {
                telemetryInterval = _requestResult;
                Console.WriteLine($"Interval set to {telemetryInterval} seconds.");

                // { "result": "Executed direct method SetTelemetryInterval. Telemetry interval set to xs." }
                var json = 
                    "{\"result\": \"Executed method " 
                    + request.Name + ". Telemetry interval set to "
                    + telemetryInterval + "s.\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 200));
            }
            else
            {
                Console.WriteLine("Request could not be parsed. Telemetry interval unchanged.");

                // { "result": "Bad request. Payload data must be (convertable to) an integer above or equal to 1." }
                string json = $"{{\"result\": \"Bad request. Payload data must be (convertable to) an integer between 1-{int.MaxValue}\"}}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 400));
            }
        }

        public static Task<MethodResponse> DefaultHandler(MethodRequest request, object userContext)
        {
            Console.WriteLine($"Remote call tried executing a method ({request.Name}) not implemented.");
            // { "result": "Method (method name) not implemented." }
            var json = "{\"result\": \"Method (" + request.Name + ") not implemented.\"}";
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 501));
        }

    }
}
