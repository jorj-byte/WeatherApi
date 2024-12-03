using Microsoft.EntityFrameworkCore;
using WeatherApi.Model.Dbs;
using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WeatherContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<WeatherService>().ConfigureHttpClient((sp, c) =>
{
    c.Timeout = TimeSpan.FromSeconds(5000);
}).AddStandardResilienceHandler(c =>
{
    c.Retry.Delay = TimeSpan.FromMilliseconds(1);
    c.Retry.MaxRetryAttempts = 5;
    c.AttemptTimeout.Timeout = TimeSpan.FromMilliseconds(2000);
    c.TotalRequestTimeout.Timeout = TimeSpan.FromMilliseconds(5000);
    c.CircuitBreaker.BreakDuration = TimeSpan.FromMinutes(10);
    c.CircuitBreaker.FailureRatio = 0.6;
});
var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();