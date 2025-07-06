using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface ICarritoItemService
    {
        Task<IEnumerable<CarritoItem>> ListarAsync();
        Task<CarritoItem?> ObtenerPorIdAsync(int carritoItemId);
        Task<IEnumerable<CarritoItem>> ListarPorCarritoAsync(int carritoId);
        Task<int> CrearAsync(CarritoItem carritoItem);
        Task<bool> ActualizarAsync(CarritoItem carritoItem);
        Task<bool> EliminarAsync(int carritoItemId);
        Task<bool> ActualizarCantidadAsync(int carritoItemId, int cantidad);
        Task<int> ObtenerCantidadProductosAsync(int carritoId);
        Task<bool> AgregarProductoAsync(int carritoId, int productoTallaColorId, int cantidad, decimal precioUnitario);
        Task<bool> RemoverProductoAsync(int carritoItemId);
        Task<object> ObtenerDetalleItemAsync(int carritoItemId);
    }
} 