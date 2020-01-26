using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherMvcCoreTest.Models;

namespace WeatherMvcCoreTest.Services
{
    public class WeatherService
    {
        private readonly OpenWeatherConfiguration _openWeather;
        public WeatherService(OpenWeatherConfiguration openWeather) {
            _openWeather = openWeather;
        }

        public List<City> GetAllCities() {

            var cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText("Content/citylist.json"));

            return cities;
        }

        public ResultViewModel GetWeatherByCity(string city) {

            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", city, _openWeather.AppId);

            using (WebClient client = new WebClient())
            {
                    string json = client.DownloadString(url);
                    RootObject weatherInfo = JsonConvert.DeserializeObject<RootObject>(json);

                    ResultViewModel rslt = new ResultViewModel();

                    rslt.Country = weatherInfo.sys.country;
                    rslt.City = weatherInfo.name;
                    rslt.Main = weatherInfo.weather[0].main;
                    rslt.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                    rslt.TempMin = Convert.ToString(weatherInfo.main.temp_min);

                    return rslt;
            }
        }

    }
}
