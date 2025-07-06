using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface ITallaRepository
    {
        Task<IEnumerable<Talla>> ListarTallasAsync();
        Task<Talla?> ObtenerPorIdAsync(int id);
        Task<int> InsertarTallaAsync(Talla talla);
        Task<bool> ActualizarTallaAsync(Talla talla);
        Task<bool> EliminarTallaAsync(int id);
        Task<IEnumerable<Talla>> ListarTallasPorGeneroAsync(string genero);
    }
} 