using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Ecommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponController : ControllerBase
    {
        private readonly ICuponService _cuponService;
        private readonly ILogService _logService;

        public CuponController(ICuponService cuponService, ILogService logService)
        {
            _cuponService = cuponService;
            _logService = logService;
        }

        // GET: api/cupon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupone>>> GetCupones()
        {
            try
            {
                var cupones = await _cuponService.ListarCuponesAsync();
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Lista de cupones consultada exitosamente",
                    Origen = "CuponController",
                    Metodo = "GetCupones",
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
                    Mensaje = $"Error al consultar cupones: {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "GetCupones",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cupon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cupone>> GetCupon(int id)
        {
            try
            {
                var cupon = await _cuponService.ObtenerPorIdAsync(id);
                if (cupon == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Cupón con ID {id} no encontrado",
                        Origen = "CuponController",
                        Metodo = "GetCupon",
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
                    Mensaje = $"Cupón con ID {id} consultado exitosamente",
                    Origen = "CuponController",
                    Metodo = "GetCupon",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    DatosSalida = JsonSerializer.Serialize(cupon),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(cupon);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar cupón con ID {id}: {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "GetCupon",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cupon/codigo/{codigo}
        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<Cupone>> GetCuponPorCodigo(string codigo)
        {
            try
            {
                var cupon = await _cuponService.ObtenerPorCodigoAsync(codigo);
                if (cupon == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Cupón con código '{codigo}' no encontrado",
                        Origen = "CuponController",
                        Metodo = "GetCuponPorCodigo",
                        DatosEntrada = JsonSerializer.Serialize(new { codigo }),
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
                    Mensaje = $"Cupón con código '{codigo}' consultado exitosamente",
                    Origen = "CuponController",
                    Metodo = "GetCuponPorCodigo",
                    DatosEntrada = JsonSerializer.Serialize(new { codigo }),
                    DatosSalida = JsonSerializer.Serialize(cupon),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(cupon);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al consultar cupón con código '{codigo}': {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "GetCuponPorCodigo",
                    DatosEntrada = JsonSerializer.Serialize(new { codigo }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cupon/activos
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<Cupone>>> GetCuponesActivos()
        {
            try
            {
                var cupones = await _cuponService.ListarCuponesActivosAsync();
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Cupones activos consultados exitosamente",
                    Origen = "CuponController",
                    Metodo = "GetCuponesActivos",
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
                    Mensaje = $"Error al consultar cupones activos: {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "GetCuponesActivos",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cupon/fecha/{fecha}
        [HttpGet("fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<Cupone>>> GetCuponesPorFecha(DateTime fecha)
        {
            try
            {
                var cupones = await _cuponService.ListarCuponesPorFechaAsync(fecha);
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Cupones por fecha '{fecha:yyyy-MM-dd}' consultados exitosamente",
                    Origen = "CuponController",
                    Metodo = "GetCuponesPorFecha",
                    DatosEntrada = JsonSerializer.Serialize(new { fecha }),
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
                    Mensaje = $"Error al consultar cupones por fecha '{fecha:yyyy-MM-dd}': {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "GetCuponesPorFecha",
                    DatosEntrada = JsonSerializer.Serialize(new { fecha }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/cupon/tipo/{tipoDescuento}
        [HttpGet("tipo/{tipoDescuento}")]
        public async Task<ActionResult<IEnumerable<Cupone>>> GetCuponesPorTipo(string tipoDescuento)
        {
            try
            {
                var cupones = await _cuponService.ListarCuponesPorTipoAsync(tipoDescuento);
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Cupones por tipo '{tipoDescuento}' consultados exitosamente",
                    Origen = "CuponController",
                    Metodo = "GetCuponesPorTipo",
                    DatosEntrada = JsonSerializer.Serialize(new { tipoDescuento }),
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
                    Mensaje = $"Error al consultar cupones por tipo '{tipoDescuento}': {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "GetCuponesPorTipo",
                    DatosEntrada = JsonSerializer.Serialize(new { tipoDescuento }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/cupon/validar
        [HttpPost("validar")]
        public async Task<ActionResult> ValidarCupon([FromBody] ValidarCuponRequest request)
        {
            try
            {
                var esValido = await _cuponService.ValidarCuponAsync(request.Codigo, request.MontoCompra);
                
                // Registrar log de validación
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "VALIDACION",
                    Mensaje = $"Cupón '{request.Codigo}' validado: {(esValido ? "Válido" : "Inválido")}",
                    Origen = "CuponController",
                    Metodo = "ValidarCupon",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    DatosSalida = JsonSerializer.Serialize(new { esValido }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new ValidarCuponResponse 
                { 
                    EsValido = esValido,
                    Mensaje = esValido ? "Cupón válido" : "Cupón inválido"
                });
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al validar cupón '{request.Codigo}': {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "ValidarCupon",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/cupon
        [HttpPost]
        public async Task<ActionResult> PostCupon([FromBody] CuponCreateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "Datos de cupón nulos",
                        Origen = "CuponController",
                        Metodo = "PostCupon",
                        DatosEntrada = "Request: null",
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("Los datos del cupón son requeridos");
                }

                var cupon = new Cupone
                {
                    Codigo = request.Codigo,
                    Descripcion = request.Descripcion,
                    TipoDescuento = request.TipoDescuento,
                    ValorDescuento = request.ValorDescuento,
                    MontoMinimo = request.MontoMinimo,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin,
                    LimiteUso = request.LimiteUso,
                    UsosRealizados = 0,
                    Activo = request.Activo ?? true
                };

                var id = await _cuponService.InsertarCuponAsync(cupon);
                
                // Registrar log de creación exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CREACION",
                    Mensaje = $"Cupón creado exitosamente con ID {id}",
                    Origen = "CuponController",
                    Metodo = "PostCupon",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    DatosSalida = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(GetCupon), new { id }, new CuponResponse
                {
                    CuponId = id,
                    Codigo = cupon.Codigo,
                    Descripcion = cupon.Descripcion,
                    TipoDescuento = cupon.TipoDescuento,
                    ValorDescuento = cupon.ValorDescuento,
                    MontoMinimo = cupon.MontoMinimo,
                    FechaInicio = cupon.FechaInicio,
                    FechaFin = cupon.FechaFin,
                    LimiteUso = cupon.LimiteUso,
                    UsosRealizados = cupon.UsosRealizados,
                    Activo = cupon.Activo
                });
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al crear cupón: {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "PostCupon",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/cupon/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCupon(int id, [FromBody] CuponUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "Datos de cupón nulos",
                        Origen = "CuponController",
                        Metodo = "PutCupon",
                        DatosEntrada = $"ID: {id}, Request: null",
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("Los datos del cupón son requeridos");
                }

                if (id != request.CuponId)
                {
                    // Registrar log de error de validación
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "ERROR_VALIDACION",
                        Mensaje = "El ID de la URL no coincide con el ID del cupón",
                        Origen = "CuponController",
                        Metodo = "PutCupon",
                        DatosEntrada = JsonSerializer.Serialize(new { id, cuponId = request.CuponId }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest("El ID no coincide.");
                }

                var cupon = new Cupone
                {
                    CuponId = request.CuponId,
                    Codigo = request.Codigo,
                    Descripcion = request.Descripcion,
                    TipoDescuento = request.TipoDescuento,
                    ValorDescuento = request.ValorDescuento,
                    MontoMinimo = request.MontoMinimo,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin,
                    LimiteUso = request.LimiteUso,
                    Activo = request.Activo
                };

                var actualizado = await _cuponService.ActualizarCuponAsync(cupon);
                if (!actualizado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Cupón con ID {id} no encontrado para actualizar",
                        Origen = "CuponController",
                        Metodo = "PutCupon",
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
                    Mensaje = $"Cupón con ID {id} actualizado exitosamente",
                    Origen = "CuponController",
                    Metodo = "PutCupon",
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
                    Mensaje = $"Error al actualizar cupón con ID {id}: {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "PutCupon",
                    DatosEntrada = JsonSerializer.Serialize(new { id, request }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/cupon/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCupon(int id)
        {
            try
            {
                var eliminado = await _cuponService.EliminarCuponAsync(id);
                if (!eliminado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Cupón con ID {id} no encontrado para eliminar",
                        Origen = "CuponController",
                        Metodo = "DeleteCupon",
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
                    Mensaje = $"Cupón con ID {id} eliminado exitosamente",
                    Origen = "CuponController",
                    Metodo = "DeleteCupon",
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
                    Mensaje = $"Error al eliminar cupón con ID {id}: {ex.Message}",
                    Origen = "CuponController",
                    Metodo = "DeleteCupon",
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