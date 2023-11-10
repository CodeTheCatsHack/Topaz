using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Index("MeasureGroupId", Name = "fk_MessagingMetric_MeasureGroup1_idx", IsUnique = true)]
public partial class MessagingMetric
{
    [Key]
    public int MeasureGroupId { get; set; }

    /// <summary>
    /// Доля недоставленных SMS сообщений [%]
    /// </summary>
    public float UndeliveredMessagePercentage { get; set; }

    /// <summary>
    /// Среднее время доставки SMS сообщений [сек]
    /// </summary>
    public float AverageMessageDeliveryTime { get; set; }

    [ForeignKey("MeasureGroupId")]
    [InverseProperty("MessagingMetric")]
    public virtual MeasureGroup MeasureGroup { get; set; } = null!;
}
