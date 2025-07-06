using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Promocione
{
    [Key]
    public int PromocionId { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [StringLength(20)]
    public string? TipoDescuento { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? ValorDescuento { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaInicio { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaFin { get; set; }

    public bool? Activo { get; set; }

    [InverseProperty("Promocion")]
    public virtual ICollection<PromocionCategorium> PromocionCategoria { get; set; } = new List<PromocionCategorium>();

    [InverseProperty("Promocion")]
    public virtual ICollection<PromocionProducto> PromocionProductos { get; set; } = new List<PromocionProducto>();
}
