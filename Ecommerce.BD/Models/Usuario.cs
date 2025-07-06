using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

[Index("UsuEmail", Name = "UQ__Usuarios__0FE50E26ADAFCEB2", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int UsuId { get; set; }

    [StringLength(100)]
    public string UsuNombre { get; set; } = null!;

    [StringLength(100)]
    public string UsuApellido { get; set; } = null!;

    [StringLength(100)]
    public string UsuEmail { get; set; } = null!;

    [StringLength(255)]
    public string UsuPasswordHash { get; set; } = null!;

    [StringLength(20)]
    public string? UsuTelefono { get; set; }

    [StringLength(20)]
    public string? UsuGenero { get; set; }

    public DateOnly? UsuFechaNacimiento { get; set; }

    [StringLength(255)]
    public string? UsuImagenPerfil { get; set; }

    [StringLength(50)]
    public string? UsuEstadoCuenta { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UsuFechaRegistro { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UsuFechaUltimoLogin { get; set; }

    [StringLength(100)]
    public string? UsuProveedorAutenticacion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UsuFechaActualizacion { get; set; }

    [InverseProperty("Usu")]
    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    [InverseProperty("Usu")]
    public virtual ICollection<CuponesUsuario> CuponesUsuarios { get; set; } = new List<CuponesUsuario>();

    [InverseProperty("Usu")]
    public virtual ICollection<Direccione> Direcciones { get; set; } = new List<Direccione>();

    [InverseProperty("Usu")]
    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    [InverseProperty("Usu")]
    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    [InverseProperty("Usu")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("Usu")]
    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
}
