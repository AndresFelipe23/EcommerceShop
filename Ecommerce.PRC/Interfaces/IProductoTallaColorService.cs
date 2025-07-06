using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface IProductoTallaColorService
    {
        Task<IEnumerable<ProductoTallaColor>> ListarCombinacionesAsync();
        Task<ProductoTallaColor?> ObtenerCombinacionAsync(int productoTallaColorId);
        Task<IEnumerable<ProductoTallaColor>> ListarPorProductoAsync(int proId);
        Task<IEnumerable<ProductoTallaColor>> ListarPorTallaAsync(int tallaId);
        Task<IEnumerable<ProductoTallaColor>> ListarPorColorAsync(int colorId);
        Task<int> CrearCombinacionAsync(ProductoTallaColor productoTallaColor);
        Task<bool> ActualizarCombinacionAsync(ProductoTallaColor productoTallaColor);
        Task<bool> EliminarCombinacionAsync(int productoTallaColorId);
        Task<bool> ExisteCombinacionAsync(int proId, int tallaId, int colorId);
        Task<ProductoTallaColor?> ObtenerCombinacionEspecificaAsync(int proId, int tallaId, int colorId);
        Task<bool> ActualizarStockAsync(int productoTallaColorId, int nuevoStock);
        Task<bool> ActualizarPrecioOfertaAsync(int productoTallaColorId, decimal? precioOferta);
        Task<IEnumerable<ProductoTallaColor>> ListarConStockAsync();
        Task<IEnumerable<ProductoTallaColor>> ListarSinStockAsync();
        Task<IEnumerable<ProductoTallaColor>> ListarConOfertasAsync();
        Task<object> ObtenerEstadisticasStockAsync();
        Task<bool> VerificarStockDisponibleAsync(int productoTallaColorId, int cantidad);
        Task<decimal> ObtenerPrecioMinimoAsync(int proId);
        Task<decimal> ObtenerPrecioMaximoAsync(int proId);
        Task<bool> ValidarCombinacionAsync(ProductoTallaColor productoTallaColor);
        Task<bool> ReducirStockAsync(int productoTallaColorId, int cantidad);
        Task<bool> AumentarStockAsync(int productoTallaColorId, int cantidad);
    }
} 