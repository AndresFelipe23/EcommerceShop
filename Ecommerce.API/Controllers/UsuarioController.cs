using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.API.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogService _logService;

        public UsuarioController(IUsuarioService usuarioService, ILogService logService)
        {
            _usuarioService = usuarioService;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.ListarUsuariosAsync();
            await _logService.InsertarLogAsync(new Log
            {
                Tipo = "Consulta",
                Mensaje = "Se listaron todos los usuarios",
                Fecha = DateTime.Now,
                Metodo = "GetUsuarios",
                Origen = "UsuarioController"
            });

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if (usuario == null)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Advertencia",
                    Mensaje = $"Usuario con ID {id} no encontrado",
                    Fecha = DateTime.Now,
                    Metodo = "GetUsuario",
                    Origen = "UsuarioController"
                });

                return NotFound();
            }

            await _logService.InsertarLogAsync(new Log
            {
                Tipo = "Consulta",
                Mensaje = $"Se obtuvo el usuario con ID {id}",
                Fecha = DateTime.Now,
                Metodo = "GetUsuario",
                Origen = "UsuarioController"
            });

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] CrearUsuarioDTO usuarioDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (usuarioDto == null)
                    return BadRequest(new { mensaje = "Los datos del usuario son requeridos" });

                // Mapear DTO a modelo
                var usuario = new Usuario
                {
                    UsuNombre = usuarioDto.UsuNombre,
                    UsuApellido = usuarioDto.UsuApellido,
                    UsuEmail = usuarioDto.UsuEmail,
                    UsuPasswordHash = usuarioDto.UsuPassword, // En producción, esto debería hashearse
                    UsuTelefono = usuarioDto.UsuTelefono,
                    UsuGenero = usuarioDto.UsuGenero,
                    UsuFechaNacimiento = usuarioDto.UsuFechaNacimiento,
                    UsuImagenPerfil = usuarioDto.UsuImagenPerfil,
                    UsuEstadoCuenta = usuarioDto.UsuEstadoCuenta ?? "Activo",
                    UsuProveedorAutenticacion = usuarioDto.UsuProveedorAutenticacion,
                    UsuFechaRegistro = DateTime.Now
                };

                var id = await _usuarioService.InsertarUsuarioAsync(usuario);

                await _logService.InsertarLogAsync(new Log
                {
                    UsuId = id,
                    Tipo = "Creación",
                    Mensaje = $"Se creó el usuario con ID {id}",
                    Fecha = DateTime.Now,
                    Metodo = "PostUsuario",
                    Origen = "UsuarioController",
                    DatosEntrada = JsonSerializer.Serialize(new { email = usuarioDto.UsuEmail, nombre = usuarioDto.UsuNombre }),
                    DatosSalida = JsonSerializer.Serialize(new { id })
                });

                return CreatedAtAction(nameof(GetUsuario), new { id }, new { mensaje = "Usuario creado exitosamente", id });
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al crear usuario: {ex.Message}",
                    Fecha = DateTime.Now,
                    Metodo = "PostUsuario",
                    Origen = "UsuarioController",
                    DatosEntrada = JsonSerializer.Serialize(usuarioDto)
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] ActualizarUsuarioDTO usuarioDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (usuarioDto == null)
                    return BadRequest(new { mensaje = "Los datos del usuario son requeridos" });

                // Obtener el usuario existente
                var usuarioExistente = await _usuarioService.ObtenerPorIdAsync(id);
                if (usuarioExistente == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "Error",
                        Mensaje = $"Usuario con ID {id} no encontrado para actualizar",
                        Fecha = DateTime.Now,
                        Metodo = "PutUsuario",
                        Origen = "UsuarioController",
                        DatosEntrada = JsonSerializer.Serialize(new { id, usuarioDto })
                    });

                    return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado" });
                }

                // Actualizar solo los campos proporcionados
                if (usuarioDto.UsuNombre != null)
                    usuarioExistente.UsuNombre = usuarioDto.UsuNombre;
                if (usuarioDto.UsuApellido != null)
                    usuarioExistente.UsuApellido = usuarioDto.UsuApellido;
                if (usuarioDto.UsuEmail != null)
                    usuarioExistente.UsuEmail = usuarioDto.UsuEmail;
                if (usuarioDto.UsuPassword != null)
                    usuarioExistente.UsuPasswordHash = usuarioDto.UsuPassword; // En producción, esto debería hashearse
                if (usuarioDto.UsuTelefono != null)
                    usuarioExistente.UsuTelefono = usuarioDto.UsuTelefono;
                if (usuarioDto.UsuGenero != null)
                    usuarioExistente.UsuGenero = usuarioDto.UsuGenero;
                if (usuarioDto.UsuFechaNacimiento.HasValue)
                    usuarioExistente.UsuFechaNacimiento = usuarioDto.UsuFechaNacimiento;
                if (usuarioDto.UsuImagenPerfil != null)
                    usuarioExistente.UsuImagenPerfil = usuarioDto.UsuImagenPerfil;
                if (usuarioDto.UsuEstadoCuenta != null)
                    usuarioExistente.UsuEstadoCuenta = usuarioDto.UsuEstadoCuenta;
                if (usuarioDto.UsuProveedorAutenticacion != null)
                    usuarioExistente.UsuProveedorAutenticacion = usuarioDto.UsuProveedorAutenticacion;

                usuarioExistente.UsuFechaActualizacion = DateTime.Now;

                var success = await _usuarioService.ActualizarUsuarioAsync(usuarioExistente);

                await _logService.InsertarLogAsync(new Log
                {
                    UsuId = id,
                    Tipo = success ? "Actualización" : "Error",
                    Mensaje = success ? $"Se actualizó el usuario con ID {id}" : $"No se pudo actualizar el usuario con ID {id}",
                    Fecha = DateTime.Now,
                    Metodo = "PutUsuario",
                    Origen = "UsuarioController",
                    DatosEntrada = JsonSerializer.Serialize(new { id, camposActualizados = usuarioDto })
                });

                if (!success)
                    return StatusCode(500, new { mensaje = "Error al actualizar el usuario" });

                return Ok(new { mensaje = "Usuario actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al actualizar usuario: {ex.Message}",
                    Fecha = DateTime.Now,
                    Metodo = "PutUsuario",
                    Origen = "UsuarioController",
                    DatosEntrada = JsonSerializer.Serialize(new { id, usuarioDto })
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.FindFirst("UsuId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { mensaje = "Token inválido o expirado" });
                }

                // Obtener el usuario
                var usuario = await _usuarioService.ObtenerPorIdAsync(userId);
                if (usuario == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                }

                await _logService.InsertarLogAsync(new Log
                {
                    UsuId = userId,
                    Tipo = "Consulta",
                    Mensaje = $"Usuario autenticado consultó su información",
                    Fecha = DateTime.Now,
                    Metodo = "GetCurrentUser",
                    Origen = "UsuarioController"
                });

                return Ok(new { 
                    mensaje = "Autenticación exitosa",
                    usuario = new {
                        usuId = usuario.UsuId,
                        usuNombre = usuario.UsuNombre,
                        usuApellido = usuario.UsuApellido,
                        usuEmail = usuario.UsuEmail,
                        usuEstadoCuenta = usuario.UsuEstadoCuenta
                    }
                });
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener usuario actual: {ex.Message}",
                    Fecha = DateTime.Now,
                    Metodo = "GetCurrentUser",
                    Origen = "UsuarioController"
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var success = await _usuarioService.EliminarUsuarioAsync(id);

            await _logService.InsertarLogAsync(new Log
            {
                UsuId = id,
                Tipo = success ? "Eliminación" : "Error",
                Mensaje = success ? $"Se eliminó el usuario con ID {id}" : $"No se pudo eliminar el usuario con ID {id}",
                Fecha = DateTime.Now,
                Metodo = "DeleteUsuario",
                Origen = "UsuarioController"
            });

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
