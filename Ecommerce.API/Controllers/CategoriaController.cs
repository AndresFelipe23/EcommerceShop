using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly ILogService _logService;

        public CategoriaController(ICategoriaService categoriaService, ILogService logService)
        {
            _categoriaService = categoriaService;
            _logService = logService;
        }

        // GET: api/categoria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetCategorias()
        {
            try
            {
                var categorias = await _categoriaService.ListarCategoriasAsync();
                var response = categorias.Select(c => new CategoriaResponse {
                    CatId = c.CatId,
                    CatNombre = c.CatNombre,
                    CatDescripcion = c.CatDescripcion,
                    CatPadreId = c.CatPadreId,
                    CatPadreNombre = c.CatPadre?.CatNombre,
                    CatOrden = c.CatOrden,
                    CatActivo = c.CatActivo,
                    CatFechaCreacion = c.CatFechaCreacion
                });
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Lista de categorías consultada exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "GetCategorias",
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
                    Mensaje = $"Error al consultar categorías: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "GetCategorias",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/categoria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponse>> GetCategoria(int id)
        {
            try
            {
                var categoria = await _categoriaService.ObtenerPorIdAsync(id);
                if (categoria == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Categoría con ID {id} no encontrada",
                        Origen = "CategoriaController",
                        Metodo = "GetCategoria",
                        DatosEntrada = JsonSerializer.Serialize(new { id }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }
                var response = new CategoriaResponse {
                    CatId = categoria.CatId,
                    CatNombre = categoria.CatNombre,
                    CatDescripcion = categoria.CatDescripcion,
                    CatPadreId = categoria.CatPadreId,
                    CatPadreNombre = categoria.CatPadre?.CatNombre,
                    CatOrden = categoria.CatOrden,
                    CatActivo = categoria.CatActivo,
                    CatFechaCreacion = categoria.CatFechaCreacion
                };
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Categoría con ID {id} consultada exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoria",
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
                    Mensaje = $"Error al consultar categoría con ID {id}: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoria",
                    DatosEntrada = JsonSerializer.Serialize(new { id }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/categoria/nombre/{nombre}
        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<CategoriaResponse>> GetCategoriaPorNombre(string nombre)
        {
            try
            {
                var categoria = await _categoriaService.ObtenerPorNombreAsync(nombre);
                if (categoria == null)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Categoría con nombre '{nombre}' no encontrada",
                        Origen = "CategoriaController",
                        Metodo = "GetCategoriaPorNombre",
                        DatosEntrada = JsonSerializer.Serialize(new { nombre }),
                        Fecha = DateTime.Now,
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound();
                }
                var response = new CategoriaResponse {
                    CatId = categoria.CatId,
                    CatNombre = categoria.CatNombre,
                    CatDescripcion = categoria.CatDescripcion,
                    CatPadreId = categoria.CatPadreId,
                    CatPadreNombre = categoria.CatPadre?.CatNombre,
                    CatOrden = categoria.CatOrden,
                    CatActivo = categoria.CatActivo,
                    CatFechaCreacion = categoria.CatFechaCreacion
                };
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Categoría con nombre '{nombre}' consultada exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoriaPorNombre",
                    DatosEntrada = JsonSerializer.Serialize(new { nombre }),
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
                    Mensaje = $"Error al consultar categoría con nombre '{nombre}': {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoriaPorNombre",
                    DatosEntrada = JsonSerializer.Serialize(new { nombre }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/categoria/activas
        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetCategoriasActivas()
        {
            try
            {
                var categorias = await _categoriaService.ListarCategoriasActivasAsync();
                var response = categorias.Select(c => new CategoriaResponse {
                    CatId = c.CatId,
                    CatNombre = c.CatNombre,
                    CatDescripcion = c.CatDescripcion,
                    CatPadreId = c.CatPadreId,
                    CatPadreNombre = c.CatPadre?.CatNombre,
                    CatOrden = c.CatOrden,
                    CatActivo = c.CatActivo,
                    CatFechaCreacion = c.CatFechaCreacion
                });
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Categorías activas consultadas exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoriasActivas",
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
                    Mensaje = $"Error al consultar categorías activas: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoriasActivas",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/categoria/padres
        [HttpGet("padres")]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetCategoriasPadre()
        {
            try
            {
                var categorias = await _categoriaService.ListarCategoriasPadreAsync();
                var response = categorias.Select(c => new CategoriaResponse {
                    CatId = c.CatId,
                    CatNombre = c.CatNombre,
                    CatDescripcion = c.CatDescripcion,
                    CatPadreId = c.CatPadreId,
                    CatPadreNombre = c.CatPadre?.CatNombre,
                    CatOrden = c.CatOrden,
                    CatActivo = c.CatActivo,
                    CatFechaCreacion = c.CatFechaCreacion
                });
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Categorías padre consultadas exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoriasPadre",
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
                    Mensaje = $"Error al consultar categorías padre: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoriasPadre",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/categoria/subcategorias/{categoriaPadreId}
        [HttpGet("subcategorias/{categoriaPadreId}")]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetSubcategorias(int categoriaPadreId)
        {
            try
            {
                var categorias = await _categoriaService.ListarSubcategoriasAsync(categoriaPadreId);
                var response = categorias.Select(c => new CategoriaResponse {
                    CatId = c.CatId,
                    CatNombre = c.CatNombre,
                    CatDescripcion = c.CatDescripcion,
                    CatPadreId = c.CatPadreId,
                    CatPadreNombre = c.CatPadre?.CatNombre,
                    CatOrden = c.CatOrden,
                    CatActivo = c.CatActivo,
                    CatFechaCreacion = c.CatFechaCreacion
                });
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = $"Subcategorías de categoría padre {categoriaPadreId} consultadas exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "GetSubcategorias",
                    DatosEntrada = JsonSerializer.Serialize(new { categoriaPadreId }),
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
                    Mensaje = $"Error al consultar subcategorías de categoría padre {categoriaPadreId}: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "GetSubcategorias",
                    DatosEntrada = JsonSerializer.Serialize(new { categoriaPadreId }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/categoria/con-productos
        [HttpGet("con-productos")]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetCategoriasConProductos()
        {
            try
            {
                var categorias = await _categoriaService.ListarCategoriasConProductosAsync();
                var response = categorias.Select(c => new CategoriaResponse {
                    CatId = c.CatId,
                    CatNombre = c.CatNombre,
                    CatDescripcion = c.CatDescripcion,
                    CatPadreId = c.CatPadreId,
                    CatPadreNombre = c.CatPadre?.CatNombre,
                    CatOrden = c.CatOrden,
                    CatActivo = c.CatActivo,
                    CatFechaCreacion = c.CatFechaCreacion
                });
                
                // Registrar log de consulta exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CONSULTA",
                    Mensaje = "Categorías con productos consultadas exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoriasConProductos",
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
                    Mensaje = $"Error al consultar categorías con productos: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "GetCategoriasConProductos",
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/categoria
        [HttpPost]
        public async Task<ActionResult> PostCategoria([FromBody] CategoriaRequest request)
        {
            try
            {
                var categoria = new Categoria {
                    CatNombre = request.CatNombre,
                    CatDescripcion = request.CatDescripcion,
                    CatPadreId = request.CatPadreId,
                    CatOrden = request.CatOrden,
                    CatActivo = request.CatActivo,
                    CatFechaCreacion = DateTime.Now
                };
                var id = await _categoriaService.InsertarCategoriaAsync(categoria);
                var creada = await _categoriaService.ObtenerPorIdAsync(id);
                var response = new CategoriaResponse {
                    CatId = creada.CatId,
                    CatNombre = creada.CatNombre,
                    CatDescripcion = creada.CatDescripcion,
                    CatPadreId = creada.CatPadreId,
                    CatPadreNombre = creada.CatPadre?.CatNombre,
                    CatOrden = creada.CatOrden,
                    CatActivo = creada.CatActivo,
                    CatFechaCreacion = creada.CatFechaCreacion
                };
                
                // Registrar log de creación exitosa
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "CREACION",
                    Mensaje = $"Categoría creada exitosamente con ID {id}",
                    Origen = "CategoriaController",
                    Metodo = "PostCategoria",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    DatosSalida = JsonSerializer.Serialize(response),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(GetCategoria), new { id = response.CatId }, response);
            }
            catch (Exception ex)
            {
                // Registrar log de error
                await _logService.InsertarLogAsync(new Log
                {
                    Tipo = "ERROR",
                    Mensaje = $"Error al crear categoría: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "PostCategoria",
                    DatosEntrada = JsonSerializer.Serialize(request),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/categoria/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCategoria(int id, [FromBody] CategoriaUpdateRequest request)
        {
            try
            {
                var categoria = new Categoria {
                    CatId = id,
                    CatNombre = request.CatNombre,
                    CatDescripcion = request.CatDescripcion,
                    CatPadreId = request.CatPadreId,
                    CatOrden = request.CatOrden,
                    CatActivo = request.CatActivo
                };
                var actualizado = await _categoriaService.ActualizarCategoriaAsync(categoria);
                if (!actualizado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Categoría con ID {id} no encontrada para actualizar",
                        Origen = "CategoriaController",
                        Metodo = "PutCategoria",
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
                    Mensaje = $"Categoría con ID {id} actualizada exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "PutCategoria",
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
                    Mensaje = $"Error al actualizar categoría con ID {id}: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "PutCategoria",
                    DatosEntrada = JsonSerializer.Serialize(new { id, request }),
                    Fecha = DateTime.Now,
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/categoria/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoria(int id)
        {
            try
            {
                var eliminado = await _categoriaService.EliminarCategoriaAsync(id);
                if (!eliminado)
                {
                    // Registrar log de no encontrado
                    await _logService.InsertarLogAsync(new Log
                    {
                        Tipo = "NO_ENCONTRADO",
                        Mensaje = $"Categoría con ID {id} no encontrada para eliminar",
                        Origen = "CategoriaController",
                        Metodo = "DeleteCategoria",
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
                    Mensaje = $"Categoría con ID {id} eliminada exitosamente",
                    Origen = "CategoriaController",
                    Metodo = "DeleteCategoria",
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
                    Mensaje = $"Error al eliminar categoría con ID {id}: {ex.Message}",
                    Origen = "CategoriaController",
                    Metodo = "DeleteCategoria",
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