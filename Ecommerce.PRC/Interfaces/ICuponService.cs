using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface ICuponService
    {
        Task<IEnumerable<Cupone>> ListarCuponesAsync();
        Task<Cupone?> ObtenerPorIdAsync(int id);
        Task<int> InsertarCuponAsync(Cupone cupon);
        Task<bool> ActualizarCuponAsync(Cupone cupon);
        Task<bool> EliminarCuponAsync(int id);
        Task<Cupone?> ObtenerPorCodigoAsync(string codigo);
        Task<IEnumerable<Cupone>> ListarCuponesActivosAsync();
        Task<IEnumerable<Cupone>> ListarCuponesPorFechaAsync(DateTime fecha);
        Task<IEnumerable<Cupone>> ListarCuponesPorTipoAsync(string tipoDescuento);
        Task<bool> ValidarCuponAsync(string codigo, decimal montoCompra);
        Task<bool> IncrementarUsoCuponAsync(int cuponId);
    }
} 