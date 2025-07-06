using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Table("Carrito")]
public partial class Carrito
{
    [Key]
    public int CarritoId { get; set; }

    public int UsuId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [InverseProperty("Carrito")]
    public virtual ICollection<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();

    [ForeignKey("UsuId")]
    [InverseProperty("Carritos")]
    public virtual Usuario Usu { get; set; } = null!;
}
