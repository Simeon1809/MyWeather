using Microsoft.AspNetCore.Mvc;
using MyWeather.Application;
using MyWeather.WebUI.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace MyWeather.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpRequest;
        private readonly IWeatherRepository _repository;
        public HomeController(IWeatherRepository repository, IHttpClientFactory httpRequest)
        {
            _httpRequest = httpRequest;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FetchData(RequestModel model)
        {
            try
            {
                //var Url = $"https://api.weatherapi.com/v1/forecast.json?key=e5feb2eac13440e895f122140240905={model.city}&days={model.days}";
               // var Url = $"https://api.weatherapi.com/v1/forecast.json?key=e5feb2eac13440e895f122140240905{model.city}&days={model.days}";

                var Url = $"https://api.weatherapi.com/v1/forecast.json?key=e5feb2eac13440e895f122140240905&q={model.city}&days={model.days}";

                var requestData = new HttpRequestMessage(
                    HttpMethod.Get, Url);

                var httpClient = _httpRequest.CreateClient();
                var responseData = await httpClient.SendAsync(requestData);


                if (responseData.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await responseData.Content.ReadAsStreamAsync();

                    var jsonResult = await JsonSerializer.DeserializeAsync
                        <WeatherForcastViewModel>(contentStream);

                    if (jsonResult != null)
                    {
                        var weatherDto = new List<WeatherDTO>();

                        var dataCount = jsonResult.forecast.forecastday.Count();

                        foreach (var item in jsonResult.forecast.forecastday)
                        {
                            weatherDto.Add(new WeatherDTO()
                            {
                                AvTempratureC = Convert.ToDecimal(item.day.avgtemp_c),
                                AvTempratureF = Convert.ToDecimal(item.day.avgtemp_f),
                                City = jsonResult.location.name,
                                Country = jsonResult.location.country,
                                ForecastDate = item.date.ToCustomDate(),
                                Humidity = item.day.avghumidity,
                                MaxWindKPH = Convert.ToDecimal(item.day.maxwind_kph),
                                MaxWindMPH = Convert.ToDecimal(item.day.maxwind_mph),
                                Weather_Condition = item.day.condition.text,
                            });
                        }

                        await _repository.AddForecasts(weatherDto);

                        return Json(new
                        {
                            error = false,
                            message = "Data Saved Successfully"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        error = true,
                        message = $"There was an Error"
                    });
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    error = true,
                    message = e.Message
                });
            }

            return Json(new
            {
                error = true,
                message = "An error occurred"
            });


        }

        public IActionResult Chat()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadChatData()
        {
            var forecastsList = await _repository.FetchWeatherHistories();
            var chatData = new List<ChatDataModel>();

            foreach (var item in forecastsList)
            {
                chatData.Add(new ChatDataModel
                {
                    date = item.ForecastDate.ToString("dd-MMM-yyyy"),
                    value = item.AvTempratureC
                });
            }

            return Json(new { data = chatData });
        }
    }
}
