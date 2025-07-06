using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Services
{
    public class PedidoDetalleService : IPedidoDetalleService
    {
        private readonly IPedidoDetalleRepository _pedidoDetalleRepository;

        public PedidoDetalleService(IPedidoDetalleRepository pedidoDetalleRepository)
        {
            _pedidoDetalleRepository = pedidoDetalleRepository;
        }

        public async Task<IEnumerable<PedidoDetalle>> ListarAsync()
        {
            return await _pedidoDetalleRepository.ListarAsync();
        }

        public async Task<PedidoDetalle?> ObtenerPorIdAsync(int pedidoDetalleId)
        {
            if (pedidoDetalleId <= 0)
                throw new ArgumentException("El ID del detalle del pedido debe ser mayor a 0", nameof(pedidoDetalleId));

            return await _pedidoDetalleRepository.ObtenerPorIdAsync(pedidoDetalleId);
        }

        public async Task<IEnumerable<PedidoDetalle>> ListarPorPedidoAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            return await _pedidoDetalleRepository.ListarPorPedidoAsync(pedidoId);
        }

        public async Task<int> CrearAsync(PedidoDetalle pedidoDetalle)
        {
            if (pedidoDetalle == null)
                throw new ArgumentNullException(nameof(pedidoDetalle));

            if (pedidoDetalle.PedidoId <= 0)
                throw new ArgumentException("El ID del pedido es requerido", nameof(pedidoDetalle.PedidoId));

            if (pedidoDetalle.ProductoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color es requerido", nameof(pedidoDetalle.ProductoTallaColorId));

            if (pedidoDetalle.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(pedidoDetalle.Cantidad));

            if (pedidoDetalle.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a 0", nameof(pedidoDetalle.PrecioUnitario));

            return await _pedidoDetalleRepository.CrearAsync(pedidoDetalle);
        }

        public async Task<bool> ActualizarAsync(PedidoDetalle pedidoDetalle)
        {
            if (pedidoDetalle == null)
                throw new ArgumentNullException(nameof(pedidoDetalle));

            if (pedidoDetalle.PedidoDetalleId <= 0)
                throw new ArgumentException("El ID del detalle del pedido debe ser mayor a 0", nameof(pedidoDetalle.PedidoDetalleId));

            if (pedidoDetalle.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(pedidoDetalle.Cantidad));

            if (pedidoDetalle.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a 0", nameof(pedidoDetalle.PrecioUnitario));

            // Verificar que el detalle existe
            var existe = await _pedidoDetalleRepository.ExisteAsync(pedidoDetalle.PedidoDetalleId);
            if (!existe)
                throw new InvalidOperationException($"El detalle del pedido con ID {pedidoDetalle.PedidoDetalleId} no existe");

            return await _pedidoDetalleRepository.ActualizarAsync(pedidoDetalle);
        }

        public async Task<bool> EliminarAsync(int pedidoDetalleId)
        {
            if (pedidoDetalleId <= 0)
                throw new ArgumentException("El ID del detalle del pedido debe ser mayor a 0", nameof(pedidoDetalleId));

            // Verificar que el detalle existe
            var existe = await _pedidoDetalleRepository.ExisteAsync(pedidoDetalleId);
            if (!existe)
                throw new InvalidOperationException($"El detalle del pedido con ID {pedidoDetalleId} no existe");

            return await _pedidoDetalleRepository.EliminarAsync(pedidoDetalleId);
        }

        public async Task<bool> EliminarPorPedidoAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            return await _pedidoDetalleRepository.EliminarPorPedidoAsync(pedidoId);
        }

        public async Task<decimal> CalcularSubtotalAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            return await _pedidoDetalleRepository.CalcularSubtotalAsync(pedidoId);
        }

        public async Task<int> ObtenerCantidadProductosAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            return await _pedidoDetalleRepository.ObtenerCantidadProductosAsync(pedidoId);
        }

        public async Task<IEnumerable<PedidoDetalle>> ListarPorProductoAsync(int proId)
        {
            if (proId <= 0)
                throw new ArgumentException("El ID del producto debe ser mayor a 0", nameof(proId));

            return await _pedidoDetalleRepository.ListarPorProductoAsync(proId);
        }

        public async Task<bool> VerificarProductoEnPedidoAsync(int pedidoId, int productoTallaColorId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            return await _pedidoDetalleRepository.VerificarProductoEnPedidoAsync(pedidoId, productoTallaColorId);
        }

        public async Task<bool> AgregarProductoAsync(int pedidoId, int productoTallaColorId, int cantidad, decimal precioUnitario)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

            if (precioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a 0", nameof(precioUnitario));

            // Verificar si el producto ya existe en el pedido
            var existe = await _pedidoDetalleRepository.VerificarProductoEnPedidoAsync(pedidoId, productoTallaColorId);
            if (existe)
                throw new InvalidOperationException("El producto ya existe en este pedido");

            var pedidoDetalle = new PedidoDetalle
            {
                PedidoId = pedidoId,
                ProductoTallaColorId = productoTallaColorId,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario
            };

            var resultado = await _pedidoDetalleRepository.CrearAsync(pedidoDetalle);
            return resultado > 0;
        }

        public async Task<bool> ActualizarCantidadAsync(int pedidoDetalleId, int nuevaCantidad)
        {
            if (pedidoDetalleId <= 0)
                throw new ArgumentException("El ID del detalle del pedido debe ser mayor a 0", nameof(pedidoDetalleId));

            if (nuevaCantidad <= 0)
                throw new ArgumentException("La nueva cantidad debe ser mayor a 0", nameof(nuevaCantidad));

            // Verificar que el detalle existe
            var detalle = await _pedidoDetalleRepository.ObtenerPorIdAsync(pedidoDetalleId);
            if (detalle == null)
                throw new InvalidOperationException($"El detalle del pedido con ID {pedidoDetalleId} no existe");

            detalle.Cantidad = nuevaCantidad;

            return await _pedidoDetalleRepository.ActualizarAsync(detalle);
        }

        public async Task<bool> EliminarProductoAsync(int pedidoDetalleId)
        {
            if (pedidoDetalleId <= 0)
                throw new ArgumentException("El ID del detalle del pedido debe ser mayor a 0", nameof(pedidoDetalleId));

            // Verificar que el detalle existe
            var existe = await _pedidoDetalleRepository.ExisteAsync(pedidoDetalleId);
            if (!existe)
                throw new InvalidOperationException($"El detalle del pedido con ID {pedidoDetalleId} no existe");

            return await _pedidoDetalleRepository.EliminarAsync(pedidoDetalleId);
        }

        public async Task<object> ObtenerResumenPedidoAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            var detalles = await _pedidoDetalleRepository.ListarPorPedidoAsync(pedidoId);
            var subtotal = await _pedidoDetalleRepository.CalcularSubtotalAsync(pedidoId);
            var cantidadProductos = await _pedidoDetalleRepository.ObtenerCantidadProductosAsync(pedidoId);

            var resumen = new
            {
                PedidoId = pedidoId,
                CantidadProductos = cantidadProductos,
                Subtotal = subtotal,
                TotalDetalles = detalles.Count(),
                Detalles = detalles,
                FechaConsulta = DateTime.Now
            };

            return resumen;
        }
    }
} 