using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffold.Model;

public class Measure
{
    /// <summary>
    ///     Идентификатор измерения
    /// </summary>
    [Key]
    public int id { get; set; }

    /// <summary>
    ///     Дата начала контроля
    /// </summary>
    public DateOnly StartMeasure { get; set; }

    /// <summary>
    ///     Дата конца контроля
    /// </summary>
    public DateOnly EndMeasure { get; set; }

    /// <summary>
    ///     Место проведения контроля
    /// </summary>
    [StringLength(100)]
    public string Place { get; set; } = null!;

    /// <summary>
    ///     Условия проведения контроля
    /// </summary>
    [StringLength(255)]
    public string Conditions { get; set; } = null!;

    /// <summary>
    ///     Измерительное оборудование
    /// </summary>
    [StringLength(255)]
    public string Equipment { get; set; } = null!;

    [InverseProperty("Measure")] public virtual HttpTransmittingMetric? HttpTransmittingMetric { get; set; }

    [InverseProperty("Measure")] public virtual MeasureInfo? MeasureInfo { get; set; }

    [InverseProperty("Measure")] public virtual MessagingMetric? MessagingMetric { get; set; }

    [InverseProperty("Measure")] public virtual ReferenceInfo? ReferenceInfo { get; set; }

    [InverseProperty("Measure")] public virtual VoiceConnectionMetric? VoiceConnectionMetric { get; set; }
}