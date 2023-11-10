using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Index("MeasureId", Name = "fk_MeasureInfo_Measure1_idx")]
public class MeasureInfo
{
    [Key] public int MeasureId { get; set; }

    [StringLength(100)] public string CompanyName { get; set; } = null!;

    [StringLength(100)] public string CompanyType { get; set; } = null!;

    [StringLength(20)] public string CompanyAbbr { get; set; } = null!;

    [StringLength(255)] public string CompanyFullname { get; set; } = null!;

    [StringLength(255)] public string Protocol { get; set; } = null!;

    [ForeignKey("MeasureId")]
    [InverseProperty("MeasureInfo")]
    public virtual Measure Measure { get; set; } = null!;
}