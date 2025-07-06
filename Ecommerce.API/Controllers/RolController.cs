using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;
        private readonly ILogService _logService;

        public RolController(IRolService rolService, ILogService logService)
        {
            _rolService = rolService;
            _logService = logService;
        }

        // GET: api/rol
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolResponse>>> GetRoles()
        {
            try
            {
                var roles = await _rolService.ListarRolesAsync();
                var response = roles.Select(r => new RolResponse
                {
                    RolId = r.RolId,
                    RolNombre = r.RolNombre
                });
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Lista de roles consultada exitosamente",
                    Origen = "RolController",
                    Metodo = "GetRoles",
                    DatosSalida = JsonSerializer.Serialize(response),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar roles: {ex.Message}",
                    Origen = "RolController",
                    Metodo = "GetRoles",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/rol/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolResponse>> GetRol(int id)
        {
            try
            {
                var rol = await _rolService.ObtenerPorIdAsync(id);
                if (rol == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Rol con ID {id} no encontrado",
                        Origen = "RolController",
                        Metodo = "GetRol",
                        DatosEntrada = JsonSerializer.Serialize(new { id }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }
                var response = new RolResponse
                {
                    RolId = rol.RolId,
                    RolNombre = rol.RolNombre
                };

                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Rol con ID {id} consultado exitosamente",
                    Origen = "RolController",
                    Metodo = "GetRol",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    DatosSalida = JsonSerializer.Serialize(response),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar rol con ID {id}: {ex.Message}",
                    Origen = "RolController",
                    Metodo = "GetRol",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/rol
        [HttpPost]
        public async Task<ActionResult> PostRol([FromBody] RolCreateRequest request)
        {
            try
            {
                var rol = new Role { RolNombre = request.RolNombre };
                var id = await _rolService.InsertarRolAsync(rol);
                
                // Registrar log de creación exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CREACION",
                    Mensaje = $"Rol creado exitosamente con ID {id}",
                    Origen = "RolController",
                    Metodo = "PostRol",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    DatosSalida = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                var response = new RolResponse { RolId = id, RolNombre = request.RolNombre };
                return CreatedAtAction(nameof(GetRol), new { id }, response);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al crear rol: {ex.Message}",
                    Origen = "RolController",
                    Metodo = "PostRol",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/rol/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutRol(int id, [FromBody] RolUpdateRequest request)
        {
            try
            {
                if (id != request.RolId)
                {
                    // Registrar log de error de validación
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "El ID de la URL no coincide con el ID del rol",
                        Origen = "RolController",
                        Metodo = "PutRol",
                        DatosEntrada = JsonSerializer.Serialize(new { id, request }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("El ID no coincide.");
                }
                var rol = new Role { RolId = request.RolId, RolNombre = request.RolNombre };
                var actualizado = await _rolService.ActualizarRolAsync(rol);
                if (!actualizado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Rol con ID {id} no encontrado para actualizar",
                        Origen = "RolController",
                        Metodo = "PutRol",
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
                    Mensaje = $"Rol con ID {id} actualizado exitosamente",
                    Origen = "RolController",
                    Metodo = "PutRol",
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
                    Mensaje = $"Error al actualizar rol con ID {id}: {ex.Message}",
                    Origen = "RolController",
                    Metodo = "PutRol",
                    DatosEntrada = JsonSerializer.Serialize(new { id, request }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/rol/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRol(int id)
        {
            try
            {
                var eliminado = await _rolService.EliminarRolAsync(id);
                if (!eliminado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Rol con ID {id} no encontrado para eliminar",
                        Origen = "RolController",
                        Metodo = "DeleteRol",
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
                    Mensaje = $"Rol con ID {id} eliminado exitosamente",
                    Origen = "RolController",
                    Metodo = "DeleteRol",
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
                    Mensaje = $"Error al eliminar rol con ID {id}: {ex.Message}",
                    Origen = "RolController",
                    Metodo = "DeleteRol",
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
