using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Table("CarritoItem")]
public partial class CarritoItem
{
    [Key]
    public int CarritoItemId { get; set; }

    public int CarritoId { get; set; }

    public int ProductoTallaColorId { get; set; }

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal PrecioUnitario { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaAgregado { get; set; }

    [ForeignKey("CarritoId")]
    [InverseProperty("CarritoItems")]
    public virtual Carrito Carrito { get; set; } = null!;

    [ForeignKey("ProductoTallaColorId")]
    [InverseProperty("CarritoItems")]
    public virtual ProductoTallaColor ProductoTallaColor { get; set; } = null!;
}
