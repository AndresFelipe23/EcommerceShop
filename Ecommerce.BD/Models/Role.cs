using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Index("RolNombre", Name = "UQ__Roles__65F09DC1F72C07DC", IsUnique = true)]
public partial class Role
{
    [Key]
    public int RolId { get; set; }

    [StringLength(50)]
    public string RolNombre { get; set; } = null!;

    [InverseProperty("Rol")]
    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
}
