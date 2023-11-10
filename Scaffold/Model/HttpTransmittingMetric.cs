using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Index("MeasureId", Name = "fk_HttpTransmittingMetric_Measure1_idx", IsUnique = true)]
public class HttpTransmittingMetric
{
    /// <summary>
    ///     Показатели качества услуг связи по передаче данных, за исключением услуг связи по передаче данных для целей
    ///     передачи голосовой информации
    /// </summary>
    [Key]
    public int id { get; set; }

    /// <summary>
    ///     Внешний идентификатор измерения
    /// </summary>
    public int MeasureId { get; set; }

    /// <summary>
    ///     Доля неуспешных сессий по протоколу HTTP (HTTP Session Failure Ratio) [%]
    /// </summary>
    public float SessionFailureRatio { get; set; }

    /// <summary>
    ///     Среднее значение скорости передачи данных от абонента (HTTP UL Mean User Data Rate) [kbit/sec]
    /// </summary>
    public float ULMeanUserDataRate { get; set; }

    /// <summary>
    ///     Среднее значение скорости передачи данных к абоненту (HTTP DL Mean User Data Rate) [kbit/sec]
    /// </summary>
    public float DLMeanUserDataRate { get; set; }

    /// <summary>
    ///     Продолжительность успешной сессии (HTTP Session Time) [s]
    /// </summary>
    public float SessionTime { get; set; }

    [ForeignKey("MeasureId")]
    [InverseProperty("HttpTransmittingMetric")]
    public virtual Measure Measure { get; set; } = null!;
}