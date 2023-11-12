using Scaffold.Context;
using Scaffold.Model;

namespace Topaz.Data.Service;

public class ServiceDataDataBase
{
    public readonly ILogger<ServiceDataDataBase> Logger;
    public readonly TopazContext TopazContext;

    public ServiceDataDataBase(TopazContext context, ILogger<ServiceDataDataBase> logger)
    {
        TopazContext = context;
        Logger = logger;
    }

    public async Task<bool> DataMeasure(Measure measure)
    {
        try
        {
            await TopazContext.Measures.AddAsync(measure);
            await TopazContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            return false;
        }

        return true;
    }
}