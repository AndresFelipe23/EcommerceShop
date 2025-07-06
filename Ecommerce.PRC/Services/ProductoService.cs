using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;


namespace Ecommerce.PRC.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ILogService _logService;

        public ProductoService(IProductoRepository productoRepository, ILogService logService)
        {
            _productoRepository = productoRepository;
            _logService = logService;
        }

        public async Task<IEnumerable<Producto>> ListarProductosAsync()
        {
            try
            {
                var productos = await _productoRepository.ListarAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listados {productos.Count()} productos", Origen = "ProductoService", Metodo = "ListarProductosAsync" });
                return productos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar productos: {ex.Message}", Origen = "ProductoService", Metodo = "ListarProductosAsync" });
                throw;
            }
        }

        public async Task<Producto?> ObtenerProductoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoService", Metodo = "ObtenerProductoAsync" });
                    return null;
                }

                var producto = await _productoRepository.ObtenerPorIdAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Producto obtenido: {proId}", Origen = "ProductoService", Metodo = "ObtenerProductoAsync" });
                return producto;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener producto {proId}: {ex.Message}", Origen = "ProductoService", Metodo = "ObtenerProductoAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<Producto>> ListarPorCategoriaAsync(int catId)
        {
            try
            {
                if (catId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de categoría inválido: {catId}", Origen = "ProductoService", Metodo = "ListarPorCategoriaAsync" });
                    return Enumerable.Empty<Producto>();
                }

                var productos = await _productoRepository.ListarPorCategoriaAsync(catId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listados {productos.Count()} productos de categoría {catId}", Origen = "ProductoService", Metodo = "ListarPorCategoriaAsync" });
                return productos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar productos por categoría {catId}: {ex.Message}", Origen = "ProductoService", Metodo = "ListarPorCategoriaAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<Producto>> ListarPorGeneroAsync(string genero)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(genero))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Género no especificado", Origen = "ProductoService", Metodo = "ListarPorGeneroAsync" });
                    return Enumerable.Empty<Producto>();
                }

                var productos = await _productoRepository.ListarPorGeneroAsync(genero);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listados {productos.Count()} productos de género {genero}", Origen = "ProductoService", Metodo = "ListarPorGeneroAsync" });
                return productos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar productos por género {genero}: {ex.Message}", Origen = "ProductoService", Metodo = "ListarPorGeneroAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<Producto>> ListarActivosAsync()
        {
            try
            {
                var productos = await _productoRepository.ListarActivosAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listados {productos.Count()} productos activos", Origen = "ProductoService", Metodo = "ListarActivosAsync" });
                return productos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar productos activos: {ex.Message}", Origen = "ProductoService", Metodo = "ListarActivosAsync" });
                throw;
            }
        }

        public async Task<int> CrearProductoAsync(Producto producto)
        {
            try
            {
                if (!await ValidarProductoAsync(producto))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Validación de producto fallida", Origen = "ProductoService", Metodo = "CrearProductoAsync" });
                    return 0;
                }

                var proId = await _productoRepository.CrearAsync(producto);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Producto creado con ID: {proId}", Origen = "ProductoService", Metodo = "CrearProductoAsync" });
                return proId;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al crear producto: {ex.Message}", Origen = "ProductoService", Metodo = "CrearProductoAsync" });
                throw;
            }
        }

        public async Task<bool> ActualizarProductoAsync(Producto producto)
        {
            try
            {
                if (producto.ProId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "ID de producto inválido para actualización", Origen = "ProductoService", Metodo = "ActualizarProductoAsync" });
                    return false;
                }

                if (!await ValidarProductoAsync(producto))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Validación de producto fallida en actualización", Origen = "ProductoService", Metodo = "ActualizarProductoAsync" });
                    return false;
                }

                var resultado = await _productoRepository.ActualizarAsync(producto);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Producto {producto.ProId} actualizado: {resultado}", Origen = "ProductoService", Metodo = "ActualizarProductoAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al actualizar producto {producto.ProId}: {ex.Message}", Origen = "ProductoService", Metodo = "ActualizarProductoAsync" });
                throw;
            }
        }

        public async Task<bool> EliminarProductoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido para eliminación: {proId}", Origen = "ProductoService", Metodo = "EliminarProductoAsync" });
                    return false;
                }

                var resultado = await _productoRepository.EliminarAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Producto {proId} eliminado: {resultado}", Origen = "ProductoService", Metodo = "EliminarProductoAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al eliminar producto {proId}: {ex.Message}", Origen = "ProductoService", Metodo = "EliminarProductoAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<Producto>> BuscarProductosAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Nombre de búsqueda vacío", Origen = "ProductoService", Metodo = "BuscarProductosAsync" });
                    return Enumerable.Empty<Producto>();
                }

                var productos = await _productoRepository.BuscarPorNombreAsync(nombre);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Búsqueda de '{nombre}' retornó {productos.Count()} productos", Origen = "ProductoService", Metodo = "BuscarProductosAsync" });
                return productos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error en búsqueda de productos '{nombre}': {ex.Message}", Origen = "ProductoService", Metodo = "BuscarProductosAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<Producto>> ListarPorRangoPrecioAsync(decimal precioMin, decimal precioMax)
        {
            try
            {
                if (precioMin < 0 || precioMax < 0 || precioMin > precioMax)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Rango de precios inválido: {precioMin} - {precioMax}", Origen = "ProductoService", Metodo = "ListarPorRangoPrecioAsync" });
                    return Enumerable.Empty<Producto>();
                }

                var productos = await _productoRepository.ListarPorRangoPrecioAsync(precioMin, precioMax);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listados {productos.Count()} productos en rango de precio {precioMin}-{precioMax}", Origen = "ProductoService", Metodo = "ListarPorRangoPrecioAsync" });
                return productos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar productos por rango de precio: {ex.Message}", Origen = "ProductoService", Metodo = "ListarPorRangoPrecioAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<Producto>> ListarProductosRecientesAsync(int cantidad = 10)
        {
            try
            {
                if (cantidad <= 0 || cantidad > 100)
                {
                    cantidad = 10;
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Cantidad ajustada a 10 (valor original: {cantidad})", Origen = "ProductoService", Metodo = "ListarProductosRecientesAsync" });
                }

                var productos = await _productoRepository.ListarProductosRecientesAsync(cantidad);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listados {productos.Count()} productos recientes", Origen = "ProductoService", Metodo = "ListarProductosRecientesAsync" });
                return productos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar productos recientes: {ex.Message}", Origen = "ProductoService", Metodo = "ListarProductosRecientesAsync" });
                throw;
            }
        }

        public async Task<object> ObtenerEstadisticasAsync()
        {
            try
            {
                var estadisticas = await _productoRepository.ObtenerEstadisticasAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = "Estadísticas de productos obtenidas", Origen = "ProductoService", Metodo = "ObtenerEstadisticasAsync" });
                return estadisticas;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener estadísticas: {ex.Message}", Origen = "ProductoService", Metodo = "ObtenerEstadisticasAsync" });
                throw;
            }
        }

        public async Task<bool> ActivarProductoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido para activación: {proId}", Origen = "ProductoService", Metodo = "ActivarProductoAsync" });
                    return false;
                }

                var resultado = await _productoRepository.ActualizarEstadoAsync(proId, true);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Producto {proId} activado: {resultado}", Origen = "ProductoService", Metodo = "ActivarProductoAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al activar producto {proId}: {ex.Message}", Origen = "ProductoService", Metodo = "ActivarProductoAsync" });
                throw;
            }
        }

        public async Task<bool> DesactivarProductoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido para desactivación: {proId}", Origen = "ProductoService", Metodo = "DesactivarProductoAsync" });
                    return false;
                }

                var resultado = await _productoRepository.ActualizarEstadoAsync(proId, false);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Producto {proId} desactivado: {resultado}", Origen = "ProductoService", Metodo = "DesactivarProductoAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al desactivar producto {proId}: {ex.Message}", Origen = "ProductoService", Metodo = "DesactivarProductoAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<Producto>> ListarConOfertasAsync()
        {
            try
            {
                var productos = await _productoRepository.ListarConOfertasAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listados {productos.Count()} productos con ofertas", Origen = "ProductoService", Metodo = "ListarConOfertasAsync" });
                return productos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar productos con ofertas: {ex.Message}", Origen = "ProductoService", Metodo = "ListarConOfertasAsync" });
                throw;
            }
        }

        public async Task<decimal> ObtenerPrecioPromedioAsync()
        {
            try
            {
                var promedio = await _productoRepository.ObtenerPrecioPromedioAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Precio promedio obtenido: {promedio}", Origen = "ProductoService", Metodo = "ObtenerPrecioPromedioAsync" });
                return promedio;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener precio promedio: {ex.Message}", Origen = "ProductoService", Metodo = "ObtenerPrecioPromedioAsync" });
                throw;
            }
        }

        public async Task<bool> ValidarProductoAsync(Producto producto)
        {
            try
            {
                if (producto == null)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Producto es null", Origen = "ProductoService", Metodo = "ValidarProductoAsync" });
                    return false;
                }

                if (string.IsNullOrWhiteSpace(producto.ProNombre) || producto.ProNombre.Length > 150)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Nombre de producto inválido", Origen = "ProductoService", Metodo = "ValidarProductoAsync" });
                    return false;
                }

                if (producto.ProPrecio <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Precio de producto inválido", Origen = "ProductoService", Metodo = "ValidarProductoAsync" });
                    return false;
                }

                if (producto.ProCategoriaId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Categoría de producto inválida", Origen = "ProductoService", Metodo = "ValidarProductoAsync" });
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(producto.ProGenero) && 
                    !new[] { "Masculino", "Femenino", "Unisex" }.Contains(producto.ProGenero))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Género de producto inválido", Origen = "ProductoService", Metodo = "ValidarProductoAsync" });
                    return false;
                }

                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = "Producto validado correctamente", Origen = "ProductoService", Metodo = "ValidarProductoAsync" });
                return true;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error en validación de producto: {ex.Message}", Origen = "ProductoService", Metodo = "ValidarProductoAsync" });
                return false;
            }
        }

        public async Task<bool> VerificarDisponibilidadAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido para verificación: {proId}", Origen = "ProductoService", Metodo = "VerificarDisponibilidadAsync" });
                    return false;
                }

                var producto = await _productoRepository.ObtenerPorIdAsync(proId);
                if (producto == null)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Producto {proId} no encontrado", Origen = "ProductoService", Metodo = "VerificarDisponibilidadAsync" });
                    return false;
                }

                var disponible = producto.ProActivo ?? false;
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Disponibilidad del producto {proId}: {disponible}", Origen = "ProductoService", Metodo = "VerificarDisponibilidadAsync" });
                return disponible;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al verificar disponibilidad del producto {proId}: {ex.Message}", Origen = "ProductoService", Metodo = "VerificarDisponibilidadAsync" });
                throw;
            }
        }
    }
} 