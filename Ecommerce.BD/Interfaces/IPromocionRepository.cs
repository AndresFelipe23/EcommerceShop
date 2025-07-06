using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IPromocionRepository
    {
        Task<IEnumerable<Promocione>> ListarPromocionesAsync();
        Task<Promocione?> ObtenerPorIdAsync(int id);
        Task<int> InsertarPromocionAsync(Promocione promocion);
        Task<bool> ActualizarPromocionAsync(Promocione promocion);
        Task<bool> EliminarPromocionAsync(int id);
        Task<IEnumerable<Promocione>> ListarPromocionesActivasAsync();
        Task<IEnumerable<Promocione>> ListarPromocionesPorFechaAsync(DateTime fecha);
        Task<IEnumerable<Promocione>> ListarPromocionesPorTipoAsync(string tipoDescuento);
    }
} 