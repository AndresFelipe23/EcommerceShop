using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TallaController : ControllerBase
    {
        private readonly ITallaService _tallaService;
        private readonly ILogService _logService;

        public TallaController(ITallaService tallaService, ILogService logService)
        {
            _tallaService = tallaService;
            _logService = logService;
        }

        // GET: api/talla
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Talla>>> GetTallas()
        {
            try
            {
                var tallas = await _tallaService.ListarTallasAsync();
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Lista de tallas consultada exitosamente",
                    Origen = "TallaController",
                    Metodo = "GetTallas",
                    DatosSalida = JsonSerializer.Serialize(tallas),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(tallas);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar tallas: {ex.Message}",
                    Origen = "TallaController",
                    Metodo = "GetTallas",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/talla/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Talla>> GetTalla(int id)
        {
            try
            {
                var talla = await _tallaService.ObtenerPorIdAsync(id);
                if (talla == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Talla con ID {id} no encontrada",
                        Origen = "TallaController",
                        Metodo = "GetTalla",
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
                    Mensaje = $"Talla con ID {id} consultada exitosamente",
                    Origen = "TallaController",
                    Metodo = "GetTalla",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    DatosSalida = JsonSerializer.Serialize(talla),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(talla);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar talla con ID {id}: {ex.Message}",
                    Origen = "TallaController",
                    Metodo = "GetTalla",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/talla/genero/{genero}
        [HttpGet("genero/{genero}")]
        public async Task<ActionResult<IEnumerable<Talla>>> GetTallasPorGenero(string genero)
        {
            try
            {
                var tallas = await _tallaService.ListarTallasPorGeneroAsync(genero);
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Tallas por género '{genero}' consultadas exitosamente",
                    Origen = "TallaController",
                    Metodo = "GetTallasPorGenero",
                    DatosEntrada = JsonSerializer.Serialize(new { genero }),
                    DatosSalida = JsonSerializer.Serialize(tallas),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(tallas);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar tallas por género '{genero}': {ex.Message}",
                    Origen = "TallaController",
                    Metodo = "GetTallasPorGenero",
                    DatosEntrada = JsonSerializer.Serialize(new { genero }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/talla
        [HttpPost]
        public async Task<ActionResult> PostTalla([FromBody] CrearTallaDTO tallaDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Mapear DTO a modelo
                var talla = new Talla
                {
                    TalNombre = tallaDto.TalNombre,
                    TalGenero = tallaDto.TalGenero,
                    TalOrdenVisualizacion = tallaDto.TalOrdenVisualizacion
                };

                var id = await _tallaService.InsertarTallaAsync(talla);
                
                // Registrar log de creación exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CREACION",
                    Mensaje = $"Talla creada exitosamente con ID {id}",
                    Origen = "TallaController",
                    Metodo = "PostTalla",
                    DatosEntrada = JsonSerializer.Serialize(tallaDto),
                    DatosSalida = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(GetTalla), new { id }, talla);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al crear talla: {ex.Message}",
                    Origen = "TallaController",
                    Metodo = "PostTalla",
                    DatosEntrada = JsonSerializer.Serialize(tallaDto),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/talla/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTalla(int id, [FromBody] ActualizarTallaDTO tallaDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Obtener la talla existente
                var tallaExistente = await _tallaService.ObtenerPorIdAsync(id);
                if (tallaExistente == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Talla con ID {id} no encontrada para actualizar",
                        Origen = "TallaController",
                        Metodo = "PutTalla",
                        DatosEntrada = JsonSerializer.Serialize(new { id, tallaDto }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }

                // Actualizar solo los campos proporcionados
                if (tallaDto.TalNombre != null)
                    tallaExistente.TalNombre = tallaDto.TalNombre;
                if (tallaDto.TalGenero != null)
                    tallaExistente.TalGenero = tallaDto.TalGenero;
                if (tallaDto.TalOrdenVisualizacion.HasValue)
                    tallaExistente.TalOrdenVisualizacion = tallaDto.TalOrdenVisualizacion;

                var actualizado = await _tallaService.ActualizarTallaAsync(tallaExistente);
                if (!actualizado)
                {
                    // Registrar log de error en actualización
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR",
                        Mensaje = $"Error al actualizar talla con ID {id}",
                        Origen = "TallaController",
                        Metodo = "PutTalla",
                        DatosEntrada = JsonSerializer.Serialize(new { id, tallaDto }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return StatusCode(500, "Error al actualizar la talla");
                }

                // Registrar log de actualización exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ACTUALIZACION",
                    Mensaje = $"Talla con ID {id} actualizada exitosamente",
                    Origen = "TallaController",
                    Metodo = "PutTalla",
                    DatosEntrada = JsonSerializer.Serialize(new { id, tallaDto }),
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
                    Mensaje = $"Error al actualizar talla con ID {id}: {ex.Message}",
                    Origen = "TallaController",
                    Metodo = "PutTalla",
                    DatosEntrada = JsonSerializer.Serialize(new { id, tallaDto }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/talla/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTalla(int id)
        {
            try
            {
                var eliminado = await _tallaService.EliminarTallaAsync(id);
                if (!eliminado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Talla con ID {id} no encontrada para eliminar",
                        Origen = "TallaController",
                        Metodo = "DeleteTalla",
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
                    Mensaje = $"Talla con ID {id} eliminada exitosamente",
                    Origen = "TallaController",
                    Metodo = "DeleteTalla",
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
                    Mensaje = $"Error al eliminar talla con ID {id}: {ex.Message}",
                    Origen = "TallaController",
                    Metodo = "DeleteTalla",
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