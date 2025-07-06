using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Producto
{
    [Key]
    public int ProId { get; set; }

    [StringLength(150)]
    public string ProNombre { get; set; } = null!;

    public string? ProDescripcion { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal ProPrecio { get; set; }

    public string? ProImagenPrincipal { get; set; }

    [StringLength(50)]
    public string? ProGenero { get; set; }

    public int ProCategoriaId { get; set; }

    public bool? ProActivo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProFechaCreacion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProFechaActualizacion { get; set; }

    [ForeignKey("ProCategoriaId")]
    [InverseProperty("Productos")]
    public virtual Categoria ProCategoria { get; set; } = null!;

    [InverseProperty("Pro")]
    public virtual ICollection<ProductoImagene> ProductoImagenes { get; set; } = new List<ProductoImagene>();

    [InverseProperty("Pro")]
    public virtual ICollection<ProductoTallaColor> ProductoTallaColors { get; set; } = new List<ProductoTallaColor>();

    [InverseProperty("Pro")]
    public virtual ICollection<PromocionProducto> PromocionProductos { get; set; } = new List<PromocionProducto>();
}
