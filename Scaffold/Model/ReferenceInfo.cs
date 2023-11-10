using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

[Index("MeasureId", Name = "fk_ReferenceInfo_Measure1_idx", IsUnique = true)]
public partial class ReferenceInfo
{
    /// <summary>
    ///     Идентификатор справочной информации
    /// </summary>
    [Key]
    public int id { get; set; }

    public int MeasureId { get; set; }

    /// <summary>
    ///     Общее количество тестовых голосовых соединений
    /// </summary>
    public int TotalTestVoiceConnections { get; set; }

    /// <summary>
    ///     Общее количество голосовых последовательностей в оцениваемых соединениях (POLQA)
    /// </summary>
    public int TotalVoiceSequences { get; set; }

    /// <summary>
    ///     Количество голосовых соединений с низкой разборчивостью (Negative MOS samples Count, MOS POLQA&lt;2,6)[%]
    /// </summary>
    public int NegativeMOSsamplesCount { get; set; }

    /// <summary>
    ///     Общее количество отправленных SMS - сообщений
    /// </summary>
    public int TotalMessagesSent { get; set; }

    /// <summary>
    ///     Общее количество попыток соединений с сервером передачи данных HTTP (Загрузка файлов)
    /// </summary>
    public int TotalConnectionAttempts { get; set; }

    /// <summary>
    ///     Общее количество тестовых сессий по протоколу HTTP (Web-browsing)
    /// </summary>
    public int TotalTestSessions { get; set; }

    [ForeignKey("MeasureId")]
    [InverseProperty("ReferenceInfo")]
    public virtual Measure Measure { get; set; } = null!;
}