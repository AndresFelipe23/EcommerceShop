using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Ecommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _inventarioService;
        private readonly ILogRepository _logRepository;

        public InventarioController(IInventarioService inventarioService, ILogRepository logRepository)
        {
            _inventarioService = inventarioService;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene todos los movimientos de inventario
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var inventarios = await _inventarioService.ListarAsync();
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Listado de inventario exitoso. Total: {inventarios.Count()}",
                    Origen = "InventarioController",
                    Metodo = "Listar",
                    DatosSalida = $"Total movimientos: {inventarios.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(inventarios);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar inventario: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "Listar",
                    DatosEntrada = "Sin parámetros",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar inventario" });
            }
        }

        /// <summary>
        /// Obtiene un movimiento de inventario por su ID
        /// </summary>
        [HttpGet("{inventarioId}")]
        public async Task<IActionResult> ObtenerPorId(int inventarioId)
        {
            try
            {
                if (inventarioId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener inventario con ID inválido",
                        Origen = "InventarioController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"InventarioId: {inventarioId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del inventario debe ser mayor a 0" });
                }

                var inventario = await _inventarioService.ObtenerPorIdAsync(inventarioId);
                
                if (inventario == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"Inventario con ID {inventarioId} no encontrado",
                        Origen = "InventarioController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"InventarioId: {inventarioId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"Inventario con ID {inventarioId} no encontrado" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Inventario con ID {inventarioId} obtenido exitosamente",
                    Origen = "InventarioController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"InventarioId: {inventarioId}",
                    DatosSalida = $"Producto: {inventario.ProductoTallaColorId}, Tipo: {inventario.TipoMovimiento}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(inventario);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener inventario con ID {inventarioId}: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"InventarioId: {inventarioId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el inventario" });
            }
        }

        /// <summary>
        /// Obtiene los movimientos de inventario por producto
        /// </summary>
        [HttpGet("producto/{proId}")]
        public async Task<IActionResult> ListarPorProducto(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de listar inventario con ID de producto inválido",
                        Origen = "InventarioController",
                        Metodo = "ListarPorProducto",
                        DatosEntrada = $"ProId: {proId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del producto debe ser mayor a 0" });
                }

                var inventarios = await _inventarioService.ListarPorProductoAsync(proId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Inventario del producto {proId} listado exitosamente. Total: {inventarios.Count()}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorProducto",
                    DatosEntrada = $"ProId: {proId}",
                    DatosSalida = $"Total movimientos: {inventarios.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(inventarios);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar inventario del producto {proId}: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorProducto",
                    DatosEntrada = $"ProId: {proId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar el inventario del producto" });
            }
        }

        /// <summary>
        /// Obtiene los movimientos de inventario por producto-talla-color
        /// </summary>
        [HttpGet("producto-talla-color/{productoTallaColorId}")]
        public async Task<IActionResult> ListarPorProductoTallaColor(int productoTallaColorId)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de listar inventario con ID de producto-talla-color inválido",
                        Origen = "InventarioController",
                        Metodo = "ListarPorProductoTallaColor",
                        DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del producto-talla-color debe ser mayor a 0" });
                }

                var inventarios = await _inventarioService.ListarPorProductoTallaColorAsync(productoTallaColorId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Inventario del producto-talla-color {productoTallaColorId} listado exitosamente. Total: {inventarios.Count()}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorProductoTallaColor",
                    DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                    DatosSalida = $"Total movimientos: {inventarios.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(inventarios);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar inventario del producto-talla-color {productoTallaColorId}: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorProductoTallaColor",
                    DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar el inventario del producto-talla-color" });
            }
        }

        /// <summary>
        /// Obtiene el stock actual de un producto-talla-color
        /// </summary>
        [HttpGet("stock/{productoTallaColorId}")]
        public async Task<IActionResult> ObtenerStockActual(int productoTallaColorId)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener stock con ID de producto-talla-color inválido",
                        Origen = "InventarioController",
                        Metodo = "ObtenerStockActual",
                        DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del producto-talla-color debe ser mayor a 0" });
                }

                var stock = await _inventarioService.ObtenerStockActualAsync(productoTallaColorId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Stock actual del producto-talla-color {productoTallaColorId} obtenido exitosamente: {stock}",
                    Origen = "InventarioController",
                    Metodo = "ObtenerStockActual",
                    DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                    DatosSalida = $"Stock: {stock}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { productoTallaColorId, stockActual = stock });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener stock del producto-talla-color {productoTallaColorId}: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ObtenerStockActual",
                    DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el stock" });
            }
        }

        /// <summary>
        /// Obtiene el resumen de inventario de un producto-talla-color
        /// </summary>
        [HttpGet("resumen/{productoTallaColorId}")]
        public async Task<IActionResult> ObtenerResumen(int productoTallaColorId)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener resumen con ID de producto-talla-color inválido",
                        Origen = "InventarioController",
                        Metodo = "ObtenerResumen",
                        DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del producto-talla-color debe ser mayor a 0" });
                }

                var resumen = await _inventarioService.ObtenerResumenInventarioAsync(productoTallaColorId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Resumen de inventario del producto-talla-color {productoTallaColorId} obtenido exitosamente",
                    Origen = "InventarioController",
                    Metodo = "ObtenerResumen",
                    DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener resumen del producto-talla-color {productoTallaColorId}: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ObtenerResumen",
                    DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el resumen" });
            }
        }

        /// <summary>
        /// Lista movimientos por tipo
        /// </summary>
        [HttpGet("tipo/{tipoMovimiento}")]
        public async Task<IActionResult> ListarPorTipo(string tipoMovimiento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tipoMovimiento))
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de listar inventario con tipo de movimiento vacío",
                        Origen = "InventarioController",
                        Metodo = "ListarPorTipo",
                        DatosEntrada = $"TipoMovimiento: {tipoMovimiento}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El tipo de movimiento es requerido" });
                }

                var inventarios = await _inventarioService.ListarPorTipoMovimientoAsync(tipoMovimiento);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Inventario por tipo {tipoMovimiento} listado exitosamente. Total: {inventarios.Count()}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorTipo",
                    DatosEntrada = $"TipoMovimiento: {tipoMovimiento}",
                    DatosSalida = $"Total movimientos: {inventarios.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(inventarios);
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al listar por tipo: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorTipo",
                    DatosEntrada = $"TipoMovimiento: {tipoMovimiento}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar inventario por tipo {tipoMovimiento}: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorTipo",
                    DatosEntrada = $"TipoMovimiento: {tipoMovimiento}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar por tipo" });
            }
        }

        /// <summary>
        /// Lista movimientos por rango de fechas
        /// </summary>
        [HttpGet("fecha")]
        public async Task<IActionResult> ListarPorFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                var inventarios = await _inventarioService.ListarPorFechaAsync(fechaInicio, fechaFin);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Inventario por fecha listado exitosamente. Total: {inventarios.Count()}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorFecha",
                    DatosEntrada = $"FechaInicio: {fechaInicio}, FechaFin: {fechaFin}",
                    DatosSalida = $"Total movimientos: {inventarios.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(inventarios);
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al listar por fecha: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorFecha",
                    DatosEntrada = $"FechaInicio: {fechaInicio}, FechaFin: {fechaFin}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar inventario por fecha: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "ListarPorFecha",
                    DatosEntrada = $"FechaInicio: {fechaInicio}, FechaFin: {fechaFin}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar por fecha" });
            }
        }

        /// <summary>
        /// Crea un nuevo movimiento de inventario
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] InventarioCreateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de crear inventario con datos nulos",
                        Origen = "InventarioController",
                        Metodo = "Crear",
                        DatosEntrada = "Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos del inventario son requeridos" });
                }

                var inventario = new Inventario
                {
                    ProductoTallaColorId = request.ProductoTallaColorId,
                    TipoMovimiento = request.TipoMovimiento,
                    Cantidad = request.Cantidad,
                    Descripcion = request.Descripcion,
                    FechaMovimiento = DateTime.Now,
                    UsuId = request.UsuId
                };

                var inventarioId = await _inventarioService.CrearAsync(inventario);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Inventario creado exitosamente con ID {inventarioId}",
                    Origen = "InventarioController",
                    Metodo = "Crear",
                    DatosEntrada = $"Producto: {request.ProductoTallaColorId}, Tipo: {request.TipoMovimiento}, Cantidad: {request.Cantidad}",
                    DatosSalida = $"InventarioId: {inventarioId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(ObtenerPorId), new { inventarioId }, 
                    new { mensaje = "Inventario creado exitosamente", inventarioId });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al crear inventario: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "Crear",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Tipo: {request?.TipoMovimiento}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al crear inventario: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "Crear",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Tipo: {request?.TipoMovimiento}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al crear el inventario" });
            }
        }

        /// <summary>
        /// Registra una entrada de inventario
        /// </summary>
        [HttpPost("entrada")]
        public async Task<IActionResult> RegistrarEntrada([FromBody] RegistrarMovimientoRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de registrar entrada con datos nulos",
                        Origen = "InventarioController",
                        Metodo = "RegistrarEntrada",
                        DatosEntrada = "Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos son requeridos" });
                }

                var resultado = await _inventarioService.RegistrarEntradaAsync(
                    request.ProductoTallaColorId, 
                    request.Cantidad, 
                    request.Descripcion, 
                    request.UsuId
                );
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Entrada de inventario registrada exitosamente para producto {request.ProductoTallaColorId}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarEntrada",
                    DatosEntrada = $"Producto: {request.ProductoTallaColorId}, Cantidad: {request.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                var stockActual = await _inventarioService.ObtenerStockActualAsync(request.ProductoTallaColorId);
                return Ok(new RegistrarMovimientoResponse 
                { 
                    Registrado = true, 
                    Mensaje = "Entrada de inventario registrada exitosamente",
                    InventarioId = null, // Los métodos devuelven bool, no int
                    StockActual = (int)stockActual
                });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al registrar entrada: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarEntrada",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al registrar entrada de inventario: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarEntrada",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al registrar la entrada" });
            }
        }

        /// <summary>
        /// Registra una salida de inventario
        /// </summary>
        [HttpPost("salida")]
        public async Task<IActionResult> RegistrarSalida([FromBody] RegistrarMovimientoRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de registrar salida con datos nulos",
                        Origen = "InventarioController",
                        Metodo = "RegistrarSalida",
                        DatosEntrada = "Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos son requeridos" });
                }

                var resultado = await _inventarioService.RegistrarSalidaAsync(
                    request.ProductoTallaColorId, 
                    request.Cantidad, 
                    request.Descripcion, 
                    request.UsuId
                );
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Salida de inventario registrada exitosamente para producto {request.ProductoTallaColorId}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarSalida",
                    DatosEntrada = $"Producto: {request.ProductoTallaColorId}, Cantidad: {request.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                var stockActual = await _inventarioService.ObtenerStockActualAsync(request.ProductoTallaColorId);
                return Ok(new RegistrarMovimientoResponse 
                { 
                    Registrado = true, 
                    Mensaje = "Salida de inventario registrada exitosamente",
                    InventarioId = null, // Los métodos devuelven bool, no int
                    StockActual = (int)stockActual
                });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al registrar salida: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarSalida",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al registrar salida: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarSalida",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al registrar salida de inventario: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarSalida",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al registrar la salida" });
            }
        }

        /// <summary>
        /// Registra una venta en el inventario
        /// </summary>
        [HttpPost("venta")]
        public async Task<IActionResult> RegistrarVenta([FromBody] RegistrarVentaRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de registrar venta con datos nulos",
                        Origen = "InventarioController",
                        Metodo = "RegistrarVenta",
                        DatosEntrada = "Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos son requeridos" });
                }

                var resultado = await _inventarioService.RegistrarVentaAsync(
                    request.ProductoTallaColorId, 
                    request.Cantidad, 
                    request.UsuId
                );
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Venta registrada exitosamente para producto {request.ProductoTallaColorId}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarVenta",
                    DatosEntrada = $"Producto: {request.ProductoTallaColorId}, Cantidad: {request.Cantidad}, Usuario: {request.UsuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                var stockActual = await _inventarioService.ObtenerStockActualAsync(request.ProductoTallaColorId);
                return Ok(new RegistrarVentaResponse 
                { 
                    Registrado = true, 
                    Mensaje = "Venta registrada exitosamente",
                    InventarioId = null, // Los métodos devuelven bool, no int
                    StockActual = (int)stockActual
                });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al registrar venta: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarVenta",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al registrar venta: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarVenta",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al registrar venta: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "RegistrarVenta",
                    DatosEntrada = $"Producto: {request?.ProductoTallaColorId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al registrar la venta" });
            }
        }

        /// <summary>
        /// Verifica si hay stock disponible
        /// </summary>
        [HttpGet("verificar-stock/{productoTallaColorId}")]
        public async Task<IActionResult> VerificarStock(int productoTallaColorId, [FromQuery] int cantidad)
        {
            try
            {
                if (productoTallaColorId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de verificar stock con ID de producto-talla-color inválido",
                        Origen = "InventarioController",
                        Metodo = "VerificarStock",
                        DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}, Cantidad: {cantidad}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del producto-talla-color debe ser mayor a 0" });
                }

                if (cantidad <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de verificar stock con cantidad inválida",
                        Origen = "InventarioController",
                        Metodo = "VerificarStock",
                        DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}, Cantidad: {cantidad}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "La cantidad debe ser mayor a 0" });
                }

                var disponible = await _inventarioService.VerificarStockDisponibleAsync(productoTallaColorId, cantidad);
                var stockActual = await _inventarioService.ObtenerStockActualAsync(productoTallaColorId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Stock verificado para producto {productoTallaColorId}. Disponible: {disponible}",
                    Origen = "InventarioController",
                    Metodo = "VerificarStock",
                    DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}, Cantidad: {cantidad}",
                    DatosSalida = $"Disponible: {disponible}, StockActual: {stockActual}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new VerificarStockResponse 
                { 
                    ProductoTallaColorId = productoTallaColorId,
                    CantidadSolicitada = cantidad,
                    StockActual = (int)stockActual,
                    Disponible = disponible,
                    Mensaje = disponible ? "Stock disponible" : "Stock insuficiente"
                });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al verificar stock del producto {productoTallaColorId}: {ex.Message}",
                    Origen = "InventarioController",
                    Metodo = "VerificarStock",
                    DatosEntrada = $"ProductoTallaColorId: {productoTallaColorId}, Cantidad: {cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al verificar el stock" });
            }
        }
    }
} 