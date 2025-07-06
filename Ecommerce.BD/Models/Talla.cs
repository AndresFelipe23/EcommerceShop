using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Talla
{
    [Key]
    public int TallaId { get; set; }

    [StringLength(10)]
    public string TalNombre { get; set; } = null!;

    [StringLength(20)]
    public string TalGenero { get; set; } = null!;

    public int? TalOrdenVisualizacion { get; set; }

    [InverseProperty("Talla")]
    public virtual ICollection<ProductoTallaColor> ProductoTallaColors { get; set; } = new List<ProductoTallaColor>();
}
