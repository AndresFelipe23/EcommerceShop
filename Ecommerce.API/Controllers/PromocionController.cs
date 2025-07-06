using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        private readonly IPromocionService _promocionService;
        private readonly ILogService _logService;

        public PromocionController(IPromocionService promocionService, ILogService logService)
        {
            _promocionService = promocionService;
            _logService = logService;
        }

        // GET: api/promocion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promocione>>> GetPromociones()
        {
            try
            {
                var promociones = await _promocionService.ListarPromocionesAsync();
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Lista de promociones consultada exitosamente",
                    Origen = "PromocionController",
                    Metodo = "GetPromociones",
                    DatosSalida = JsonSerializer.Serialize(promociones),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(promociones);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar promociones: {ex.Message}",
                    Origen = "PromocionController",
                    Metodo = "GetPromociones",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/promocion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Promocione>> GetPromocion(int id)
        {
            try
            {
                var promocion = await _promocionService.ObtenerPorIdAsync(id);
                if (promocion == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Promoción con ID {id} no encontrada",
                        Origen = "PromocionController",
                        Metodo = "GetPromocion",
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
                    Mensaje = $"Promoción con ID {id} consultada exitosamente",
                    Origen = "PromocionController",
                    Metodo = "GetPromocion",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    DatosSalida = JsonSerializer.Serialize(promocion),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(promocion);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar promoción con ID {id}: {ex.Message}",
                    Origen = "PromocionController",
                    Metodo = "GetPromocion",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/promocion/activas
        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<Promocione>>> GetPromocionesActivas()
        {
            try
            {
                var promociones = await _promocionService.ListarPromocionesActivasAsync();
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Promociones activas consultadas exitosamente",
                    Origen = "PromocionController",
                    Metodo = "GetPromocionesActivas",
                    DatosSalida = JsonSerializer.Serialize(promociones),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(promociones);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar promociones activas: {ex.Message}",
                    Origen = "PromocionController",
                    Metodo = "GetPromocionesActivas",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/promocion/fecha/{fecha}
        [HttpGet("fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<Promocione>>> GetPromocionesPorFecha(DateTime fecha)
        {
            try
            {
                var promociones = await _promocionService.ListarPromocionesPorFechaAsync(fecha);
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Promociones por fecha '{fecha:yyyy-MM-dd}' consultadas exitosamente",
                    Origen = "PromocionController",
                    Metodo = "GetPromocionesPorFecha",
                    DatosEntrada = JsonSerializer.Serialize(new { fecha }),
                    DatosSalida = JsonSerializer.Serialize(promociones),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(promociones);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar promociones por fecha '{fecha:yyyy-MM-dd}': {ex.Message}",
                    Origen = "PromocionController",
                    Metodo = "GetPromocionesPorFecha",
                    DatosEntrada = JsonSerializer.Serialize(new { fecha }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/promocion/tipo/{tipoDescuento}
        [HttpGet("tipo/{tipoDescuento}")]
        public async Task<ActionResult<IEnumerable<Promocione>>> GetPromocionesPorTipo(string tipoDescuento)
        {
            try
            {
                var promociones = await _promocionService.ListarPromocionesPorTipoAsync(tipoDescuento);
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Promociones por tipo '{tipoDescuento}' consultadas exitosamente",
                    Origen = "PromocionController",
                    Metodo = "GetPromocionesPorTipo",
                    DatosEntrada = JsonSerializer.Serialize(new { tipoDescuento }),
                    DatosSalida = JsonSerializer.Serialize(promociones),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(promociones);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar promociones por tipo '{tipoDescuento}': {ex.Message}",
                    Origen = "PromocionController",
                    Metodo = "GetPromocionesPorTipo",
                    DatosEntrada = JsonSerializer.Serialize(new { tipoDescuento }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/promocion
        [HttpPost]
        public async Task<ActionResult> PostPromocion([FromBody] CrearPromocionDTO promocionDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Mapear DTO a modelo
                var promocion = new Promocione
                {
                    Nombre = promocionDto.Nombre,
                    Descripcion = promocionDto.Descripcion,
                    TipoDescuento = promocionDto.TipoDescuento,
                    ValorDescuento = promocionDto.ValorDescuento,
                    FechaInicio = promocionDto.FechaInicio,
                    FechaFin = promocionDto.FechaFin,
                    Activo = promocionDto.Activo
                };

                var id = await _promocionService.InsertarPromocionAsync(promocion);
                
                // Registrar log de creación exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CREACION",
                    Mensaje = $"Promoción creada exitosamente con ID {id}",
                    Origen = "PromocionController",
                    Metodo = "PostPromocion",
                    DatosEntrada = JsonSerializer.Serialize(promocionDto),
                    DatosSalida = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(GetPromocion), new { id }, promocion);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al crear promoción: {ex.Message}",
                    Origen = "PromocionController",
                    Metodo = "PostPromocion",
                    DatosEntrada = JsonSerializer.Serialize(promocionDto),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/promocion/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPromocion(int id, [FromBody] ActualizarPromocionDTO promocionDto)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Obtener la promoción existente
                var promocionExistente = await _promocionService.ObtenerPorIdAsync(id);
                if (promocionExistente == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Promoción con ID {id} no encontrada para actualizar",
                        Origen = "PromocionController",
                        Metodo = "PutPromocion",
                        DatosEntrada = JsonSerializer.Serialize(new { id, promocionDto }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }

                // Actualizar solo los campos proporcionados
                if (promocionDto.Nombre != null)
                    promocionExistente.Nombre = promocionDto.Nombre;
                if (promocionDto.Descripcion != null)
                    promocionExistente.Descripcion = promocionDto.Descripcion;
                if (promocionDto.TipoDescuento != null)
                    promocionExistente.TipoDescuento = promocionDto.TipoDescuento;
                if (promocionDto.ValorDescuento.HasValue)
                    promocionExistente.ValorDescuento = promocionDto.ValorDescuento;
                if (promocionDto.FechaInicio.HasValue)
                    promocionExistente.FechaInicio = promocionDto.FechaInicio;
                if (promocionDto.FechaFin.HasValue)
                    promocionExistente.FechaFin = promocionDto.FechaFin;
                if (promocionDto.Activo.HasValue)
                    promocionExistente.Activo = promocionDto.Activo;

                var actualizado = await _promocionService.ActualizarPromocionAsync(promocionExistente);
                if (!actualizado)
                {
                    // Registrar log de error en actualización
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR",
                        Mensaje = $"Error al actualizar promoción con ID {id}",
                        Origen = "PromocionController",
                        Metodo = "PutPromocion",
                        DatosEntrada = JsonSerializer.Serialize(new { id, promocionDto }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return StatusCode(500, "Error al actualizar la promoción");
                }

                // Registrar log de actualización exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ACTUALIZACION",
                    Mensaje = $"Promoción con ID {id} actualizada exitosamente",
                    Origen = "PromocionController",
                    Metodo = "PutPromocion",
                    DatosEntrada = JsonSerializer.Serialize(new { id, promocionDto }),
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
                    Mensaje = $"Error al actualizar promoción con ID {id}: {ex.Message}",
                    Origen = "PromocionController",
                    Metodo = "PutPromocion",
                    DatosEntrada = JsonSerializer.Serialize(new { id, promocionDto }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/promocion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePromocion(int id)
        {
            try
            {
                var eliminado = await _promocionService.EliminarPromocionAsync(id);
                if (!eliminado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Promoción con ID {id} no encontrada para eliminar",
                        Origen = "PromocionController",
                        Metodo = "DeletePromocion",
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
                    Mensaje = $"Promoción con ID {id} eliminada exitosamente",
                    Origen = "PromocionController",
                    Metodo = "DeletePromocion",
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
                    Mensaje = $"Error al eliminar promoción con ID {id}: {ex.Message}",
                    Origen = "PromocionController",
                    Metodo = "DeletePromocion",
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