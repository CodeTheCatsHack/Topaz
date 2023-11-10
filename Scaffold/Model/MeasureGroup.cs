using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Index("HttpTransmittingMetricId", Name = "HttpTransmittingMetric_id_UNIQUE", IsUnique = true)]
[Index("MessagingMetricId", Name = "MessagingMetric_id_UNIQUE", IsUnique = true)]
[Index("ReferenceInfoId", Name = "ReferenceInfo_id_UNIQUE", IsUnique = true)]
[Index("VoiceConnectionMetricId", Name = "VoiceConnectionMetric_Id_UNIQUE", IsUnique = true)]
[Index("MeasureId", Name = "fk_MeasureGroup_Measure1_idx")]
public partial class MeasureGroup
{
    [Key]
    public int Id { get; set; }

    public int MeasureId { get; set; }

    public int VoiceConnectionMetricId { get; set; }

    public int MessagingMetricId { get; set; }

    public int HttpTransmittingMetricId { get; set; }

    public int ReferenceInfoId { get; set; }

    [StringLength(50)]
    public string MeasureSubject { get; set; } = null!;

    [InverseProperty("MeasureGroup")]
    public virtual HttpTransmittingMetric? HttpTransmittingMetric { get; set; }

    [ForeignKey("MeasureId")]
    [InverseProperty("MeasureGroup")]
    public virtual Measure Measure { get; set; } = null!;

    [InverseProperty("MeasureGroup")]
    public virtual MessagingMetric? MessagingMetric { get; set; }

    [InverseProperty("MeasureGroup")]
    public virtual ReferenceInfoMetric? ReferenceInfoMetric { get; set; }

    [InverseProperty("MeasureGroup")]
    public virtual VoiceConnectionMetric? VoiceConnectionMetric { get; set; }
}
