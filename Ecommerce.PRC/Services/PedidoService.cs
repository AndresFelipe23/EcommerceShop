using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoDetalleRepository _pedidoDetalleRepository;

        public PedidoService(IPedidoRepository pedidoRepository, IPedidoDetalleRepository pedidoDetalleRepository)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoDetalleRepository = pedidoDetalleRepository;
        }

        public async Task<IEnumerable<Pedido>> ListarAsync()
        {
            return await _pedidoRepository.ListarAsync();
        }

        public async Task<Pedido?> ObtenerPorIdAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            return await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
        }

        public async Task<IEnumerable<Pedido>> ListarPorUsuarioAsync(int usuId)
        {
            if (usuId <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(usuId));

            return await _pedidoRepository.ListarPorUsuarioAsync(usuId);
        }

        public async Task<IEnumerable<Pedido>> ListarPorEstadoAsync(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("El estado es requerido", nameof(estado));

            var estadosValidos = new[] { "Pendiente", "Enviado", "Entregado", "Cancelado" };
            if (!estadosValidos.Contains(estado))
                throw new ArgumentException($"Estado inválido. Valores permitidos: {string.Join(", ", estadosValidos)}", nameof(estado));

            return await _pedidoRepository.ListarPorEstadoAsync(estado);
        }

        public async Task<IEnumerable<Pedido>> ListarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor a la fecha fin", nameof(fechaInicio));

            return await _pedidoRepository.ListarPorFechaAsync(fechaInicio, fechaFin);
        }

        public async Task<int> CrearAsync(Pedido pedido)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido));

            if (pedido.UsuId <= 0)
                throw new ArgumentException("El ID del usuario es requerido", nameof(pedido.UsuId));

            if (pedido.DireccionEnvioId <= 0)
                throw new ArgumentException("El ID de la dirección de envío es requerido", nameof(pedido.DireccionEnvioId));

            if (string.IsNullOrWhiteSpace(pedido.MetodoPago))
                throw new ArgumentException("El método de pago es requerido", nameof(pedido.MetodoPago));

            if (pedido.Total <= 0)
                throw new ArgumentException("El total debe ser mayor a 0", nameof(pedido.Total));

            // Establecer estado por defecto
            pedido.Estado = "Pendiente";

            return await _pedidoRepository.CrearAsync(pedido);
        }

        public async Task<bool> ActualizarAsync(Pedido pedido)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido));

            if (pedido.PedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedido.PedidoId));

            if (string.IsNullOrWhiteSpace(pedido.Estado))
                throw new ArgumentException("El estado es requerido", nameof(pedido.Estado));

            if (string.IsNullOrWhiteSpace(pedido.MetodoPago))
                throw new ArgumentException("El método de pago es requerido", nameof(pedido.MetodoPago));

            if (pedido.Total <= 0)
                throw new ArgumentException("El total debe ser mayor a 0", nameof(pedido.Total));

            // Verificar que el pedido existe
            var existe = await _pedidoRepository.ExisteAsync(pedido.PedidoId);
            if (!existe)
                throw new InvalidOperationException($"El pedido con ID {pedido.PedidoId} no existe");

            return await _pedidoRepository.ActualizarAsync(pedido);
        }

        public async Task<bool> EliminarAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            // Verificar que el pedido existe
            var existe = await _pedidoRepository.ExisteAsync(pedidoId);
            if (!existe)
                throw new InvalidOperationException($"El pedido con ID {pedidoId} no existe");

            // Verificar que el pedido no esté en estado finalizado
            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido?.Estado == "Entregado")
                throw new InvalidOperationException("No se puede eliminar un pedido entregado");

            return await _pedidoRepository.EliminarAsync(pedidoId);
        }

        public async Task<bool> ActualizarEstadoAsync(int pedidoId, string estado)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("El estado es requerido", nameof(estado));

            var estadosValidos = new[] { "Pendiente", "Enviado", "Entregado", "Cancelado" };
            if (!estadosValidos.Contains(estado))
                throw new ArgumentException($"Estado inválido. Valores permitidos: {string.Join(", ", estadosValidos)}", nameof(estado));

            // Verificar que el pedido existe
            var existe = await _pedidoRepository.ExisteAsync(pedidoId);
            if (!existe)
                throw new InvalidOperationException($"El pedido con ID {pedidoId} no existe");

            return await _pedidoRepository.ActualizarEstadoAsync(pedidoId, estado);
        }

        public async Task<decimal> CalcularTotalAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            return await _pedidoRepository.CalcularTotalAsync(pedidoId);
        }

        public async Task<IEnumerable<Pedido>> ListarPorMetodoPagoAsync(string metodoPago)
        {
            if (string.IsNullOrWhiteSpace(metodoPago))
                throw new ArgumentException("El método de pago es requerido", nameof(metodoPago));

            return await _pedidoRepository.ListarPorMetodoPagoAsync(metodoPago);
        }

        public async Task<object> ObtenerEstadisticasAsync()
        {
            return await _pedidoRepository.ObtenerEstadisticasAsync();
        }

        public async Task<IEnumerable<Pedido>> ListarPedidosRecientesAsync(int cantidad = 10)
        {
            if (cantidad <= 0 || cantidad > 100)
                throw new ArgumentException("La cantidad debe estar entre 1 y 100", nameof(cantidad));

            return await _pedidoRepository.ListarPedidosRecientesAsync(cantidad);
        }

        public async Task<bool> VerificarPedidoUsuarioAsync(int pedidoId, int usuId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            if (usuId <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(usuId));

            return await _pedidoRepository.VerificarPedidoUsuarioAsync(pedidoId, usuId);
        }

        public async Task<object> ObtenerPedidoCompletoAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException($"El pedido con ID {pedidoId} no existe");

            var detalles = await _pedidoDetalleRepository.ListarPorPedidoAsync(pedidoId);
            var totalCalculado = await _pedidoRepository.CalcularTotalAsync(pedidoId);
            var cantidadProductos = await _pedidoDetalleRepository.ObtenerCantidadProductosAsync(pedidoId);

            var pedidoCompleto = new
            {
                Pedido = pedido,
                Detalles = detalles,
                TotalCalculado = totalCalculado,
                CantidadProductos = cantidadProductos,
                FechaConsulta = DateTime.Now
            };

            return pedidoCompleto;
        }

        public async Task<bool> CancelarPedidoAsync(int pedidoId, string motivo)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de cancelación es requerido", nameof(motivo));

            // Verificar que el pedido existe
            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException($"El pedido con ID {pedidoId} no existe");

            // Verificar que el pedido no esté en estado finalizado
            if (pedido.Estado == "Entregado")
                throw new InvalidOperationException("No se puede cancelar un pedido entregado");

            if (pedido.Estado == "Cancelado")
                throw new InvalidOperationException("El pedido ya está cancelado");

            // Actualizar observaciones con el motivo de cancelación
            pedido.Observaciones = $"{pedido.Observaciones}\nCancelado: {motivo}";

            return await _pedidoRepository.ActualizarEstadoAsync(pedidoId, "Cancelado");
        }

        public async Task<bool> ProcesarPedidoAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            // Verificar que el pedido existe
            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException($"El pedido con ID {pedidoId} no existe");

            if (pedido.Estado != "Pendiente")
                throw new InvalidOperationException($"El pedido debe estar en estado 'Pendiente' para procesarlo. Estado actual: {pedido.Estado}");

            return await _pedidoRepository.ActualizarEstadoAsync(pedidoId, "Pendiente");
        }

        public async Task<bool> EnviarPedidoAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            // Verificar que el pedido existe
            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException($"El pedido con ID {pedidoId} no existe");

            if (pedido.Estado != "Pendiente")
                throw new InvalidOperationException($"El pedido debe estar en estado 'Pendiente' para enviarlo. Estado actual: {pedido.Estado}");

            return await _pedidoRepository.ActualizarEstadoAsync(pedidoId, "Enviado");
        }

        public async Task<bool> EntregarPedidoAsync(int pedidoId)
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0", nameof(pedidoId));

            // Verificar que el pedido existe
            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException($"El pedido con ID {pedidoId} no existe");

            if (pedido.Estado != "Enviado")
                throw new InvalidOperationException($"El pedido debe estar en estado 'Enviado' para marcarlo como entregado. Estado actual: {pedido.Estado}");

            return await _pedidoRepository.ActualizarEstadoAsync(pedidoId, "Entregado");
        }
    }
} 