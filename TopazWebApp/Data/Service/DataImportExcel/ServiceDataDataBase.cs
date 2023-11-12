using Microsoft.EntityFrameworkCore;
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

    public async Task<Measure?> GetDataMeasureById(int measureId)
    {
        if (measureId == 0)
        {
            Measure? result = await TopazContext.Measures
                .Include(x => x.MeasureInfo)
                .Include(x => x.MeasureGroups).ThenInclude(x => x.VoiceConnectionMetric)
                .Include(x => x.MeasureGroups).ThenInclude(x => x.MessagingMetric)
                .Include(x => x.MeasureGroups).ThenInclude(x => x.HttpTransmittingMetric)
                .Include(x => x.MeasureGroups).ThenInclude(x => x.ReferenceInfoMetric)
                .OrderBy(x => x.IdMeasure)
                .LastOrDefaultAsync();
            return result ?? new Measure();
        }
        else
        {
            return await TopazContext.Measures
                .Include(x => x.MeasureInfo)
                .Include(x => x.MeasureGroups).ThenInclude(x => x.VoiceConnectionMetric)
                .Include(x => x.MeasureGroups).ThenInclude(x => x.MessagingMetric)
                .Include(x => x.MeasureGroups).ThenInclude(x => x.HttpTransmittingMetric)
                .Include(x => x.MeasureGroups).ThenInclude(x => x.ReferenceInfoMetric)
                .FirstOrDefaultAsync(m => m.IdMeasure == measureId);
        }        
    }

    public async Task<List<Measure>?> GetLastTenMeasures()
    {
        return (await TopazContext.Measures
            .Include(x => x.MeasureInfo)
            .Include(x => x.MeasureGroups).ThenInclude(x => x.VoiceConnectionMetric)
            .Include(x => x.MeasureGroups).ThenInclude(x => x.MessagingMetric)
            .Include(x => x.MeasureGroups).ThenInclude(x => x.HttpTransmittingMetric)
            .Include(x => x.MeasureGroups).ThenInclude(x => x.ReferenceInfoMetric)
            .ToListAsync())
            .TakeLast(10)
            .ToList();
    }

    public async Task SaveMeasure(Measure measure)
    {
        if (measure.IdMeasure == 0)
        {
            TopazContext.Add(measure);
        }
        else
        {
            TopazContext.Update(measure);
        }
        
        await TopazContext.SaveChangesAsync();
    }
}