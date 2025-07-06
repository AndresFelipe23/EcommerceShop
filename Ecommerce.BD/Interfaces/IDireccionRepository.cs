using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IDireccionRepository
    {
        Task<IEnumerable<Direccione>> ListarAsync();
        Task<Direccione?> ObtenerPorIdAsync(int dirId);
        Task<IEnumerable<Direccione>> ListarPorUsuarioAsync(int usuId);
        Task<int> CrearAsync(Direccione direccion);
        Task<bool> ActualizarAsync(Direccione direccion);
        Task<bool> EliminarAsync(int dirId);
        Task<bool> ExisteAsync(int dirId);
        Task<bool> EsPrincipalAsync(int dirId);
        Task<bool> MarcarComoPrincipalAsync(int dirId, int usuId);
    }
} 