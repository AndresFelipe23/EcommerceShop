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
    public class DireccionController : ControllerBase
    {
        private readonly IDireccionService _direccionService;
        private readonly ILogRepository _logRepository;

        public DireccionController(IDireccionService direccionService, ILogRepository logRepository)
        {
            _direccionService = direccionService;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene todas las direcciones
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var direcciones = await _direccionService.ListarAsync();
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Listado de direcciones exitoso. Total: {direcciones.Count()}",
                    Origen = "DireccionController",
                    Metodo = "Listar",
                    DatosSalida = $"Total direcciones: {direcciones.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(direcciones);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar direcciones: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Listar",
                    DatosEntrada = "Sin parámetros",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar direcciones" });
            }
        }

        /// <summary>
        /// Obtiene una dirección por su ID
        /// </summary>
        [HttpGet("{dirId}")]
        public async Task<IActionResult> ObtenerPorId(int dirId)
        {
            try
            {
                if (dirId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener dirección con ID inválido",
                        Origen = "DireccionController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"DirId: {dirId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de dirección debe ser mayor a 0" });
                }

                var direccion = await _direccionService.ObtenerPorIdAsync(dirId);
                
                if (direccion == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"Dirección con ID {dirId} no encontrada",
                        Origen = "DireccionController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"DirId: {dirId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"Dirección con ID {dirId} no encontrada" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Dirección con ID {dirId} obtenida exitosamente",
                    Origen = "DireccionController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"DirId: {dirId}",
                    DatosSalida = $"Dirección: {direccion.DirNombre}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(direccion);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener dirección con ID {dirId}: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"DirId: {dirId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener la dirección" });
            }
        }

        /// <summary>
        /// Obtiene las direcciones de un usuario específico
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
                        Mensaje = "Intento de listar direcciones con ID de usuario inválido",
                        Origen = "DireccionController",
                        Metodo = "ListarPorUsuario",
                        DatosEntrada = $"UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de usuario debe ser mayor a 0" });
                }

                var direcciones = await _direccionService.ListarPorUsuarioAsync(usuId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Direcciones del usuario {usuId} listadas exitosamente. Total: {direcciones.Count()}",
                    Origen = "DireccionController",
                    Metodo = "ListarPorUsuario",
                    DatosEntrada = $"UsuId: {usuId}",
                    DatosSalida = $"Total direcciones: {direcciones.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(direcciones);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar direcciones del usuario {usuId}: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "ListarPorUsuario",
                    DatosEntrada = $"UsuId: {usuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar las direcciones del usuario" });
            }
        }

        /// <summary>
        /// Obtiene la dirección principal de un usuario
        /// </summary>
        [HttpGet("usuario/{usuId}/principal")]
        public async Task<IActionResult> ObtenerDireccionPrincipal(int usuId)
        {
            try
            {
                if (usuId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener dirección principal con ID de usuario inválido",
                        Origen = "DireccionController",
                        Metodo = "ObtenerDireccionPrincipal",
                        DatosEntrada = $"UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de usuario debe ser mayor a 0" });
                }

                var direccion = await _direccionService.ObtenerDireccionPrincipalAsync(usuId);
                
                if (direccion == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se encontró dirección principal para el usuario {usuId}",
                        Origen = "DireccionController",
                        Metodo = "ObtenerDireccionPrincipal",
                        DatosEntrada = $"UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se encontró dirección principal para el usuario {usuId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Dirección principal del usuario {usuId} obtenida exitosamente",
                    Origen = "DireccionController",
                    Metodo = "ObtenerDireccionPrincipal",
                    DatosEntrada = $"UsuId: {usuId}",
                    DatosSalida = $"Dirección: {direccion.DirNombre}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(direccion);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener dirección principal del usuario {usuId}: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "ObtenerDireccionPrincipal",
                    DatosEntrada = $"UsuId: {usuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener la dirección principal" });
            }
        }

        /// <summary>
        /// Crea una nueva dirección
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] DireccionCreateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de crear dirección con datos nulos",
                        Origen = "DireccionController",
                        Metodo = "Crear",
                        DatosEntrada = "Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos de la dirección son requeridos" });
                }

                var direccion = new Direccione
                {
                    UsuId = request.UsuId,
                    DirTitulo = request.DirTitulo,
                    DirNombre = request.DirNombre,
                    DirTelefono = request.DirTelefono,
                    DirPais = request.DirPais,
                    DirDepartamento = request.DirDepartamento,
                    DirCiudad = request.DirCiudad,
                    DirCodigoPostal = request.DirCodigoPostal,
                    DirLinea1 = request.DirLinea1,
                    DirLinea2 = request.DirLinea2,
                    DirReferencia = request.DirReferencia,
                    DirEsPrincipal = request.DirEsPrincipal ?? false,
                    DirFechaCreacion = DateTime.Now
                };

                var dirId = await _direccionService.CrearAsync(direccion);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Dirección creada exitosamente con ID {dirId}",
                    Origen = "DireccionController",
                    Metodo = "Crear",
                    DatosEntrada = $"Dirección: {request.DirNombre}, Usuario: {request.UsuId}",
                    DatosSalida = $"DirId: {dirId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(ObtenerPorId), new { dirId }, 
                    new { mensaje = "Dirección creada exitosamente", dirId });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al crear dirección: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Crear",
                    DatosEntrada = $"Dirección: {request?.DirNombre}",
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
                    Mensaje = $"Error al crear dirección: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Crear",
                    DatosEntrada = $"Dirección: {request?.DirNombre}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al crear la dirección" });
            }
        }

        /// <summary>
        /// Actualiza una dirección existente
        /// </summary>
        [HttpPut("{dirId}")]
        public async Task<IActionResult> Actualizar(int dirId, [FromBody] DireccionUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de actualizar dirección con datos nulos",
                        Origen = "DireccionController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"DirId: {dirId}, Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos de la dirección son requeridos" });
                }

                if (dirId != request.DirId)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "ID de dirección en URL no coincide con el del cuerpo",
                        Origen = "DireccionController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"DirId URL: {dirId}, DirId Body: {request.DirId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de dirección en la URL no coincide con el del cuerpo" });
                }

                var direccion = new Direccione
                {
                    DirId = request.DirId,
                    UsuId = request.UsuId,
                    DirTitulo = request.DirTitulo,
                    DirNombre = request.DirNombre,
                    DirTelefono = request.DirTelefono,
                    DirPais = request.DirPais,
                    DirDepartamento = request.DirDepartamento,
                    DirCiudad = request.DirCiudad,
                    DirCodigoPostal = request.DirCodigoPostal,
                    DirLinea1 = request.DirLinea1,
                    DirLinea2 = request.DirLinea2,
                    DirReferencia = request.DirReferencia,
                    DirEsPrincipal = request.DirEsPrincipal
                };

                var resultado = await _direccionService.ActualizarAsync(direccion);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar la dirección con ID {dirId}",
                        Origen = "DireccionController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"DirId: {dirId}, Dirección: {request.DirNombre}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo actualizar la dirección con ID {dirId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Dirección con ID {dirId} actualizada exitosamente",
                    Origen = "DireccionController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"DirId: {dirId}, Dirección: {request.DirNombre}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Dirección actualizada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al actualizar dirección: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"DirId: {dirId}, Dirección: {request?.DirNombre}",
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
                    Mensaje = $"Error de operación al actualizar dirección: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"DirId: {dirId}, Dirección: {request?.DirNombre}",
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
                    Mensaje = $"Error al actualizar dirección con ID {dirId}: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"DirId: {dirId}, Dirección: {request?.DirNombre}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al actualizar la dirección" });
            }
        }

        /// <summary>
        /// Elimina una dirección
        /// </summary>
        [HttpDelete("{dirId}")]
        public async Task<IActionResult> Eliminar(int dirId)
        {
            try
            {
                if (dirId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de eliminar dirección con ID inválido",
                        Origen = "DireccionController",
                        Metodo = "Eliminar",
                        DatosEntrada = $"DirId: {dirId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de dirección debe ser mayor a 0" });
                }

                var resultado = await _direccionService.EliminarAsync(dirId);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo eliminar la dirección con ID {dirId}",
                        Origen = "DireccionController",
                        Metodo = "Eliminar",
                        DatosEntrada = $"DirId: {dirId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo eliminar la dirección con ID {dirId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Dirección con ID {dirId} eliminada exitosamente",
                    Origen = "DireccionController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"DirId: {dirId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Dirección eliminada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al eliminar dirección: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"DirId: {dirId}",
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
                    Mensaje = $"Error de operación al eliminar dirección: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"DirId: {dirId}",
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
                    Mensaje = $"Error al eliminar dirección con ID {dirId}: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"DirId: {dirId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al eliminar la dirección" });
            }
        }

        /// <summary>
        /// Marca una dirección como principal para un usuario
        /// </summary>
        [HttpPut("{dirId}/marcar-principal")]
        public async Task<IActionResult> MarcarComoPrincipal(int dirId, [FromQuery] int usuId)
        {
            try
            {
                if (dirId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de marcar dirección como principal con ID inválido",
                        Origen = "DireccionController",
                        Metodo = "MarcarComoPrincipal",
                        DatosEntrada = $"DirId: {dirId}, UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de dirección debe ser mayor a 0" });
                }

                if (usuId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de marcar dirección como principal con ID de usuario inválido",
                        Origen = "DireccionController",
                        Metodo = "MarcarComoPrincipal",
                        DatosEntrada = $"DirId: {dirId}, UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID de usuario debe ser mayor a 0" });
                }

                var resultado = await _direccionService.MarcarComoPrincipalAsync(dirId, usuId);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo marcar la dirección {dirId} como principal para el usuario {usuId}",
                        Origen = "DireccionController",
                        Metodo = "MarcarComoPrincipal",
                        DatosEntrada = $"DirId: {dirId}, UsuId: {usuId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo marcar la dirección como principal" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Dirección {dirId} marcada como principal para el usuario {usuId} exitosamente",
                    Origen = "DireccionController",
                    Metodo = "MarcarComoPrincipal",
                    DatosEntrada = $"DirId: {dirId}, UsuId: {usuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new MarcarDireccionPrincipalResponse 
                { 
                    Marcado = true, 
                    Mensaje = "Dirección marcada como principal exitosamente",
                    DirId = dirId,
                    UsuId = usuId
                });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al marcar dirección como principal: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "MarcarComoPrincipal",
                    DatosEntrada = $"DirId: {dirId}, UsuId: {usuId}",
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
                    Mensaje = $"Error de operación al marcar dirección como principal: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "MarcarComoPrincipal",
                    DatosEntrada = $"DirId: {dirId}, UsuId: {usuId}",
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
                    Mensaje = $"Error al marcar dirección {dirId} como principal para usuario {usuId}: {ex.Message}",
                    Origen = "DireccionController",
                    Metodo = "MarcarComoPrincipal",
                    DatosEntrada = $"DirId: {dirId}, UsuId: {usuId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al marcar la dirección como principal" });
            }
        }
    }
} 