using System.Collections.ObjectModel;
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

    public static ObservableCollection<Measure> Measures { get; set; } = new();

    public List<Measure>? GetDataMeasures()
    {
        var fileProvider = new PhysicalFileProvider(HostingEnvironment.WebRootPath);
        var files = fileProvider.GetDirectoryContents("file");
        List<Measure> localMeasures = new();
        Measures.Clear();

        foreach (var file in files)
        {
            Interpreter interpreter = new(file.PhysicalPath!);
            var localMeasure = interpreter.ParseMeasure();
            if (localMeasure != null)
            {
                localMeasures.Add(localMeasure);
                Measures.Add(localMeasure);
            }
        }

        return localMeasures.Count == 0 ? null : localMeasures;
    }
}