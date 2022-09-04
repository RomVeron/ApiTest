using Api.Test.Domain.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Test.Helpers;

public enum DatabaseEngine
{
    Default,
    Oracle,
    Postgres
}

public class DatabaseOptions
{
    public DatabaseEngine Engine { get; set; }
    public string Version { get; set; }
}

public static class Extension
{
    public static IServiceCollection AddSpiDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Database");
        var options = new DatabaseOptions();
        section.Bind(options);
        services.Configure<DatabaseOptions>(section);

        return options.Engine switch
        {
            DatabaseEngine.Postgres => services.AddDbContext<ApiDbContext>(o =>
            {
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                o.UseNpgsql(configuration.GetConnectionString("SpiDbContext"));
            }),
            DatabaseEngine.Oracle => services.AddDbContext<ApiDbContext>(o =>
            {
                o.UseOracle(configuration.GetConnectionString("SpiDbContext"),
                    options.Version.Equals("11") || options.Version.Equals("12")
                        ? config => config.UseOracleSQLCompatibility(options.Version) : null);
            }),
            _ => throw new Exception("No se establecio el motor de base de datos"),
        };
    }
}
