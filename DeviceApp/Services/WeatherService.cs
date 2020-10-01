using DeviceApp.Models;
using Newtonsoft.Json;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DeviceApp.Services
{
    public class WeatherService
    {
        private static HttpResponseMessage _response;
        private static readonly HttpClient _client = new HttpClient();

        private static readonly string _baseUrl = "https://api.openweathermap.org/data/2.5/";
        private static readonly string _apiUrl = $"{_baseUrl}weather?q=Örebro,se&units=metric&appid=" + Config.ApiKey;

        public static async Task<HttpResponseMessage> ConnectToAPI()
            => await _client.GetAsync(_apiUrl);

        public static async Task<TemperatureModel> FetchWeatherData()
        {
            try
            {
                _response = await ConnectToAPI();

                // Status = 2XX
                if (_response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Resultat i string/JSON -> APIModel
                        return JsonConvert.DeserializeObject<APIModel>(await _response.Content.ReadAsStringAsync());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing data from API - {ex.Message}");
                    }
                }
                else
                    Console.WriteLine($"Error - Status code: {_response.StatusCode}");
            }
            // _response fail, url svarar inte
            catch (Exception ex)
            {
                Console.WriteLine($"{_baseUrl} is not responding - {ex.Message}");
                Console.WriteLine($"Full query: {_apiUrl.Substring(_baseUrl.Length)}");
            }

            return null;
        }
    }
}
