using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> ListarProductosAsync();
        Task<Producto?> ObtenerProductoAsync(int proId);
        Task<IEnumerable<Producto>> ListarPorCategoriaAsync(int catId);
        Task<IEnumerable<Producto>> ListarPorGeneroAsync(string genero);
        Task<IEnumerable<Producto>> ListarActivosAsync();
        Task<int> CrearProductoAsync(Producto producto);
        Task<bool> ActualizarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int proId);
        Task<IEnumerable<Producto>> BuscarProductosAsync(string nombre);
        Task<IEnumerable<Producto>> ListarPorRangoPrecioAsync(decimal precioMin, decimal precioMax);
        Task<IEnumerable<Producto>> ListarProductosRecientesAsync(int cantidad = 10);
        Task<object> ObtenerEstadisticasAsync();
        Task<bool> ActivarProductoAsync(int proId);
        Task<bool> DesactivarProductoAsync(int proId);
        Task<IEnumerable<Producto>> ListarConOfertasAsync();
        Task<decimal> ObtenerPrecioPromedioAsync();
        Task<bool> ValidarProductoAsync(Producto producto);
        Task<bool> VerificarDisponibilidadAsync(int proId);
    }
} 