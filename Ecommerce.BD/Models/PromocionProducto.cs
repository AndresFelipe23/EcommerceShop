using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Table("PromocionProducto")]
public partial class PromocionProducto
{
    [Key]
    public int PromocionProductoId { get; set; }

    public int? PromocionId { get; set; }

    public int? ProId { get; set; }

    [ForeignKey("ProId")]
    [InverseProperty("PromocionProductos")]
    public virtual Producto? Pro { get; set; }

    [ForeignKey("PromocionId")]
    [InverseProperty("PromocionProductos")]
    public virtual Promocione? Promocion { get; set; }
}
