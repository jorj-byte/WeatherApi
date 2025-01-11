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
        var tasks= new List<Task<ApiWeatherDto?>>
        {
            weatherService.GetWeatherAsync(latitude, longitude),
            weatherService.GetWeatherFromDbAsync(latitude, longitude),
            
        };
        var tasksExec = Task.WhenAll(tasks);
        var result=Task.WhenAny(  tasksExec,Task.Delay(5000));
        if (result.Result == tasksExec)
        {
            if (tasks[0].IsCompletedSuccessfully)
            {
             return tasks[0].Result;
            }

            return tasks[1].Result;
        }
        return  default;
    }
}