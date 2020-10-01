using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using ServiceApp.Interfaces;
using ServiceApp.Services;
using SharedLibrary;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ServiceApp
{
    // Dependency injection för att kunna testa/simulera input

    public class UserInput : IUserInput
    {
        public string ReadLine() => Console.ReadLine().Trim();
    }

    public class Program 
    {
        static void Main(string[] args)
        {
            UserInput input = new UserInput();
            DeviceService.ReadAndSendInput(input).Wait();
        }
    }
}
