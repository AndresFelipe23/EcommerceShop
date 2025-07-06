using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class CuponesUsuario
{
    [Key]
    public int CuponUsuarioId { get; set; }

    public int CuponId { get; set; }

    public int UsuId { get; set; }

    public bool? Usado { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaUso { get; set; }

    [ForeignKey("CuponId")]
    [InverseProperty("CuponesUsuarios")]
    public virtual Cupone Cupon { get; set; } = null!;

    [ForeignKey("UsuId")]
    [InverseProperty("CuponesUsuarios")]
    public virtual Usuario Usu { get; set; } = null!;
}
