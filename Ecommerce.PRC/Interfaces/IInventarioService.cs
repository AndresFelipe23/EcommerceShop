using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface IInventarioService
    {
        Task<IEnumerable<Inventario>> ListarAsync();
        Task<Inventario?> ObtenerPorIdAsync(int inventarioId);
        Task<IEnumerable<Inventario>> ListarPorProductoAsync(int proId);
        Task<IEnumerable<Inventario>> ListarPorProductoTallaColorAsync(int productoTallaColorId);
        Task<int> CrearAsync(Inventario inventario);
        Task<bool> ActualizarAsync(Inventario inventario);
        Task<bool> EliminarAsync(int inventarioId);
        Task<decimal> ObtenerStockActualAsync(int productoTallaColorId);
        Task<bool> RegistrarEntradaAsync(int productoTallaColorId, int cantidad, string descripcion, int? usuId = null);
        Task<bool> RegistrarSalidaAsync(int productoTallaColorId, int cantidad, string descripcion, int? usuId = null);
        Task<bool> RegistrarVentaAsync(int productoTallaColorId, int cantidad, int usuId);
        Task<bool> RegistrarAjusteAsync(int productoTallaColorId, int cantidad, string descripcion, int? usuId = null);
        Task<IEnumerable<Inventario>> ListarPorTipoMovimientoAsync(string tipoMovimiento);
        Task<IEnumerable<Inventario>> ListarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<Inventario>> ListarPorUsuarioAsync(int usuId);
        Task<object> ObtenerResumenInventarioAsync(int productoTallaColorId);
        Task<bool> VerificarStockDisponibleAsync(int productoTallaColorId, int cantidad);
    }
} 