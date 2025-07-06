using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Services
{
    public class CarritoItemService : ICarritoItemService
    {
        private readonly ICarritoItemRepository _carritoItemRepository;
        private readonly ICarritoRepository _carritoRepository;

        public CarritoItemService(ICarritoItemRepository carritoItemRepository, ICarritoRepository carritoRepository)
        {
            _carritoItemRepository = carritoItemRepository;
            _carritoRepository = carritoRepository;
        }

        public async Task<IEnumerable<CarritoItem>> ListarAsync()
        {
            return await _carritoItemRepository.ListarAsync();
        }

        public async Task<CarritoItem?> ObtenerPorIdAsync(int carritoItemId)
        {
            if (carritoItemId <= 0)
                throw new ArgumentException("El ID del item del carrito debe ser mayor a 0", nameof(carritoItemId));

            return await _carritoItemRepository.ObtenerPorIdAsync(carritoItemId);
        }

        public async Task<IEnumerable<CarritoItem>> ListarPorCarritoAsync(int carritoId)
        {
            if (carritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carritoId));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carritoId} no existe");

            return await _carritoItemRepository.ListarPorCarritoAsync(carritoId);
        }

        public async Task<int> CrearAsync(CarritoItem carritoItem)
        {
            if (carritoItem == null)
                throw new ArgumentNullException(nameof(carritoItem));

            if (carritoItem.CarritoId <= 0)
                throw new ArgumentException("El ID del carrito es requerido", nameof(carritoItem.CarritoId));

            if (carritoItem.ProductoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto es requerido", nameof(carritoItem.ProductoTallaColorId));

            if (carritoItem.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(carritoItem.Cantidad));

            if (carritoItem.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a 0", nameof(carritoItem.PrecioUnitario));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carritoItem.CarritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carritoItem.CarritoId} no existe");

            return await _carritoItemRepository.CrearAsync(carritoItem);
        }

        public async Task<bool> ActualizarAsync(CarritoItem carritoItem)
        {
            if (carritoItem == null)
                throw new ArgumentNullException(nameof(carritoItem));

            if (carritoItem.CarritoItemId <= 0)
                throw new ArgumentException("El ID del item del carrito debe ser mayor a 0", nameof(carritoItem.CarritoItemId));

            if (carritoItem.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(carritoItem.Cantidad));

            if (carritoItem.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a 0", nameof(carritoItem.PrecioUnitario));

            // Verificar que el item existe
            var existe = await _carritoItemRepository.ExisteAsync(carritoItem.CarritoItemId);
            if (!existe)
                throw new InvalidOperationException($"El item del carrito con ID {carritoItem.CarritoItemId} no existe");

            return await _carritoItemRepository.ActualizarAsync(carritoItem);
        }

        public async Task<bool> EliminarAsync(int carritoItemId)
        {
            if (carritoItemId <= 0)
                throw new ArgumentException("El ID del item del carrito debe ser mayor a 0", nameof(carritoItemId));

            // Verificar que el item existe
            var existe = await _carritoItemRepository.ExisteAsync(carritoItemId);
            if (!existe)
                throw new InvalidOperationException($"El item del carrito con ID {carritoItemId} no existe");

            return await _carritoItemRepository.EliminarAsync(carritoItemId);
        }

        public async Task<bool> ActualizarCantidadAsync(int carritoItemId, int cantidad)
        {
            if (carritoItemId <= 0)
                throw new ArgumentException("El ID del item del carrito debe ser mayor a 0", nameof(carritoItemId));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

            // Verificar que el item existe
            var existe = await _carritoItemRepository.ExisteAsync(carritoItemId);
            if (!existe)
                throw new InvalidOperationException($"El item del carrito con ID {carritoItemId} no existe");

            return await _carritoItemRepository.ActualizarCantidadAsync(carritoItemId, cantidad);
        }

        public async Task<int> ObtenerCantidadProductosAsync(int carritoId)
        {
            if (carritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carritoId));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carritoId} no existe");

            return await _carritoItemRepository.ObtenerCantidadProductosAsync(carritoId);
        }

        public async Task<bool> AgregarProductoAsync(int carritoId, int productoTallaColorId, int cantidad, decimal precioUnitario)
        {
            if (carritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carritoId));

            if (productoTallaColorId <= 0)
                throw new ArgumentException("El ID del producto es requerido", nameof(productoTallaColorId));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

            if (precioUnitario <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a 0", nameof(precioUnitario));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carritoId} no existe");

            // Verificar si el producto ya está en el carrito
            var productoExiste = await _carritoItemRepository.ExisteProductoEnCarritoAsync(carritoId, productoTallaColorId);
            
            if (productoExiste)
            {
                // Si ya existe, actualizar la cantidad
                var itemExistente = await _carritoItemRepository.ObtenerPorProductoAsync(carritoId, productoTallaColorId);
                if (itemExistente != null)
                {
                    var nuevaCantidad = itemExistente.Cantidad + cantidad;
                    return await _carritoItemRepository.ActualizarCantidadAsync(itemExistente.CarritoItemId, nuevaCantidad);
                }
            }

            // Si no existe, crear nuevo item
            var nuevoItem = new CarritoItem
            {
                CarritoId = carritoId,
                ProductoTallaColorId = productoTallaColorId,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario
            };

            var itemId = await _carritoItemRepository.CrearAsync(nuevoItem);
            return itemId > 0;
        }

        public async Task<bool> RemoverProductoAsync(int carritoItemId)
        {
            return await EliminarAsync(carritoItemId);
        }

        public async Task<object> ObtenerDetalleItemAsync(int carritoItemId)
        {
            if (carritoItemId <= 0)
                throw new ArgumentException("El ID del item del carrito debe ser mayor a 0", nameof(carritoItemId));

            var item = await _carritoItemRepository.ObtenerPorIdAsync(carritoItemId);
            if (item == null)
                throw new InvalidOperationException($"El item del carrito con ID {carritoItemId} no existe");

            return new
            {
                CarritoItemId = item.CarritoItemId,
                CarritoId = item.CarritoId,
                ProductoTallaColorId = item.ProductoTallaColorId,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario,
                Subtotal = item.Cantidad * item.PrecioUnitario,
                FechaAgregado = item.FechaAgregado
            };
        }
    }
} 