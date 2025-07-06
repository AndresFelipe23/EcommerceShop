using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ListarAsync();
        Task<Producto?> ObtenerPorIdAsync(int proId);
        Task<IEnumerable<Producto>> ListarPorCategoriaAsync(int catId);
        Task<IEnumerable<Producto>> ListarPorGeneroAsync(string genero);
        Task<IEnumerable<Producto>> ListarActivosAsync();
        Task<int> CrearAsync(Producto producto);
        Task<bool> ActualizarAsync(Producto producto);
        Task<bool> EliminarAsync(int proId);
        Task<bool> ExisteAsync(int proId);
        Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre);
        Task<IEnumerable<Producto>> ListarPorRangoPrecioAsync(decimal precioMin, decimal precioMax);
        Task<IEnumerable<Producto>> ListarProductosRecientesAsync(int cantidad = 10);
        Task<object> ObtenerEstadisticasAsync();
        Task<bool> ActualizarEstadoAsync(int proId, bool activo);
        Task<IEnumerable<Producto>> ListarConOfertasAsync();
        Task<decimal> ObtenerPrecioPromedioAsync();
    }
} 