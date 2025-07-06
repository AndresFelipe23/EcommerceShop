using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        private readonly ILogRepository _logRepository;

        public PedidoController(IPedidoService pedidoService, ILogRepository logRepository)
        {
            _pedidoService = pedidoService;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene todos los pedidos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var pedidos = await _pedidoService.ListarAsync();
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Listado de pedidos exitoso. Total: {pedidos.Count()}",
                    Origen = "PedidoController",
                    Metodo = "Listar",
                    DatosSalida = $"Total pedidos: {pedidos.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar pedidos: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "Listar",
                    DatosEntrada = "Sin parámetros",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar pedidos" });
            }
        }

        /// <summary>
        /// Obtiene un pedido por su ID
        /// </summary>
        [HttpGet("{pedidoId}")]
        public async Task<IActionResult> ObtenerPorId(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener pedido con ID inválido",
                        Origen = "PedidoController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"PedidoId: {pedidoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                var pedido = await _pedidoService.ObtenerPorIdAsync(pedidoId);
                
                if (pedido == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"Pedido con ID {pedidoId} no encontrado",
                        Origen = "PedidoController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"PedidoId: {pedidoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"Pedido con ID {pedidoId} no encontrado" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedido con ID {pedidoId} obtenido exitosamente",
                    Origen = "PedidoController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    DatosSalida = $"Usuario: {pedido.UsuId}, Estado: {pedido.Estado}, Total: {pedido.Total}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener pedido con ID {pedidoId}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el pedido" });
            }
        }

        /// <summary>
        /// Obtiene los pedidos de un usuario específico
        /// </summary>
        [HttpGet("usuario/{usuId}")]
        public async Task<IActionResult> ListarPorUsuario(int usuId)
        {
            try
            {
                if (usuId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de listar pedidos con ID de usuario inválido",
                        Origen = "PedidoController",
                        Metodo = "ListarPorUsuario",
                        DatosEntrada = $"UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del usuario debe ser mayor a 0" });
                }

                var pedidos = await _pedidoService.ListarPorUsuarioAsync(usuId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedidos del usuario {usuId} listados exitosamente. Total: {pedidos.Count()}",
                    Origen = "PedidoController",
                    Metodo = "ListarPorUsuario",
                    DatosEntrada = $"UsuId: {usuId}",
                    DatosSalida = $"Total pedidos: {pedidos.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar pedidos del usuario {usuId}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ListarPorUsuario",
                    DatosEntrada = $"UsuId: {usuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar los pedidos del usuario" });
            }
        }

        /// <summary>
        /// Obtiene los pedidos por estado
        /// </summary>
        [HttpGet("estado/{estado}")]
        public async Task<IActionResult> ListarPorEstado(string estado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(estado))
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de listar pedidos con estado vacío",
                        Origen = "PedidoController",
                        Metodo = "ListarPorEstado",
                        DatosEntrada = $"Estado: {estado}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El estado es requerido" });
                }

                var pedidos = await _pedidoService.ListarPorEstadoAsync(estado);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedidos con estado {estado} listados exitosamente. Total: {pedidos.Count()}",
                    Origen = "PedidoController",
                    Metodo = "ListarPorEstado",
                    DatosEntrada = $"Estado: {estado}",
                    DatosSalida = $"Total pedidos: {pedidos.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(pedidos);
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al listar por estado: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ListarPorEstado",
                    DatosEntrada = $"Estado: {estado}",
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
                    Mensaje = $"Error al listar pedidos por estado {estado}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ListarPorEstado",
                    DatosEntrada = $"Estado: {estado}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar por estado" });
            }
        }

        /// <summary>
        /// Obtiene los pedidos por rango de fechas
        /// </summary>
        [HttpGet("fecha")]
        public async Task<IActionResult> ListarPorFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                var pedidos = await _pedidoService.ListarPorFechaAsync(fechaInicio, fechaFin);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedidos por fecha listados exitosamente. Total: {pedidos.Count()}",
                    Origen = "PedidoController",
                    Metodo = "ListarPorFecha",
                    DatosEntrada = $"FechaInicio: {fechaInicio}, FechaFin: {fechaFin}",
                    DatosSalida = $"Total pedidos: {pedidos.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(pedidos);
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al listar por fecha: {ex.Message}",
                    Origen = "PedidoController",
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
                    Mensaje = $"Error al listar pedidos por fecha: {ex.Message}",
                    Origen = "PedidoController",
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
        /// Obtiene las estadísticas de pedidos
        /// </summary>
        [HttpGet("estadisticas")]
        public async Task<IActionResult> ObtenerEstadisticas()
        {
            try
            {
                var estadisticas = await _pedidoService.ObtenerEstadisticasAsync();
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = "Estadísticas de pedidos obtenidas exitosamente",
                    Origen = "PedidoController",
                    Metodo = "ObtenerEstadisticas",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener estadísticas de pedidos: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ObtenerEstadisticas",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener estadísticas" });
            }
        }

        /// <summary>
        /// Obtiene los pedidos más recientes
        /// </summary>
        [HttpGet("recientes")]
        public async Task<IActionResult> ListarRecientes([FromQuery] int cantidad = 10)
        {
            try
            {
                if (cantidad <= 0 || cantidad > 100)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de listar pedidos recientes con cantidad inválida",
                        Origen = "PedidoController",
                        Metodo = "ListarRecientes",
                        DatosEntrada = $"Cantidad: {cantidad}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "La cantidad debe estar entre 1 y 100" });
                }

                var pedidos = await _pedidoService.ListarPedidosRecientesAsync(cantidad);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedidos recientes listados exitosamente. Cantidad: {pedidos.Count()}",
                    Origen = "PedidoController",
                    Metodo = "ListarRecientes",
                    DatosEntrada = $"Cantidad: {cantidad}",
                    DatosSalida = $"Total pedidos: {pedidos.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar pedidos recientes: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ListarRecientes",
                    DatosEntrada = $"Cantidad: {cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar pedidos recientes" });
            }
        }

        /// <summary>
        /// Obtiene un pedido completo con sus detalles
        /// </summary>
        [HttpGet("completo/{pedidoId}")]
        public async Task<IActionResult> ObtenerCompleto(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener pedido completo con ID inválido",
                        Origen = "PedidoController",
                        Metodo = "ObtenerCompleto",
                        DatosEntrada = $"PedidoId: {pedidoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                var pedidoCompleto = await _pedidoService.ObtenerPedidoCompletoAsync(pedidoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedido completo con ID {pedidoId} obtenido exitosamente",
                    Origen = "PedidoController",
                    Metodo = "ObtenerCompleto",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(pedidoCompleto);
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Pedido no encontrado: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ObtenerCompleto",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener pedido completo con ID {pedidoId}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ObtenerCompleto",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el pedido completo" });
            }
        }

        /// <summary>
        /// Crea un nuevo pedido
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearPedidoDto pedidoDto)
        {
            try
            {
                if (pedidoDto == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de crear pedido con datos nulos",
                        Origen = "PedidoController",
                        Metodo = "Crear",
                        DatosEntrada = "PedidoDto: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos del pedido son requeridos" });
                }

                var pedido = new Pedido
                {
                    UsuId = pedidoDto.UsuId,
                    DireccionEnvioId = pedidoDto.DireccionEnvioId,
                    Estado = pedidoDto.Estado,
                    MetodoPago = pedidoDto.MetodoPago,
                    Total = pedidoDto.Total,
                    Observaciones = pedidoDto.Observaciones,
                    FechaPedido = DateTime.Now
                };

                var pedidoId = await _pedidoService.CrearAsync(pedido);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedido creado exitosamente con ID {pedidoId}",
                    Origen = "PedidoController",
                    Metodo = "Crear",
                    DatosEntrada = $"Usuario: {pedidoDto.UsuId}, Total: {pedidoDto.Total}, Método: {pedidoDto.MetodoPago}",
                    DatosSalida = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(ObtenerPorId), new { pedidoId }, 
                    new { mensaje = "Pedido creado exitosamente", pedidoId });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al crear pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "Crear",
                    DatosEntrada = $"Usuario: {pedidoDto?.UsuId}, Total: {pedidoDto?.Total}",
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
                    Mensaje = $"Error al crear pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "Crear",
                    DatosEntrada = $"Usuario: {pedidoDto?.UsuId}, Total: {pedidoDto?.Total}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al crear el pedido" });
            }
        }

        /// <summary>
        /// Actualiza el estado de un pedido
        /// </summary>
        [HttpPut("{pedidoId}/estado")]
        public async Task<IActionResult> ActualizarEstado(int pedidoId, [FromBody] ActualizarEstadoPedidoDto request)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de actualizar estado con ID de pedido inválido",
                        Origen = "PedidoController",
                        Metodo = "ActualizarEstado",
                        DatosEntrada = $"PedidoId: {pedidoId}, Estado: {request?.Estado}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                if (request == null || string.IsNullOrWhiteSpace(request.Estado))
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de actualizar estado con datos inválidos",
                        Origen = "PedidoController",
                        Metodo = "ActualizarEstado",
                        DatosEntrada = $"PedidoId: {pedidoId}, Request: {request}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El estado es requerido" });
                }

                var resultado = await _pedidoService.ActualizarEstadoAsync(pedidoId, request.Estado);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Estado del pedido {pedidoId} actualizado exitosamente a {request.Estado}",
                    Origen = "PedidoController",
                    Metodo = "ActualizarEstado",
                    DatosEntrada = $"PedidoId: {pedidoId}, Estado: {request.Estado}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Estado actualizado exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al actualizar estado: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ActualizarEstado",
                    DatosEntrada = $"PedidoId: {pedidoId}, Estado: {request?.Estado}",
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
                    Mensaje = $"Error de operación al actualizar estado: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ActualizarEstado",
                    DatosEntrada = $"PedidoId: {pedidoId}, Estado: {request?.Estado}",
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
                    Mensaje = $"Error al actualizar estado del pedido {pedidoId}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ActualizarEstado",
                    DatosEntrada = $"PedidoId: {pedidoId}, Estado: {request?.Estado}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al actualizar el estado" });
            }
        }

        /// <summary>
        /// Cancela un pedido
        /// </summary>
        [HttpPost("{pedidoId}/cancelar")]
        public async Task<IActionResult> CancelarPedido(int pedidoId, [FromBody] CancelarPedidoDto request)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de cancelar pedido con ID inválido",
                        Origen = "PedidoController",
                        Metodo = "CancelarPedido",
                        DatosEntrada = $"PedidoId: {pedidoId}, Motivo: {request?.Motivo}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                if (request == null || string.IsNullOrWhiteSpace(request.Motivo))
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de cancelar pedido sin motivo",
                        Origen = "PedidoController",
                        Metodo = "CancelarPedido",
                        DatosEntrada = $"PedidoId: {pedidoId}, Request: {request}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El motivo de cancelación es requerido" });
                }

                var resultado = await _pedidoService.CancelarPedidoAsync(pedidoId, request.Motivo);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedido {pedidoId} cancelado exitosamente. Motivo: {request.Motivo}",
                    Origen = "PedidoController",
                    Metodo = "CancelarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}, Motivo: {request.Motivo}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Pedido cancelado exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al cancelar pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "CancelarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}, Motivo: {request?.Motivo}",
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
                    Mensaje = $"Error de operación al cancelar pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "CancelarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}, Motivo: {request?.Motivo}",
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
                    Mensaje = $"Error al cancelar pedido {pedidoId}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "CancelarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}, Motivo: {request?.Motivo}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al cancelar el pedido" });
            }
        }

        /// <summary>
        /// Procesa un pedido
        /// </summary>
        [HttpPost("{pedidoId}/procesar")]
        public async Task<IActionResult> ProcesarPedido(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de procesar pedido con ID inválido",
                        Origen = "PedidoController",
                        Metodo = "ProcesarPedido",
                        DatosEntrada = $"PedidoId: {pedidoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                var resultado = await _pedidoService.ProcesarPedidoAsync(pedidoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedido {pedidoId} procesado exitosamente",
                    Origen = "PedidoController",
                    Metodo = "ProcesarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Pedido procesado exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al procesar pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ProcesarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
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
                    Mensaje = $"Error de operación al procesar pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ProcesarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
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
                    Mensaje = $"Error al procesar pedido {pedidoId}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "ProcesarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al procesar el pedido" });
            }
        }

        /// <summary>
        /// Envía un pedido
        /// </summary>
        [HttpPost("{pedidoId}/enviar")]
        public async Task<IActionResult> EnviarPedido(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de enviar pedido con ID inválido",
                        Origen = "PedidoController",
                        Metodo = "EnviarPedido",
                        DatosEntrada = $"PedidoId: {pedidoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                var resultado = await _pedidoService.EnviarPedidoAsync(pedidoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedido {pedidoId} enviado exitosamente",
                    Origen = "PedidoController",
                    Metodo = "EnviarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Pedido enviado exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al enviar pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "EnviarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
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
                    Mensaje = $"Error de operación al enviar pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "EnviarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
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
                    Mensaje = $"Error al enviar pedido {pedidoId}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "EnviarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al enviar el pedido" });
            }
        }

        /// <summary>
        /// Marca un pedido como entregado
        /// </summary>
        [HttpPost("{pedidoId}/entregar")]
        public async Task<IActionResult> EntregarPedido(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de entregar pedido con ID inválido",
                        Origen = "PedidoController",
                        Metodo = "EntregarPedido",
                        DatosEntrada = $"PedidoId: {pedidoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del pedido debe ser mayor a 0" });
                }

                var resultado = await _pedidoService.EntregarPedidoAsync(pedidoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Pedido {pedidoId} marcado como entregado exitosamente",
                    Origen = "PedidoController",
                    Metodo = "EntregarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Pedido marcado como entregado exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al entregar pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "EntregarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
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
                    Mensaje = $"Error de operación al entregar pedido: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "EntregarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
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
                    Mensaje = $"Error al entregar pedido {pedidoId}: {ex.Message}",
                    Origen = "PedidoController",
                    Metodo = "EntregarPedido",
                    DatosEntrada = $"PedidoId: {pedidoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al entregar el pedido" });
            }
        }
    }
} 