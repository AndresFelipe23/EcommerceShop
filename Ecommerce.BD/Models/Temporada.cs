using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Temporada
{
    [Key]
    public int TemporadaId { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaInicio { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaFin { get; set; }

    public bool? Activo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }
}
