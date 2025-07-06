using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class UsuarioRole
{
    [Key]
    public int UsuarioRolId { get; set; }

    public int UsuId { get; set; }

    public int RolId { get; set; }

    [ForeignKey("RolId")]
    [InverseProperty("UsuarioRoles")]
    public virtual Role Rol { get; set; } = null!;

    [ForeignKey("UsuId")]
    [InverseProperty("UsuarioRoles")]
    public virtual Usuario Usu { get; set; } = null!;
}
