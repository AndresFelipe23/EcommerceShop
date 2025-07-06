using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IProductoImagenRepository
    {
        Task<IEnumerable<ProductoImagene>> ListarAsync();
        Task<ProductoImagene?> ObtenerPorIdAsync(int imgId);
        Task<IEnumerable<ProductoImagene>> ListarPorProductoAsync(int proId);
        Task<int> CrearAsync(ProductoImagene productoImagen);
        Task<bool> ActualizarAsync(ProductoImagene productoImagen);
        Task<bool> EliminarAsync(int imgId);
        Task<bool> ExisteAsync(int imgId);
        Task<bool> EliminarPorProductoAsync(int proId);
        Task<int> ObtenerCantidadImagenesAsync(int proId);
        Task<ProductoImagene?> ObtenerImagenPrincipalAsync(int proId);
        Task<bool> ActualizarOrdenAsync(int imgId, int nuevoOrden);
        Task<IEnumerable<ProductoImagene>> ListarPorOrdenAsync(int proId);
    }
} 