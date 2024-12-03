using Microsoft.AspNetCore.Mvc;
using WeatherApi.Model.Dtos;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
public class WeatherController(WeatherService weatherService) : ControllerBase
{
    
    [Route("/GetWeather")]
    [HttpGet]
    public async Task<ActionResult<ApiWeatherDto>> GetWeather([FromQuery] double latitude, [FromQuery] double longitude)
    {
        var weatherInfo = await weatherService.GetWeatherAsync(latitude, longitude);
        return weatherInfo != null ? Ok(weatherInfo) : NoContent();
    }
}