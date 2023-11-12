using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffold.Model;

public partial class MeasureGroup : IModelContext
{
    [NotMapped] public Guid GuidCollection { get; set; } = Guid.NewGuid();

    [NotMapped]
    public Measure IMeasure
    {
        get => Measure;
        set => Measure = value;
    }
}