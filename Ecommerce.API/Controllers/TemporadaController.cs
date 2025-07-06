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
    public class TemporadaController : ControllerBase
    {
        private readonly ITemporadaService _temporadaService;
        private readonly ILogRepository _logRepository;

        public TemporadaController(ITemporadaService temporadaService, ILogRepository logRepository)
        {
            _temporadaService = temporadaService;
            _logRepository = logRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var temporadas = await _temporadaService.ListarTemporadasAsync();
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Listado de temporadas exitoso. Total: {temporadas.Count()}",
                    Origen = "TemporadaController",
                    Metodo = "Listar",
                    DatosSalida = $"Total temporadas: {temporadas.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });
                return Ok(temporadas);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar temporadas: {ex.Message}",
                    Origen = "TemporadaController",
                    Metodo = "Listar",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor al listar temporadas" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { mensaje = "El ID debe ser mayor a 0" });

                var temporada = await _temporadaService.ObtenerPorIdAsync(id);
                if (temporada == null)
                    return NotFound(new { mensaje = $"Temporada con ID {id} no encontrada" });

                return Ok(temporada);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener temporada: {ex.Message}",
                    Origen = "TemporadaController",
                    Metodo = "ObtenerPorId",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearTemporadaDTO temporadaDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (temporadaDto == null)
                    return BadRequest(new { mensaje = "Los datos de la temporada son requeridos" });

                // Validar que la fecha de fin sea posterior a la fecha de inicio
                if (temporadaDto.FechaFin <= temporadaDto.FechaInicio)
                {
                    return BadRequest(new { mensaje = "La fecha de fin debe ser posterior a la fecha de inicio" });
                }

                // Mapear DTO a modelo
                var temporada = new Temporada
                {
                    Nombre = temporadaDto.Nombre,
                    Descripcion = temporadaDto.Descripcion,
                    FechaInicio = temporadaDto.FechaInicio,
                    FechaFin = temporadaDto.FechaFin,
                    Activo = temporadaDto.Activo,
                    FechaCreacion = DateTime.Now
                };

                var id = await _temporadaService.InsertarTemporadaAsync(temporada);
                
                // Registrar log de creación exitosa
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Temporada creada exitosamente con ID {id}",
                    Origen = "TemporadaController",
                    Metodo = "Crear",
                    DatosEntrada = $"Nombre: {temporadaDto.Nombre}, FechaInicio: {temporadaDto.FechaInicio:yyyy-MM-dd}, FechaFin: {temporadaDto.FechaFin:yyyy-MM-dd}",
                    DatosSalida = $"ID: {id}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(ObtenerPorId), new { id }, new { mensaje = "Temporada creada exitosamente", id });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al crear temporada: {ex.Message}",
                    Origen = "TemporadaController",
                    Metodo = "Crear",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarTemporadaDTO temporadaDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (temporadaDto == null)
                    return BadRequest(new { mensaje = "Los datos de la temporada son requeridos" });

                // Obtener la temporada existente
                var temporadaExistente = await _temporadaService.ObtenerPorIdAsync(id);
                if (temporadaExistente == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Error",
                        Mensaje = $"Temporada con ID {id} no encontrada para actualizar",
                        Origen = "TemporadaController",
                        Metodo = "Actualizar",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });
                    return NotFound(new { mensaje = $"Temporada con ID {id} no encontrada" });
                }

                // Validar fechas si se proporcionan ambas
                if (temporadaDto.FechaInicio.HasValue && temporadaDto.FechaFin.HasValue)
                {
                    if (temporadaDto.FechaFin.Value <= temporadaDto.FechaInicio.Value)
                    {
                        return BadRequest(new { mensaje = "La fecha de fin debe ser posterior a la fecha de inicio" });
                    }
                }

                // Actualizar solo los campos proporcionados
                if (temporadaDto.Nombre != null)
                    temporadaExistente.Nombre = temporadaDto.Nombre;
                if (temporadaDto.Descripcion != null)
                    temporadaExistente.Descripcion = temporadaDto.Descripcion;
                if (temporadaDto.FechaInicio.HasValue)
                    temporadaExistente.FechaInicio = temporadaDto.FechaInicio.Value;
                if (temporadaDto.FechaFin.HasValue)
                    temporadaExistente.FechaFin = temporadaDto.FechaFin.Value;
                if (temporadaDto.Activo.HasValue)
                    temporadaExistente.Activo = temporadaDto.Activo;

                var actualizado = await _temporadaService.ActualizarTemporadaAsync(temporadaExistente);
                if (!actualizado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Error",
                        Mensaje = $"Error al actualizar temporada con ID {id}",
                        Origen = "TemporadaController",
                        Metodo = "Actualizar",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { mensaje = "Error al actualizar la temporada" });
                }

                // Registrar log de actualización exitosa
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Temporada con ID {id} actualizada exitosamente",
                    Origen = "TemporadaController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"ID: {id}, Nombre: {temporadaDto.Nombre ?? "No cambiado"}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Temporada actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al actualizar temporada: {ex.Message}",
                    Origen = "TemporadaController",
                    Metodo = "Actualizar",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await _temporadaService.EliminarTemporadaAsync(id);
                if (!eliminado)
                    return NotFound(new { mensaje = $"No se pudo eliminar la temporada con ID {id}" });

                return Ok(new { mensaje = "Temporada eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al eliminar temporada: {ex.Message}",
                    Origen = "TemporadaController",
                    Metodo = "Eliminar",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });
                return StatusCode((int)HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor" });
            }
        }
    }
} 