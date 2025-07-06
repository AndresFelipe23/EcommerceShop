using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class Log
{
    [Key]
    public int LogId { get; set; }

    public int? UsuId { get; set; }

    [StringLength(50)]
    public string Tipo { get; set; } = null!;

    [StringLength(1000)]
    public string Mensaje { get; set; } = null!;

    [StringLength(100)]
    public string? Origen { get; set; }

    [StringLength(100)]
    public string? Metodo { get; set; }

    public string? DatosEntrada { get; set; }

    public string? DatosSalida { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Fecha { get; set; }

    [Column("IP")]
    [StringLength(45)]
    public string? Ip { get; set; }

    [StringLength(255)]
    public string? Navegador { get; set; }

    [ForeignKey("UsuId")]
    [InverseProperty("Logs")]
    public virtual Usuario? Usu { get; set; }
}
