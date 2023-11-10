using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Scaffold;

public static class CoreDiConfiguration
{
    public static readonly Action<HostBuilderContext, LoggerConfiguration> ConfigurationSerilog = (_, configuration) =>
        configuration.MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.File(new CompactJsonFormatter(), "log.json");
}