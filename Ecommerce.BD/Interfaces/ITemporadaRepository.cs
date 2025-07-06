using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface ITemporadaRepository
    {
        Task<IEnumerable<Temporada>> ListarTemporadasAsync();
        Task<Temporada?> ObtenerPorIdAsync(int id);
        Task<int> InsertarTemporadaAsync(Temporada temporada);
        Task<bool> ActualizarTemporadaAsync(Temporada temporada);
        Task<bool> EliminarTemporadaAsync(int id);
        Task<IEnumerable<Temporada>> ListarTemporadasActivasAsync();
        Task<IEnumerable<Temporada>> ListarTemporadasPorFechaAsync(DateTime fecha);
    }
} 