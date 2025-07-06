using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;


namespace Ecommerce.PRC.Services
{
    public class ProductoTallaColorService : IProductoTallaColorService
    {
        private readonly IProductoTallaColorRepository _productoTallaColorRepository;
        private readonly ILogService _logService;

        public ProductoTallaColorService(IProductoTallaColorRepository productoTallaColorRepository, ILogService logService)
        {
            _productoTallaColorRepository = productoTallaColorRepository;
            _logService = logService;
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarCombinacionesAsync()
        {
            try
            {
                var combinaciones = await _productoTallaColorRepository.ListarAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {combinaciones.Count()} combinaciones producto-talla-color", Origen = "ProductoTallaColorService", Metodo = "ListarCombinacionesAsync" });
                return combinaciones;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar combinaciones: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ListarCombinacionesAsync" });
                throw;
            }
        }

        public async Task<ProductoTallaColor?> ObtenerCombinacionAsync(int productoTallaColorId)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de combinación inválido: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "ObtenerCombinacionAsync" });
                    return null;
                }

                var combinacion = await _productoTallaColorRepository.ObtenerPorIdAsync(productoTallaColorId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Combinación obtenida: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "ObtenerCombinacionAsync" });
                return combinacion;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener combinación {productoTallaColorId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ObtenerCombinacionAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarPorProductoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoTallaColorService", Metodo = "ListarPorProductoAsync" });
                    return Enumerable.Empty<ProductoTallaColor>();
                }

                var combinaciones = await _productoTallaColorRepository.ListarPorProductoAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {combinaciones.Count()} combinaciones del producto {proId}", Origen = "ProductoTallaColorService", Metodo = "ListarPorProductoAsync" });
                return combinaciones;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar combinaciones del producto {proId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ListarPorProductoAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarPorTallaAsync(int tallaId)
        {
            try
            {
                if (tallaId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de talla inválido: {tallaId}", Origen = "ProductoTallaColorService", Metodo = "ListarPorTallaAsync" });
                    return Enumerable.Empty<ProductoTallaColor>();
                }

                var combinaciones = await _productoTallaColorRepository.ListarPorTallaAsync(tallaId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {combinaciones.Count()} combinaciones de talla {tallaId}", Origen = "ProductoTallaColorService", Metodo = "ListarPorTallaAsync" });
                return combinaciones;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar combinaciones de talla {tallaId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ListarPorTallaAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarPorColorAsync(int colorId)
        {
            try
            {
                if (colorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de color inválido: {colorId}", Origen = "ProductoTallaColorService", Metodo = "ListarPorColorAsync" });
                    return Enumerable.Empty<ProductoTallaColor>();
                }

                var combinaciones = await _productoTallaColorRepository.ListarPorColorAsync(colorId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {combinaciones.Count()} combinaciones de color {colorId}", Origen = "ProductoTallaColorService", Metodo = "ListarPorColorAsync" });
                return combinaciones;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar combinaciones de color {colorId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ListarPorColorAsync" });
                throw;
            }
        }

        public async Task<int> CrearCombinacionAsync(ProductoTallaColor productoTallaColor)
        {
            try
            {
                if (!await ValidarCombinacionAsync(productoTallaColor))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Validación de combinación fallida", Origen = "ProductoTallaColorService", Metodo = "CrearCombinacionAsync" });
                    return 0;
                }

                // Verificar si ya existe la combinación
                var existe = await _productoTallaColorRepository.ExisteCombinacionAsync(
                    productoTallaColor.ProId, 
                    productoTallaColor.TallaId, 
                    productoTallaColor.ColorId
                );

                if (existe)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Combinación ya existe: ProId={productoTallaColor.ProId}, TallaId={productoTallaColor.TallaId}, ColorId={productoTallaColor.ColorId}", Origen = "ProductoTallaColorService", Metodo = "CrearCombinacionAsync" });
                    return 0;
                }

                var productoTallaColorId = await _productoTallaColorRepository.CrearAsync(productoTallaColor);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Combinación creada con ID: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "CrearCombinacionAsync" });
                return productoTallaColorId;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al crear combinación: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "CrearCombinacionAsync" });
                throw;
            }
        }

        public async Task<bool> ActualizarCombinacionAsync(ProductoTallaColor productoTallaColor)
        {
            try
            {
                if (productoTallaColor.ProductoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "ID de combinación inválido para actualización", Origen = "ProductoTallaColorService", Metodo = "ActualizarCombinacionAsync" });
                    return false;
                }

                if (!await ValidarCombinacionAsync(productoTallaColor))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Validación de combinación fallida en actualización", Origen = "ProductoTallaColorService", Metodo = "ActualizarCombinacionAsync" });
                    return false;
                }

                var resultado = await _productoTallaColorRepository.ActualizarAsync(productoTallaColor);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Combinación {productoTallaColor.ProductoTallaColorId} actualizada: {resultado}", Origen = "ProductoTallaColorService", Metodo = "ActualizarCombinacionAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al actualizar combinación {productoTallaColor.ProductoTallaColorId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ActualizarCombinacionAsync" });
                throw;
            }
        }

        public async Task<bool> EliminarCombinacionAsync(int productoTallaColorId)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de combinación inválido para eliminación: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "EliminarCombinacionAsync" });
                    return false;
                }

                var resultado = await _productoTallaColorRepository.EliminarAsync(productoTallaColorId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Combinación {productoTallaColorId} eliminada: {resultado}", Origen = "ProductoTallaColorService", Metodo = "EliminarCombinacionAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al eliminar combinación {productoTallaColorId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "EliminarCombinacionAsync" });
                throw;
            }
        }

        public async Task<bool> ExisteCombinacionAsync(int proId, int tallaId, int colorId)
        {
            try
            {
                if (proId <= 0 || tallaId <= 0 || colorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"IDs inválidos: ProId={proId}, TallaId={tallaId}, ColorId={colorId}", Origen = "ProductoTallaColorService", Metodo = "ExisteCombinacionAsync" });
                    return false;
                }

                var existe = await _productoTallaColorRepository.ExisteCombinacionAsync(proId, tallaId, colorId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Verificación de combinación: ProId={proId}, TallaId={tallaId}, ColorId={colorId}, Existe={existe}", Origen = "ProductoTallaColorService", Metodo = "ExisteCombinacionAsync" });
                return existe;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al verificar combinación: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ExisteCombinacionAsync" });
                throw;
            }
        }

        public async Task<ProductoTallaColor?> ObtenerCombinacionEspecificaAsync(int proId, int tallaId, int colorId)
        {
            try
            {
                if (proId <= 0 || tallaId <= 0 || colorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"IDs inválidos: ProId={proId}, TallaId={tallaId}, ColorId={colorId}", Origen = "ProductoTallaColorService", Metodo = "ObtenerCombinacionEspecificaAsync" });
                    return null;
                }

                var combinacion = await _productoTallaColorRepository.ObtenerCombinacionAsync(proId, tallaId, colorId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Combinación específica obtenida: ProId={proId}, TallaId={tallaId}, ColorId={colorId}", Origen = "ProductoTallaColorService", Metodo = "ObtenerCombinacionEspecificaAsync" });
                return combinacion;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener combinación específica: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ObtenerCombinacionEspecificaAsync" });
                throw;
            }
        }

        public async Task<bool> ActualizarStockAsync(int productoTallaColorId, int nuevoStock)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de combinación inválido: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "ActualizarStockAsync" });
                    return false;
                }

                if (nuevoStock < 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Stock inválido: {nuevoStock}", Origen = "ProductoTallaColorService", Metodo = "ActualizarStockAsync" });
                    return false;
                }

                var resultado = await _productoTallaColorRepository.ActualizarStockAsync(productoTallaColorId, nuevoStock);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Stock de combinación {productoTallaColorId} actualizado a {nuevoStock}: {resultado}", Origen = "ProductoTallaColorService", Metodo = "ActualizarStockAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al actualizar stock de combinación {productoTallaColorId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ActualizarStockAsync" });
                throw;
            }
        }

        public async Task<bool> ActualizarPrecioOfertaAsync(int productoTallaColorId, decimal? precioOferta)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de combinación inválido: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "ActualizarPrecioOfertaAsync" });
                    return false;
                }

                if (precioOferta.HasValue && precioOferta.Value < 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Precio de oferta inválido: {precioOferta}", Origen = "ProductoTallaColorService", Metodo = "ActualizarPrecioOfertaAsync" });
                    return false;
                }

                var resultado = await _productoTallaColorRepository.ActualizarPrecioOfertaAsync(productoTallaColorId, precioOferta);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Precio de oferta de combinación {productoTallaColorId} actualizado a {precioOferta}: {resultado}", Origen = "ProductoTallaColorService", Metodo = "ActualizarPrecioOfertaAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al actualizar precio de oferta de combinación {productoTallaColorId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ActualizarPrecioOfertaAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarConStockAsync()
        {
            try
            {
                var combinaciones = await _productoTallaColorRepository.ListarConStockAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {combinaciones.Count()} combinaciones con stock", Origen = "ProductoTallaColorService", Metodo = "ListarConStockAsync" });
                return combinaciones;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar combinaciones con stock: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ListarConStockAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarSinStockAsync()
        {
            try
            {
                var combinaciones = await _productoTallaColorRepository.ListarSinStockAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {combinaciones.Count()} combinaciones sin stock", Origen = "ProductoTallaColorService", Metodo = "ListarSinStockAsync" });
                return combinaciones;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar combinaciones sin stock: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ListarSinStockAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarConOfertasAsync()
        {
            try
            {
                var combinaciones = await _productoTallaColorRepository.ListarConOfertasAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {combinaciones.Count()} combinaciones con ofertas", Origen = "ProductoTallaColorService", Metodo = "ListarConOfertasAsync" });
                return combinaciones;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar combinaciones con ofertas: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ListarConOfertasAsync" });
                throw;
            }
        }

        public async Task<object> ObtenerEstadisticasStockAsync()
        {
            try
            {
                var estadisticas = await _productoTallaColorRepository.ObtenerEstadisticasStockAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = "Estadísticas de stock obtenidas", Origen = "ProductoTallaColorService", Metodo = "ObtenerEstadisticasStockAsync" });
                return estadisticas;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener estadísticas de stock: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ObtenerEstadisticasStockAsync" });
                throw;
            }
        }

        public async Task<bool> VerificarStockDisponibleAsync(int productoTallaColorId, int cantidad)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de combinación inválido: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "VerificarStockDisponibleAsync" });
                    return false;
                }

                if (cantidad <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Cantidad inválida: {cantidad}", Origen = "ProductoTallaColorService", Metodo = "VerificarStockDisponibleAsync" });
                    return false;
                }

                var disponible = await _productoTallaColorRepository.VerificarStockDisponibleAsync(productoTallaColorId, cantidad);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Verificación de stock: Combinación={productoTallaColorId}, Cantidad={cantidad}, Disponible={disponible}", Origen = "ProductoTallaColorService", Metodo = "VerificarStockDisponibleAsync" });
                return disponible;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al verificar stock disponible: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "VerificarStockDisponibleAsync" });
                throw;
            }
        }

        public async Task<decimal> ObtenerPrecioMinimoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoTallaColorService", Metodo = "ObtenerPrecioMinimoAsync" });
                    return 0;
                }

                var precioMinimo = await _productoTallaColorRepository.ObtenerPrecioMinimoAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Precio mínimo del producto {proId}: {precioMinimo}", Origen = "ProductoTallaColorService", Metodo = "ObtenerPrecioMinimoAsync" });
                return precioMinimo;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener precio mínimo del producto {proId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ObtenerPrecioMinimoAsync" });
                throw;
            }
        }

        public async Task<decimal> ObtenerPrecioMaximoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoTallaColorService", Metodo = "ObtenerPrecioMaximoAsync" });
                    return 0;
                }

                var precioMaximo = await _productoTallaColorRepository.ObtenerPrecioMaximoAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Precio máximo del producto {proId}: {precioMaximo}", Origen = "ProductoTallaColorService", Metodo = "ObtenerPrecioMaximoAsync" });
                return precioMaximo;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener precio máximo del producto {proId}: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ObtenerPrecioMaximoAsync" });
                throw;
            }
        }

        public async Task<bool> ValidarCombinacionAsync(ProductoTallaColor productoTallaColor)
        {
            try
            {
                if (productoTallaColor == null)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Combinación es null", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                    return false;
                }

                if (productoTallaColor.ProId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "ID de producto inválido", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                    return false;
                }

                if (productoTallaColor.TallaId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "ID de talla inválido", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                    return false;
                }

                if (productoTallaColor.ColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "ID de color inválido", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                    return false;
                }

                if (productoTallaColor.Stock < 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Stock inválido", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                    return false;
                }

                if (productoTallaColor.PrecioOferta.HasValue && productoTallaColor.PrecioOferta.Value < 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Precio de oferta inválido", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(productoTallaColor.Sku) && productoTallaColor.Sku.Length > 50)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "SKU demasiado largo", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                    return false;
                }

                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = "Combinación validada correctamente", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                return true;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error en validación de combinación: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ValidarCombinacionAsync" });
                return false;
            }
        }

        public async Task<bool> ReducirStockAsync(int productoTallaColorId, int cantidad)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de combinación inválido: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "ReducirStockAsync" });
                    return false;
                }

                if (cantidad <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Cantidad inválida: {cantidad}", Origen = "ProductoTallaColorService", Metodo = "ReducirStockAsync" });
                    return false;
                }

                // Verificar stock disponible
                var disponible = await _productoTallaColorRepository.VerificarStockDisponibleAsync(productoTallaColorId, cantidad);
                if (!disponible)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Stock insuficiente: Combinación={productoTallaColorId}, Cantidad={cantidad}", Origen = "ProductoTallaColorService", Metodo = "ReducirStockAsync" });
                    return false;
                }

                // Obtener stock actual
                var combinacion = await _productoTallaColorRepository.ObtenerPorIdAsync(productoTallaColorId);
                if (combinacion == null)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Combinación no encontrada: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "ReducirStockAsync" });
                    return false;
                }

                var nuevoStock = combinacion.Stock - cantidad;
                var resultado = await _productoTallaColorRepository.ActualizarStockAsync(productoTallaColorId, nuevoStock);
                
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Stock reducido: Combinación={productoTallaColorId}, Cantidad={cantidad}, StockAnterior={combinacion.Stock}, StockNuevo={nuevoStock}, Resultado={resultado}", Origen = "ProductoTallaColorService", Metodo = "ReducirStockAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al reducir stock: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "ReducirStockAsync" });
                throw;
            }
        }

        public async Task<bool> AumentarStockAsync(int productoTallaColorId, int cantidad)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de combinación inválido: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "AumentarStockAsync" });
                    return false;
                }

                if (cantidad <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Cantidad inválida: {cantidad}", Origen = "ProductoTallaColorService", Metodo = "AumentarStockAsync" });
                    return false;
                }

                // Obtener stock actual
                var combinacion = await _productoTallaColorRepository.ObtenerPorIdAsync(productoTallaColorId);
                if (combinacion == null)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Combinación no encontrada: {productoTallaColorId}", Origen = "ProductoTallaColorService", Metodo = "AumentarStockAsync" });
                    return false;
                }

                var nuevoStock = combinacion.Stock + cantidad;
                var resultado = await _productoTallaColorRepository.ActualizarStockAsync(productoTallaColorId, nuevoStock);
                
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Stock aumentado: Combinación={productoTallaColorId}, Cantidad={cantidad}, StockAnterior={combinacion.Stock}, StockNuevo={nuevoStock}, Resultado={resultado}", Origen = "ProductoTallaColorService", Metodo = "AumentarStockAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al aumentar stock: {ex.Message}", Origen = "ProductoTallaColorService", Metodo = "AumentarStockAsync" });
                throw;
            }
        }
    }
} 