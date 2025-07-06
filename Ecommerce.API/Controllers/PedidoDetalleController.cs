using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Ecommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Models = Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoDetalleController : ControllerBase
    {
        private readonly IPedidoDetalleService _pedidoDetalleService;
        private readonly ILogRepository _logRepository;

        public PedidoDetalleController(IPedidoDetalleService pedidoDetalleService, ILogRepository logRepository)
        {
            _pedidoDetalleService = pedidoDetalleService;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene todos los detalles de pedidos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var detalles = await _pedidoDetalleService.ListarAsync();
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Listado de detalles de pedidos exitoso. Total: {detalles.Count()}",
                    Origen = "PedidoDetalleController",
                    Metodo = "Listar",
                    DatosSalida = $"Total detalles: {detalles.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(detalles);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar detalles de pedidos: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "Listar",
                    DatosEntrada = "Sin parámetros",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar detalles de pedidos" });
            }
        }

        /// <summary>
        /// Obtiene un detalle de pedido por su ID
        /// </summary>
        [HttpGet("{pedidoDetalleId}")]
        public async Task<IActionResult> ObtenerPorId(int pedidoDetalleId)
        {
            try
            {
                if (pedidoDetalleId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener detalle de pedido con ID inválido",
                        Origen = "PedidoDetalleController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del detalle del pedido debe ser mayor a 0" });
                }

                var detalle = await _pedidoDetalleService.ObtenerPorIdAsync(pedidoDetalleId);
                
                if (detalle == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"Detalle de pedido con ID {pedidoDetalleId} no encontrado",
                        Origen = "PedidoDetalleController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"Detalle de pedido con ID {pedidoDetalleId} no encontrado" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Detalle de pedido con ID {pedidoDetalleId} obtenido exitosamente",
                    Origen = "PedidoDetalleController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
                    DatosSalida = $"Pedido: {detalle.PedidoId}, Producto: {detalle.ProductoTallaColorId}, Cantidad: {detalle.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(detalle);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener detalle de pedido con ID {pedidoDetalleId}: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el detalle del pedido" });
            }
        }

        /// <summary>
        /// Obtiene los detalles de un pedido específico
        /// </summary>
        [HttpGet("pedido/{pedidoId}")]
        public async Task<IActionResult> ListarPorPedido(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de listar detalles con ID de pedido inválido",
                        Origen = "PedidoDetalleController",
                        Metodo = "ListarPorPedido",
                        DatosEntrada = $"PedidoId: {pedidoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                var detalles = await _pedidoDetalleService.ListarPorPedidoAsync(pedidoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Detalles del pedido {pedidoId} listados exitosamente. Total: {detalles.Count()}",
                    Origen = "PedidoDetalleController",
                    Metodo = "ListarPorPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    DatosSalida = $"Total detalles: {detalles.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(detalles);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar detalles del pedido {pedidoId}: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "ListarPorPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar los detalles del pedido" });
            }
        }

        /// <summary>
        /// Obtiene el resumen de un pedido
        /// </summary>
        [HttpGet("resumen/{pedidoId}")]
        public async Task<IActionResult> ObtenerResumen(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener resumen con ID de pedido inválido",
                        Origen = "PedidoDetalleController",
                        Metodo = "ObtenerResumen",
                        DatosEntrada = $"PedidoId: {pedidoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                var resumen = await _pedidoDetalleService.ObtenerResumenPedidoAsync(pedidoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Resumen del pedido {pedidoId} obtenido exitosamente",
                    Origen = "PedidoDetalleController",
                    Metodo = "ObtenerResumen",
                    DatosEntrada = $"PedidoId: {pedidoId}",
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
                    Mensaje = $"Error al obtener resumen del pedido {pedidoId}: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "ObtenerResumen",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el resumen del pedido" });
            }
        }

        /// <summary>
        /// Crea un nuevo detalle de pedido
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearPedidoDetalleDto pedidoDetalleDto)
        {
            try
            {
                if (pedidoDetalleDto == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de crear detalle de pedido con datos nulos",
                        Origen = "PedidoDetalleController",
                        Metodo = "Crear",
                        DatosEntrada = "PedidoDetalleDto: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos del detalle del pedido son requeridos" });
                }

                var pedidoDetalle = new PedidoDetalle
                {
                    PedidoId = pedidoDetalleDto.PedidoId,
                    ProductoTallaColorId = pedidoDetalleDto.ProductoTallaColorId,
                    Cantidad = pedidoDetalleDto.Cantidad,
                    PrecioUnitario = pedidoDetalleDto.PrecioUnitario,
                    Subtotal = pedidoDetalleDto.Cantidad * pedidoDetalleDto.PrecioUnitario
                };

                var pedidoDetalleId = await _pedidoDetalleService.CrearAsync(pedidoDetalle);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Detalle de pedido creado exitosamente con ID {pedidoDetalleId}",
                    Origen = "PedidoDetalleController",
                    Metodo = "Crear",
                    DatosEntrada = $"Pedido: {pedidoDetalleDto.PedidoId}, Producto: {pedidoDetalleDto.ProductoTallaColorId}, Cantidad: {pedidoDetalleDto.Cantidad}",
                    DatosSalida = $"PedidoDetalleId: {pedidoDetalleId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(ObtenerPorId), new { pedidoDetalleId }, 
                    new { mensaje = "Detalle de pedido creado exitosamente", pedidoDetalleId });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al crear detalle de pedido: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "Crear",
                    DatosEntrada = $"Pedido: {pedidoDetalleDto?.PedidoId}, Producto: {pedidoDetalleDto?.ProductoTallaColorId}",
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
                    Mensaje = $"Error al crear detalle de pedido: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "Crear",
                    DatosEntrada = $"Pedido: {pedidoDetalleDto?.PedidoId}, Producto: {pedidoDetalleDto?.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al crear el detalle del pedido" });
            }
        }

        /// <summary>
        /// Agrega un producto a un pedido
        /// </summary>
        [HttpPost("agregar-producto")]
        public async Task<IActionResult> AgregarProducto([FromBody] Ecommerce.API.Models.AgregarProductoRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de agregar producto con datos nulos",
                        Origen = "PedidoDetalleController",
                        Metodo = "AgregarProducto",
                        DatosEntrada = "Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos son requeridos" });
                }

                var resultado = await _pedidoDetalleService.AgregarProductoAsync(
                    request.PedidoId, 
                    request.ProductoTallaColorId, 
                    request.Cantidad, 
                    request.PrecioUnitario
                );
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Producto agregado al pedido {request.PedidoId} exitosamente",
                    Origen = "PedidoDetalleController",
                    Metodo = "AgregarProducto",
                    DatosEntrada = $"Pedido: {request.PedidoId}, Producto: {request.ProductoTallaColorId}, Cantidad: {request.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Producto agregado al pedido exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al agregar producto: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "AgregarProducto",
                    DatosEntrada = $"Pedido: {request?.PedidoId}, Producto: {request?.ProductoTallaColorId}",
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
                    Mensaje = $"Error de operación al agregar producto: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "AgregarProducto",
                    DatosEntrada = $"Pedido: {request?.PedidoId}, Producto: {request?.ProductoTallaColorId}",
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
                    Mensaje = $"Error al agregar producto al pedido: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "AgregarProducto",
                    DatosEntrada = $"Pedido: {request?.PedidoId}, Producto: {request?.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al agregar el producto" });
            }
        }

        /// <summary>
        /// Actualiza la cantidad de un detalle de pedido
        /// </summary>
        [HttpPut("{pedidoDetalleId}/cantidad")]
        public async Task<IActionResult> ActualizarCantidad(int pedidoDetalleId, [FromBody] ActualizarCantidadPedidoRequest request)
        {
            try
            {
                if (pedidoDetalleId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de actualizar cantidad con ID de detalle inválido",
                        Origen = "PedidoDetalleController",
                        Metodo = "ActualizarCantidad",
                        DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}, NuevaCantidad: {request?.NuevaCantidad}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del detalle del pedido debe ser mayor a 0" });
                }

                if (request == null || request.NuevaCantidad <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de actualizar cantidad con datos inválidos",
                        Origen = "PedidoDetalleController",
                        Metodo = "ActualizarCantidad",
                        DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}, Request: {request}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "La nueva cantidad debe ser mayor a 0" });
                }

                var resultado = await _pedidoDetalleService.ActualizarCantidadAsync(pedidoDetalleId, request.NuevaCantidad);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Cantidad del detalle {pedidoDetalleId} actualizada exitosamente a {request.NuevaCantidad}",
                    Origen = "PedidoDetalleController",
                    Metodo = "ActualizarCantidad",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}, NuevaCantidad: {request.NuevaCantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Cantidad actualizada exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al actualizar cantidad: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "ActualizarCantidad",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}, NuevaCantidad: {request?.NuevaCantidad}",
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
                    Mensaje = $"Error de operación al actualizar cantidad: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "ActualizarCantidad",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}, NuevaCantidad: {request?.NuevaCantidad}",
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
                    Mensaje = $"Error al actualizar cantidad del detalle {pedidoDetalleId}: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "ActualizarCantidad",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}, NuevaCantidad: {request?.NuevaCantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al actualizar la cantidad" });
            }
        }

        /// <summary>
        /// Elimina un producto de un pedido
        /// </summary>
        [HttpDelete("{pedidoDetalleId}")]
        public async Task<IActionResult> EliminarProducto(int pedidoDetalleId)
        {
            try
            {
                if (pedidoDetalleId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de eliminar producto con ID de detalle inválido",
                        Origen = "PedidoDetalleController",
                        Metodo = "EliminarProducto",
                        DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del detalle del pedido debe ser mayor a 0" });
                }

                var resultado = await _pedidoDetalleService.EliminarProductoAsync(pedidoDetalleId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Producto del detalle {pedidoDetalleId} eliminado exitosamente",
                    Origen = "PedidoDetalleController",
                    Metodo = "EliminarProducto",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Producto eliminado del pedido exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al eliminar producto: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "EliminarProducto",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
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
                    Mensaje = $"Error de operación al eliminar producto: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "EliminarProducto",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
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
                    Mensaje = $"Error al eliminar producto del detalle {pedidoDetalleId}: {ex.Message}",
                    Origen = "PedidoDetalleController",
                    Metodo = "EliminarProducto",
                    DatosEntrada = $"PedidoDetalleId: {pedidoDetalleId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al eliminar el producto" });
            }
        }
    }
} 