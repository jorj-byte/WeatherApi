using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherApi.Model.Dtos;

namespace WeatherApi.Model;


public class WeatherInfo
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double GenerationtimeMs { get; set; }
    public double UtcOffsetSeconds { get; set; }
    public string Timezone { get; set; }
    public string TimezoneAbbreviation { get; set; }
    public double Elevation { get; set; }
    public string HourlyUnitsTime { get; set; }
    public string HourlyUnitsTemperature { get; set; }
    public string[] HourlyTimes { get; set; }
    public double[] HourlyTemperatures { get; set; }
    public DateTime LastUpdateTime { get; set; } = DateTime.Now;


    public ApiWeatherDto ToApiWeatherDto()
    {
        return new ApiWeatherDto(Latitude, Longitude, GenerationtimeMs, UtcOffsetSeconds, Timezone,
            TimezoneAbbreviation,Elevation, new HourlyUnitsDto(HourlyUnitsTime, HourlyUnitsTemperature),  new HourlyDto(HourlyTimes, HourlyTemperatures));
    }
}


