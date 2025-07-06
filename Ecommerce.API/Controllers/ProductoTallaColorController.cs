using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoTallaColorController : ControllerBase
    {
        private readonly IProductoTallaColorService _productoTallaColorService;
        private readonly ILogService _logService;

        public ProductoTallaColorController(IProductoTallaColorService productoTallaColorService, ILogService logService)
        {
            _productoTallaColorService = productoTallaColorService;
            _logService = logService;
        }

        /// <summary>
        /// Obtiene todas las combinaciones producto-talla-color
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoTallaColor>>> GetCombinaciones()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/ProductoTallaColor",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinaciones"
                });
                
                var combinaciones = await _productoTallaColorService.ListarCombinacionesAsync();
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {combinaciones.Count()} combinaciones",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinaciones"
                });
                return Ok(combinaciones);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinaciones"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una combinación por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoTallaColor>> GetCombinacion(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/{id}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacion"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetCombinacion"
                    });
                    return BadRequest(new { mensaje = "ID de combinación inválido" });
                }

                var combinacion = await _productoTallaColorService.ObtenerCombinacionAsync(id);
                
                if (combinacion == null)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"Combinación no encontrada: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetCombinacion"
                    });
                    return NotFound(new { mensaje = "Combinación no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Combinación {id} obtenida exitosamente",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacion"
                });
                return Ok(combinacion);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/{id}: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacion"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene combinaciones por producto
        /// </summary>
        [HttpGet("producto/{proId}")]
        public async Task<ActionResult<IEnumerable<ProductoTallaColor>>> GetCombinacionesPorProducto(int proId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/producto/{proId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorProducto"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetCombinacionesPorProducto"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var combinaciones = await _productoTallaColorService.ListarPorProductoAsync(proId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {combinaciones.Count()} combinaciones del producto {proId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorProducto"
                });
                return Ok(combinaciones);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/producto/{proId}: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene combinaciones por talla
        /// </summary>
        [HttpGet("talla/{tallaId}")]
        public async Task<ActionResult<IEnumerable<ProductoTallaColor>>> GetCombinacionesPorTalla(int tallaId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/talla/{tallaId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorTalla"
                });
                
                if (tallaId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de talla inválido: {tallaId}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetCombinacionesPorTalla"
                    });
                    return BadRequest(new { mensaje = "ID de talla inválido" });
                }

                var combinaciones = await _productoTallaColorService.ListarPorTallaAsync(tallaId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {combinaciones.Count()} combinaciones de talla {tallaId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorTalla"
                });
                return Ok(combinaciones);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/talla/{tallaId}: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorTalla"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene combinaciones por color
        /// </summary>
        [HttpGet("color/{colorId}")]
        public async Task<ActionResult<IEnumerable<ProductoTallaColor>>> GetCombinacionesPorColor(int colorId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/color/{colorId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorColor"
                });
                
                if (colorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de color inválido: {colorId}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetCombinacionesPorColor"
                    });
                    return BadRequest(new { mensaje = "ID de color inválido" });
                }

                var combinaciones = await _productoTallaColorService.ListarPorColorAsync(colorId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {combinaciones.Count()} combinaciones de color {colorId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorColor"
                });
                return Ok(combinaciones);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/color/{colorId}: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesPorColor"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene combinaciones con stock disponible
        /// </summary>
        [HttpGet("con-stock")]
        public async Task<ActionResult<IEnumerable<ProductoTallaColor>>> GetCombinacionesConStock()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/ProductoTallaColor/con-stock",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesConStock"
                });
                
                var combinaciones = await _productoTallaColorService.ListarConStockAsync();
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {combinaciones.Count()} combinaciones con stock",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesConStock"
                });
                return Ok(combinaciones);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/con-stock: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesConStock"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene combinaciones sin stock
        /// </summary>
        [HttpGet("sin-stock")]
        public async Task<ActionResult<IEnumerable<ProductoTallaColor>>> GetCombinacionesSinStock()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/ProductoTallaColor/sin-stock",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesSinStock"
                });
                
                var combinaciones = await _productoTallaColorService.ListarSinStockAsync();
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {combinaciones.Count()} combinaciones sin stock",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesSinStock"
                });
                return Ok(combinaciones);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/sin-stock: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesSinStock"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene combinaciones con ofertas
        /// </summary>
        [HttpGet("con-ofertas")]
        public async Task<ActionResult<IEnumerable<ProductoTallaColor>>> GetCombinacionesConOfertas()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/ProductoTallaColor/con-ofertas",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesConOfertas"
                });
                
                var combinaciones = await _productoTallaColorService.ListarConOfertasAsync();
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {combinaciones.Count()} combinaciones con ofertas",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesConOfertas"
                });
                return Ok(combinaciones);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/con-ofertas: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionesConOfertas"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de stock
        /// </summary>
        [HttpGet("estadisticas-stock")]
        public async Task<ActionResult<object>> GetEstadisticasStock()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/ProductoTallaColor/estadisticas-stock",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetEstadisticasStock"
                });
                
                var estadisticas = await _productoTallaColorService.ObtenerEstadisticasStockAsync();
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Estadísticas de stock obtenidas",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetEstadisticasStock"
                });
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/estadisticas-stock: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetEstadisticasStock"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene precio mínimo de un producto
        /// </summary>
        [HttpGet("producto/{proId}/precio-minimo")]
        public async Task<ActionResult<decimal>> GetPrecioMinimo(int proId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/producto/{proId}/precio-minimo",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetPrecioMinimo"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetPrecioMinimo"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var precioMinimo = await _productoTallaColorService.ObtenerPrecioMinimoAsync(proId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Precio mínimo del producto {proId}: {precioMinimo}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetPrecioMinimo"
                });
                return Ok(precioMinimo);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/producto/{proId}/precio-minimo: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetPrecioMinimo"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene precio máximo de un producto
        /// </summary>
        [HttpGet("producto/{proId}/precio-maximo")]
        public async Task<ActionResult<decimal>> GetPrecioMaximo(int proId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/producto/{proId}/precio-maximo",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetPrecioMaximo"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetPrecioMaximo"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var precioMaximo = await _productoTallaColorService.ObtenerPrecioMaximoAsync(proId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Precio máximo del producto {proId}: {precioMaximo}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetPrecioMaximo"
                });
                return Ok(precioMaximo);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/producto/{proId}/precio-maximo: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetPrecioMaximo"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Verifica si existe una combinación específica
        /// </summary>
        [HttpGet("existe")]
        public async Task<ActionResult<bool>> VerificarExistencia([FromQuery] int proId, [FromQuery] int tallaId, [FromQuery] int colorId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/existe?proId={proId}&tallaId={tallaId}&colorId={colorId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "VerificarExistencia"
                });
                
                if (proId <= 0 || tallaId <= 0 || colorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"IDs inválidos: ProId={proId}, TallaId={tallaId}, ColorId={colorId}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "VerificarExistencia"
                    });
                    return BadRequest(new { mensaje = "IDs inválidos" });
                }

                var existe = await _productoTallaColorService.ExisteCombinacionAsync(proId, tallaId, colorId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Verificación de existencia: ProId={proId}, TallaId={tallaId}, ColorId={colorId}, Existe={existe}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "VerificarExistencia"
                });
                return Ok(existe);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/existe: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "VerificarExistencia"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una combinación específica
        /// </summary>
        [HttpGet("combinacion")]
        public async Task<ActionResult<ProductoTallaColor>> GetCombinacionEspecifica([FromQuery] int proId, [FromQuery] int tallaId, [FromQuery] int colorId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/combinacion?proId={proId}&tallaId={tallaId}&colorId={colorId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionEspecifica"
                });
                
                if (proId <= 0 || tallaId <= 0 || colorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"IDs inválidos: ProId={proId}, TallaId={tallaId}, ColorId={colorId}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetCombinacionEspecifica"
                    });
                    return BadRequest(new { mensaje = "IDs inválidos" });
                }

                var combinacion = await _productoTallaColorService.ObtenerCombinacionEspecificaAsync(proId, tallaId, colorId);
                
                if (combinacion == null)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"Combinación no encontrada: ProId={proId}, TallaId={tallaId}, ColorId={colorId}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "GetCombinacionEspecifica"
                    });
                    return NotFound(new { mensaje = "Combinación no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Combinación específica obtenida: ProId={proId}, TallaId={tallaId}, ColorId={colorId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionEspecifica"
                });
                return Ok(combinacion);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/combinacion: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "GetCombinacionEspecifica"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Verifica stock disponible
        /// </summary>
        [HttpGet("{id}/stock-disponible")]
        public async Task<ActionResult<bool>> VerificarStockDisponible(int id, [FromQuery] int cantidad)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoTallaColor/{id}/stock-disponible?cantidad={cantidad}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "VerificarStockDisponible"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "VerificarStockDisponible"
                    });
                    return BadRequest(new { mensaje = "ID de combinación inválido" });
                }

                if (cantidad <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"Cantidad inválida: {cantidad}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "VerificarStockDisponible"
                    });
                    return BadRequest(new { mensaje = "Cantidad inválida" });
                }

                var disponible = await _productoTallaColorService.VerificarStockDisponibleAsync(id, cantidad);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Verificación de stock: Combinación={id}, Cantidad={cantidad}, Disponible={disponible}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "VerificarStockDisponible"
                });
                return Ok(disponible);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoTallaColor/{id}/stock-disponible: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "VerificarStockDisponible"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva combinación
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductoTallaColor>> CrearCombinacion([FromBody] CrearProductoTallaColorDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud POST /api/ProductoTallaColor",
                    Origen = "ProductoTallaColorController",
                    Metodo = "CrearCombinacion"
                });
                
                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en creación de combinación",
                        Origen = "ProductoTallaColorController",
                        Metodo = "CrearCombinacion"
                    });
                    return BadRequest(ModelState);
                }

                var productoTallaColor = new ProductoTallaColor
                {
                    ProId = request.ProId,
                    TallaId = request.TallaId,
                    ColorId = request.ColorId,
                    Stock = request.Stock,
                    PrecioOferta = request.PrecioOferta,
                    Sku = request.Sku,
                    FechaCreacion = DateTime.Now
                };

                var productoTallaColorId = await _productoTallaColorService.CrearCombinacionAsync(productoTallaColor);
                
                if (productoTallaColorId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "No se pudo crear la combinación",
                        Origen = "ProductoTallaColorController",
                        Metodo = "CrearCombinacion"
                    });
                    return BadRequest(new { mensaje = "No se pudo crear la combinación" });
                }

                productoTallaColor.ProductoTallaColorId = productoTallaColorId;
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Combinación creada exitosamente con ID: {productoTallaColorId}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "CrearCombinacion"
                });
                return CreatedAtAction(nameof(GetCombinacion), new { id = productoTallaColorId }, productoTallaColor);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en POST /api/ProductoTallaColor: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "CrearCombinacion"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una combinación existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCombinacion(int id, [FromBody] ActualizarProductoTallaColorDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PUT /api/ProductoTallaColor/{id}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarCombinacion"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para actualización: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarCombinacion"
                    });
                    return BadRequest(new { mensaje = "ID de combinación inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en actualización de combinación",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarCombinacion"
                    });
                    return BadRequest(ModelState);
                }

                var productoTallaColor = new ProductoTallaColor
                {
                    ProductoTallaColorId = id,
                    ProId = request.ProId,
                    TallaId = request.TallaId,
                    ColorId = request.ColorId,
                    Stock = request.Stock,
                    PrecioOferta = request.PrecioOferta,
                    Sku = request.Sku,
                    FechaActualizacion = DateTime.Now
                };

                var resultado = await _productoTallaColorService.ActualizarCombinacionAsync(productoTallaColor);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar la combinación {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarCombinacion"
                    });
                    return NotFound(new { mensaje = "Combinación no encontrada o no se pudo actualizar" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Combinación {id} actualizada exitosamente",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarCombinacion"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PUT /api/ProductoTallaColor/{id}: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarCombinacion"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una combinación
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCombinacion(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud DELETE /api/ProductoTallaColor/{id}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "EliminarCombinacion"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para eliminación: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "EliminarCombinacion"
                    });
                    return BadRequest(new { mensaje = "ID de combinación inválido" });
                }

                var resultado = await _productoTallaColorService.EliminarCombinacionAsync(id);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo eliminar la combinación {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "EliminarCombinacion"
                    });
                    return NotFound(new { mensaje = "Combinación no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Combinación {id} eliminada exitosamente",
                    Origen = "ProductoTallaColorController",
                    Metodo = "EliminarCombinacion"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en DELETE /api/ProductoTallaColor/{id}: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "EliminarCombinacion"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza el stock de una combinación
        /// </summary>
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> ActualizarStock(int id, [FromBody] ActualizarStockProductoTallaColorDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/ProductoTallaColor/{id}/stock",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarStock"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarStock"
                    });
                    return BadRequest(new { mensaje = "ID de combinación inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en actualización de stock",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarStock"
                    });
                    return BadRequest(ModelState);
                }

                var resultado = await _productoTallaColorService.ActualizarStockAsync(id, request.NuevoStock);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar el stock de la combinación {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarStock"
                    });
                    return NotFound(new { mensaje = "Combinación no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Stock de combinación {id} actualizado a {request.NuevoStock}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarStock"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/ProductoTallaColor/{id}/stock: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarStock"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza el precio de oferta de una combinación
        /// </summary>
        [HttpPatch("{id}/precio-oferta")]
        public async Task<IActionResult> ActualizarPrecioOferta(int id, [FromBody] ActualizarPrecioOfertaProductoTallaColorDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/ProductoTallaColor/{id}/precio-oferta",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarPrecioOferta"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarPrecioOferta"
                    });
                    return BadRequest(new { mensaje = "ID de combinación inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en actualización de precio de oferta",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarPrecioOferta"
                    });
                    return BadRequest(ModelState);
                }

                var resultado = await _productoTallaColorService.ActualizarPrecioOfertaAsync(id, request.PrecioOferta);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar el precio de oferta de la combinación {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ActualizarPrecioOferta"
                    });
                    return NotFound(new { mensaje = "Combinación no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Precio de oferta de combinación {id} actualizado a {request.PrecioOferta}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarPrecioOferta"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/ProductoTallaColor/{id}/precio-oferta: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ActualizarPrecioOferta"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Reduce el stock de una combinación
        /// </summary>
        [HttpPatch("{id}/reducir-stock")]
        public async Task<IActionResult> ReducirStock(int id, [FromBody] ReducirStockProductoTallaColorDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/ProductoTallaColor/{id}/reducir-stock",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ReducirStock"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ReducirStock"
                    });
                    return BadRequest(new { mensaje = "ID de combinación inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en reducción de stock",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ReducirStock"
                    });
                    return BadRequest(ModelState);
                }

                var resultado = await _productoTallaColorService.ReducirStockAsync(id, request.Cantidad);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo reducir el stock de la combinación {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "ReducirStock"
                    });
                    return BadRequest(new { mensaje = "No se pudo reducir el stock" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Stock de combinación {id} reducido en {request.Cantidad}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ReducirStock"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/ProductoTallaColor/{id}/reducir-stock: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "ReducirStock"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Aumenta el stock de una combinación
        /// </summary>
        [HttpPatch("{id}/aumentar-stock")]
        public async Task<IActionResult> AumentarStock(int id, [FromBody] AumentarStockProductoTallaColorDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/ProductoTallaColor/{id}/aumentar-stock",
                    Origen = "ProductoTallaColorController",
                    Metodo = "AumentarStock"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "AumentarStock"
                    });
                    return BadRequest(new { mensaje = "ID de combinación inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en aumento de stock",
                        Origen = "ProductoTallaColorController",
                        Metodo = "AumentarStock"
                    });
                    return BadRequest(ModelState);
                }

                var resultado = await _productoTallaColorService.AumentarStockAsync(id, request.Cantidad);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo aumentar el stock de la combinación {id}",
                        Origen = "ProductoTallaColorController",
                        Metodo = "AumentarStock"
                    });
                    return BadRequest(new { mensaje = "No se pudo aumentar el stock" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Stock de combinación {id} aumentado en {request.Cantidad}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "AumentarStock"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/ProductoTallaColor/{id}/aumentar-stock: {ex.Message}",
                    Origen = "ProductoTallaColorController",
                    Metodo = "AumentarStock"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }
    }
} 