using System.Collections.ObjectModel;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.FileProviders;
using Scaffold.Model;
using Topaz.Data.Service;
using static Topaz.Data.Service.ServiceInfoMeasure;

namespace Topaz.Data;

public class ImportDataComponent : ComponentBase
{
    [Inject] private ServiceInfoMeasure ServiceInfoMeasure { get; set; }
    [Inject] private IWebHostEnvironment HostingEnvironment { get; set; }
    [Inject] private ServiceDataDataBase ServiceDataDataBase { get; set; }

    protected ViewData ViewDataStatus { get; set; } = ViewData.Measure;
    protected Measure SelectedMeasure { get; set; } = new();
    protected MeasureInfo SelectedMeasureInfo { get; set; } = new();
    protected ReferenceInfoMetric SelectedReferenceInfoMetric { get; set; } = new();

    protected int Total { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    #region Валидация

    public async Task Save()
    {
        var wwwrootPath = Path.Combine(HostingEnvironment.WebRootPath);

        if (!Directory.Exists(wwwrootPath))
            return;

        foreach (var fileMeasure in FilesMeasures)
        {
            if (!await ServiceDataDataBase.DataMeasure(fileMeasure.Value!))
                continue;

            var fileName = Path.Combine("file", $"{fileMeasure.Key}.xlsx");
            var filePath = Path.Combine(HostingEnvironment.WebRootPath, fileName);
            File.Delete(filePath);
        }
    }

    #endregion

    #region Навигационные кнопки

    protected enum ViewData
    {
        Measure,
        MeasureInfo,
        ReferanceInfo,
        MessagingMetrics,
        HttpTransmittingMetric,
        VoiceConnectionMetric
    }

    protected void OnMeasureClick()
    {
        ViewDataStatus = ViewData.Measure;
    }

    protected void OnMeasureInfoClick()
    {
        ViewDataStatus = ViewData.MeasureInfo;
    }

    protected void OnVoiceConnectionMetricClick()
    {
        ViewDataStatus = ViewData.VoiceConnectionMetric;
    }

    protected void OnMessagingMetricClick()
    {
        ViewDataStatus = ViewData.MessagingMetrics;
    }

    protected void OnHttpTransmittingMetricClick()
    {
        ViewDataStatus = ViewData.HttpTransmittingMetric;
    }

    protected void OnReferenceInfoMetricClick()
    {
        ViewDataStatus = ViewData.ReferanceInfo;
    }

    #endregion

    #region Работа с файловой системой

    protected List<IFileEntry> selectedFiles = new();
    protected FileEdit fileEdit;

    protected async Task OnChanged(FileChangedEventArgs e)
    {
        selectedFiles.AddRange(e.Files);
    }

    protected async Task UploadFiles()
    {
        var wwwrootFileProvider = new PhysicalFileProvider(HostingEnvironment.WebRootPath);
        var directoryContents = wwwrootFileProvider.GetDirectoryContents("file");

        if (!directoryContents.Exists)
            Directory.CreateDirectory(Path.Combine(HostingEnvironment.WebRootPath, "file"));
        var filesToUpload = new List<IFileEntry>(selectedFiles);
        selectedFiles.Clear();
        await ServiceInfoMeasure.GetDataMeasures(filesToUpload);
    }

    #endregion

    #region Динамический Css

    protected bool _collapseNavMenu = true;
    protected string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    protected void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }

    #endregion
}

public static class ObservableExpression
{
    public static ObservableCollection<T> CastTo<T>(this IEnumerable<Measure> list)
    {
        return new ObservableCollection<T>(list
            .Select(x => x.MeasureGroups.Select(group =>
                    (T)group.GetType().GetProperties().First(prop => prop.PropertyType == typeof(T))
                        .GetValue(group, null)!)
                .ToList()).Aggregate((list1, metrics) =>
            {
                list1.AddRange(metrics);
                return list1;
            }));
    }
}