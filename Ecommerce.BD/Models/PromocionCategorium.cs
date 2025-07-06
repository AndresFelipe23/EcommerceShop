using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class PromocionCategorium
{
    [Key]
    public int PromocionCategoriaId { get; set; }

    public int? PromocionId { get; set; }

    public int? CatId { get; set; }

    [ForeignKey("CatId")]
    [InverseProperty("PromocionCategoria")]
    public virtual Categoria? Cat { get; set; }

    [ForeignKey("PromocionId")]
    [InverseProperty("PromocionCategoria")]
    public virtual Promocione? Promocion { get; set; }
}
