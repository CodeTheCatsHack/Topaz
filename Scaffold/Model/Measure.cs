using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Scaffold.Model;

public partial class Measure
{
    /// <summary>
    /// Идентификатор измерения
    /// </summary>
    [Key]
    public int id { get; set; }

    /// <summary>
    /// Дата начала контроля
    /// </summary>
    public DateOnly StartMeasure { get; set; }

    /// <summary>
    /// Дата конца контроля
    /// </summary>
    public DateOnly EndMeasure { get; set; }

    /// <summary>
    /// Место проведения контроля
    /// </summary>
    [StringLength(100)]
    public string Place { get; set; } = null!;

    /// <summary>
    /// Условия проведения контроля
    /// </summary>
    [StringLength(255)]
    public string Conditions { get; set; } = null!;

    /// <summary>
    /// Измерительное оборудование
    /// </summary>
    [StringLength(255)]
    public string Equipment { get; set; } = null!;

    [InverseProperty("Measure")]
    public virtual ICollection<MeasureGroup> MeasureGroup { get; } = new List<MeasureGroup>();

    [InverseProperty("Measure")]
    public virtual MeasureInfo? MeasureInfo { get; set; }
}
