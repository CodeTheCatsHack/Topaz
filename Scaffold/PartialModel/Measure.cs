using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffold.Model;

public partial class Measure : IModelContext
{
    [NotMapped] public Guid? FileGuid { get; set; }
}