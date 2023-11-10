using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scaffold.Context;
using Serilog;

namespace Scaffold;

public static class CoreDi
{
    public static IServiceCollection AddCoreDi(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddLoggers()
            //.AddDataBaseContext(configuration)
            .AddCors();
    }

    public static IServiceCollection AddLoggers(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
    }

    public static IServiceCollection AddDataBaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<TopazContext>(g => g.ConfigurationMySql(configuration));
    }
}