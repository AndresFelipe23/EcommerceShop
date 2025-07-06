using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Direccione
{
    [Key]
    public int DirId { get; set; }

    public int UsuId { get; set; }

    [StringLength(100)]
    public string? DirTitulo { get; set; }

    [StringLength(100)]
    public string DirNombre { get; set; } = null!;

    [StringLength(20)]
    public string? DirTelefono { get; set; }

    [StringLength(100)]
    public string DirPais { get; set; } = null!;

    [StringLength(100)]
    public string? DirDepartamento { get; set; }

    [StringLength(100)]
    public string DirCiudad { get; set; } = null!;

    [StringLength(20)]
    public string? DirCodigoPostal { get; set; }

    [StringLength(255)]
    public string DirLinea1 { get; set; } = null!;

    [StringLength(255)]
    public string? DirLinea2 { get; set; }

    [StringLength(255)]
    public string? DirReferencia { get; set; }

    public bool? DirEsPrincipal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DirFechaCreacion { get; set; }

    [InverseProperty("DireccionEnvio")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [ForeignKey("UsuId")]
    [InverseProperty("Direcciones")]
    public virtual Usuario Usu { get; set; } = null!;
}
