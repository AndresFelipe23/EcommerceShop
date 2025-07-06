using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Colore
{
    [Key]
    public int ColorId { get; set; }

    [StringLength(50)]
    public string ColNombre { get; set; } = null!;

    [StringLength(10)]
    public string? ColCodigoHex { get; set; }

    [InverseProperty("Color")]
    public virtual ICollection<ProductoTallaColor> ProductoTallaColors { get; set; } = new List<ProductoTallaColor>();
}
