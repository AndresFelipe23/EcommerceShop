using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Pedido
{
    [Key]
    public int PedidoId { get; set; }

    public int UsuId { get; set; }

    public int DireccionEnvioId { get; set; }

    [StringLength(50)]
    public string? Estado { get; set; }

    [StringLength(50)]
    public string? MetodoPago { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Total { get; set; }

    [StringLength(500)]
    public string? Observaciones { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaPedido { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaActualizacion { get; set; }

    [ForeignKey("DireccionEnvioId")]
    [InverseProperty("Pedidos")]
    public virtual Direccione DireccionEnvio { get; set; } = null!;

    [InverseProperty("Pedido")]
    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();

    [ForeignKey("UsuId")]
    [InverseProperty("Pedidos")]
    public virtual Usuario Usu { get; set; } = null!;
}
