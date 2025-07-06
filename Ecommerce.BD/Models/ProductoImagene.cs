using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class ProductoImagene
{
    [Key]
    public int ImgId { get; set; }

    public int ProId { get; set; }

    [StringLength(255)]
    public string ImgUrl { get; set; } = null!;

    public int? ImgOrden { get; set; }

    [StringLength(150)]
    public string? ImgAlt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ImgFechaCreacion { get; set; }

    [ForeignKey("ProId")]
    [InverseProperty("ProductoImagenes")]
    public virtual Producto Pro { get; set; } = null!;
}
