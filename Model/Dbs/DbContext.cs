using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WeatherApi.Model.Dbs;

public class WeatherContext:DbContext
{
    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
    {
        
    }
    public DbSet<WeatherInfo> WeatherInfos { get; set; } = null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var decimalConverter = new ValueConverter<decimal, decimal>(
            v => v,
            v => v
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(decimal))
                {
                    property.SetValueConverter(decimalConverter);
                }
            }
        }

        modelBuilder.Entity<WeatherInfo>().HasKey(e =>new { e.Longitude, e.Latitude
    });
        base.OnModelCreating(modelBuilder);
    }

   
}
