using MyWeather.Application;

public interface IWeatherRepository
{
        Task<bool> AddForecasts(List<WeatherDTO> forecasts);

        Task<IReadOnlyList<WeatherDTO>> FetchWeatherHistories();

}
