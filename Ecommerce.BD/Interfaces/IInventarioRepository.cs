using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IInventarioRepository
    {
        Task<IEnumerable<Inventario>> ListarAsync();
        Task<Inventario?> ObtenerPorIdAsync(int inventarioId);
        Task<IEnumerable<Inventario>> ListarPorProductoAsync(int proId);
        Task<IEnumerable<Inventario>> ListarPorProductoTallaColorAsync(int productoTallaColorId);
        Task<int> CrearAsync(Inventario inventario);
        Task<bool> ActualizarAsync(Inventario inventario);
        Task<bool> EliminarAsync(int inventarioId);
        Task<bool> ExisteAsync(int inventarioId);
        Task<decimal> ObtenerStockActualAsync(int productoTallaColorId);
        Task<bool> RegistrarMovimientoAsync(int productoTallaColorId, string tipoMovimiento, int cantidad, string descripcion, int? usuId = null);
        Task<IEnumerable<Inventario>> ListarPorTipoMovimientoAsync(string tipoMovimiento);
        Task<IEnumerable<Inventario>> ListarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<Inventario>> ListarPorUsuarioAsync(int usuId);
    }
} 