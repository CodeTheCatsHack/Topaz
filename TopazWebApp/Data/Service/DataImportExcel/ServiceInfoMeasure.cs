using System.Collections.ObjectModel;
using Blazorise;
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

    public async Task GetDataMeasures(List<IFileEntry> filesToUpload)
    {
        foreach (var file in filesToUpload)
        {
            var guidFileMeasure = Guid.NewGuid();
            var fileName = Path.Combine("file", $"{guidFileMeasure}{Path.GetExtension(file.Name)}");
            var filePath = Path.Combine(HostingEnvironment.WebRootPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.OpenReadStream().CopyToAsync(stream);

            Interpreter interpreter = new(filePath);
            var localMeasure = interpreter.ParseMeasure();

            if (localMeasure == null)
                continue;

            localMeasure.FileGuid = guidFileMeasure;

            if (Measures.All(x => x.FileGuid != guidFileMeasure))
            {
                Measures.Add(localMeasure);
            }
        }
    }
}