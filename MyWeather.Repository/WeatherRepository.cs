using Microsoft.EntityFrameworkCore;
using MyWeather.Application;
using MyWeather.Domain;
using MyWeather.Infrastructure;
using System;

public class WeatherRepository : IWeatherRepository
{
    private readonly AppDbContext _context;
    public WeatherRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddForecasts(List<WeatherDTO> forecasts)
    {

        var existingForecast = _context.WeatherHistories;

        if (existingForecast.Any())
        {
            _context.WeatherHistories.RemoveRange(existingForecast.ToList());
        }

        var weatherList = forecasts.Select(x => new WeatherHistory()
        {
            AvTempratureC = x.AvTempratureC,
            AvTempratureF = x.AvTempratureF,
            City = x.City,
            Country = x.Country,
            ForecastDate = x.ForecastDate,
            Humidity = x.Humidity,
            MaxWindKPH = x.MaxWindKPH,
            MaxWindMPH = x.MaxWindMPH,
            WeatherCondition = x.Weather_Condition,

        });

        await _context.WeatherHistories.AddRangeAsync(weatherList);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<WeatherDTO>> FetchWeatherHistories()
    {
        return await _context.WeatherHistories.Select(x => new WeatherDTO()
        {
            AvTempratureC = x.AvTempratureC,
            AvTempratureF = x.AvTempratureF,
            City = x.City,
            Country = x.Country,
            ForecastDate = x.ForecastDate,
            Humidity = x.Humidity,
            MaxWindKPH = x.MaxWindKPH,
            MaxWindMPH = x.MaxWindMPH,
            Weather_Condition = x.WeatherCondition,
        }).ToListAsync();
    }

}
