﻿using System.Collections.ObjectModel;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.FileProviders;
using Parser;
using Scaffold.Model;
using Topaz.Data.Service;
using static Topaz.Data.Service.ServiceInfoMeasure;

namespace Topaz.Data;

public class ImportDataComponent : ComponentBase
{
    [Inject] private ServiceInfoMeasure ServiceInfoMeasure { get; set; }
    [Inject] private IWebHostEnvironment HostingEnvironment { get; set; }
    [Inject] private ServiceDataDataBase ServiceDataDataBase { get; set; }

    protected bool UpdateOne { get; set; }
    protected ViewData ViewDataStatus { get; set; } = ViewData.Measure;

    protected int Total { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (!UpdateOne)
        {
            var fileProvider = new PhysicalFileProvider(HostingEnvironment.WebRootPath);
            var files = fileProvider.GetDirectoryContents("file");

            foreach (var file in files)
            {
                Interpreter interpreter = new(file.PhysicalPath!);
                var localMeasure = interpreter.ParseMeasure();

                if (localMeasure == null)
                    continue;

                if (Guid.TryParse(file.Name[..file.Name.IndexOf('.')], out var guidFile))
                    localMeasure.FileGuid = guidFile;

                if (Measures.All(x => x.FileGuid != guidFile)) Measures.Add(localMeasure);
            }

            UpdateOne = true;
        }

        await base.OnInitializedAsync();
    }

    #region Запросы DataGrid

    protected void CallbackRemove(Measure obj)
    {
        var fileName = Path.Combine("file", $"{obj.FileGuid}.xlsx");
        var filePath = Path.Combine(HostingEnvironment.WebRootPath, fileName);
        File.Delete(filePath);
    }

    #endregion

    #region Выбранные строки разных таблиц

    protected List<Measure> SelectedsMeasure { get; set; } = new();
    protected MeasureInfo SelectedMeasureInfo { get; set; } = new();
    protected ReferenceInfoMetric SelectedReferenceInfoMetric { get; set; } = new();
    protected MessagingMetric SelectedMessagingMetric { get; set; }
    protected HttpTransmittingMetric SelectedHttpTransMetric { get; set; }
    protected VoiceConnectionMetric SelectedVoiceConnectionMetric { get; set; }

    #endregion

    #region Работа с данными DataGrd

    protected async Task SaveMultiplyOrCancel(bool save)
    {
        foreach (var measure in SelectedsMeasure)
        {
            var fileName = $"{measure.FileGuid}.xlsx";
            var filePath = Path.Combine(HostingEnvironment.WebRootPath, "file", fileName);

            if (File.Exists(filePath))
                switch (save)
                {
                    case true when await ServiceDataDataBase.DataMeasure(measure):
                    case false:
                        File.Delete(filePath);
                        break;
                }

            Measures.Remove(measure);
        }
    }

    protected async Task SaveOrCancel(bool save)
    {
        foreach (var measure in Measures)
        {
            var fileName = $"{measure.FileGuid}.xlsx";
            var filePath = Path.Combine(HostingEnvironment.WebRootPath, "file", fileName);

            if (File.Exists(filePath))
                switch (save)
                {
                    case true when await ServiceDataDataBase.DataMeasure(measure):
                    case false:
                        File.Delete(filePath);
                        break;
                }
        }

        Measures.Clear();
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

    protected void OnNavImport(ViewData viewData)
    {
        ViewDataStatus = viewData;
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