using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IPedidoDetalleRepository
    {
        Task<IEnumerable<PedidoDetalle>> ListarAsync();
        Task<PedidoDetalle?> ObtenerPorIdAsync(int pedidoDetalleId);
        Task<IEnumerable<PedidoDetalle>> ListarPorPedidoAsync(int pedidoId);
        Task<int> CrearAsync(PedidoDetalle pedidoDetalle);
        Task<bool> ActualizarAsync(PedidoDetalle pedidoDetalle);
        Task<bool> EliminarAsync(int pedidoDetalleId);
        Task<bool> ExisteAsync(int pedidoDetalleId);
        Task<bool> EliminarPorPedidoAsync(int pedidoId);
        Task<decimal> CalcularSubtotalAsync(int pedidoId);
        Task<int> ObtenerCantidadProductosAsync(int pedidoId);
        Task<IEnumerable<PedidoDetalle>> ListarPorProductoAsync(int proId);
        Task<bool> VerificarProductoEnPedidoAsync(int pedidoId, int productoTallaColorId);
    }
} 