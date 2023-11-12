using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffold.Model;

public partial class MeasureInfo : IModelContext
{
    [NotMapped]
    public Measure IMeasure
    {
        get => Measure;
        set => Measure = value;
    }
}