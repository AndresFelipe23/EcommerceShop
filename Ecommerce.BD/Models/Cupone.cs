using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Index("Codigo", Name = "UQ__Cupones__06370DACECA246F6", IsUnique = true)]
public partial class Cupone
{
    [Key]
    public int CuponId { get; set; }

    [StringLength(50)]
    public string Codigo { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [StringLength(20)]
    public string? TipoDescuento { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? ValorDescuento { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? MontoMinimo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaInicio { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaFin { get; set; }

    public int? LimiteUso { get; set; }

    public int? UsosRealizados { get; set; }

    public bool? Activo { get; set; }

    [InverseProperty("Cupon")]
    public virtual ICollection<CuponesUsuario> CuponesUsuarios { get; set; } = new List<CuponesUsuario>();
}
