using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IProductoTallaColorRepository
    {
        Task<IEnumerable<ProductoTallaColor>> ListarAsync();
        Task<ProductoTallaColor?> ObtenerPorIdAsync(int productoTallaColorId);
        Task<IEnumerable<ProductoTallaColor>> ListarPorProductoAsync(int proId);
        Task<IEnumerable<ProductoTallaColor>> ListarPorTallaAsync(int tallaId);
        Task<IEnumerable<ProductoTallaColor>> ListarPorColorAsync(int colorId);
        Task<int> CrearAsync(ProductoTallaColor productoTallaColor);
        Task<bool> ActualizarAsync(ProductoTallaColor productoTallaColor);
        Task<bool> EliminarAsync(int productoTallaColorId);
        Task<bool> ExisteAsync(int productoTallaColorId);
        Task<bool> ExisteCombinacionAsync(int proId, int tallaId, int colorId);
        Task<ProductoTallaColor?> ObtenerCombinacionAsync(int proId, int tallaId, int colorId);
        Task<bool> ActualizarStockAsync(int productoTallaColorId, int nuevoStock);
        Task<bool> ActualizarPrecioOfertaAsync(int productoTallaColorId, decimal? precioOferta);
        Task<IEnumerable<ProductoTallaColor>> ListarConStockAsync();
        Task<IEnumerable<ProductoTallaColor>> ListarSinStockAsync();
        Task<IEnumerable<ProductoTallaColor>> ListarConOfertasAsync();
        Task<object> ObtenerEstadisticasStockAsync();
        Task<bool> VerificarStockDisponibleAsync(int productoTallaColorId, int cantidad);
        Task<decimal> ObtenerPrecioMinimoAsync(int proId);
        Task<decimal> ObtenerPrecioMaximoAsync(int proId);
    }
} 