using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface ICarritoItemRepository
    {
        Task<IEnumerable<CarritoItem>> ListarAsync();
        Task<CarritoItem?> ObtenerPorIdAsync(int carritoItemId);
        Task<IEnumerable<CarritoItem>> ListarPorCarritoAsync(int carritoId);
        Task<int> CrearAsync(CarritoItem carritoItem);
        Task<bool> ActualizarAsync(CarritoItem carritoItem);
        Task<bool> EliminarAsync(int carritoItemId);
        Task<bool> ExisteAsync(int carritoItemId);
        Task<bool> ExisteProductoEnCarritoAsync(int carritoId, int productoTallaColorId);
        Task<CarritoItem?> ObtenerPorProductoAsync(int carritoId, int productoTallaColorId);
        Task<bool> ActualizarCantidadAsync(int carritoItemId, int cantidad);
        Task<int> ObtenerCantidadProductosAsync(int carritoId);
    }
} 