using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.API.Models;
using System.Text.Json;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioRoleController : ControllerBase
    {
        private readonly IUsuarioRoleService _usuarioRoleService;
        private readonly ILogService _logService;

        public UsuarioRoleController(IUsuarioRoleService usuarioRoleService, ILogService logService)
        {
            _usuarioRoleService = usuarioRoleService;
            _logService = logService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioRole>>> Get()
        {
            var resultado = await _usuarioRoleService.ListarUsuarioRolesAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioRole>> Get(int id)
        {
            var resultado = await _usuarioRoleService.ObtenerPorIdAsync(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearUsuarioRoleDTO usuarioRoleDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (usuarioRoleDto == null)
                    return BadRequest(new { mensaje = "Los datos de la asignación de rol son requeridos" });

                // Mapear DTO a modelo
                var usuarioRole = new UsuarioRole
                {
                    UsuId = usuarioRoleDto.UsuId,
                    RolId = usuarioRoleDto.RolId
                };

                var id = await _usuarioRoleService.InsertarUsuarioRoleAsync(usuarioRole);

                await _logService.InsertarLogAsync(new Log
                {
                    UsuId = usuarioRoleDto.UsuId,
                    Tipo = "Creación",
                    Mensaje = $"Asignado rol {usuarioRoleDto.RolId} al usuario {usuarioRoleDto.UsuId}",
                    Origen = nameof(UsuarioRoleController),
                    Metodo = nameof(Post),
                    DatosEntrada = JsonSerializer.Serialize(usuarioRoleDto),
                    DatosSalida = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = Request.Headers["User-Agent"]
                });

                return CreatedAtAction(nameof(Get), new { id }, new { mensaje = "Rol asignado exitosamente", id });
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al asignar rol: {ex.Message}",
                    Origen = nameof(UsuarioRoleController),
                    Metodo = nameof(Post),
                    DatosEntrada = JsonSerializer.Serialize(usuarioRoleDto),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = Request.Headers["User-Agent"]
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActualizarUsuarioRoleDTO usuarioRoleDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (usuarioRoleDto == null)
                    return BadRequest(new { mensaje = "Los datos de la asignación de rol son requeridos" });

                // Obtener la asignación existente
                var usuarioRoleExistente = await _usuarioRoleService.ObtenerPorIdAsync(id);
                if (usuarioRoleExistente == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "Error",
                        Mensaje = $"Asignación de rol con ID {id} no encontrada para actualizar",
                        Origen = nameof(UsuarioRoleController),
                        Metodo = nameof(Put),
                        DatosEntrada = JsonSerializer.Serialize(new { id, usuarioRoleDto }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = Request.Headers["User-Agent"]
                    });

                    return NotFound(new { mensaje = $"Asignación de rol con ID {id} no encontrada" });
                }

                // Actualizar los campos
                usuarioRoleExistente.UsuId = usuarioRoleDto.UsuId;
                usuarioRoleExistente.RolId = usuarioRoleDto.RolId;

                var actualizado = await _usuarioRoleService.ActualizarUsuarioRoleAsync(usuarioRoleExistente);

                if (actualizado)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        UsuId = usuarioRoleDto.UsuId,
                        Tipo = "Actualización",
                        Mensaje = $"Actualizada asignación de rol {id}",
                        Origen = nameof(UsuarioRoleController),
                        Metodo = nameof(Put),
                        DatosEntrada = JsonSerializer.Serialize(new { id, usuarioRoleDto }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = Request.Headers["User-Agent"]
                    });

                    return Ok(new { mensaje = "Asignación de rol actualizada exitosamente" });
                }

                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al actualizar asignación de rol con ID {id}",
                    Origen = nameof(UsuarioRoleController),
                    Metodo = nameof(Put),
                    DatosEntrada = JsonSerializer.Serialize(new { id, usuarioRoleDto }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = Request.Headers["User-Agent"]
                });

                return StatusCode(500, new { mensaje = "Error al actualizar la asignación de rol" });
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al actualizar asignación de rol: {ex.Message}",
                    Origen = nameof(UsuarioRoleController),
                    Metodo = nameof(Put),
                    DatosEntrada = JsonSerializer.Serialize(new { id, usuarioRoleDto }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = Request.Headers["User-Agent"]
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminado = await _usuarioRoleService.EliminarUsuarioRoleAsync(id);

            if (eliminado)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Eliminación",
                    Mensaje = $"Eliminado UsuarioRol con ID {id}",
                    Origen = nameof(UsuarioRoleController),
                    Metodo = nameof(Delete),
                    DatosEntrada = $"UsuarioRolId={id}",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = Request.Headers["User-Agent"]
                });

                return NoContent();
            }

            return NotFound();
        }
    }
}
