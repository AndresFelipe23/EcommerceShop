using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Table("PedidoDetalle")]
public partial class PedidoDetalle
{
    [Key]
    public int PedidoDetalleId { get; set; }

    public int PedidoId { get; set; }

    public int ProductoTallaColorId { get; set; }

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal PrecioUnitario { get; set; }

    [Column(TypeName = "decimal(21, 2)")]
    public decimal? Subtotal { get; set; }

    [ForeignKey("PedidoId")]
    [InverseProperty("PedidoDetalles")]
    public virtual Pedido Pedido { get; set; } = null!;

    [ForeignKey("ProductoTallaColorId")]
    [InverseProperty("PedidoDetalles")]
    public virtual ProductoTallaColor ProductoTallaColor { get; set; } = null!;
}
