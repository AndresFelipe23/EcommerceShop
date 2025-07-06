using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Categoria
{
    [Key]
    public int CatId { get; set; }

    [StringLength(100)]
    public string CatNombre { get; set; } = null!;

    [StringLength(255)]
    public string? CatDescripcion { get; set; }

    public int? CatPadreId { get; set; }

    public int? CatOrden { get; set; }

    public bool? CatActivo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CatFechaCreacion { get; set; }

    [ForeignKey("CatPadreId")]
    [InverseProperty("InverseCatPadre")]
    public virtual Categoria? CatPadre { get; set; }

    [InverseProperty("CatPadre")]
    public virtual ICollection<Categoria> InverseCatPadre { get; set; } = new List<Categoria>();

    [InverseProperty("ProCategoria")]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    [InverseProperty("Cat")]
    public virtual ICollection<PromocionCategorium> PromocionCategoria { get; set; } = new List<PromocionCategorium>();
}
