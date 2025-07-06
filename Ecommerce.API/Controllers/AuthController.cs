using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.API.Models;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using Ecommerce.API.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogService _logService;
        private readonly IJwtService _jwtService;
        private readonly IUsuarioRoleService _usuarioRoleService;

        public AuthController(IUsuarioService usuarioService, ILogService logService, IJwtService jwtService, IUsuarioRoleService usuarioRoleService)
        {
            _usuarioService = usuarioService;
            _logService = logService;
            _jwtService = jwtService;
            _usuarioRoleService = usuarioRoleService;
        }

        /// <summary>
        /// Autentica un usuario y devuelve un token JWT
        /// </summary>
        /// <param name="loginDto">Datos de login (email y contraseña)</param>
        /// <returns>Token JWT y información del usuario</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginRespuestaDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDTO loginDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (loginDto == null)
                    return BadRequest(new { mensaje = "Los datos de login son requeridos" });

                // Buscar usuario por email
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var usuario = usuarios.FirstOrDefault(u => u.UsuEmail.ToLower() == loginDto.Email.ToLower());

                if (usuario == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "Error",
                        Mensaje = $"Intento de login fallido - Email no encontrado: {loginDto.Email}",
                        Origen = "AuthController",
                        Metodo = "Login",
                        DatosEntrada = JsonSerializer.Serialize(new { email = loginDto.Email }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return Unauthorized(new { mensaje = "Credenciales inválidas" });
                }

                // Verificar contraseña usando BCrypt
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.UsuPasswordHash))
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        UsuId = usuario.UsuId,
                        Tipo = "Error",
                        Mensaje = $"Intento de login fallido - Contraseña incorrecta para usuario: {usuario.UsuEmail}",
                        Origen = "AuthController",
                        Metodo = "Login",
                        DatosEntrada = JsonSerializer.Serialize(new { email = loginDto.Email }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return Unauthorized(new { mensaje = "Credenciales inválidas" });
                }

                // Verificar si la cuenta está activa
                if (usuario.UsuEstadoCuenta != "Activo")
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        UsuId = usuario.UsuId,
                        Tipo = "Error",
                        Mensaje = $"Intento de login fallido - Cuenta inactiva: {usuario.UsuEmail}",
                        Origen = "AuthController",
                        Metodo = "Login",
                        DatosEntrada = JsonSerializer.Serialize(new { email = loginDto.Email }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return Unauthorized(new { mensaje = "Cuenta inactiva" });
                }

                // Actualizar último login
                usuario.UsuFechaUltimoLogin = DateTime.Now;
                await _usuarioService.ActualizarUsuarioAsync(usuario);

                // Crear respuesta
                var usuarioResumen = new UsuarioResumenDTO
                {
                    UsuId = usuario.UsuId,
                    UsuNombre = usuario.UsuNombre,
                    UsuApellido = usuario.UsuApellido,
                    UsuEmail = usuario.UsuEmail,
                    UsuEstadoCuenta = usuario.UsuEstadoCuenta,
                    UsuFechaRegistro = usuario.UsuFechaRegistro,
                    NombreCompleto = $"{usuario.UsuNombre} {usuario.UsuApellido}"
                };

                var respuesta = new LoginRespuestaDTO
                {
                    Exito = true,
                    Mensaje = "Login exitoso",
                    Token = _jwtService.GenerateToken(usuario),
                    RefreshToken = _jwtService.GenerateRefreshToken(),
                    Usuario = usuarioResumen
                };

                // Registrar login exitoso
                await _logService.InsertarLogAsync(new Log
                {
                    UsuId = usuario.UsuId,
                    Tipo = "Info",
                    Mensaje = $"Login exitoso para usuario: {usuario.UsuEmail}",
                    Origen = "AuthController",
                    Metodo = "Login",
                    DatosEntrada = JsonSerializer.Serialize(new { email = loginDto.Email }),
                    DatosSalida = JsonSerializer.Serialize(new { usuarioId = usuario.UsuId }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error en login: {ex.Message}",
                    Origen = "AuthController",
                    Metodo = "Login",
                    DatosEntrada = JsonSerializer.Serialize(loginDto),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Registra un nuevo usuario y devuelve un token JWT
        /// </summary>
        /// <param name="usuarioDto">Datos del nuevo usuario</param>
        /// <returns>Token JWT y información del usuario creado</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(LoginRespuestaDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] CrearUsuarioDTO usuarioDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (usuarioDto == null)
                    return BadRequest(new { mensaje = "Los datos de registro son requeridos" });

                // Verificar si el email ya existe
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                if (usuarios.Any(u => u.UsuEmail.ToLower() == usuarioDto.UsuEmail.ToLower()))
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "Error",
                        Mensaje = $"Intento de registro fallido - Email ya existe: {usuarioDto.UsuEmail}",
                        Origen = "AuthController",
                        Metodo = "Register",
                        DatosEntrada = JsonSerializer.Serialize(new { email = usuarioDto.UsuEmail }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El email ya está registrado" });
                }

                // Mapear DTO a modelo
                var usuario = new Usuario
                {
                    UsuNombre = usuarioDto.UsuNombre,
                    UsuApellido = usuarioDto.UsuApellido,
                    UsuEmail = usuarioDto.UsuEmail,
                    UsuPasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.UsuPassword),
                    UsuTelefono = usuarioDto.UsuTelefono,
                    UsuGenero = usuarioDto.UsuGenero,
                    UsuFechaNacimiento = usuarioDto.UsuFechaNacimiento,
                    UsuImagenPerfil = usuarioDto.UsuImagenPerfil,
                    UsuEstadoCuenta = usuarioDto.UsuEstadoCuenta ?? "Activo",
                    UsuProveedorAutenticacion = usuarioDto.UsuProveedorAutenticacion,
                    UsuFechaRegistro = DateTime.Now
                };

                // Log antes de insertar
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Intentando insertar usuario: {usuario.UsuEmail}",
                    Origen = "AuthController",
                    Metodo = "Register",
                    DatosEntrada = JsonSerializer.Serialize(new { email = usuario.UsuEmail, nombre = usuario.UsuNombre }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                var id = await _usuarioService.InsertarUsuarioAsync(usuario);

                // Asignar rol Cliente automáticamente (ID=2)
                await _usuarioRoleService.InsertarUsuarioRoleAsync(new UsuarioRole
                {
                    UsuId = id,
                    RolId = 3 // Ajusta si el ID de Cliente es diferente
                });

                // Crear respuesta
                var usuarioResumen = new UsuarioResumenDTO
                {
                    UsuId = id,
                    UsuNombre = usuario.UsuNombre,
                    UsuApellido = usuario.UsuApellido,
                    UsuEmail = usuario.UsuEmail,
                    UsuEstadoCuenta = usuario.UsuEstadoCuenta,
                    UsuFechaRegistro = usuario.UsuFechaRegistro,
                    NombreCompleto = $"{usuario.UsuNombre} {usuario.UsuApellido}"
                };

                var respuesta = new LoginRespuestaDTO
                {
                    Exito = true,
                    Mensaje = "Registro exitoso",
                    Token = _jwtService.GenerateToken(usuario),
                    RefreshToken = _jwtService.GenerateRefreshToken(),
                    Usuario = usuarioResumen
                };

                // Registrar registro exitoso
                await _logService.InsertarLogAsync(new Log
                {
                    UsuId = id,
                    Tipo = "Info",
                    Mensaje = $"Registro exitoso para usuario: {usuario.UsuEmail}",
                    Origen = "AuthController",
                    Metodo = "Register",
                    DatosEntrada = JsonSerializer.Serialize(new { email = usuarioDto.UsuEmail, nombre = usuarioDto.UsuNombre }),
                    DatosSalida = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(Login), new { }, respuesta);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error en registro: {ex.Message} - StackTrace: {ex.StackTrace}",
                    Origen = "AuthController",
                    Metodo = "Register",
                    DatosEntrada = JsonSerializer.Serialize(usuarioDto),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor", detalles = ex.Message });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] CambiarPasswordDTO changePasswordDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Obtener usuario actual desde el token JWT
                // Por ahora, requerimos el ID del usuario en el DTO
                // En producción, esto vendría del token JWT

                return Ok(new { mensaje = "Contraseña cambiada exitosamente" });
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al cambiar contraseña: {ex.Message}",
                    Origen = "AuthController",
                    Metodo = "ChangePassword",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Renueva el token JWT usando un token válido
        /// </summary>
        /// <param name="refreshTokenDto">Token actual</param>
        /// <returns>Nuevo token JWT y refresh token</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(RefreshTokenRespuestaDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDto)
        {
            try
            {
                if (refreshTokenDto == null || string.IsNullOrEmpty(refreshTokenDto.Token))
                {
                    return BadRequest(new { mensaje = "Token requerido" });
                }

                // Validar el token actual
                var principal = _jwtService.ValidateToken(refreshTokenDto.Token);
                if (principal == null)
                {
                    return Unauthorized(new { mensaje = "Token inválido" });
                }

                // Obtener el ID del usuario del token
                var userIdClaim = principal.FindFirst("UsuId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { mensaje = "Token inválido" });
                }

                // Obtener el usuario
                var usuario = await _usuarioService.ObtenerPorIdAsync(userId);
                if (usuario == null)
                {
                    return Unauthorized(new { mensaje = "Usuario no encontrado" });
                }

                // Generar nuevo token
                var newToken = _jwtService.GenerateToken(usuario);
                var refreshToken = _jwtService.GenerateRefreshToken();

                var respuesta = new RefreshTokenRespuestaDTO
                {
                    Exito = true,
                    Mensaje = "Token renovado exitosamente",
                    Token = newToken,
                    RefreshToken = refreshToken
                };

                await _logService.InsertarLogAsync(new Log
                {
                    UsuId = userId,
                    Tipo = "Info",
                    Mensaje = "Token renovado exitosamente",
                    Origen = "AuthController",
                    Metodo = "RefreshToken",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al renovar token: {ex.Message}",
                    Origen = "AuthController",
                    Metodo = "RefreshToken",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene la información del usuario autenticado
        /// </summary>
        /// <returns>Información del usuario actual</returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioResumenDTO), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized(new { mensaje = "Usuario no autenticado" });
                }

                var usuario = await _usuarioService.ObtenerPorIdAsync(userId.Value);
                if (usuario == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                }

                var usuarioResumen = new UsuarioResumenDTO
                {
                    UsuId = usuario.UsuId,
                    UsuNombre = usuario.UsuNombre,
                    UsuApellido = usuario.UsuApellido,
                    UsuEmail = usuario.UsuEmail,
                    UsuEstadoCuenta = usuario.UsuEstadoCuenta,
                    UsuFechaRegistro = usuario.UsuFechaRegistro,
                    NombreCompleto = $"{usuario.UsuNombre} {usuario.UsuApellido}"
                };

                return Ok(usuarioResumen);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener usuario actual: {ex.Message}",
                    Origen = "AuthController",
                    Metodo = "GetCurrentUser",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Endpoint de prueba para verificar la conexión a la base de datos
        /// </summary>
        /// <returns>Estado de la conexión</returns>
        [HttpGet("test-db")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                // Intentar listar usuarios para probar la conexión
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                
                return Ok(new { 
                    mensaje = "Conexión a base de datos exitosa", 
                    totalUsuarios = usuarios.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error de conexión a base de datos", 
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// Endpoint de prueba para verificar que la autenticación funciona
        /// </summary>
        /// <returns>Mensaje de confirmación</returns>
        [HttpGet("test-auth")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> TestAuth()
        {
            var userId = GetCurrentUserId();
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            return Ok(new { 
                mensaje = "¡Autenticación exitosa!", 
                userId = userId,
                userEmail = userEmail,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Cierra la sesión del usuario
        /// </summary>
        /// <returns>Confirmación de logout</returns>
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // TODO: Invalidar token JWT si es necesario
                // Por ahora, solo registramos el logout

                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = "Logout exitoso",
                    Origen = "AuthController",
                    Metodo = "Logout",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Logout exitoso" });
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error en logout: {ex.Message}",
                    Origen = "AuthController",
                    Metodo = "Logout",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        // Método auxiliar para obtener usuario actual desde el token
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UsuId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
} 