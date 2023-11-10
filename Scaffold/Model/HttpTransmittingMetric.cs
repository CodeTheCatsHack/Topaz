using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Index("MeasureGroupId", Name = "fk_HttpTransmittingMetric_MeasureGroup1_idx", IsUnique = true)]
public partial class HttpTransmittingMetric
{
    [Key]
    public int MeasureGroupId { get; set; }

    /// <summary>
    /// Доля неуспешных сессий по протоколу HTTP (HTTP Session Failure Ratio) [%]
    /// </summary>
    public float SessionFailureRatio { get; set; }

    /// <summary>
    /// Среднее значение скорости передачи данных от абонента (HTTP UL Mean User Data Rate) [kbit/sec]
    /// </summary>
    public float ULMeanUserDataRate { get; set; }

    /// <summary>
    /// Среднее значение скорости передачи данных к абоненту (HTTP DL Mean User Data Rate) [kbit/sec]
    /// </summary>
    public float DLMeanUserDataRate { get; set; }

    /// <summary>
    /// Продолжительность успешной сессии (HTTP Session Time) [s]
    /// </summary>
    public float SessionTime { get; set; }

    [ForeignKey("MeasureGroupId")]
    [InverseProperty("HttpTransmittingMetric")]
    public virtual MeasureGroup MeasureGroup { get; set; } = null!;
}
