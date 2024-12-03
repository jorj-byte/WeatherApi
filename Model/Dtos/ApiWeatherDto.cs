namespace WeatherApi.Model.Dtos;

public record ApiWeatherDto(
    double Latitude,
    double Longitude,
    double Generationtime_Ms,
    double UtcOffsetSeconds,
    string Timezone,
    string Timezone_Abbreviation,
    double Elevation,
    HourlyUnitsDto Hourly_Units,
    HourlyDto Hourly
)
{
    public WeatherInfo WeatherInfo()
    {
        return new WeatherInfo
        {
            Latitude = Latitude,
            Longitude = Longitude,
            GenerationtimeMs = Generationtime_Ms,
            UtcOffsetSeconds = UtcOffsetSeconds,
            Elevation = Elevation,
            Timezone = Timezone,
            TimezoneAbbreviation = Timezone_Abbreviation,
            HourlyUnitsTime =Hourly_Units.Time,
            HourlyUnitsTemperature = Hourly_Units.Temperature_2m,
            HourlyTemperatures = Hourly.Temperature_2m,
            HourlyTimes = Hourly.Time
        };
    }
}

public record HourlyUnitsDto(
    string Time,
    string Temperature_2m
);


public record HourlyDto(
    string[] Time,
    double[] Temperature_2m
);
