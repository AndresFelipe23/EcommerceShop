using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ILogService _logService;

        public ProductoController(IProductoService productoService, ILogService logService)
        {
            _productoService = productoService;
            _logService = logService;
        }

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductos()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/Producto",
                    Origen = "ProductoController",
                    Metodo = "GetProductos"
                });
                
                var productos = await _productoService.ListarProductosAsync();
                
                var response = productos.Select(p => new ProductoResponse {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProDescripcion = p.ProDescripcion,
                    ProPrecio = p.ProPrecio,
                    ProImagenPrincipal = p.ProImagenPrincipal,
                    ProGenero = p.ProGenero,
                    ProCategoriaId = p.ProCategoriaId,
                    ProCategoriaNombre = p.ProCategoria?.CatNombre,
                    ProActivo = p.ProActivo,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProFechaActualizacion = p.ProFechaActualizacion
                });
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornados {response.Count()} productos",
                    Origen = "ProductoController",
                    Metodo = "GetProductos"
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetProductos"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponse>> GetProducto(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/Producto/{id}",
                    Origen = "ProductoController",
                    Metodo = "GetProducto"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoController",
                        Metodo = "GetProducto"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var producto = await _productoService.ObtenerProductoAsync(id);
                
                if (producto == null)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"Producto no encontrado: {id}",
                        Origen = "ProductoController",
                        Metodo = "GetProducto"
                    });
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Producto {id} obtenido exitosamente",
                    Origen = "ProductoController",
                    Metodo = "GetProducto"
                });
                var response = new ProductoResponse {
                    ProId = producto.ProId,
                    ProNombre = producto.ProNombre,
                    ProDescripcion = producto.ProDescripcion,
                    ProPrecio = producto.ProPrecio,
                    ProImagenPrincipal = producto.ProImagenPrincipal,
                    ProGenero = producto.ProGenero,
                    ProCategoriaId = producto.ProCategoriaId,
                    ProCategoriaNombre = producto.ProCategoria?.CatNombre,
                    ProActivo = producto.ProActivo,
                    ProFechaCreacion = producto.ProFechaCreacion,
                    ProFechaActualizacion = producto.ProFechaActualizacion
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/{id}: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        [HttpGet("categoria/{catId}")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductosPorCategoria(int catId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/Producto/categoria/{catId}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorCategoria"
                });
                
                if (catId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de categoría inválido: {catId}",
                        Origen = "ProductoController",
                        Metodo = "GetProductosPorCategoria"
                    });
                    return BadRequest(new { mensaje = "ID de categoría inválido" });
                }

                var productos = await _productoService.ListarPorCategoriaAsync(catId);
                
                var response = productos.Select(p => new ProductoResponse {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProDescripcion = p.ProDescripcion,
                    ProPrecio = p.ProPrecio,
                    ProImagenPrincipal = p.ProImagenPrincipal,
                    ProGenero = p.ProGenero,
                    ProCategoriaId = p.ProCategoriaId,
                    ProCategoriaNombre = p.ProCategoria?.CatNombre,
                    ProActivo = p.ProActivo,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProFechaActualizacion = p.ProFechaActualizacion
                });
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornados {response.Count()} productos de categoría {catId}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorCategoria"
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/categoria/{catId}: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorCategoria"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene productos por género
        /// </summary>
        [HttpGet("genero/{genero}")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductosPorGenero(string genero)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/Producto/genero/{genero}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorGenero"
                });
                
                if (string.IsNullOrWhiteSpace(genero))
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Género no especificado",
                        Origen = "ProductoController",
                        Metodo = "GetProductosPorGenero"
                    });
                    return BadRequest(new { mensaje = "Género no especificado" });
                }

                var productos = await _productoService.ListarPorGeneroAsync(genero);
                
                var response = productos.Select(p => new ProductoResponse {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProDescripcion = p.ProDescripcion,
                    ProPrecio = p.ProPrecio,
                    ProImagenPrincipal = p.ProImagenPrincipal,
                    ProGenero = p.ProGenero,
                    ProCategoriaId = p.ProCategoriaId,
                    ProCategoriaNombre = p.ProCategoria?.CatNombre,
                    ProActivo = p.ProActivo,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProFechaActualizacion = p.ProFechaActualizacion
                });
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornados {response.Count()} productos de género {genero}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorGenero"
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/genero/{genero}: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorGenero"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene productos activos
        /// </summary>
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductosActivos()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/Producto/activos",
                    Origen = "ProductoController",
                    Metodo = "GetProductosActivos"
                });
                
                var productos = await _productoService.ListarActivosAsync();
                
                var response = productos.Select(p => new ProductoResponse {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProDescripcion = p.ProDescripcion,
                    ProPrecio = p.ProPrecio,
                    ProImagenPrincipal = p.ProImagenPrincipal,
                    ProGenero = p.ProGenero,
                    ProCategoriaId = p.ProCategoriaId,
                    ProCategoriaNombre = p.ProCategoria?.CatNombre,
                    ProActivo = p.ProActivo,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProFechaActualizacion = p.ProFechaActualizacion
                });
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornados {response.Count()} productos activos",
                    Origen = "ProductoController",
                    Metodo = "GetProductosActivos"
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/activos: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosActivos"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        [HttpGet("buscar/{nombre}")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> BuscarProductos(string nombre)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/Producto/buscar/{nombre}",
                    Origen = "ProductoController",
                    Metodo = "BuscarProductos"
                });
                
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Nombre de búsqueda vacío",
                        Origen = "ProductoController",
                        Metodo = "BuscarProductos"
                    });
                    return BadRequest(new { mensaje = "Nombre de búsqueda requerido" });
                }

                var productos = await _productoService.BuscarProductosAsync(nombre);
                
                var response = productos.Select(p => new ProductoResponse {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProDescripcion = p.ProDescripcion,
                    ProPrecio = p.ProPrecio,
                    ProImagenPrincipal = p.ProImagenPrincipal,
                    ProGenero = p.ProGenero,
                    ProCategoriaId = p.ProCategoriaId,
                    ProCategoriaNombre = p.ProCategoria?.CatNombre,
                    ProActivo = p.ProActivo,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProFechaActualizacion = p.ProFechaActualizacion
                });
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Búsqueda '{nombre}' retornó {response.Count()} productos",
                    Origen = "ProductoController",
                    Metodo = "BuscarProductos"
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/buscar/{nombre}: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "BuscarProductos"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene productos por rango de precio
        /// </summary>
        [HttpGet("precio")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductosPorPrecio(
            [FromQuery] decimal precioMin, 
            [FromQuery] decimal precioMax)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/Producto/precio?precioMin={precioMin}&precioMax={precioMax}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorPrecio"
                });
                
                if (precioMin < 0 || precioMax < 0 || precioMin > precioMax)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"Rango de precios inválido: {precioMin}-{precioMax}",
                        Origen = "ProductoController",
                        Metodo = "GetProductosPorPrecio"
                    });
                    return BadRequest(new { mensaje = "Rango de precios inválido" });
                }

                var productos = await _productoService.ListarPorRangoPrecioAsync(precioMin, precioMax);
                
                var response = productos.Select(p => new ProductoResponse {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProDescripcion = p.ProDescripcion,
                    ProPrecio = p.ProPrecio,
                    ProImagenPrincipal = p.ProImagenPrincipal,
                    ProGenero = p.ProGenero,
                    ProCategoriaId = p.ProCategoriaId,
                    ProCategoriaNombre = p.ProCategoria?.CatNombre,
                    ProActivo = p.ProActivo,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProFechaActualizacion = p.ProFechaActualizacion
                });
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornados {response.Count()} productos en rango {precioMin}-{precioMax}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorPrecio"
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/precio: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosPorPrecio"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene productos recientes
        /// </summary>
        [HttpGet("recientes")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductosRecientes([FromQuery] int cantidad = 10)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/Producto/recientes?cantidad={cantidad}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosRecientes"
                });
                
                if (cantidad <= 0 || cantidad > 100)
                {
                    cantidad = 10;
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"Cantidad ajustada a 10 (valor original: {cantidad})",
                        Origen = "ProductoController",
                        Metodo = "GetProductosRecientes"
                    });
                }

                var productos = await _productoService.ListarProductosRecientesAsync(cantidad);
                
                var response = productos.Select(p => new ProductoResponse {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProDescripcion = p.ProDescripcion,
                    ProPrecio = p.ProPrecio,
                    ProImagenPrincipal = p.ProImagenPrincipal,
                    ProGenero = p.ProGenero,
                    ProCategoriaId = p.ProCategoriaId,
                    ProCategoriaNombre = p.ProCategoria?.CatNombre,
                    ProActivo = p.ProActivo,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProFechaActualizacion = p.ProFechaActualizacion
                });
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornados {response.Count()} productos recientes",
                    Origen = "ProductoController",
                    Metodo = "GetProductosRecientes"
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/recientes: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosRecientes"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene productos con ofertas
        /// </summary>
        [HttpGet("ofertas")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductosConOfertas()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/Producto/ofertas",
                    Origen = "ProductoController",
                    Metodo = "GetProductosConOfertas"
                });
                
                var productos = await _productoService.ListarConOfertasAsync();
                
                var response = productos.Select(p => new ProductoResponse {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProDescripcion = p.ProDescripcion,
                    ProPrecio = p.ProPrecio,
                    ProImagenPrincipal = p.ProImagenPrincipal,
                    ProGenero = p.ProGenero,
                    ProCategoriaId = p.ProCategoriaId,
                    ProCategoriaNombre = p.ProCategoria?.CatNombre,
                    ProActivo = p.ProActivo,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProFechaActualizacion = p.ProFechaActualizacion
                });
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornados {response.Count()} productos con ofertas",
                    Origen = "ProductoController",
                    Metodo = "GetProductosConOfertas"
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/ofertas: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetProductosConOfertas"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de productos
        /// </summary>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticas()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/Producto/estadisticas",
                    Origen = "ProductoController",
                    Metodo = "GetEstadisticas"
                });
                
                var estadisticas = await _productoService.ObtenerEstadisticasAsync();
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Estadísticas obtenidas exitosamente",
                    Origen = "ProductoController",
                    Metodo = "GetEstadisticas"
                });
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/estadisticas: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetEstadisticas"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el precio promedio de productos
        /// </summary>
        [HttpGet("precio-promedio")]
        public async Task<ActionResult<decimal>> GetPrecioPromedio()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/Producto/precio-promedio",
                    Origen = "ProductoController",
                    Metodo = "GetPrecioPromedio"
                });
                
                var promedio = await _productoService.ObtenerPrecioPromedioAsync();
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Precio promedio obtenido: {promedio}",
                    Origen = "ProductoController",
                    Metodo = "GetPrecioPromedio"
                });
                return Ok(promedio);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/precio-promedio: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "GetPrecioPromedio"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductoResponse>> CrearProducto([FromBody] ProductoRequest request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud POST /api/Producto",
                    Origen = "ProductoController",
                    Metodo = "CrearProducto"
                });
                
                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en creación de producto",
                        Origen = "ProductoController",
                        Metodo = "CrearProducto"
                    });
                    return BadRequest(ModelState);
                }

                var producto = new Producto {
                    ProNombre = request.ProNombre,
                    ProDescripcion = request.ProDescripcion,
                    ProPrecio = request.ProPrecio,
                    ProImagenPrincipal = request.ProImagenPrincipal,
                    ProGenero = request.ProGenero,
                    ProCategoriaId = request.ProCategoriaId,
                    ProActivo = true,
                    ProFechaCreacion = DateTime.Now
                };

                var proId = await _productoService.CrearProductoAsync(producto);
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "No se pudo crear el producto",
                        Origen = "ProductoController",
                        Metodo = "CrearProducto"
                    });
                    return BadRequest(new { mensaje = "No se pudo crear el producto" });
                }

                producto.ProId = proId;
                
                var creado = await _productoService.ObtenerProductoAsync(proId);
                var response = new ProductoResponse {
                    ProId = creado.ProId,
                    ProNombre = creado.ProNombre,
                    ProDescripcion = creado.ProDescripcion,
                    ProPrecio = creado.ProPrecio,
                    ProImagenPrincipal = creado.ProImagenPrincipal,
                    ProGenero = creado.ProGenero,
                    ProCategoriaId = creado.ProCategoriaId,
                    ProCategoriaNombre = creado.ProCategoria?.CatNombre,
                    ProActivo = creado.ProActivo,
                    ProFechaCreacion = creado.ProFechaCreacion,
                    ProFechaActualizacion = creado.ProFechaActualizacion
                };
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Producto creado exitosamente con ID: {proId}",
                    Origen = "ProductoController",
                    Metodo = "CrearProducto"
                });
                return CreatedAtAction(nameof(GetProducto), new { id = response.ProId }, response);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en POST /api/Producto: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "CrearProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ProductoUpdateRequest request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PUT /api/Producto/{id}",
                    Origen = "ProductoController",
                    Metodo = "ActualizarProducto"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para actualización: {id}",
                        Origen = "ProductoController",
                        Metodo = "ActualizarProducto"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en actualización de producto",
                        Origen = "ProductoController",
                        Metodo = "ActualizarProducto"
                    });
                    return BadRequest(ModelState);
                }

                var producto = new Producto {
                    ProId = id,
                    ProNombre = request.ProNombre,
                    ProDescripcion = request.ProDescripcion,
                    ProPrecio = request.ProPrecio,
                    ProImagenPrincipal = request.ProImagenPrincipal,
                    ProGenero = request.ProGenero,
                    ProCategoriaId = request.ProCategoriaId,
                    ProActivo = request.ProActivo,
                    ProFechaActualizacion = DateTime.Now
                };

                var actualizado = await _productoService.ActualizarProductoAsync(producto);
                
                if (!actualizado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar el producto {id}",
                        Origen = "ProductoController",
                        Metodo = "ActualizarProducto"
                    });
                    return NotFound(new { mensaje = "Producto no encontrado o no se pudo actualizar" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Producto {id} actualizado exitosamente",
                    Origen = "ProductoController",
                    Metodo = "ActualizarProducto"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PUT /api/Producto/{id}: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "ActualizarProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud DELETE /api/Producto/{id}",
                    Origen = "ProductoController",
                    Metodo = "EliminarProducto"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para eliminación: {id}",
                        Origen = "ProductoController",
                        Metodo = "EliminarProducto"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var resultado = await _productoService.EliminarProductoAsync(id);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo eliminar el producto {id}",
                        Origen = "ProductoController",
                        Metodo = "EliminarProducto"
                    });
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Producto {id} eliminado exitosamente",
                    Origen = "ProductoController",
                    Metodo = "EliminarProducto"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en DELETE /api/Producto/{id}: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "EliminarProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Activa un producto
        /// </summary>
        [HttpPatch("{id}/activar")]
        public async Task<IActionResult> ActivarProducto(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/Producto/{id}/activar",
                    Origen = "ProductoController",
                    Metodo = "ActivarProducto"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para activación: {id}",
                        Origen = "ProductoController",
                        Metodo = "ActivarProducto"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var resultado = await _productoService.ActivarProductoAsync(id);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo activar el producto {id}",
                        Origen = "ProductoController",
                        Metodo = "ActivarProducto"
                    });
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Producto {id} activado exitosamente",
                    Origen = "ProductoController",
                    Metodo = "ActivarProducto"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/Producto/{id}/activar: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "ActivarProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Desactiva un producto
        /// </summary>
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> DesactivarProducto(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/Producto/{id}/desactivar",
                    Origen = "ProductoController",
                    Metodo = "DesactivarProducto"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para desactivación: {id}",
                        Origen = "ProductoController",
                        Metodo = "DesactivarProducto"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var resultado = await _productoService.DesactivarProductoAsync(id);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo desactivar el producto {id}",
                        Origen = "ProductoController",
                        Metodo = "DesactivarProducto"
                    });
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Producto {id} desactivado exitosamente",
                    Origen = "ProductoController",
                    Metodo = "DesactivarProducto"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/Producto/{id}/desactivar: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "DesactivarProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Verifica la disponibilidad de un producto
        /// </summary>
        [HttpGet("{id}/disponibilidad")]
        public async Task<ActionResult<bool>> VerificarDisponibilidad(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/Producto/{id}/disponibilidad",
                    Origen = "ProductoController",
                    Metodo = "VerificarDisponibilidad"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para verificación: {id}",
                        Origen = "ProductoController",
                        Metodo = "VerificarDisponibilidad"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var disponible = await _productoService.VerificarDisponibilidadAsync(id);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Disponibilidad del producto {id}: {disponible}",
                    Origen = "ProductoController",
                    Metodo = "VerificarDisponibilidad"
                });
                return Ok(disponible);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/Producto/{id}/disponibilidad: {ex.Message}",
                    Origen = "ProductoController",
                    Metodo = "VerificarDisponibilidad"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }
    }

    // Clases de Request
    public class ProductoRequest
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        public string ProNombre { get; set; } = string.Empty;

        public string? ProDescripcion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal ProPrecio { get; set; }

        public string? ProImagenPrincipal { get; set; }

        [StringLength(50, ErrorMessage = "El género no puede exceder 50 caracteres")]
        public string? ProGenero { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de categoría inválido")]
        public int ProCategoriaId { get; set; }
    }

    public class ProductoUpdateRequest
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        public string ProNombre { get; set; } = string.Empty;

        public string? ProDescripcion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal ProPrecio { get; set; }

        public string? ProImagenPrincipal { get; set; }

        [StringLength(50, ErrorMessage = "El género no puede exceder 50 caracteres")]
        public string? ProGenero { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de categoría inválido")]
        public int ProCategoriaId { get; set; }

        public bool ProActivo { get; set; } = true;
    }
} 