using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Index("MeasureId", Name = "fk_VoiceConnectionMetric_Measure1_idx", IsUnique = true)]
public partial class VoiceConnectionMetric
{
    /// <summary>
    ///     Идентификатор качества услуг подвижной радиотелефонной связи в части голосового соединения
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    ///     Внешний идентификатор измерения
    /// </summary>
    public int MeasureId { get; set; }

    /// <summary>
    ///     Доля неуспешных попыток установления голосового соединения
    /// </summary>
    public float VoiceServiceNonAcessibility { get; set; }

    /// <summary>
    ///     Доля обрывов голосовых соединений
    /// </summary>
    public float VoiceServiceCutOfffRatio { get; set; }

    /// <summary>
    ///     Средняя разборчивость речи на соединение
    /// </summary>
    public float SpeechQualityCallBasis { get; set; }

    /// <summary>
    ///     Доля голосовых соединений с низкой разборчивостью речи
    /// </summary>
    public float NegativeMOSsamplesRatio { get; set; }

    [ForeignKey("MeasureId")]
    [InverseProperty("VoiceConnectionMetric")]
    public virtual Measure Measure { get; set; } = null!;
}