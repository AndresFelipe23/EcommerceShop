using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface IProductoImagenService
    {
        Task<IEnumerable<ProductoImagene>> ListarImagenesAsync();
        Task<ProductoImagene?> ObtenerImagenAsync(int imgId);
        Task<IEnumerable<ProductoImagene>> ListarPorProductoAsync(int proId);
        Task<int> CrearImagenAsync(ProductoImagene productoImagen);
        Task<bool> ActualizarImagenAsync(ProductoImagene productoImagen);
        Task<bool> EliminarImagenAsync(int imgId);
        Task<bool> EliminarImagenesPorProductoAsync(int proId);
        Task<int> ObtenerCantidadImagenesAsync(int proId);
        Task<ProductoImagene?> ObtenerImagenPrincipalAsync(int proId);
        Task<bool> ActualizarOrdenImagenAsync(int imgId, int nuevoOrden);
        Task<IEnumerable<ProductoImagene>> ListarPorOrdenAsync(int proId);
        Task<bool> ValidarImagenAsync(ProductoImagene productoImagen);
        Task<bool> EstablecerImagenPrincipalAsync(int imgId, int proId);
        Task<bool> ReordenarImagenesAsync(int proId, List<int> ordenImagenes);
    }
} 