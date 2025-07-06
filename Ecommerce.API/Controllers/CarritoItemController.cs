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
    public class CarritoItemController : ControllerBase
    {
        private readonly ICarritoItemService _carritoItemService;
        private readonly ILogRepository _logRepository;

        public CarritoItemController(ICarritoItemService carritoItemService, ILogRepository logRepository)
        {
            _carritoItemService = carritoItemService;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene todos los items del carrito
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var items = await _carritoItemService.ListarAsync();
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Listado de items del carrito exitoso. Total: {items.Count()}",
                    Origen = "CarritoItemController",
                    Metodo = "Listar",
                    DatosSalida = $"Total items: {items.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(items);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar items del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Listar",
                    DatosEntrada = "Sin parámetros",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar items del carrito" });
            }
        }

        /// <summary>
        /// Obtiene un item del carrito por su ID
        /// </summary>
        [HttpGet("{carritoItemId}")]
        public async Task<IActionResult> ObtenerPorId(int carritoItemId)
        {
            try
            {
                if (carritoItemId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener item del carrito con ID inválido",
                        Origen = "CarritoItemController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del item del carrito debe ser mayor a 0" });
                }

                var item = await _carritoItemService.ObtenerPorIdAsync(carritoItemId);
                
                if (item == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"Item del carrito con ID {carritoItemId} no encontrado",
                        Origen = "CarritoItemController",
                        Metodo = "ObtenerPorId",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"Item del carrito con ID {carritoItemId} no encontrado" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Item del carrito con ID {carritoItemId} obtenido exitosamente",
                    Origen = "CarritoItemController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    DatosSalida = $"Carrito: {item.CarritoId}, Producto: {item.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(item);
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener item del carrito con ID {carritoItemId}: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ObtenerPorId",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el item del carrito" });
            }
        }

        /// <summary>
        /// Obtiene los items de un carrito específico
        /// </summary>
        [HttpGet("carrito/{carritoId}")]
        public async Task<IActionResult> ListarPorCarrito(int carritoId)
        {
            try
            {
                if (carritoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de listar items con ID de carrito inválido",
                        Origen = "CarritoItemController",
                        Metodo = "ListarPorCarrito",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del carrito debe ser mayor a 0" });
                }

                var items = await _carritoItemService.ListarPorCarritoAsync(carritoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Items del carrito {carritoId} listados exitosamente. Total: {items.Count()}",
                    Origen = "CarritoItemController",
                    Metodo = "ListarPorCarrito",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    DatosSalida = $"Total items: {items.Count()}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(items);
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al listar items del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ListarPorCarrito",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al listar items del carrito {carritoId}: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ListarPorCarrito",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al listar los items del carrito" });
            }
        }

        /// <summary>
        /// Obtiene el detalle de un item del carrito
        /// </summary>
        [HttpGet("{carritoItemId}/detalle")]
        public async Task<IActionResult> ObtenerDetalle(int carritoItemId)
        {
            try
            {
                if (carritoItemId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener detalle de item con ID inválido",
                        Origen = "CarritoItemController",
                        Metodo = "ObtenerDetalle",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del item del carrito debe ser mayor a 0" });
                }

                var detalle = await _carritoItemService.ObtenerDetalleItemAsync(carritoItemId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Detalle del item {carritoItemId} obtenido exitosamente",
                    Origen = "CarritoItemController",
                    Metodo = "ObtenerDetalle",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(detalle);
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al obtener detalle del item: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ObtenerDetalle",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener detalle del item {carritoItemId}: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ObtenerDetalle",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener el detalle del item" });
            }
        }

        /// <summary>
        /// Crea un nuevo item en el carrito
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CarritoItem carritoItem)
        {
            try
            {
                if (carritoItem == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de crear item del carrito con datos nulos",
                        Origen = "CarritoItemController",
                        Metodo = "Crear",
                        DatosEntrada = "CarritoItem: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos del item del carrito son requeridos" });
                }

                var carritoItemId = await _carritoItemService.CrearAsync(carritoItem);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Item del carrito creado exitosamente con ID {carritoItemId}",
                    Origen = "CarritoItemController",
                    Metodo = "Crear",
                    DatosEntrada = $"Carrito: {carritoItem.CarritoId}, Producto: {carritoItem.ProductoTallaColorId}",
                    DatosSalida = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return CreatedAtAction(nameof(ObtenerPorId), new { carritoItemId }, 
                    new { mensaje = "Item del carrito creado exitosamente", carritoItemId });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al crear item del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Crear",
                    DatosEntrada = $"Carrito: {carritoItem?.CarritoId}, Producto: {carritoItem?.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al crear item del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Crear",
                    DatosEntrada = $"Carrito: {carritoItem?.CarritoId}, Producto: {carritoItem?.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al crear item del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Crear",
                    DatosEntrada = $"Carrito: {carritoItem?.CarritoId}, Producto: {carritoItem?.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al crear el item del carrito" });
            }
        }

        /// <summary>
        /// Agrega un producto al carrito
        /// </summary>
        [HttpPost("agregar-producto")]
        public async Task<IActionResult> AgregarProducto([FromBody] AgregarProductoRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de agregar producto con datos nulos",
                        Origen = "CarritoItemController",
                        Metodo = "AgregarProducto",
                        DatosEntrada = "Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos son requeridos" });
                }

                var resultado = await _carritoItemService.AgregarProductoAsync(
                    request.CarritoId, 
                    request.ProductoTallaColorId, 
                    request.Cantidad, 
                    request.PrecioUnitario
                );
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Producto agregado al carrito {request.CarritoId} exitosamente",
                    Origen = "CarritoItemController",
                    Metodo = "AgregarProducto",
                    DatosEntrada = $"Carrito: {request.CarritoId}, Producto: {request.ProductoTallaColorId}, Cantidad: {request.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Producto agregado al carrito exitosamente", resultado });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al agregar producto: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "AgregarProducto",
                    DatosEntrada = $"Carrito: {request?.CarritoId}, Producto: {request?.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al agregar producto: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "AgregarProducto",
                    DatosEntrada = $"Carrito: {request?.CarritoId}, Producto: {request?.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al agregar producto al carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "AgregarProducto",
                    DatosEntrada = $"Carrito: {request?.CarritoId}, Producto: {request?.ProductoTallaColorId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al agregar el producto al carrito" });
            }
        }

        /// <summary>
        /// Actualiza un item del carrito
        /// </summary>
        [HttpPut("{carritoItemId}")]
        public async Task<IActionResult> Actualizar(int carritoItemId, [FromBody] CarritoItem carritoItem)
        {
            try
            {
                if (carritoItem == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de actualizar item del carrito con datos nulos",
                        Origen = "CarritoItemController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}, CarritoItem: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos del item del carrito son requeridos" });
                }

                if (carritoItemId != carritoItem.CarritoItemId)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "ID de item del carrito en URL no coincide con el del cuerpo",
                        Origen = "CarritoItemController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"CarritoItemId URL: {carritoItemId}, CarritoItemId Body: {carritoItem.CarritoItemId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del item del carrito en la URL no coincide con el del cuerpo" });
                }

                var resultado = await _carritoItemService.ActualizarAsync(carritoItem);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar el item del carrito con ID {carritoItemId}",
                        Origen = "CarritoItemController",
                        Metodo = "Actualizar",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}, Carrito: {carritoItem.CarritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo actualizar el item del carrito con ID {carritoItemId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Item del carrito con ID {carritoItemId} actualizado exitosamente",
                    Origen = "CarritoItemController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}, Carrito: {carritoItem.CarritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Item del carrito actualizado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al actualizar item del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}, Carrito: {carritoItem?.CarritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al actualizar item del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}, Carrito: {carritoItem?.CarritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al actualizar item del carrito con ID {carritoItemId}: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Actualizar",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}, Carrito: {carritoItem?.CarritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al actualizar el item del carrito" });
            }
        }

        /// <summary>
        /// Actualiza la cantidad de un item del carrito
        /// </summary>
        [HttpPut("{carritoItemId}/cantidad")]
        public async Task<IActionResult> ActualizarCantidad(int carritoItemId, [FromBody] ActualizarCantidadRequest request)
        {
            try
            {
                if (request == null)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de actualizar cantidad con datos nulos",
                        Origen = "CarritoItemController",
                        Metodo = "ActualizarCantidad",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}, Request: null",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "Los datos son requeridos" });
                }

                var resultado = await _carritoItemService.ActualizarCantidadAsync(carritoItemId, request.Cantidad);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo actualizar la cantidad del item {carritoItemId}",
                        Origen = "CarritoItemController",
                        Metodo = "ActualizarCantidad",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}, Cantidad: {request.Cantidad}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo actualizar la cantidad del item {carritoItemId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Cantidad del item {carritoItemId} actualizada exitosamente a {request.Cantidad}",
                    Origen = "CarritoItemController",
                    Metodo = "ActualizarCantidad",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}, Cantidad: {request.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Cantidad actualizada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al actualizar cantidad: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ActualizarCantidad",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al actualizar cantidad: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ActualizarCantidad",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al actualizar cantidad del item {carritoItemId}: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ActualizarCantidad",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}, Cantidad: {request?.Cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al actualizar la cantidad" });
            }
        }

        /// <summary>
        /// Elimina un item del carrito
        /// </summary>
        [HttpDelete("{carritoItemId}")]
        public async Task<IActionResult> Eliminar(int carritoItemId)
        {
            try
            {
                if (carritoItemId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de eliminar item del carrito con ID inválido",
                        Origen = "CarritoItemController",
                        Metodo = "Eliminar",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del item del carrito debe ser mayor a 0" });
                }

                var resultado = await _carritoItemService.EliminarAsync(carritoItemId);
                
                if (!resultado)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = $"No se pudo eliminar el item del carrito con ID {carritoItemId}",
                        Origen = "CarritoItemController",
                        Metodo = "Eliminar",
                        DatosEntrada = $"CarritoItemId: {carritoItemId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return NotFound(new { mensaje = $"No se pudo eliminar el item del carrito con ID {carritoItemId}" });
                }

                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Item del carrito con ID {carritoItemId} eliminado exitosamente",
                    Origen = "CarritoItemController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { mensaje = "Item del carrito eliminado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de validación al eliminar item del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al eliminar item del carrito: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al eliminar item del carrito con ID {carritoItemId}: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "Eliminar",
                    DatosEntrada = $"CarritoItemId: {carritoItemId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al eliminar el item del carrito" });
            }
        }

        /// <summary>
        /// Obtiene la cantidad total de productos en un carrito
        /// </summary>
        [HttpGet("carrito/{carritoId}/cantidad-productos")]
        public async Task<IActionResult> ObtenerCantidadProductos(int carritoId)
        {
            try
            {
                if (carritoId <= 0)
                {
                    await _logRepository.InsertarLogAsync(new Log
                    {
                        Tipo = "Warning",
                        Mensaje = "Intento de obtener cantidad de productos con ID de carrito inválido",
                        Origen = "CarritoItemController",
                        Metodo = "ObtenerCantidadProductos",
                        DatosEntrada = $"CarritoId: {carritoId}",
                        Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                    });

                    return BadRequest(new { mensaje = "El ID del carrito debe ser mayor a 0" });
                }

                var cantidad = await _carritoItemService.ObtenerCantidadProductosAsync(carritoId);
                
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Info",
                    Mensaje = $"Cantidad de productos del carrito {carritoId} obtenida exitosamente: {cantidad}",
                    Origen = "CarritoItemController",
                    Metodo = "ObtenerCantidadProductos",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    DatosSalida = $"Cantidad: {cantidad}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return Ok(new { carritoId, cantidadProductos = cantidad });
            }
            catch (InvalidOperationException ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Warning",
                    Mensaje = $"Error de operación al obtener cantidad de productos: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ObtenerCantidadProductos",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                await _logRepository.InsertarLogAsync(new Log
                {
                    Tipo = "Error",
                    Mensaje = $"Error al obtener cantidad de productos del carrito {carritoId}: {ex.Message}",
                    Origen = "CarritoItemController",
                    Metodo = "ObtenerCantidadProductos",
                    DatosEntrada = $"CarritoId: {carritoId}",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
                });

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new { mensaje = "Error interno del servidor al obtener la cantidad de productos" });
            }
        }
    }

    // Clases de request para operaciones específicas
    public class ActualizarCantidadRequest
    {
        public int Cantidad { get; set; }
    }
} 