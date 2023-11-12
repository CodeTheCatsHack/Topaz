using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffold.Model;

public partial class Measure : IModelContext
{
    [InverseProperty("Measure")]
    public virtual ICollection<MeasureGroup> MeasureGroups { get; set; } = new List<MeasureGroup>();

    [NotMapped] public Guid? FileGuid { get; set; }
}