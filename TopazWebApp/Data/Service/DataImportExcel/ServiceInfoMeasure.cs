using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Blazorise;
using Microsoft.Extensions.FileProviders;
using Parser;
using Scaffold.Model;

namespace Topaz.Data.Service;

public class ServiceInfoMeasure
{
    public ServiceInfoMeasure(IWebHostEnvironment webHostEnvironment)
    {
        HostingEnvironment = webHostEnvironment;
    }

    private IWebHostEnvironment HostingEnvironment { get; }

    public static ConcurrentDictionary<Guid, Measure?> FilesMeasures { get; set; } = new();
    public static ObservableCollection<Measure> Measures { get; set; } = new();

    public async Task<List<Measure>?> GetDataMeasures(List<IFileEntry> filesToUpload)
    {
        foreach (var file in filesToUpload)
        {
            var guidFileMeasure = Guid.NewGuid();
            var fileName = Path.Combine("file", $"{guidFileMeasure}{Path.GetExtension(file.Name)}");
            var filePath = Path.Combine(HostingEnvironment.WebRootPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.OpenReadStream().CopyToAsync(stream);
        }

        var fileProvider = new PhysicalFileProvider(HostingEnvironment.WebRootPath);
        var files = fileProvider.GetDirectoryContents("file");
        List<Measure> localMeasures = new();
        Measures.Clear();
        FilesMeasures.Clear();

        foreach (var file in files)
        {
            Interpreter interpreter = new(file.PhysicalPath!);
            var localMeasure = interpreter.ParseMeasure();
            if (localMeasure != null)
            {
                localMeasures.Add(localMeasure);
                Measures.Add(localMeasure);
                if (Guid.TryParse(file.Name[..file.Name.IndexOf('.')], out var guidFile))
                    FilesMeasures.TryAdd(guidFile, localMeasure);
            }
        }

        return localMeasures.Count == 0 ? null : localMeasures;
    }
}