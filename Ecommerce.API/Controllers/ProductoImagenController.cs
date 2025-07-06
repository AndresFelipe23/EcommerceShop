using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Ecommerce.API.Models;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoImagenController : ControllerBase
    {
        private readonly IProductoImagenService _productoImagenService;
        private readonly ILogService _logService;

        public ProductoImagenController(IProductoImagenService productoImagenService, ILogService logService)
        {
            _productoImagenService = productoImagenService;
            _logService = logService;
        }

        /// <summary>
        /// Obtiene todas las imágenes de productos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoImagene>>> GetImagenes()
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud GET /api/ProductoImagen",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenes"
                });
                
                var imagenes = await _productoImagenService.ListarImagenesAsync();
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {imagenes.Count()} imágenes",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenes"
                });
                return Ok(imagenes);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoImagen: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenes"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una imagen por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoImagene>> GetImagen(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoImagen/{id}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagen"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "GetImagen"
                    });
                    return BadRequest(new { mensaje = "ID de imagen inválido" });
                }

                var imagen = await _productoImagenService.ObtenerImagenAsync(id);
                
                if (imagen == null)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"Imagen no encontrada: {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "GetImagen"
                    });
                    return NotFound(new { mensaje = "Imagen no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Imagen {id} obtenida exitosamente",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagen"
                });
                return Ok(imagen);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoImagen/{id}: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagen"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene imágenes por producto
        /// </summary>
        [HttpGet("producto/{proId}")]
        public async Task<ActionResult<IEnumerable<ProductoImagene>>> GetImagenesPorProducto(int proId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoImagen/producto/{proId}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenesPorProducto"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoImagenController",
                        Metodo = "GetImagenesPorProducto"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var imagenes = await _productoImagenService.ListarPorProductoAsync(proId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {imagenes.Count()} imágenes del producto {proId}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenesPorProducto"
                });
                return Ok(imagenes);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoImagen/producto/{proId}: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenesPorProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la imagen principal de un producto
        /// </summary>
        [HttpGet("producto/{proId}/principal")]
        public async Task<ActionResult<ProductoImagene>> GetImagenPrincipal(int proId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoImagen/producto/{proId}/principal",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenPrincipal"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoImagenController",
                        Metodo = "GetImagenPrincipal"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var imagen = await _productoImagenService.ObtenerImagenPrincipalAsync(proId);
                
                if (imagen == null)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"Imagen principal no encontrada para producto {proId}",
                        Origen = "ProductoImagenController",
                        Metodo = "GetImagenPrincipal"
                    });
                    return NotFound(new { mensaje = "Imagen principal no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Imagen principal del producto {proId} obtenida",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenPrincipal"
                });
                return Ok(imagen);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoImagen/producto/{proId}/principal: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenPrincipal"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene imágenes ordenadas de un producto
        /// </summary>
        [HttpGet("producto/{proId}/ordenadas")]
        public async Task<ActionResult<IEnumerable<ProductoImagene>>> GetImagenesOrdenadas(int proId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoImagen/producto/{proId}/ordenadas",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenesOrdenadas"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoImagenController",
                        Metodo = "GetImagenesOrdenadas"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var imagenes = await _productoImagenService.ListarPorOrdenAsync(proId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Retornadas {imagenes.Count()} imágenes ordenadas del producto {proId}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenesOrdenadas"
                });
                return Ok(imagenes);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoImagen/producto/{proId}/ordenadas: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetImagenesOrdenadas"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la cantidad de imágenes de un producto
        /// </summary>
        [HttpGet("producto/{proId}/cantidad")]
        public async Task<ActionResult<int>> GetCantidadImagenes(int proId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud GET /api/ProductoImagen/producto/{proId}/cantidad",
                    Origen = "ProductoImagenController",
                    Metodo = "GetCantidadImagenes"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoImagenController",
                        Metodo = "GetCantidadImagenes"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var cantidad = await _productoImagenService.ObtenerCantidadImagenesAsync(proId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Cantidad de imágenes del producto {proId}: {cantidad}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetCantidadImagenes"
                });
                return Ok(cantidad);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en GET /api/ProductoImagen/producto/{proId}/cantidad: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "GetCantidadImagenes"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva imagen de producto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductoImagene>> CrearImagen([FromBody] CrearProductoImagenDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = "Solicitud POST /api/ProductoImagen",
                    Origen = "ProductoImagenController",
                    Metodo = "CrearImagen"
                });
                
                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en creación de imagen",
                        Origen = "ProductoImagenController",
                        Metodo = "CrearImagen"
                    });
                    return BadRequest(ModelState);
                }

                var productoImagen = new ProductoImagene
                {
                    ProId = request.ProId,
                    ImgUrl = request.ImgUrl,
                    ImgOrden = request.ImgOrden,
                    ImgAlt = request.ImgAlt,
                    ImgFechaCreacion = DateTime.Now
                };

                var imgId = await _productoImagenService.CrearImagenAsync(productoImagen);
                
                if (imgId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "No se pudo crear la imagen",
                        Origen = "ProductoImagenController",
                        Metodo = "CrearImagen"
                    });
                    return BadRequest(new { mensaje = "No se pudo crear la imagen" });
                }

                productoImagen.ImgId = imgId;
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Imagen creada exitosamente con ID: {imgId}",
                    Origen = "ProductoImagenController",
                    Metodo = "CrearImagen"
                });
                return CreatedAtAction(nameof(GetImagen), new { id = imgId }, productoImagen);
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en POST /api/ProductoImagen: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "CrearImagen"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una imagen existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarImagen(int id, [FromBody] ActualizarProductoImagenDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PUT /api/ProductoImagen/{id}",
                    Origen = "ProductoImagenController",
                    Metodo = "ActualizarImagen"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para actualización: {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "ActualizarImagen"
                    });
                    return BadRequest(new { mensaje = "ID de imagen inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en actualización de imagen",
                        Origen = "ProductoImagenController",
                        Metodo = "ActualizarImagen"
                    });
                    return BadRequest(ModelState);
                }

                var productoImagen = new ProductoImagene
                {
                    ImgId = id,
                    ProId = request.ProId,
                    ImgUrl = request.ImgUrl,
                    ImgOrden = request.ImgOrden,
                    ImgAlt = request.ImgAlt
                };

                var resultado = await _productoImagenService.ActualizarImagenAsync(productoImagen);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar la imagen {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "ActualizarImagen"
                    });
                    return NotFound(new { mensaje = "Imagen no encontrada o no se pudo actualizar" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Imagen {id} actualizada exitosamente",
                    Origen = "ProductoImagenController",
                    Metodo = "ActualizarImagen"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PUT /api/ProductoImagen/{id}: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "ActualizarImagen"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una imagen
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarImagen(int id)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud DELETE /api/ProductoImagen/{id}",
                    Origen = "ProductoImagenController",
                    Metodo = "EliminarImagen"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido para eliminación: {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "EliminarImagen"
                    });
                    return BadRequest(new { mensaje = "ID de imagen inválido" });
                }

                var resultado = await _productoImagenService.EliminarImagenAsync(id);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo eliminar la imagen {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "EliminarImagen"
                    });
                    return NotFound(new { mensaje = "Imagen no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Imagen {id} eliminada exitosamente",
                    Origen = "ProductoImagenController",
                    Metodo = "EliminarImagen"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en DELETE /api/ProductoImagen/{id}: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "EliminarImagen"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina todas las imágenes de un producto
        /// </summary>
        [HttpDelete("producto/{proId}")]
        public async Task<IActionResult> EliminarImagenesPorProducto(int proId)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud DELETE /api/ProductoImagen/producto/{proId}",
                    Origen = "ProductoImagenController",
                    Metodo = "EliminarImagenesPorProducto"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoImagenController",
                        Metodo = "EliminarImagenesPorProducto"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                var resultado = await _productoImagenService.EliminarImagenesPorProductoAsync(proId);
                
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Imágenes del producto {proId} eliminadas: {resultado}",
                    Origen = "ProductoImagenController",
                    Metodo = "EliminarImagenesPorProducto"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en DELETE /api/ProductoImagen/producto/{proId}: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "EliminarImagenesPorProducto"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza el orden de una imagen
        /// </summary>
        [HttpPatch("{id}/orden")]
        public async Task<IActionResult> ActualizarOrdenImagen(int id, [FromBody] ActualizarOrdenImagenDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/ProductoImagen/{id}/orden",
                    Origen = "ProductoImagenController",
                    Metodo = "ActualizarOrdenImagen"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "ActualizarOrdenImagen"
                    });
                    return BadRequest(new { mensaje = "ID de imagen inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en actualización de orden",
                        Origen = "ProductoImagenController",
                        Metodo = "ActualizarOrdenImagen"
                    });
                    return BadRequest(ModelState);
                }

                var resultado = await _productoImagenService.ActualizarOrdenImagenAsync(id, request.NuevoOrden);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar el orden de la imagen {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "ActualizarOrdenImagen"
                    });
                    return NotFound(new { mensaje = "Imagen no encontrada" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Orden de imagen {id} actualizado a {request.NuevoOrden}",
                    Origen = "ProductoImagenController",
                    Metodo = "ActualizarOrdenImagen"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/ProductoImagen/{id}/orden: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "ActualizarOrdenImagen"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Establece una imagen como principal
        /// </summary>
        [HttpPatch("{id}/principal")]
        public async Task<IActionResult> EstablecerImagenPrincipal(int id, [FromBody] EstablecerImagenPrincipalDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/ProductoImagen/{id}/principal",
                    Origen = "ProductoImagenController",
                    Metodo = "EstablecerImagenPrincipal"
                });
                
                if (id <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID inválido: {id}",
                        Origen = "ProductoImagenController",
                        Metodo = "EstablecerImagenPrincipal"
                    });
                    return BadRequest(new { mensaje = "ID de imagen inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en establecimiento de imagen principal",
                        Origen = "ProductoImagenController",
                        Metodo = "EstablecerImagenPrincipal"
                    });
                    return BadRequest(ModelState);
                }

                var resultado = await _productoImagenService.EstablecerImagenPrincipalAsync(id, request.ProId);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo establecer la imagen {id} como principal",
                        Origen = "ProductoImagenController",
                        Metodo = "EstablecerImagenPrincipal"
                    });
                    return NotFound(new { mensaje = "Imagen o producto no encontrado" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Imagen {id} establecida como principal del producto {request.ProId}",
                    Origen = "ProductoImagenController",
                    Metodo = "EstablecerImagenPrincipal"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/ProductoImagen/{id}/principal: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "EstablecerImagenPrincipal"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Reordena las imágenes de un producto
        /// </summary>
        [HttpPatch("producto/{proId}/reordenar")]
        public async Task<IActionResult> ReordenarImagenes(int proId, [FromBody] ReordenarImagenesDto request)
        {
            try
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Solicitud PATCH /api/ProductoImagen/producto/{proId}/reordenar",
                    Origen = "ProductoImagenController",
                    Metodo = "ReordenarImagenes"
                });
                
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"ID de producto inválido: {proId}",
                        Origen = "ProductoImagenController",
                        Metodo = "ReordenarImagenes"
                    });
                    return BadRequest(new { mensaje = "ID de producto inválido" });
                }

                if (!ModelState.IsValid)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = "Modelo inválido en reordenamiento",
                        Origen = "ProductoImagenController",
                        Metodo = "ReordenarImagenes"
                    });
                    return BadRequest(ModelState);
                }

                var resultado = await _productoImagenService.ReordenarImagenesAsync(proId, request.OrdenImagenes);
                
                if (!resultado)
                {
                    await _logService.InsertarLogAsync(new Log {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo reordenar las imágenes del producto {proId}",
                        Origen = "ProductoImagenController",
                        Metodo = "ReordenarImagenes"
                    });
                    return BadRequest(new { mensaje = "No se pudo reordenar las imágenes" });
                }

                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Info",
                    Mensaje = $"Imágenes del producto {proId} reordenadas exitosamente",
                    Origen = "ProductoImagenController",
                    Metodo = "ReordenarImagenes"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log {
                    Tipo = "Error",
                    Mensaje = $"Error en PATCH /api/ProductoImagen/producto/{proId}/reordenar: {ex.Message}",
                    Origen = "ProductoImagenController",
                    Metodo = "ReordenarImagenes"
                });
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }
    }
} 