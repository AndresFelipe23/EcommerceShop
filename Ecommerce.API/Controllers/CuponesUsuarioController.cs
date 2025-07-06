using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Ecommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponesUsuarioController : ControllerBase
    {
        private readonly ICuponesUsuarioService _cuponesUsuarioService;
        private readonly ILogService _logService;

        public CuponesUsuarioController(ICuponesUsuarioService cuponesUsuarioService, ILogService logService)
        {
            _cuponesUsuarioService = cuponesUsuarioService;
            _logService = logService;
        }

        // GET: api/cuponesusuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuponesUsuario>>> GetCuponesUsuarios()
        {
            try
            {
                var cuponesUsuarios = await _cuponesUsuarioService.ListarCuponesUsuariosAsync();
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Lista de cupones de usuarios consultada exitosamente",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponesUsuarios",
                    DatosSalida = JsonSerializer.Serialize(cuponesUsuarios),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(cuponesUsuarios);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar cupones de usuarios: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponesUsuarios",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cuponesusuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CuponesUsuario>> GetCuponUsuario(int id)
        {
            try
            {
                var cuponUsuario = await _cuponesUsuarioService.ObtenerPorIdAsync(id);
                if (cuponUsuario == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Cupón de usuario con ID {id} no encontrado",
                        Origen = "CuponesUsuarioController",
                        Metodo = "GetCuponUsuario",
                        DatosEntrada = JsonSerializer.Serialize(new { id }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }

                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Cupón de usuario con ID {id} consultado exitosamente",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    DatosSalida = JsonSerializer.Serialize(cuponUsuario),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(cuponUsuario);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar cupón de usuario con ID {id}: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cuponesusuario/usuario/{usuId}
        [HttpGet("usuario/{usuId}")]
        public async Task<ActionResult<IEnumerable<CuponesUsuario>>> GetCuponesPorUsuario(int usuId)
        {
            try
            {
                var cupones = await _cuponesUsuarioService.ListarCuponesPorUsuarioAsync(usuId);
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Cupones del usuario {usuId} consultados exitosamente",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponesPorUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { usuId }),
                    DatosSalida = JsonSerializer.Serialize(cupones),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(cupones);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar cupones del usuario {usuId}: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponesPorUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { usuId }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cuponesusuario/usuario/{usuId}/usados
        [HttpGet("usuario/{usuId}/usados")]
        public async Task<ActionResult<IEnumerable<CuponesUsuario>>> GetCuponesUsadosPorUsuario(int usuId)
        {
            try
            {
                var cupones = await _cuponesUsuarioService.ListarCuponesUsadosPorUsuarioAsync(usuId);
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Cupones usados del usuario {usuId} consultados exitosamente",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponesUsadosPorUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { usuId }),
                    DatosSalida = JsonSerializer.Serialize(cupones),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(cupones);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar cupones usados del usuario {usuId}: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponesUsadosPorUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { usuId }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cuponesusuario/usuario/{usuId}/disponibles
        [HttpGet("usuario/{usuId}/disponibles")]
        public async Task<ActionResult<IEnumerable<CuponesUsuario>>> GetCuponesDisponiblesPorUsuario(int usuId)
        {
            try
            {
                var cupones = await _cuponesUsuarioService.ListarCuponesDisponiblesPorUsuarioAsync(usuId);
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Cupones disponibles del usuario {usuId} consultados exitosamente",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponesDisponiblesPorUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { usuId }),
                    DatosSalida = JsonSerializer.Serialize(cupones),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(cupones);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar cupones disponibles del usuario {usuId}: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponesDisponiblesPorUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { usuId }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cuponesusuario/cupon/{cuponId}/usuario/{usuId}
        [HttpGet("cupon/{cuponId}/usuario/{usuId}")]
        public async Task<ActionResult<CuponesUsuario>> GetCuponUsuario(int cuponId, int usuId)
        {
            try
            {
                var cuponUsuario = await _cuponesUsuarioService.ObtenerCuponUsuarioAsync(cuponId, usuId);
                if (cuponUsuario == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Cupón {cuponId} del usuario {usuId} no encontrado",
                        Origen = "CuponesUsuarioController",
                        Metodo = "GetCuponUsuario",
                        DatosEntrada = JsonSerializer.Serialize(new { cuponId, usuId }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }

                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Cupón {cuponId} del usuario {usuId} consultado exitosamente",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { cuponId, usuId }),
                    DatosSalida = JsonSerializer.Serialize(cuponUsuario),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(cuponUsuario);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar cupón {cuponId} del usuario {usuId}: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "GetCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { cuponId, usuId }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/cuponesusuario/asignar
        [HttpPost("asignar")]
        public async Task<ActionResult> AsignarCuponAUsuario([FromBody] AsignarCuponRequest request)
        {
            try
            {
                var asignado = await _cuponesUsuarioService.AsignarCuponAUsuarioAsync(request.CuponId, request.UsuId);
                
                // Registrar log de asignación
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ASIGNACION",
                    Mensaje = $"Cupón {request.CuponId} asignado al usuario {request.UsuId}: {(asignado ? "Exitoso" : "Fallido")}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "AsignarCuponAUsuario",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    DatosSalida = JsonSerializer.Serialize(new { asignado }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                if (!asignado)
                    return BadRequest(new AsignarCuponResponse 
                    { 
                        Asignado = false, 
                        Mensaje = "No se pudo asignar el cupón al usuario" 
                    });

                return Ok(new AsignarCuponResponse 
                { 
                    Asignado = true, 
                    Mensaje = "Cupón asignado exitosamente al usuario" 
                });
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al asignar cupón {request.CuponId} al usuario {request.UsuId}: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "AsignarCuponAUsuario",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/cuponesusuario/{id}/usar
        [HttpPut("{id}/usar")]
        public async Task<ActionResult> UsarCupon(int id)
        {
            try
            {
                var usado = await _cuponesUsuarioService.MarcarCuponComoUsadoAsync(id);
                
                // Registrar log de uso
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "USO",
                    Mensaje = $"Cupón de usuario {id} marcado como usado: {(usado ? "Exitoso" : "Fallido")}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "UsarCupon",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    DatosSalida = JsonSerializer.Serialize(new { usado }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                if (!usado)
                    return BadRequest(new UsarCuponResponse 
                    { 
                        Usado = false, 
                        Mensaje = "No se pudo marcar el cupón como usado" 
                    });

                return Ok(new UsarCuponResponse 
                { 
                    Usado = true, 
                    Mensaje = "Cupón marcado como usado exitosamente",
                    FechaUso = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al marcar cupón de usuario {id} como usado: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "UsarCupon",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/cuponesusuario
        [HttpPost]
        public async Task<ActionResult> PostCuponUsuario([FromBody] CuponUsuarioCreateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "Datos de cupón de usuario nulos",
                        Origen = "CuponesUsuarioController",
                        Metodo = "PostCuponUsuario",
                        DatosEntrada = "Request: null",
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("Los datos del cupón de usuario son requeridos");
                }

                var cuponUsuario = new CuponesUsuario
                {
                    CuponId = request.CuponId,
                    UsuId = request.UsuId,
                    Usado = request.Usado ?? false,
                    FechaUso = null
                };

                var id = await _cuponesUsuarioService.InsertarCuponesUsuarioAsync(cuponUsuario);
                
                // Registrar log de creación exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CREACION",
                    Mensaje = $"Cupón de usuario creado exitosamente con ID {id}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "PostCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    DatosSalida = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(GetCuponUsuario), new { id }, new CuponUsuarioResponse
                {
                    CuponUsuarioId = id,
                    CuponId = cuponUsuario.CuponId,
                    UsuId = cuponUsuario.UsuId,
                    Usado = cuponUsuario.Usado,
                    FechaUso = cuponUsuario.FechaUso
                });
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al crear cupón de usuario: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "PostCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/cuponesusuario/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCuponUsuario(int id, [FromBody] CuponUsuarioUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "Datos de cupón de usuario nulos",
                        Origen = "CuponesUsuarioController",
                        Metodo = "PutCuponUsuario",
                        DatosEntrada = $"ID: {id}, Request: null",
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("Los datos del cupón de usuario son requeridos");
                }

                if (id != request.CuponUsuarioId)
                {
                    // Registrar log de error de validación
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "El ID de la URL no coincide con el ID del cupón de usuario",
                        Origen = "CuponesUsuarioController",
                        Metodo = "PutCuponUsuario",
                        DatosEntrada = JsonSerializer.Serialize(new { id, cuponUsuarioId = request.CuponUsuarioId }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("El ID no coincide.");
                }

                var cuponUsuario = new CuponesUsuario
                {
                    CuponUsuarioId = request.CuponUsuarioId,
                    CuponId = request.CuponId,
                    UsuId = request.UsuId,
                    Usado = request.Usado,
                    FechaUso = request.FechaUso
                };

                var actualizado = await _cuponesUsuarioService.ActualizarCuponesUsuarioAsync(cuponUsuario);
                if (!actualizado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Cupón de usuario con ID {id} no encontrado para actualizar",
                        Origen = "CuponesUsuarioController",
                        Metodo = "PutCuponUsuario",
                        DatosEntrada = JsonSerializer.Serialize(new { id, request }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }

                // Registrar log de actualización exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ACTUALIZACION",
                    Mensaje = $"Cupón de usuario con ID {id} actualizado exitosamente",
                    Origen = "CuponesUsuarioController",
                    Metodo = "PutCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { id, request }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al actualizar cupón de usuario con ID {id}: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "PutCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { id, request }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/cuponesusuario/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCuponUsuario(int id)
        {
            try
            {
                var eliminado = await _cuponesUsuarioService.EliminarCuponesUsuarioAsync(id);
                if (!eliminado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Cupón de usuario con ID {id} no encontrado para eliminar",
                        Origen = "CuponesUsuarioController",
                        Metodo = "DeleteCuponUsuario",
                        DatosEntrada = JsonSerializer.Serialize(new { id }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }

                // Registrar log de eliminación exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ELIMINACION",
                    Mensaje = $"Cupón de usuario con ID {id} eliminado exitosamente",
                    Origen = "CuponesUsuarioController",
                    Metodo = "DeleteCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al eliminar cupón de usuario con ID {id}: {ex.Message}",
                    Origen = "CuponesUsuarioController",
                    Metodo = "DeleteCuponUsuario",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
} 