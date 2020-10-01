using DeviceApp.Services;
using Microsoft.Azure.Devices.Client;
using SharedLibrary;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DeviceApp
{
    /*
     * DeviceApp    = Skicka väderdata till molnet
     *              = Ta emot Direct Method meddelande
     *                  - Ändra intervallet
     *                  - Skriva ut att meddelande mottagits
     *                  - Plus att tiden ändrats       
     * Enhetstest
     * 
     * ServiceApp   = Skicka Direct Method till DeviceApp
     *                  - Direct Method ska ändra intervall
     *                  - Få svar tillbaka?
     */

    class Program
    {
        private static DeviceClient deviceClient =
            DeviceClient.CreateFromConnectionString(Config.IotDeviceConnectionString, TransportType.Mqtt);

        static void Main(string[] args)
        {
            deviceClient.SetMethodHandlerAsync("SetTelemetryInterval", DeviceService.SetTelemetryInterval, null);
            deviceClient.SetMethodDefaultHandlerAsync(DeviceService.DefaultHandler, null);
            DeviceService.SendMessageAsync(deviceClient).Wait();

            Console.ReadKey();
        }


    }
}
