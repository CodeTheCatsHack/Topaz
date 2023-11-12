using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Table("VoiceConnectionMetric")]
[Index("MeasureGroupId", Name = "fk_VoiceConnectionMetric_MeasureGroup1_idx", IsUnique = true)]
public partial class VoiceConnectionMetric
{
    [Key]
    [Column("idVoiceConnectionMetric")]
    public int IdVoiceConnectionMetric { get; set; }

    public int? MeasureGroupId { get; set; }

    /// <summary>
    /// Доля неуспешных попыток установления голосового соединения 
    /// </summary>
    public float VoiceServiceNonAcessibility { get; set; }

    /// <summary>
    /// Доля обрывов голосовых соединений 
    /// </summary>
    public float VoiceServiceCutOfffRatio { get; set; }

    /// <summary>
    /// Средняя разборчивость речи на соединение
    /// </summary>
    public float SpeechQualityCallBasis { get; set; }

    /// <summary>
    /// Доля голосовых соединений с низкой разборчивостью речи
    /// </summary>
    [Column("NegativeMOSsamplesRatio")]
    public float NegativeMossamplesRatio { get; set; }

    [ForeignKey("MeasureGroupId")]
    [InverseProperty("VoiceConnectionMetric")]
    public virtual MeasureGroup? MeasureGroup { get; set; }
}