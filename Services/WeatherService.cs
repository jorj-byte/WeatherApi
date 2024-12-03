using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WeatherApi.Model.Dbs;
using WeatherApi.Model.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace WeatherApi.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherContext _weatherContext;
    private readonly IMemoryCache _cache;

    public  WeatherService(HttpClient httpClientFactory, WeatherContext weatherContext, IMemoryCache cache) 
    {
        _httpClient = httpClientFactory;
        _weatherContext = weatherContext;
        _cache = cache;
    }
    public async Task<ApiWeatherDto> GetWeatherAsync(double latitude, double longitude)
    {
      ApiWeatherDto weatherInfo = null;
      var cachekey = $"WeatherInfo-{latitude}-{longitude}";
  //We can get weather first from cache if needed get from api.
  
        try
        {
            
            var response = await _httpClient.GetAsync(
                $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                weatherInfo = JsonSerializer.Deserialize<ApiWeatherDto>(content,new JsonSerializerOptions(){PropertyNameCaseInsensitive = true});
            }

            if (weatherInfo != null) await SaveWeatherDataToDatabase(weatherInfo);
        }
        catch
        {
            weatherInfo = await _cache.GetOrCreateAsync(cachekey,
                async c => { return await GetWeatherDataFromDatabase(latitude, longitude); },
                new MemoryCacheEntryOptions() {SlidingExpiration = TimeSpan.FromMinutes(10)});
        }

        return weatherInfo;
    }

    private async Task SaveWeatherDataToDatabase(ApiWeatherDto weatherInfo)
    {
        var result=_weatherContext.WeatherInfos
            .FirstOrDefault(w => w.Latitude == weatherInfo.Latitude && w.Longitude == weatherInfo.Longitude);
        if(result is null)
            await _weatherContext.WeatherInfos.AddAsync(weatherInfo!.WeatherInfo());
        else
         _weatherContext.WeatherInfos.Update(weatherInfo!.WeatherInfo());
        var cahcekey = $"WeatherInfo-{weatherInfo.Latitude}-{weatherInfo.Longitude}";
        _cache.Set(cahcekey, weatherInfo, TimeSpan.FromMinutes(10));
        await _weatherContext.SaveChangesAsync();
    }

    private async Task<ApiWeatherDto?> GetWeatherDataFromDatabase(double latitude, double longitude)
    {
        var weatherRecord = _weatherContext.WeatherInfos.AsNoTracking()
            .OrderByDescending(c => c.LastUpdateTime)
            .FirstOrDefault(w => w.Latitude == latitude && w.Longitude == longitude);

        if (weatherRecord == null)
            return null;
        return weatherRecord.ToApiWeatherDto();
    }
}