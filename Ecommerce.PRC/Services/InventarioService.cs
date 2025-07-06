using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Services
{
    public class InventarioService : IInventarioService
    {
        private readonly IInventarioRepository _inventarioRepository;

        public InventarioService(IInventarioRepository inventarioRepository)
        {
            _inventarioRepository = inventarioRepository;
        }

        public async Task<IEnumerable<Inventario>> ListarAsync()
        {
            return await _inventarioRepository.ListarAsync();
        }

        public async Task<Inventario?> ObtenerPorIdAsync(int inventarioId)
        {
            if (inventarioId <= 0)
                throw new ArgumentException("El ID del inventario debe ser mayor a 0", nameof(inventarioId));

            return await _inventarioRepository.ObtenerPorIdAsync(inventarioId);
        }

        public async Task<IEnumerable<Inventario>> ListarPorProductoAsync(int proId)
        {
            if (proId <= 0)
                throw new ArgumentException("El ID del producto debe ser mayor a 0", nameof(proId));

            return await _inventarioRepository.ListarPorProductoAsync(proId);
        }

        public async Task<IEnumerable<Inventario>> ListarPorProductoTallaColorAsync(int productoTallaColorId)
        {
            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            return await _inventarioRepository.ListarPorProductoTallaColorAsync(productoTallaColorId);
        }

        public async Task<int> CrearAsync(Inventario inventario)
        {
            if (inventario == null)
                throw new ArgumentNullException(nameof(inventario));

            if (inventario.ProductoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color es requerido", nameof(inventario.ProductoTallaColorId));

            if (string.IsNullOrWhiteSpace(inventario.TipoMovimiento))
                throw new ArgumentException("El tipo de movimiento es requerido", nameof(inventario.TipoMovimiento));

            if (inventario.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(inventario.Cantidad));

            if (string.IsNullOrWhiteSpace(inventario.Descripcion))
                throw new ArgumentException("La descripción es requerida", nameof(inventario.Descripcion));

            return await _inventarioRepository.CrearAsync(inventario);
        }

        public async Task<bool> ActualizarAsync(Inventario inventario)
        {
            if (inventario == null)
                throw new ArgumentNullException(nameof(inventario));

            if (inventario.InventarioId <= 0)
                throw new ArgumentException("El ID del inventario debe ser mayor a 0", nameof(inventario.InventarioId));

            if (inventario.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(inventario.Cantidad));

            // Verificar que el inventario existe
            var existe = await _inventarioRepository.ExisteAsync(inventario.InventarioId);
            if (!existe)
                throw new InvalidOperationException($"El inventario con ID {inventario.InventarioId} no existe");

            return await _inventarioRepository.ActualizarAsync(inventario);
        }

        public async Task<bool> EliminarAsync(int inventarioId)
        {
            if (inventarioId <= 0)
                throw new ArgumentException("El ID del inventario debe ser mayor a 0", nameof(inventarioId));

            // Verificar que el inventario existe
            var existe = await _inventarioRepository.ExisteAsync(inventarioId);
            if (!existe)
                throw new InvalidOperationException($"El inventario con ID {inventarioId} no existe");

            return await _inventarioRepository.EliminarAsync(inventarioId);
        }

        public async Task<decimal> ObtenerStockActualAsync(int productoTallaColorId)
        {
            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            return await _inventarioRepository.ObtenerStockActualAsync(productoTallaColorId);
        }

        public async Task<bool> RegistrarEntradaAsync(int productoTallaColorId, int cantidad, string descripcion, int? usuId = null)
        {
            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción es requerida", nameof(descripcion));

            return await _inventarioRepository.RegistrarMovimientoAsync(productoTallaColorId, "Entrada", cantidad, descripcion, usuId);
        }

        public async Task<bool> RegistrarSalidaAsync(int productoTallaColorId, int cantidad, string descripcion, int? usuId = null)
        {
            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción es requerida", nameof(descripcion));

            // Verificar que hay stock suficiente
            var stockActual = await _inventarioRepository.ObtenerStockActualAsync(productoTallaColorId);
            if (stockActual < cantidad)
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {stockActual}, Solicitado: {cantidad}");

            return await _inventarioRepository.RegistrarMovimientoAsync(productoTallaColorId, "Salida", cantidad, descripcion, usuId);
        }

        public async Task<bool> RegistrarVentaAsync(int productoTallaColorId, int cantidad, int usuId)
        {
            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

            if (usuId <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(usuId));

            // Verificar que hay stock suficiente
            var stockActual = await _inventarioRepository.ObtenerStockActualAsync(productoTallaColorId);
            if (stockActual < cantidad)
                throw new InvalidOperationException($"Stock insuficiente para la venta. Disponible: {stockActual}, Solicitado: {cantidad}");

            var descripcion = $"Venta realizada por usuario {usuId}";
            return await _inventarioRepository.RegistrarMovimientoAsync(productoTallaColorId, "Venta", cantidad, descripcion, usuId);
        }

        public async Task<bool> RegistrarAjusteAsync(int productoTallaColorId, int cantidad, string descripcion, int? usuId = null)
        {
            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            if (cantidad == 0)
                throw new ArgumentException("La cantidad no puede ser 0", nameof(cantidad));

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción es requerida", nameof(descripcion));

            return await _inventarioRepository.RegistrarMovimientoAsync(productoTallaColorId, "Ajuste", cantidad, descripcion, usuId);
        }

        public async Task<IEnumerable<Inventario>> ListarPorTipoMovimientoAsync(string tipoMovimiento)
        {
            if (string.IsNullOrWhiteSpace(tipoMovimiento))
                throw new ArgumentException("El tipo de movimiento es requerido", nameof(tipoMovimiento));

            var tiposValidos = new[] { "Entrada", "Salida", "Venta", "Ajuste" };
            if (!tiposValidos.Contains(tipoMovimiento))
                throw new ArgumentException($"Tipo de movimiento inválido. Valores permitidos: {string.Join(", ", tiposValidos)}", nameof(tipoMovimiento));

            return await _inventarioRepository.ListarPorTipoMovimientoAsync(tipoMovimiento);
        }

        public async Task<IEnumerable<Inventario>> ListarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor a la fecha fin", nameof(fechaInicio));

            return await _inventarioRepository.ListarPorFechaAsync(fechaInicio, fechaFin);
        }

        public async Task<IEnumerable<Inventario>> ListarPorUsuarioAsync(int usuId)
        {
            if (usuId <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(usuId));

            return await _inventarioRepository.ListarPorUsuarioAsync(usuId);
        }

        public async Task<object> ObtenerResumenInventarioAsync(int productoTallaColorId)
        {
            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            var stockActual = await _inventarioRepository.ObtenerStockActualAsync(productoTallaColorId);
            var movimientos = await _inventarioRepository.ListarPorProductoTallaColorAsync(productoTallaColorId);

            var resumen = new
            {
                ProductoTallaColorId = productoTallaColorId,
                StockActual = stockActual,
                TotalMovimientos = movimientos.Count(),
                UltimoMovimiento = movimientos.FirstOrDefault()?.FechaMovimiento,
                Movimientos = movimientos.Take(10) // Últimos 10 movimientos
            };

            return resumen;
        }

        public async Task<bool> VerificarStockDisponibleAsync(int productoTallaColorId, int cantidad)
        {
            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto-talla-color debe ser mayor a 0", nameof(productoTallaColorId));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

            var stockActual = await _inventarioRepository.ObtenerStockActualAsync(productoTallaColorId);
            return stockActual >= cantidad;
        }
    }
} 