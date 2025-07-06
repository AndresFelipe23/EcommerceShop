using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Ecommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;
        private readonly ILogService _logService;

        public ColorController(IColorService colorService, ILogService logService)
        {
            _colorService = colorService;
            _logService = logService;
        }

        // GET: api/color
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colore>>> GetColores()
        {
            try
            {
                var colores = await _colorService.ListarColoresAsync();
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Lista de colores consultada exitosamente",
                    Origen = "ColorController",
                    Metodo = "GetColores",
                    DatosSalida = JsonSerializer.Serialize(colores),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(colores);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar colores: {ex.Message}",
                    Origen = "ColorController",
                    Metodo = "GetColores",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/color/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Colore>> GetColor(int id)
        {
            try
            {
                var color = await _colorService.ObtenerPorIdAsync(id);
                if (color == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Color con ID {id} no encontrado",
                        Origen = "ColorController",
                        Metodo = "GetColor",
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
                    Mensaje = $"Color con ID {id} consultado exitosamente",
                    Origen = "ColorController",
                    Metodo = "GetColor",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    DatosSalida = JsonSerializer.Serialize(color),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(color);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar color con ID {id}: {ex.Message}",
                    Origen = "ColorController",
                    Metodo = "GetColor",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/color/nombre/{nombre}
        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<Colore>> GetColorPorNombre(string nombre)
        {
            try
            {
                var color = await _colorService.ObtenerPorNombreAsync(nombre);
                if (color == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Color con nombre '{nombre}' no encontrado",
                        Origen = "ColorController",
                        Metodo = "GetColorPorNombre",
                        DatosEntrada = JsonSerializer.Serialize(new { nombre }),
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
                    Mensaje = $"Color con nombre '{nombre}' consultado exitosamente",
                    Origen = "ColorController",
                    Metodo = "GetColorPorNombre",
                    DatosEntrada = JsonSerializer.Serialize(new { nombre }),
                    DatosSalida = JsonSerializer.Serialize(color),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(color);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar color con nombre '{nombre}': {ex.Message}",
                    Origen = "ColorController",
                    Metodo = "GetColorPorNombre",
                    DatosEntrada = JsonSerializer.Serialize(new { nombre }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/color/codigo/{codigoHex}
        [HttpGet("codigo/{codigoHex}")]
        public async Task<ActionResult<Colore>> GetColorPorCodigoHex(string codigoHex)
        {
            try
            {
                var color = await _colorService.ObtenerPorCodigoHexAsync(codigoHex);
                if (color == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Color con código hex '{codigoHex}' no encontrado",
                        Origen = "ColorController",
                        Metodo = "GetColorPorCodigoHex",
                        DatosEntrada = JsonSerializer.Serialize(new { codigoHex }),
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
                    Mensaje = $"Color con código hex '{codigoHex}' consultado exitosamente",
                    Origen = "ColorController",
                    Metodo = "GetColorPorCodigoHex",
                    DatosEntrada = JsonSerializer.Serialize(new { codigoHex }),
                    DatosSalida = JsonSerializer.Serialize(color),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(color);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar color con código hex '{codigoHex}': {ex.Message}",
                    Origen = "ColorController",
                    Metodo = "GetColorPorCodigoHex",
                    DatosEntrada = JsonSerializer.Serialize(new { codigoHex }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/color
        [HttpPost]
        public async Task<ActionResult> PostColor([FromBody] ColorCreateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "Datos de color nulos",
                        Origen = "ColorController",
                        Metodo = "PostColor",
                        DatosEntrada = "Request: null",
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("Los datos del color son requeridos");
                }

                var color = new Colore
                {
                    ColNombre = request.ColNombre,
                    ColCodigoHex = request.ColCodigoHex
                };

                var id = await _colorService.InsertarColorAsync(color);
                
                // Registrar log de creación exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CREACION",
                    Mensaje = $"Color creado exitosamente con ID {id}",
                    Origen = "ColorController",
                    Metodo = "PostColor",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    DatosSalida = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(GetColor), new { id }, new ColorResponse 
                { 
                    ColorId = id, 
                    ColNombre = color.ColNombre, 
                    ColCodigoHex = color.ColCodigoHex 
                });
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al crear color: {ex.Message}",
                    Origen = "ColorController",
                    Metodo = "PostColor",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/color/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutColor(int id, [FromBody] ColorUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "Datos de color nulos",
                        Origen = "ColorController",
                        Metodo = "PutColor",
                        DatosEntrada = $"ID: {id}, Request: null",
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("Los datos del color son requeridos");
                }

                if (id != request.ColorId)
                {
                    // Registrar log de error de validación
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "El ID de la URL no coincide con el ID del color",
                        Origen = "ColorController",
                        Metodo = "PutColor",
                        DatosEntrada = JsonSerializer.Serialize(new { id, colorId = request.ColorId }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("El ID no coincide.");
                }

                var color = new Colore
                {
                    ColorId = request.ColorId,
                    ColNombre = request.ColNombre,
                    ColCodigoHex = request.ColCodigoHex
                };

                var actualizado = await _colorService.ActualizarColorAsync(color);
                if (!actualizado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Color con ID {id} no encontrado para actualizar",
                        Origen = "ColorController",
                        Metodo = "PutColor",
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
                    Mensaje = $"Color con ID {id} actualizado exitosamente",
                    Origen = "ColorController",
                    Metodo = "PutColor",
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
                    Mensaje = $"Error al actualizar color con ID {id}: {ex.Message}",
                    Origen = "ColorController",
                    Metodo = "PutColor",
                    DatosEntrada = JsonSerializer.Serialize(new { id, request }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/color/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteColor(int id)
        {
            try
            {
                var eliminado = await _colorService.EliminarColorAsync(id);
                if (!eliminado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Color con ID {id} no encontrado para eliminar",
                        Origen = "ColorController",
                        Metodo = "DeleteColor",
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
                    Mensaje = $"Color con ID {id} eliminado exitosamente",
                    Origen = "ColorController",
                    Metodo = "DeleteColor",
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
                    Mensaje = $"Error al eliminar color con ID {id}: {ex.Message}",
                    Origen = "ColorController",
                    Metodo = "DeleteColor",
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