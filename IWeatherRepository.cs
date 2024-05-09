using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IWeatherRepository
{
	public IWeatherRepository()
	{
        Task<bool> AddForecasts(List<WeatherHistoryDTO> forecasts);

        Task<IReadOnlyList<WeatherHistoryDTO>> FetchWeatherHistories();

    }
}
