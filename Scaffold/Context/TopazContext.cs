using Microsoft.EntityFrameworkCore;
using Scaffold.Model;

namespace Scaffold.Context;

public partial class TopazContext
{
    public virtual DbSet<HttpTransmittingMetric> HttpTransmittingMetric { get; set; }

    public virtual DbSet<Measure> Measure { get; set; }

    public virtual DbSet<MeasureGroup> MeasureGroup { get; set; }

    public virtual DbSet<MeasureInfo> MeasureInfo { get; set; }

    public virtual DbSet<MessagingMetric> MessagingMetric { get; set; }

    public virtual DbSet<ReferenceInfoMetric> ReferenceInfoMetric { get; set; }

    public virtual DbSet<VoiceConnectionMetric> VoiceConnectionMetric { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<HttpTransmittingMetric>(entity =>
        {
            entity.HasKey(e => e.MeasureGroupId).HasName("PRIMARY");

            entity.Property(e => e.MeasureGroupId).ValueGeneratedNever();
            entity.Property(e => e.DLMeanUserDataRate).HasComment(
                "Среднее значение скорости передачи данных к абоненту (HTTP DL Mean User Data Rate) [kbit/sec]");
            entity.Property(e => e.SessionFailureRatio)
                .HasComment("Доля неуспешных сессий по протоколу HTTP (HTTP Session Failure Ratio) [%]");
            entity.Property(e => e.SessionTime).HasComment("Продолжительность успешной сессии (HTTP Session Time) [s]");
            entity.Property(e => e.ULMeanUserDataRate).HasComment(
                "Среднее значение скорости передачи данных от абонента (HTTP UL Mean User Data Rate) [kbit/sec]");

            entity.HasOne(d => d.MeasureGroup).WithOne(p => p.HttpTransmittingMetric)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_HttpTransmittingMetric_MeasureGroup1");
        });

        modelBuilder.Entity<Measure>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.id)
                .ValueGeneratedNever()
                .HasComment("Идентификатор измерения");
            entity.Property(e => e.Conditions).HasComment("Условия проведения контроля");
            entity.Property(e => e.EndMeasure).HasComment("Дата конца контроля");
            entity.Property(e => e.Equipment).HasComment("Измерительное оборудование");
            entity.Property(e => e.Place).HasComment("Место проведения контроля");
            entity.Property(e => e.StartMeasure).HasComment("Дата начала контроля");
        });

        modelBuilder.Entity<MeasureGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Measure).WithMany(p => p.MeasureGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MeasureGroup_Measure1");
        });

        modelBuilder.Entity<MeasureInfo>(entity =>
        {
            entity.HasKey(e => e.MeasureId).HasName("PRIMARY");

            entity.Property(e => e.MeasureId).ValueGeneratedNever();

            entity.HasOne(d => d.Measure).WithOne(p => p.MeasureInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeasureInfo_Measure");
        });

        modelBuilder.Entity<MessagingMetric>(entity =>
        {
            entity.HasKey(e => e.MeasureGroupId).HasName("PRIMARY");

            entity.Property(e => e.MeasureGroupId).ValueGeneratedNever();
            entity.Property(e => e.AverageMessageDeliveryTime).HasComment("Среднее время доставки SMS сообщений [сек]");
            entity.Property(e => e.UndeliveredMessagePercentage).HasComment("Доля недоставленных SMS сообщений [%]");

            entity.HasOne(d => d.MeasureGroup).WithOne(p => p.MessagingMetric)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MessagingMetric_MeasureGroup1");
        });

        modelBuilder.Entity<ReferenceInfoMetric>(entity =>
        {
            entity.HasKey(e => e.MeasureGroupId).HasName("PRIMARY");

            entity.Property(e => e.MeasureGroupId).ValueGeneratedNever();
            entity.Property(e => e.NegativeMOSsamplesCount).HasComment(
                "Количество голосовых соединений с низкой разборчивостью (Negative MOS samples Count, MOS POLQA<2,6)[%]");
            entity.Property(e => e.TotalConnectionAttempts)
                .HasComment("Общее количество попыток соединений с сервером передачи данных HTTP (Загрузка файлов)");
            entity.Property(e => e.TotalMessagesSent).HasComment("Общее количество отправленных SMS - сообщений");
            entity.Property(e => e.TotalTestSessions)
                .HasComment("Общее количество тестовых сессий по протоколу HTTP (Web-browsing)");
            entity.Property(e => e.TotalTestVoiceConnections)
                .HasComment("Общее количество тестовых голосовых соединений ");
            entity.Property(e => e.TotalVoiceSequences)
                .HasComment("Общее количество голосовых последовательностей в оцениваемых соединениях (POLQA) ");

            entity.HasOne(d => d.MeasureGroup).WithOne(p => p.ReferenceInfoMetric)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ReferenceInfoMetric_MeasureGroup1");
        });

        modelBuilder.Entity<VoiceConnectionMetric>(entity =>
        {
            entity.HasKey(e => e.MeasureGroupId).HasName("PRIMARY");

            entity.Property(e => e.MeasureGroupId).ValueGeneratedNever();
            entity.Property(e => e.NegativeMOSsamplesRatio)
                .HasComment("Доля голосовых соединений с низкой разборчивостью речи");
            entity.Property(e => e.SpeechQualityCallBasis).HasComment("Средняя разборчивость речи на соединение");
            entity.Property(e => e.VoiceServiceCutOfffRatio).HasComment("Доля обрывов голосовых соединений ");
            entity.Property(e => e.VoiceServiceNonAcessibility)
                .HasComment("Доля неуспешных попыток установления голосового соединения ");

            entity.HasOne(d => d.MeasureGroup).WithOne(p => p.VoiceConnectionMetric)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_VoiceConnectionMetric_MeasureGroup1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}