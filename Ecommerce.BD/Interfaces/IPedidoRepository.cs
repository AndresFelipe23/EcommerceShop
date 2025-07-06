using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> ListarAsync();
        Task<Pedido?> ObtenerPorIdAsync(int pedidoId);
        Task<IEnumerable<Pedido>> ListarPorUsuarioAsync(int usuId);
        Task<IEnumerable<Pedido>> ListarPorEstadoAsync(string estado);
        Task<IEnumerable<Pedido>> ListarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<int> CrearAsync(Pedido pedido);
        Task<bool> ActualizarAsync(Pedido pedido);
        Task<bool> EliminarAsync(int pedidoId);
        Task<bool> ExisteAsync(int pedidoId);
        Task<bool> ActualizarEstadoAsync(int pedidoId, string estado);
        Task<decimal> CalcularTotalAsync(int pedidoId);
        Task<IEnumerable<Pedido>> ListarPorMetodoPagoAsync(string metodoPago);
        Task<object> ObtenerEstadisticasAsync();
        Task<IEnumerable<Pedido>> ListarPedidosRecientesAsync(int cantidad = 10);
        Task<bool> VerificarPedidoUsuarioAsync(int pedidoId, int usuId);
    }
} 