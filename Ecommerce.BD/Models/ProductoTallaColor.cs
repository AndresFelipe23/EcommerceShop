using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Table("ProductoTallaColor")]
public partial class ProductoTallaColor
{
    [Key]
    public int ProductoTallaColorId { get; set; }

    public int ProId { get; set; }

    public int TallaId { get; set; }

    public int ColorId { get; set; }

    public int Stock { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PrecioOferta { get; set; }

    [Column("SKU")]
    [StringLength(50)]
    public string? Sku { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaActualizacion { get; set; }

    [InverseProperty("ProductoTallaColor")]
    public virtual ICollection<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();

    [ForeignKey("ColorId")]
    [InverseProperty("ProductoTallaColors")]
    public virtual Colore Color { get; set; } = null!;

    [InverseProperty("ProductoTallaColor")]
    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    [InverseProperty("ProductoTallaColor")]
    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();

    [ForeignKey("ProId")]
    [InverseProperty("ProductoTallaColors")]
    public virtual Producto Pro { get; set; } = null!;

    [ForeignKey("TallaId")]
    [InverseProperty("ProductoTallaColors")]
    public virtual Talla Talla { get; set; } = null!;
}
