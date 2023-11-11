using System.Collections.ObjectModel;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Scaffold.Model;
using Topaz.Data.Service;

namespace Topaz.Data;

public class ImportDataComponent : ComponentBase
{
    public enum ViewData
    {
        Measure,
        MeasureInfo,
        ReferanceInfo,
        MessagingMetrics,
        ReferanceMetricsInfo,
        HttpTransmittingMetric,
        VoiceConnectionMetric
    }

    protected DataGrid<Measure> _dataGrid;

    [Inject] public ServiceInfoMeasure ServiceInfoMeasure { get; set; }
    [Inject] private IWebHostEnvironment HostingEnvironment { get; set; }

    public ViewData ViewDataStatus { get; set; } = ViewData.Measure;

    protected ObservableCollection<Measure> DataViewMeasure { get; set; } = new();
    protected ObservableCollection<MeasureInfo> DataViewMeasureInfos { get; set; } = new();
    protected ObservableCollection<MessagingMetric> DataViewMessagingMetrics { get; set; } = new();
    protected ObservableCollection<ReferenceInfoMetric> DataViewReferenceInfos { get; set; } = new();
    protected ObservableCollection<HttpTransmittingMetric> DataViewHttpTransmittingMetrics { get; set; } = new();
    protected ObservableCollection<VoiceConnectionMetric> DataViewVoiceConnectionMetrics { get; set; } = new();

    protected int Total { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    public async Task OnReadData(DataGridReadDataEventArgs<Measure> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            List<Measure>? response;

            if (e.ReadDataMode is DataGridReadDataMode.Virtualize)
                response = ServiceInfoMeasure.Measures.Skip(e.VirtualizeOffset).Take(e.VirtualizeCount).ToList();
            else if (e.ReadDataMode is DataGridReadDataMode.Paging)
                response = ServiceInfoMeasure.Measures.Skip((e.Page - 1) * e.PageSize).Take(e.PageSize).ToList();
            else
                throw new Exception("Unhandled ReadDataMode");

            if (!e.CancellationToken.IsCancellationRequested)
            {
                Total = ServiceInfoMeasure.Measures.Count;
                DataViewMeasure = new ObservableCollection<Measure>(response);
            }
        }
    }

    public async Task OnReadData(DataGridReadDataEventArgs<MeasureInfo> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            List<MeasureInfo> response = new();

            foreach (var measure in ServiceInfoMeasure.Measures)
                if (measure.MeasureInfo != null)
                    response.Add(measure.MeasureInfo);
            // this can be call to anything, in this case we're calling a fictional api
            //var response = await Http.GetJsonAsync<Employee[]>( $"some-api/employees?page={e.Page}&pageSize={e.PageSize}" );
            if (e.ReadDataMode is DataGridReadDataMode.Virtualize)
                response = response.Skip(e.VirtualizeOffset).Take(e.VirtualizeCount).ToList();
            else if (e.ReadDataMode is DataGridReadDataMode.Paging)
                response = response.Skip((e.Page - 1) * e.PageSize).Take(e.PageSize).ToList();
            else
                throw new Exception("Unhandled ReadDataMode");

            if (!e.CancellationToken.IsCancellationRequested)
            {
                Total = ServiceInfoMeasure.Measures.Count;
                DataViewMeasureInfos =
                    new ObservableCollection<MeasureInfo>(response); // an actual data for the current page
            }
        }
    }

    public async Task OnReadData(DataGridReadDataEventArgs<MessagingMetric> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            List<MessagingMetric> response = new();

            foreach (var measure in ServiceInfoMeasure.Measures)
            {
                var group = measure.MeasureGroup.First();
                if (group.MessagingMetric != null)
                    response.Add(group.MessagingMetric);
            }

            // this can be call to anything, in this case we're calling a fictional api
            //var response = await Http.GetJsonAsync<Employee[]>( $"some-api/employees?page={e.Page}&pageSize={e.PageSize}" );
            if (e.ReadDataMode is DataGridReadDataMode.Virtualize)
                response = response.Skip(e.VirtualizeOffset).Take(e.VirtualizeCount).ToList();
            else if (e.ReadDataMode is DataGridReadDataMode.Paging)
                response = response.Skip((e.Page - 1) * e.PageSize).Take(e.PageSize).ToList();
            else
                throw new Exception("Unhandled ReadDataMode");

            if (!e.CancellationToken.IsCancellationRequested)
            {
                Total = ServiceInfoMeasure.Measures.Count;
                DataViewMessagingMetrics =
                    new ObservableCollection<MessagingMetric>(response); // an actual data for the current page
            }
        }
    }

    public async Task OnReadData(DataGridReadDataEventArgs<ReferenceInfoMetric> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            List<ReferenceInfoMetric> response = new();

            foreach (var measure in ServiceInfoMeasure.Measures)
            {
                var group = measure.MeasureGroup.First();
                if (group.ReferenceInfoMetric != null)
                    response.Add(group.ReferenceInfoMetric);
            }

            // this can be call to anything, in this case we're calling a fictional api
            //var response = await Http.GetJsonAsync<Employee[]>( $"some-api/employees?page={e.Page}&pageSize={e.PageSize}" );
            if (e.ReadDataMode is DataGridReadDataMode.Virtualize)
                response = response.Skip(e.VirtualizeOffset).Take(e.VirtualizeCount).ToList();
            else if (e.ReadDataMode is DataGridReadDataMode.Paging)
                response = response.Skip((e.Page - 1) * e.PageSize).Take(e.PageSize).ToList();
            else
                throw new Exception("Unhandled ReadDataMode");

            if (!e.CancellationToken.IsCancellationRequested)
            {
                Total = ServiceInfoMeasure.Measures.Count;
                DataViewReferenceInfos =
                    new ObservableCollection<ReferenceInfoMetric>(response); // an actual data for the current page
            }
        }
    }

    public async Task OnReadData(DataGridReadDataEventArgs<VoiceConnectionMetric> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            List<VoiceConnectionMetric> response = new();

            foreach (var measure in ServiceInfoMeasure.Measures)
            {
                var group = measure.MeasureGroup.First();
                if (group.VoiceConnectionMetric != null)
                    response.Add(group.VoiceConnectionMetric);
            }

            // this can be call to anything, in this case we're calling a fictional api
            //var response = await Http.GetJsonAsync<Employee[]>( $"some-api/employees?page={e.Page}&pageSize={e.PageSize}" );
            if (e.ReadDataMode is DataGridReadDataMode.Virtualize)
                response = response.Skip(e.VirtualizeOffset).Take(e.VirtualizeCount).ToList();
            else if (e.ReadDataMode is DataGridReadDataMode.Paging)
                response = response.Skip((e.Page - 1) * e.PageSize).Take(e.PageSize).ToList();
            else
                throw new Exception("Unhandled ReadDataMode");

            if (!e.CancellationToken.IsCancellationRequested)
            {
                Total = ServiceInfoMeasure.Measures.Count;
                DataViewVoiceConnectionMetrics =
                    new ObservableCollection<VoiceConnectionMetric>(response); // an actual data for the current page
            }
        }
    }

    public async Task OnReadData(DataGridReadDataEventArgs<HttpTransmittingMetric> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            List<HttpTransmittingMetric> response = new();

            foreach (var measure in ServiceInfoMeasure.Measures)
            {
                var group = measure.MeasureGroup.First();
                if (group.HttpTransmittingMetric != null)
                    response.Add(group.HttpTransmittingMetric);
            }

            // this can be call to anything, in this case we're calling a fictional api
            //var response = await Http.GetJsonAsync<Employee[]>( $"some-api/employees?page={e.Page}&pageSize={e.PageSize}" );
            if (e.ReadDataMode is DataGridReadDataMode.Virtualize)
                response = response.Skip(e.VirtualizeOffset).Take(e.VirtualizeCount).ToList();
            else if (e.ReadDataMode is DataGridReadDataMode.Paging)
                response = response.Skip((e.Page - 1) * e.PageSize).Take(e.PageSize).ToList();
            else
                throw new Exception("Unhandled ReadDataMode");

            if (!e.CancellationToken.IsCancellationRequested)
            {
                Total = ServiceInfoMeasure.Measures.Count;
                DataViewHttpTransmittingMetrics =
                    new ObservableCollection<HttpTransmittingMetric>(response); // an actual data for the current page
            }
        }
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