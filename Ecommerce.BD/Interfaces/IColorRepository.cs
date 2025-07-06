using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IColorRepository
    {
        Task<IEnumerable<Colore>> ListarColoresAsync();
        Task<Colore?> ObtenerPorIdAsync(int id);
        Task<int> InsertarColorAsync(Colore color);
        Task<bool> ActualizarColorAsync(Colore color);
        Task<bool> EliminarColorAsync(int id);
        Task<Colore?> ObtenerPorNombreAsync(string nombre);
        Task<Colore?> ObtenerPorCodigoHexAsync(string codigoHex);
    }
} 