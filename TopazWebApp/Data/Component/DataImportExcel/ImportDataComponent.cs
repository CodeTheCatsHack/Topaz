using System.Collections.ObjectModel;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.FileProviders;
using Scaffold.Model;
using Topaz.Data.Service;

namespace Topaz.Data;

public class ImportDataComponent : ComponentBase
{
    protected DataGrid<Measure> DataGridView;

    [Inject] public ServiceInfoMeasure ServiceInfoMeasure { get; set; }
    [Inject] private IWebHostEnvironment HostingEnvironment { get; set; }

    protected ViewData ViewDataStatus { get; set; } = ViewData.Measure;
    protected Measure SelectedMeasure { get; set; }
    protected ObservableCollection<Measure> DataViewMeasure { get; set; } = new();
    protected ObservableCollection<MeasureInfo> DataViewMeasureInfos { get; set; } = new();
    protected ObservableCollection<MessagingMetric> DataViewMessagingMetrics { get; set; } = new();
    protected ObservableCollection<ReferenceInfoMetric> DataViewReferenceInfos { get; set; } = new();
    protected ObservableCollection<HttpTransmittingMetric> DataViewHttpTransmittingMetrics { get; set; } = new();
    protected ObservableCollection<VoiceConnectionMetric> DataViewVoiceConnectionMetrics { get; set; } = new();

    protected int Total { get; set; }

    protected override async Task OnInitializedAsync()
    {
        DataViewMeasure = ServiceInfoMeasure.Measures;

        foreach (var measure in ServiceInfoMeasure.Measures)
        {
            var group = measure.MeasureGroup.First();
            if (group.ReferenceInfoMetric != null)
                DataViewReferenceInfos.Add(group.ReferenceInfoMetric);
        }

        foreach (var measure in ServiceInfoMeasure.Measures)
        {
            var group = measure.MeasureGroup.First();
            if (group.HttpTransmittingMetric != null)
                DataViewHttpTransmittingMetrics.Add(group.HttpTransmittingMetric);
        }

        foreach (var measure in ServiceInfoMeasure.Measures)
        {
            var group = measure.MeasureGroup.First();
            if (group.VoiceConnectionMetric != null)
                DataViewVoiceConnectionMetrics.Add(group.VoiceConnectionMetric);
        }

        foreach (var measure in ServiceInfoMeasure.Measures)
        {
            var group = measure.MeasureGroup.First();
            if (group.MessagingMetric != null)
                DataViewMessagingMetrics.Add(group.MessagingMetric);
        }

        foreach (var measure in ServiceInfoMeasure.Measures)
            if (measure.MeasureInfo != null)
                DataViewMeasureInfos.Add(measure.MeasureInfo);

        await base.OnInitializedAsync();
    }

    #region Валидация

    protected Blazorise.Validations Validator;

    protected async Task Validation()
    {
        if (await Validator.ValidateAll())
        {
        }
        else
        {
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
        ReferanceMetricsInfo,
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
        ViewDataStatus = ViewData.ReferanceMetricsInfo;
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
        var filesToUpload = new List<IFileEntry>(selectedFiles);
        var wwwrootFileProvider = new PhysicalFileProvider(HostingEnvironment.WebRootPath);
        var directoryContents = wwwrootFileProvider.GetDirectoryContents("file");

        if (!directoryContents.Exists) Directory.CreateDirectory(Path.Combine(HostingEnvironment.WebRootPath, "file"));

        foreach (var file in filesToUpload)
        {
            var fileName = Path.Combine("file", $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}");

            var filePath = Path.Combine(HostingEnvironment.WebRootPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(stream);
        }

        selectedFiles.Clear();
        ServiceInfoMeasure.GetDataMeasures();
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