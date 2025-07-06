using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepository;
        private readonly ICarritoItemRepository _carritoItemRepository;

        public CarritoService(ICarritoRepository carritoRepository, ICarritoItemRepository carritoItemRepository)
        {
            _carritoRepository = carritoRepository;
            _carritoItemRepository = carritoItemRepository;
        }

        public async Task<IEnumerable<Carrito>> ListarAsync()
        {
            return await _carritoRepository.ListarAsync();
        }

        public async Task<Carrito?> ObtenerPorIdAsync(int carritoId)
        {
            if (carritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carritoId));

            return await _carritoRepository.ObtenerPorIdAsync(carritoId);
        }

        public async Task<Carrito?> ObtenerPorUsuarioAsync(int usuId)
        {
            if (usuId <= 0)
                throw new ArgumentException("El ID de usuario debe ser mayor a 0", nameof(usuId));

            return await _carritoRepository.ObtenerPorUsuarioAsync(usuId);
        }

        public async Task<int> CrearAsync(Carrito carrito)
        {
            if (carrito == null)
                throw new ArgumentNullException(nameof(carrito));

            if (carrito.UsuId <= 0)
                throw new ArgumentException("El ID de usuario es requerido", nameof(carrito.UsuId));

            // Verificar si el usuario ya tiene un carrito
            var existe = await _carritoRepository.ExistePorUsuarioAsync(carrito.UsuId);
            if (existe)
                throw new InvalidOperationException($"El usuario {carrito.UsuId} ya tiene un carrito activo");

            return await _carritoRepository.CrearAsync(carrito);
        }

        public async Task<bool> ActualizarAsync(Carrito carrito)
        {
            if (carrito == null)
                throw new ArgumentNullException(nameof(carrito));

            if (carrito.CarritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carrito.CarritoId));

            if (carrito.UsuId <= 0)
                throw new ArgumentException("El ID de usuario es requerido", nameof(carrito.UsuId));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carrito.CarritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carrito.CarritoId} no existe");

            return await _carritoRepository.ActualizarAsync(carrito);
        }

        public async Task<bool> EliminarAsync(int carritoId)
        {
            if (carritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carritoId));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carritoId} no existe");

            return await _carritoRepository.EliminarAsync(carritoId);
        }

        public async Task<bool> VaciarCarritoAsync(int carritoId)
        {
            if (carritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carritoId));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carritoId} no existe");

            return await _carritoRepository.VaciarCarritoAsync(carritoId);
        }

        public async Task<decimal> CalcularTotalAsync(int carritoId)
        {
            if (carritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carritoId));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carritoId} no existe");

            return await _carritoRepository.CalcularTotalAsync(carritoId);
        }

        public async Task<Carrito> ObtenerOCrearCarritoAsync(int usuId)
        {
            if (usuId <= 0)
                throw new ArgumentException("El ID de usuario debe ser mayor a 0", nameof(usuId));

            // Intentar obtener el carrito existente
            var carrito = await _carritoRepository.ObtenerPorUsuarioAsync(usuId);
            
            if (carrito != null)
                return carrito;

            // Si no existe, crear uno nuevo
            var nuevoCarrito = new Carrito { UsuId = usuId };
            var carritoId = await _carritoRepository.CrearAsync(nuevoCarrito);
            nuevoCarrito.CarritoId = carritoId;
            
            return nuevoCarrito;
        }

        public async Task<object> ObtenerResumenCarritoAsync(int carritoId)
        {
            if (carritoId <= 0)
                throw new ArgumentException("El ID del carrito debe ser mayor a 0", nameof(carritoId));

            // Verificar que el carrito existe
            var existe = await _carritoRepository.ExisteAsync(carritoId);
            if (!existe)
                throw new InvalidOperationException($"El carrito con ID {carritoId} no existe");

            var carrito = await _carritoRepository.ObtenerPorIdAsync(carritoId);
            var items = await _carritoItemRepository.ListarPorCarritoAsync(carritoId);
            var total = await _carritoRepository.CalcularTotalAsync(carritoId);
            var cantidadProductos = await _carritoItemRepository.ObtenerCantidadProductosAsync(carritoId);

            return new
            {
                CarritoId = carritoId,
                UsuarioId = carrito?.UsuId,
                FechaCreacion = carrito?.FechaCreacion,
                CantidadItems = items.Count(),
                CantidadProductos = cantidadProductos,
                Total = total,
                Items = items
            };
        }
    }
} 