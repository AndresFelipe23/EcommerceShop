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
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;
        private readonly ILogRepository _logRepository;

        public CarritoController(ICarritoService carritoService, ILogRepository logRepository)
        {
            _carritoService = carritoService;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene todos los carritos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var carritos = await _carritoService.ListarAsync();
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Listado de carritos exitoso. Total: {carritos.Count()}",
                    Origen = "CarritoController",
                    Metodo = "Listar",
                    DatosSalida = $"Total carritos: {carritos.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(carritos);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar carritos: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Listar",
                    DatosEntrada = "Sin parámetros",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar carritos" });
            }
        }

        /// <summary>
        /// Obtiene un carrito por su ID
        /// </summary>
        [HttpGet("{carritoId}")]
        public async Task<IActionResult> ObtenerPorId(int carritoId)
        {
            try
            {
                if (carritoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener carrito con ID inválido",
                        Origen = "CarritoController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del carrito debe ser mayor a 0" });
                }

                var carrito = await _carritoService.ObtenerPorIdAsync(carritoId);
                
                if (carrito == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"Carrito con ID {carritoId} no encontrado",
                        Origen = "CarritoController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"Carrito con ID {carritoId} no encontrado" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Carrito con ID {carritoId} obtenido exitosamente",
                    Origen = "CarritoController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    DatosSalida = $"Usuario: {carrito.UsuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(carrito);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener carrito con ID {carritoId}: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el carrito" });
            }
        }

        /// <summary>
        /// Obtiene el carrito de un usuario específico
        /// </summary>
        [HttpGet("usuario/{usuId}")]
        public async Task<IActionResult> ObtenerPorUsuario(int usuId)
        {
            try
            {
                if (usuId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener carrito con ID de usuario inválido",
                        Origen = "CarritoController",
                        Metodo = "ObtenerPorUsuario",
                        DatosEntrada = $"UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de usuario debe ser mayor a 0" });
                }

                var carrito = await _carritoService.ObtenerPorUsuarioAsync(usuId);
                
                if (carrito == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se encontró carrito para el usuario {usuId}",
                        Origen = "CarritoController",
                        Metodo = "ObtenerPorUsuario",
                        DatosEntrada = $"UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se encontró carrito para el usuario {usuId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Carrito del usuario {usuId} obtenido exitosamente",
                    Origen = "CarritoController",
                    Metodo = "ObtenerPorUsuario",
                    DatosEntrada = $"UsuId: {usuId}",
                    DatosSalida = $"CarritoId: {carrito.CarritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(carrito);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener carrito del usuario {usuId}: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "ObtenerPorUsuario",
                    DatosEntrada = $"UsuId: {usuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el carrito del usuario" });
            }
        }

        /// <summary>
        /// Obtiene o crea un carrito para un usuario
        /// </summary>
        [HttpGet("usuario/{usuId}/obtener-o-crear")]
        public async Task<IActionResult> ObtenerOCrearCarrito(int usuId)
        {
            try
            {
                if (usuId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener o crear carrito con ID de usuario inválido",
                        Origen = "CarritoController",
                        Metodo = "ObtenerOCrearCarrito",
                        DatosEntrada = $"UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de usuario debe ser mayor a 0" });
                }

                var carrito = await _carritoService.ObtenerOCrearCarritoAsync(usuId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Carrito obtenido o creado exitosamente para el usuario {usuId}",
                    Origen = "CarritoController",
                    Metodo = "ObtenerOCrearCarrito",
                    DatosEntrada = $"UsuId: {usuId}",
                    DatosSalida = $"CarritoId: {carrito.CarritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(carrito);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener o crear carrito para el usuario {usuId}: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "ObtenerOCrearCarrito",
                    DatosEntrada = $"UsuId: {usuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener o crear el carrito" });
            }
        }

        /// <summary>
        /// Obtiene el resumen completo de un carrito
        /// </summary>
        [HttpGet("{carritoId}/resumen")]
        public async Task<IActionResult> ObtenerResumen(int carritoId)
        {
            try
            {
                if (carritoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener resumen de carrito con ID inválido",
                        Origen = "CarritoController",
                        Metodo = "ObtenerResumen",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del carrito debe ser mayor a 0" });
                }

                var resumen = await _carritoService.ObtenerResumenCarritoAsync(carritoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Resumen del carrito {carritoId} obtenido exitosamente",
                    Origen = "CarritoController",
                    Metodo = "ObtenerResumen",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(resumen);
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al obtener resumen del carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "ObtenerResumen",
                    DatosEntrada = $"CarritoId: {carritoId}",
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
                    Mensaje = $"Error al obtener resumen del carrito {carritoId}: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "ObtenerResumen",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el resumen del carrito" });
            }
        }

        /// <summary>
        /// Crea un nuevo carrito
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CarritoCreateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de crear carrito con datos nulos",
                        Origen = "CarritoController",
                        Metodo = "Crear",
                        DatosEntrada = "Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos del carrito son requeridos" });
                }

                var carrito = new Carrito
                {
                    UsuId = request.UsuId,
                    FechaCreacion = DateTime.Now
                };

                var carritoId = await _carritoService.CrearAsync(carrito);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Carrito creado exitosamente con ID {carritoId}",
                    Origen = "CarritoController",
                    Metodo = "Crear",
                    DatosEntrada = $"Usuario: {request.UsuId}",
                    DatosSalida = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(ObtenerPorId), new { carritoId }, 
                    new { mensaje = "Carrito creado exitosamente", carritoId });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al crear carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Crear",
                    DatosEntrada = $"Usuario: {request?.UsuId}",
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
                    Mensaje = $"Error de operación al crear carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Crear",
                    DatosEntrada = $"Usuario: {request?.UsuId}",
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
                    Mensaje = $"Error al crear carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Crear",
                    DatosEntrada = $"Usuario: {request?.UsuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al crear el carrito" });
            }
        }

        /// <summary>
        /// Actualiza un carrito existente
        /// </summary>
        [HttpPut("{carritoId}")]
        public async Task<IActionResult> Actualizar(int carritoId, [FromBody] CarritoUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de actualizar carrito con datos nulos",
                        Origen = "CarritoController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"CarritoId: {carritoId}, Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos del carrito son requeridos" });
                }

                if (carritoId != request.CarritoId)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "ID de carrito en URL no coincide con el del cuerpo",
                        Origen = "CarritoController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"CarritoId URL: {carritoId}, CarritoId Body: {request.CarritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del carrito en la URL no coincide con el del cuerpo" });
                }

                var carrito = new Carrito
                {
                    CarritoId = request.CarritoId,
                    UsuId = request.UsuId
                };

                var resultado = await _carritoService.ActualizarAsync(carrito);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar el carrito con ID {carritoId}",
                        Origen = "CarritoController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"CarritoId: {carritoId}, Usuario: {request.UsuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo actualizar el carrito con ID {carritoId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Carrito con ID {carritoId} actualizado exitosamente",
                    Origen = "CarritoController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"CarritoId: {carritoId}, Usuario: {request.UsuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Carrito actualizado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al actualizar carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"CarritoId: {carritoId}, Usuario: {request?.UsuId}",
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
                    Mensaje = $"Error de operación al actualizar carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"CarritoId: {carritoId}, Usuario: {request?.UsuId}",
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
                    Mensaje = $"Error al actualizar carrito con ID {carritoId}: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"CarritoId: {carritoId}, Usuario: {request?.UsuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al actualizar el carrito" });
            }
        }

        /// <summary>
        /// Elimina un carrito
        /// </summary>
        [HttpDelete("{carritoId}")]
        public async Task<IActionResult> Eliminar(int carritoId)
        {
            try
            {
                if (carritoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de eliminar carrito con ID inválido",
                        Origen = "CarritoController",
                        Metodo = "Eliminar",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del carrito debe ser mayor a 0" });
                }

                var resultado = await _carritoService.EliminarAsync(carritoId);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo eliminar el carrito con ID {carritoId}",
                        Origen = "CarritoController",
                        Metodo = "Eliminar",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo eliminar el carrito con ID {carritoId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Carrito con ID {carritoId} eliminado exitosamente",
                    Origen = "CarritoController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Carrito eliminado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al eliminar carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"CarritoId: {carritoId}",
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
                    Mensaje = $"Error de operación al eliminar carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"CarritoId: {carritoId}",
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
                    Mensaje = $"Error al eliminar carrito con ID {carritoId}: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al eliminar el carrito" });
            }
        }

        /// <summary>
        /// Vacía un carrito (elimina todos los items)
        /// </summary>
        [HttpPut("{carritoId}/vaciar")]
        public async Task<IActionResult> VaciarCarrito(int carritoId)
        {
            try
            {
                if (carritoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de vaciar carrito con ID inválido",
                        Origen = "CarritoController",
                        Metodo = "VaciarCarrito",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del carrito debe ser mayor a 0" });
                }

                var resultado = await _carritoService.VaciarCarritoAsync(carritoId);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo vaciar el carrito con ID {carritoId}",
                        Origen = "CarritoController",
                        Metodo = "VaciarCarrito",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo vaciar el carrito con ID {carritoId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Carrito con ID {carritoId} vaciado exitosamente",
                    Origen = "CarritoController",
                    Metodo = "VaciarCarrito",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Carrito vaciado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al vaciar carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "VaciarCarrito",
                    DatosEntrada = $"CarritoId: {carritoId}",
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
                    Mensaje = $"Error de operación al vaciar carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "VaciarCarrito",
                    DatosEntrada = $"CarritoId: {carritoId}",
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
                    Mensaje = $"Error al vaciar carrito con ID {carritoId}: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "VaciarCarrito",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al vaciar el carrito" });
            }
        }

        /// <summary>
        /// Calcula el total de un carrito
        /// </summary>
        [HttpGet("{carritoId}/total")]
        public async Task<IActionResult> CalcularTotal(int carritoId)
        {
            try
            {
                if (carritoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de calcular total de carrito con ID inválido",
                        Origen = "CarritoController",
                        Metodo = "CalcularTotal",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del carrito debe ser mayor a 0" });
                }

                var total = await _carritoService.CalcularTotalAsync(carritoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Total del carrito {carritoId} calculado exitosamente: {total}",
                    Origen = "CarritoController",
                    Metodo = "CalcularTotal",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    DatosSalida = $"Total: {total}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new CarritoTotalResponse { CarritoId = carritoId, Total = total });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al calcular total del carrito: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "CalcularTotal",
                    DatosEntrada = $"CarritoId: {carritoId}",
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
                    Mensaje = $"Error al calcular total del carrito {carritoId}: {ex.Message}",
                    Origen = "CarritoController",
                    Metodo = "CalcularTotal",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al calcular el total del carrito" });
            }
        }
    }
} 