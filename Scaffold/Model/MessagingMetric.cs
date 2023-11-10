using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Index("MeasureId", Name = "fk_MessagingMetric_Measure1_idx", IsUnique = true)]
public partial class MessagingMetric
{
    /// <summary>
    ///     Идентификатор качества услуг подвижной радиотелефонной связи в части передачи коротких текстовых сообщений
    /// </summary>
    [Key]
    public int id { get; set; }

    /// <summary>
    ///     Внешний идентификатор измерения
    /// </summary>
    public int MeasureId { get; set; }

    /// <summary>
    ///     Доля недоставленных SMS сообщений [%]
    /// </summary>
    public float UndeliveredMessagePercentage { get; set; }

    /// <summary>
    ///     Среднее время доставки SMS сообщений [сек]
    /// </summary>
    public float AverageMessageDeliveryTime { get; set; }

    [ForeignKey("MeasureId")]
    [InverseProperty("MessagingMetric")]
    public virtual Measure Measure { get; set; } = null!;
}