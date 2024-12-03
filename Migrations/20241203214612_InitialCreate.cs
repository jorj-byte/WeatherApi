using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherInfos",
                columns: table => new
                {
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    GenerationtimeMs = table.Column<double>(type: "float", nullable: false),
                    UtcOffsetSeconds = table.Column<double>(type: "float", nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimezoneAbbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Elevation = table.Column<double>(type: "float", nullable: false),
                    HourlyUnitsTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyUnitsTemperature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyTimes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyTemperatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherInfos", x => new { x.Longitude, x.Latitude });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherInfos");
        }
    }
}
