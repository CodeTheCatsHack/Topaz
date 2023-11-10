using Clave.Expressionify;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Scaffold;

public static class ConfiguredTopazContext
{
    public static void ConfigurationMySql(this DbContextOptionsBuilder options, IConfiguration configuration)
    {
        options.UseMySql(configuration.GetConnectionString("TopazContext"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("TopazContext")),
                x => x.UseNetTopologySuite().EnableRetryOnFailure())
            .FullDbContextOptions();
    }

    public static DbContextOptionsBuilder FullDbContextOptions(this DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder
            .UseExpressionify(o => o.WithEvaluationMode(ExpressionEvaluationMode.FullCompatibilityButSlow))
            .UseAllCheckConstraints()
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
}