using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;


namespace Ecommerce.PRC.Services
{
    public class ProductoImagenService : IProductoImagenService
    {
        private readonly IProductoImagenRepository _productoImagenRepository;
        private readonly ILogService _logService;

        public ProductoImagenService(IProductoImagenRepository productoImagenRepository, ILogService logService)
        {
            _productoImagenRepository = productoImagenRepository;
            _logService = logService;
        }

        public async Task<IEnumerable<ProductoImagene>> ListarImagenesAsync()
        {
            try
            {
                var imagenes = await _productoImagenRepository.ListarAsync();
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {imagenes.Count()} imágenes de productos", Origen = "ProductoImagenService", Metodo = "ListarImagenesAsync" });
                return imagenes;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar imágenes: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ListarImagenesAsync" });
                throw;
            }
        }

        public async Task<ProductoImagene?> ObtenerImagenAsync(int imgId)
        {
            try
            {
                if (imgId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de imagen inválido: {imgId}", Origen = "ProductoImagenService", Metodo = "ObtenerImagenAsync" });
                    return null;
                }

                var imagen = await _productoImagenRepository.ObtenerPorIdAsync(imgId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Imagen obtenida: {imgId}", Origen = "ProductoImagenService", Metodo = "ObtenerImagenAsync" });
                return imagen;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener imagen {imgId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ObtenerImagenAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<ProductoImagene>> ListarPorProductoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoImagenService", Metodo = "ListarPorProductoAsync" });
                    return Enumerable.Empty<ProductoImagene>();
                }

                var imagenes = await _productoImagenRepository.ListarPorProductoAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Listadas {imagenes.Count()} imágenes del producto {proId}", Origen = "ProductoImagenService", Metodo = "ListarPorProductoAsync" });
                return imagenes;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar imágenes del producto {proId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ListarPorProductoAsync" });
                throw;
            }
        }

        public async Task<int> CrearImagenAsync(ProductoImagene productoImagen)
        {
            try
            {
                if (!await ValidarImagenAsync(productoImagen))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Validación de imagen fallida", Origen = "ProductoImagenService", Metodo = "CrearImagenAsync" });
                    return 0;
                }

                var imgId = await _productoImagenRepository.CrearAsync(productoImagen);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Imagen creada con ID: {imgId}", Origen = "ProductoImagenService", Metodo = "CrearImagenAsync" });
                return imgId;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al crear imagen: {ex.Message}", Origen = "ProductoImagenService", Metodo = "CrearImagenAsync" });
                throw;
            }
        }

        public async Task<bool> ActualizarImagenAsync(ProductoImagene productoImagen)
        {
            try
            {
                if (productoImagen.ImgId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "ID de imagen inválido para actualización", Origen = "ProductoImagenService", Metodo = "ActualizarImagenAsync" });
                    return false;
                }

                if (!await ValidarImagenAsync(productoImagen))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Validación de imagen fallida en actualización", Origen = "ProductoImagenService", Metodo = "ActualizarImagenAsync" });
                    return false;
                }

                var resultado = await _productoImagenRepository.ActualizarAsync(productoImagen);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Imagen {productoImagen.ImgId} actualizada: {resultado}", Origen = "ProductoImagenService", Metodo = "ActualizarImagenAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al actualizar imagen {productoImagen.ImgId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ActualizarImagenAsync" });
                throw;
            }
        }

        public async Task<bool> EliminarImagenAsync(int imgId)
        {
            try
            {
                if (imgId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de imagen inválido para eliminación: {imgId}", Origen = "ProductoImagenService", Metodo = "EliminarImagenAsync" });
                    return false;
                }

                var resultado = await _productoImagenRepository.EliminarAsync(imgId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Imagen {imgId} eliminada: {resultado}", Origen = "ProductoImagenService", Metodo = "EliminarImagenAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al eliminar imagen {imgId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "EliminarImagenAsync" });
                throw;
            }
        }

        public async Task<bool> EliminarImagenesPorProductoAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido para eliminación de imágenes: {proId}", Origen = "ProductoImagenService", Metodo = "EliminarImagenesPorProductoAsync" });
                    return false;
                }

                var resultado = await _productoImagenRepository.EliminarPorProductoAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Imágenes del producto {proId} eliminadas: {resultado}", Origen = "ProductoImagenService", Metodo = "EliminarImagenesPorProductoAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al eliminar imágenes del producto {proId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "EliminarImagenesPorProductoAsync" });
                throw;
            }
        }

        public async Task<int> ObtenerCantidadImagenesAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoImagenService", Metodo = "ObtenerCantidadImagenesAsync" });
                    return 0;
                }

                var cantidad = await _productoImagenRepository.ObtenerCantidadImagenesAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Cantidad de imágenes del producto {proId}: {cantidad}", Origen = "ProductoImagenService", Metodo = "ObtenerCantidadImagenesAsync" });
                return cantidad;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener cantidad de imágenes del producto {proId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ObtenerCantidadImagenesAsync" });
                throw;
            }
        }

        public async Task<ProductoImagene?> ObtenerImagenPrincipalAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoImagenService", Metodo = "ObtenerImagenPrincipalAsync" });
                    return null;
                }

                var imagen = await _productoImagenRepository.ObtenerImagenPrincipalAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Imagen principal del producto {proId} obtenida", Origen = "ProductoImagenService", Metodo = "ObtenerImagenPrincipalAsync" });
                return imagen;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al obtener imagen principal del producto {proId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ObtenerImagenPrincipalAsync" });
                throw;
            }
        }

        public async Task<bool> ActualizarOrdenImagenAsync(int imgId, int nuevoOrden)
        {
            try
            {
                if (imgId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de imagen inválido: {imgId}", Origen = "ProductoImagenService", Metodo = "ActualizarOrdenImagenAsync" });
                    return false;
                }

                if (nuevoOrden < 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"Orden inválido: {nuevoOrden}", Origen = "ProductoImagenService", Metodo = "ActualizarOrdenImagenAsync" });
                    return false;
                }

                var resultado = await _productoImagenRepository.ActualizarOrdenAsync(imgId, nuevoOrden);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Orden de imagen {imgId} actualizado a {nuevoOrden}: {resultado}", Origen = "ProductoImagenService", Metodo = "ActualizarOrdenImagenAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al actualizar orden de imagen {imgId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ActualizarOrdenImagenAsync" });
                throw;
            }
        }

        public async Task<IEnumerable<ProductoImagene>> ListarPorOrdenAsync(int proId)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoImagenService", Metodo = "ListarPorOrdenAsync" });
                    return Enumerable.Empty<ProductoImagene>();
                }

                var imagenes = await _productoImagenRepository.ListarPorOrdenAsync(proId);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Imágenes del producto {proId} listadas por orden", Origen = "ProductoImagenService", Metodo = "ListarPorOrdenAsync" });
                return imagenes;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al listar imágenes por orden del producto {proId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ListarPorOrdenAsync" });
                throw;
            }
        }

        public async Task<bool> ValidarImagenAsync(ProductoImagene productoImagen)
        {
            try
            {
                if (productoImagen == null)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Imagen es null", Origen = "ProductoImagenService", Metodo = "ValidarImagenAsync" });
                    return false;
                }

                if (productoImagen.ProId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "ID de producto inválido", Origen = "ProductoImagenService", Metodo = "ValidarImagenAsync" });
                    return false;
                }

                if (string.IsNullOrWhiteSpace(productoImagen.ImgUrl))
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "URL de imagen requerida", Origen = "ProductoImagenService", Metodo = "ValidarImagenAsync" });
                    return false;
                }

                if (productoImagen.ImgUrl.Length > 255)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "URL de imagen demasiado larga", Origen = "ProductoImagenService", Metodo = "ValidarImagenAsync" });
                    return false;
                }

                if (productoImagen.ImgOrden < 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Orden de imagen inválido", Origen = "ProductoImagenService", Metodo = "ValidarImagenAsync" });
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(productoImagen.ImgAlt) && productoImagen.ImgAlt.Length > 150)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Texto alternativo demasiado largo", Origen = "ProductoImagenService", Metodo = "ValidarImagenAsync" });
                    return false;
                }

                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = "Imagen validada correctamente", Origen = "ProductoImagenService", Metodo = "ValidarImagenAsync" });
                return true;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error en validación de imagen: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ValidarImagenAsync" });
                return false;
            }
        }

        public async Task<bool> EstablecerImagenPrincipalAsync(int imgId, int proId)
        {
            try
            {
                if (imgId <= 0 || proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"IDs inválidos: imgId={imgId}, proId={proId}", Origen = "ProductoImagenService", Metodo = "EstablecerImagenPrincipalAsync" });
                    return false;
                }

                // Primero, establecer todas las imágenes del producto con orden > 0
                var imagenes = await _productoImagenRepository.ListarPorProductoAsync(proId);
                foreach (var imagen in imagenes)
                {
                    if (imagen.ImgId != imgId && imagen.ImgOrden == 0)
                    {
                        await _productoImagenRepository.ActualizarOrdenAsync(imagen.ImgId, 1);
                    }
                }

                // Luego, establecer la imagen seleccionada como principal (orden = 0)
                var resultado = await _productoImagenRepository.ActualizarOrdenAsync(imgId, 0);
                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Imagen {imgId} establecida como principal del producto {proId}: {resultado}", Origen = "ProductoImagenService", Metodo = "EstablecerImagenPrincipalAsync" });
                return resultado;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al establecer imagen principal {imgId} del producto {proId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "EstablecerImagenPrincipalAsync" });
                throw;
            }
        }

        public async Task<bool> ReordenarImagenesAsync(int proId, List<int> ordenImagenes)
        {
            try
            {
                if (proId <= 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = $"ID de producto inválido: {proId}", Origen = "ProductoImagenService", Metodo = "ReordenarImagenesAsync" });
                    return false;
                }

                if (ordenImagenes == null || ordenImagenes.Count == 0)
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Lista de orden de imágenes vacía", Origen = "ProductoImagenService", Metodo = "ReordenarImagenesAsync" });
                    return false;
                }

                var imagenes = await _productoImagenRepository.ListarPorProductoAsync(proId);
                if (ordenImagenes.Count != imagenes.Count())
                {
                    await _logService.InsertarLogAsync(new Log { Tipo = "Warning", Mensaje = "Cantidad de imágenes no coincide con el orden proporcionado", Origen = "ProductoImagenService", Metodo = "ReordenarImagenesAsync" });
                    return false;
                }

                bool todosExitosos = true;
                for (int i = 0; i < ordenImagenes.Count; i++)
                {
                    var resultado = await _productoImagenRepository.ActualizarOrdenAsync(ordenImagenes[i], i);
                    if (!resultado)
                    {
                        todosExitosos = false;
                    }
                }

                await _logService.InsertarLogAsync(new Log { Tipo = "Info", Mensaje = $"Reordenamiento de imágenes del producto {proId} completado: {todosExitosos}", Origen = "ProductoImagenService", Metodo = "ReordenarImagenesAsync" });
                return todosExitosos;
            }
            catch (Exception ex)
            {
                await _logService.InsertarLogAsync(new Log { Tipo = "Error", Mensaje = $"Error al reordenar imágenes del producto {proId}: {ex.Message}", Origen = "ProductoImagenService", Metodo = "ReordenarImagenesAsync" });
                throw;
            }
        }
    }
} 