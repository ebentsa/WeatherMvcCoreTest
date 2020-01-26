using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherMvcCoreTest.Models;
using WeatherMvcCoreTest.Services;

namespace WeatherMvcCoreTest.Controllers
{
    public class WeatherController : Controller
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OpenWeatherConfiguration _openWeatherconfiguration;

        public WeatherController(ILogger<WeatherController> logger, OpenWeatherConfiguration openWeatherconfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _openWeatherconfiguration = openWeatherconfiguration;
                _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public ActionResult Weather()
        {
            string cityValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["city"];
            if (cityValueFromContext != null) {
                return RedirectToAction("WeatherDetail", "Weather", new { city = cityValueFromContext });
            }

            return View(); 
        }

        [HttpPost]
        public JsonResult Weather(string Prefix)
        {
            WeatherService service = new WeatherService(_openWeatherconfiguration);

            var allCities = service.GetAllCities();

            var CityList = (from N in allCities
                            where N.Name.StartsWith(Prefix)
                            select new { N.Name });
            return Json(CityList);
        }

        public ActionResult WeatherDetail(string city)
        {
            WeatherService service = new WeatherService(_openWeatherconfiguration);
            try
            {
                Response.Cookies.Append(
                    "City",
                    city
                );

                var model = service.GetWeatherByCity(city);
                return View(model);
            }
            catch (Exception ex) {
                if (ex.Message == "Not Found") {
                   return RedirectToAction("Weather", new { message= ex.Message });
                }
            }
            return View();
        }   

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
