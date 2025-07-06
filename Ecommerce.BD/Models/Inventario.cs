using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Table("Inventario")]
public partial class Inventario
{
    [Key]
    public int InventarioId { get; set; }

    public int ProductoTallaColorId { get; set; }

    [StringLength(50)]
    public string TipoMovimiento { get; set; } = null!;

    public int Cantidad { get; set; }

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaMovimiento { get; set; }

    public int? UsuId { get; set; }

    [ForeignKey("ProductoTallaColorId")]
    [InverseProperty("Inventarios")]
    public virtual ProductoTallaColor ProductoTallaColor { get; set; } = null!;

    [ForeignKey("UsuId")]
    [InverseProperty("Inventarios")]
    public virtual Usuario? Usu { get; set; }
}
