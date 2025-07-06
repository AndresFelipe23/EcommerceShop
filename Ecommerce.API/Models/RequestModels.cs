using System.ComponentModel.DataAnnotations;

namespace Ecommerce.API.Models
{
    public class AgregarProductoRequest
    {
        public int PedidoId { get; set; }
        public int CarritoId { get; set; }
        public int ProductoTallaColorId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class ActualizarCantidadRequest
    {
        public int NuevaCantidad { get; set; }
    }

    public class RolCreateRequest
    {
        public string RolNombre { get; set; } = null!;
    }

    public class RolUpdateRequest
    {
        public int RolId { get; set; }
        public string RolNombre { get; set; } = null!;
    }

    public class RolResponse
    {
        public int RolId { get; set; }
        public string RolNombre { get; set; } = null!;
    }

    public class ProductoRequest
    {
        public string ProNombre { get; set; } = string.Empty;
        public string? ProDescripcion { get; set; }
        public decimal ProPrecio { get; set; }
        public string? ProImagenPrincipal { get; set; }
        public string? ProGenero { get; set; }
        public int ProCategoriaId { get; set; }
    }

    public class ProductoUpdateRequest
    {
        public string ProNombre { get; set; } = string.Empty;
        public string? ProDescripcion { get; set; }
        public decimal ProPrecio { get; set; }
        public string? ProImagenPrincipal { get; set; }
        public string? ProGenero { get; set; }
        public int ProCategoriaId { get; set; }
        public bool ProActivo { get; set; } = true;
    }

    public class ProductoResponse
    {
        public int ProId { get; set; }
        public string ProNombre { get; set; } = string.Empty;
        public string? ProDescripcion { get; set; }
        public decimal ProPrecio { get; set; }
        public string? ProImagenPrincipal { get; set; }
        public string? ProGenero { get; set; }
        public int ProCategoriaId { get; set; }
        public string? ProCategoriaNombre { get; set; }
        public bool? ProActivo { get; set; }
        public DateTime? ProFechaCreacion { get; set; }
        public DateTime? ProFechaActualizacion { get; set; }
    }

    public class CategoriaRequest
    {
        public string CatNombre { get; set; } = string.Empty;
        public string? CatDescripcion { get; set; }
        public int? CatPadreId { get; set; }
        public int? CatOrden { get; set; }
        public bool? CatActivo { get; set; } = true;
    }

    public class CategoriaUpdateRequest
    {
        public string CatNombre { get; set; } = string.Empty;
        public string? CatDescripcion { get; set; }
        public int? CatPadreId { get; set; }
        public int? CatOrden { get; set; }
        public bool? CatActivo { get; set; } = true;
    }

    public class CategoriaResponse
    {
        public int CatId { get; set; }
        public string CatNombre { get; set; } = string.Empty;
        public string? CatDescripcion { get; set; }
        public int? CatPadreId { get; set; }
        public string? CatPadreNombre { get; set; }
        public int? CatOrden { get; set; }
        public bool? CatActivo { get; set; }
        public DateTime? CatFechaCreacion { get; set; }
    }

    public class CarritoCreateRequest
    {
        public int UsuId { get; set; }
    }

    public class CarritoUpdateRequest
    {
        public int CarritoId { get; set; }
        public int UsuId { get; set; }
    }

    public class CarritoResponse
    {
        public int CarritoId { get; set; }
        public int UsuId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalPrecio { get; set; }
    }

    public class CarritoResumenResponse
    {
        public int CarritoId { get; set; }
        public int UsuId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public List<CarritoItemResumenResponse> Items { get; set; } = new List<CarritoItemResumenResponse>();
        public int TotalItems { get; set; }
        public decimal TotalPrecio { get; set; }
    }

    public class CarritoItemResumenResponse
    {
        public int CarritoItemId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? ProductoImagen { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
        public string? Talla { get; set; }
        public string? Color { get; set; }
    }

    public class CarritoTotalResponse
    {
        public int CarritoId { get; set; }
        public decimal Total { get; set; }
    }

    public class ColorCreateRequest
    {
        public string ColNombre { get; set; } = string.Empty;
        public string? ColCodigoHex { get; set; }
    }

    public class ColorUpdateRequest
    {
        public int ColorId { get; set; }
        public string ColNombre { get; set; } = string.Empty;
        public string? ColCodigoHex { get; set; }
    }

    public class ColorResponse
    {
        public int ColorId { get; set; }
        public string ColNombre { get; set; } = string.Empty;
        public string? ColCodigoHex { get; set; }
        public int TotalProductos { get; set; }
    }

    public class CuponCreateRequest
    {
        public string Codigo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? TipoDescuento { get; set; }
        public decimal? ValorDescuento { get; set; }
        public decimal? MontoMinimo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? LimiteUso { get; set; }
        public bool? Activo { get; set; } = true;
    }

    public class CuponUpdateRequest
    {
        public int CuponId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? TipoDescuento { get; set; }
        public decimal? ValorDescuento { get; set; }
        public decimal? MontoMinimo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? LimiteUso { get; set; }
        public bool? Activo { get; set; }
    }

    public class CuponResponse
    {
        public int CuponId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? TipoDescuento { get; set; }
        public decimal? ValorDescuento { get; set; }
        public decimal? MontoMinimo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? LimiteUso { get; set; }
        public int? UsosRealizados { get; set; }
        public bool? Activo { get; set; }
        public bool EsValido { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class ValidarCuponRequest
    {
        public string Codigo { get; set; } = string.Empty;
        public decimal MontoCompra { get; set; }
    }

    public class ValidarCuponResponse
    {
        public bool EsValido { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public decimal? DescuentoAplicado { get; set; }
        public decimal? MontoFinal { get; set; }
    }

    public class CuponUsuarioCreateRequest
    {
        public int CuponId { get; set; }
        public int UsuId { get; set; }
        public bool? Usado { get; set; } = false;
    }

    public class CuponUsuarioUpdateRequest
    {
        public int CuponUsuarioId { get; set; }
        public int CuponId { get; set; }
        public int UsuId { get; set; }
        public bool? Usado { get; set; }
        public DateTime? FechaUso { get; set; }
    }

    public class CuponUsuarioResponse
    {
        public int CuponUsuarioId { get; set; }
        public int CuponId { get; set; }
        public int UsuId { get; set; }
        public bool? Usado { get; set; }
        public DateTime? FechaUso { get; set; }
        public string CodigoCupon { get; set; } = string.Empty;
        public string? DescripcionCupon { get; set; }
        public string? TipoDescuento { get; set; }
        public decimal? ValorDescuento { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
    }

    public class AsignarCuponRequest
    {
        public int CuponId { get; set; }
        public int UsuId { get; set; }
    }

    public class AsignarCuponResponse
    {
        public bool Asignado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int? CuponUsuarioId { get; set; }
    }

    public class UsarCuponResponse
    {
        public bool Usado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime? FechaUso { get; set; }
    }

    public class DireccionCreateRequest
    {
        public int UsuId { get; set; }
        public string? DirTitulo { get; set; }
        public string DirNombre { get; set; } = string.Empty;
        public string? DirTelefono { get; set; }
        public string DirPais { get; set; } = string.Empty;
        public string? DirDepartamento { get; set; }
        public string DirCiudad { get; set; } = string.Empty;
        public string? DirCodigoPostal { get; set; }
        public string DirLinea1 { get; set; } = string.Empty;
        public string? DirLinea2 { get; set; }
        public string? DirReferencia { get; set; }
        public bool? DirEsPrincipal { get; set; } = false;
    }

    public class DireccionUpdateRequest
    {
        public int DirId { get; set; }
        public int UsuId { get; set; }
        public string? DirTitulo { get; set; }
        public string DirNombre { get; set; } = string.Empty;
        public string? DirTelefono { get; set; }
        public string DirPais { get; set; } = string.Empty;
        public string? DirDepartamento { get; set; }
        public string DirCiudad { get; set; } = string.Empty;
        public string? DirCodigoPostal { get; set; }
        public string DirLinea1 { get; set; } = string.Empty;
        public string? DirLinea2 { get; set; }
        public string? DirReferencia { get; set; }
        public bool? DirEsPrincipal { get; set; }
    }

    public class DireccionResponse
    {
        public int DirId { get; set; }
        public int UsuId { get; set; }
        public string? DirTitulo { get; set; }
        public string DirNombre { get; set; } = string.Empty;
        public string? DirTelefono { get; set; }
        public string DirPais { get; set; } = string.Empty;
        public string? DirDepartamento { get; set; }
        public string DirCiudad { get; set; } = string.Empty;
        public string? DirCodigoPostal { get; set; }
        public string DirLinea1 { get; set; } = string.Empty;
        public string? DirLinea2 { get; set; }
        public string? DirReferencia { get; set; }
        public bool? DirEsPrincipal { get; set; }
        public DateTime? DirFechaCreacion { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string DireccionCompleta { get; set; } = string.Empty;
    }

    public class MarcarDireccionPrincipalRequest
    {
        public int DirId { get; set; }
        public int UsuId { get; set; }
    }

    public class MarcarDireccionPrincipalResponse
    {
        public bool Marcado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int DirId { get; set; }
        public int UsuId { get; set; }
    }

    public class InventarioCreateRequest
    {
        public int ProductoTallaColorId { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public int? UsuId { get; set; }
    }

    public class InventarioUpdateRequest
    {
        public int InventarioId { get; set; }
        public int ProductoTallaColorId { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public int? UsuId { get; set; }
    }

    public class InventarioResponse
    {
        public int InventarioId { get; set; }
        public int ProductoTallaColorId { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public int? UsuId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? Talla { get; set; }
        public string? Color { get; set; }
        public string? NombreUsuario { get; set; }
    }

    public class RegistrarMovimientoRequest
    {
        public int ProductoTallaColorId { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int? UsuId { get; set; }
    }

    public class RegistrarMovimientoResponse
    {
        public bool Registrado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int? InventarioId { get; set; }
        public int StockActual { get; set; }
    }

    public class RegistrarVentaRequest
    {
        public int ProductoTallaColorId { get; set; }
        public int Cantidad { get; set; }
        public int UsuId { get; set; }
    }

    public class RegistrarVentaResponse
    {
        public bool Registrado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int? InventarioId { get; set; }
        public int StockActual { get; set; }
        public decimal? PrecioTotal { get; set; }
    }

    public class StockResponse
    {
        public int ProductoTallaColorId { get; set; }
        public int StockActual { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? Talla { get; set; }
        public string? Color { get; set; }
    }

    public class VerificarStockResponse
    {
        public int ProductoTallaColorId { get; set; }
        public int CantidadSolicitada { get; set; }
        public int StockActual { get; set; }
        public bool Disponible { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }

    public class ResumenInventarioResponse
    {
        public int ProductoTallaColorId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? Talla { get; set; }
        public string? Color { get; set; }
        public int StockActual { get; set; }
        public int TotalEntradas { get; set; }
        public int TotalSalidas { get; set; }
        public int TotalVentas { get; set; }
        public DateTime? UltimoMovimiento { get; set; }
    }

    // DTOs para Pedido
    public class CrearPedidoDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UsuId { get; set; }

        [Required(ErrorMessage = "El ID de la dirección de envío es requerido")]
        public int DireccionEnvioId { get; set; }

        [StringLength(50, ErrorMessage = "El estado no puede exceder 50 caracteres")]
        public string? Estado { get; set; }

        [StringLength(50, ErrorMessage = "El método de pago no puede exceder 50 caracteres")]
        public string? MetodoPago { get; set; }

        [Required(ErrorMessage = "El total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal Total { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }
    }

    public class ActualizarEstadoPedidoDto
    {
        [Required(ErrorMessage = "El estado es requerido")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder 50 caracteres")]
        public string Estado { get; set; } = string.Empty;
    }

    public class CancelarPedidoDto
    {
        [Required(ErrorMessage = "El motivo de cancelación es requerido")]
        [StringLength(500, ErrorMessage = "El motivo no puede exceder 500 caracteres")]
        public string Motivo { get; set; } = string.Empty;
    }

    public class PedidoRespuestaDto
    {
        public int PedidoId { get; set; }
        public int UsuId { get; set; }
        public int DireccionEnvioId { get; set; }
        public string? Estado { get; set; }
        public string? MetodoPago { get; set; }
        public decimal Total { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaPedido { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class PedidoCompletoRespuestaDto
    {
        public int PedidoId { get; set; }
        public int UsuId { get; set; }
        public int DireccionEnvioId { get; set; }
        public string? Estado { get; set; }
        public string? MetodoPago { get; set; }
        public decimal Total { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaPedido { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public List<PedidoDetalleRespuestaDto> PedidoDetalles { get; set; } = new List<PedidoDetalleRespuestaDto>();
    }

    public class PedidoDetalleRespuestaDto
    {
        public int PedidoDetalleId { get; set; }
        public int PedidoId { get; set; }
        public int ProductoTallaColorId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Subtotal { get; set; }
    }

    public class OperacionPedidoRespuestaDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public object? Datos { get; set; }
    }

    public class EstadisticasPedidoRespuestaDto
    {
        public int TotalPedidos { get; set; }
        public decimal TotalVentas { get; set; }
        public int PedidosPendientes { get; set; }
        public int PedidosProcesando { get; set; }
        public int PedidosEnviados { get; set; }
        public int PedidosEntregados { get; set; }
        public int PedidosCancelados { get; set; }
    }

    // DTOs para PedidoDetalle
    public class CrearPedidoDetalleDto
    {
        [Required(ErrorMessage = "El ID del pedido es requerido")]
        public int PedidoId { get; set; }

        [Required(ErrorMessage = "El ID del producto-talla-color es requerido")]
        public int ProductoTallaColorId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }
    }

    public class ResumenPedidoRespuestaDto
    {
        public int PedidoId { get; set; }
        public int TotalProductos { get; set; }
        public decimal TotalPedido { get; set; }
        public List<PedidoDetalleRespuestaDto> Detalles { get; set; } = new List<PedidoDetalleRespuestaDto>();
    }

    public class OperacionPedidoDetalleRespuestaDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public object? Datos { get; set; }
    }

    // DTOs para ProductoImagen
    public class CrearProductoImagenDto
    {
        [Required(ErrorMessage = "El ID del producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de producto inválido")]
        public int ProId { get; set; }

        [Required(ErrorMessage = "La URL de la imagen es requerida")]
        [StringLength(255, ErrorMessage = "La URL no puede exceder 255 caracteres")]
        public string ImgUrl { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "El orden debe ser mayor o igual a 0")]
        public int ImgOrden { get; set; } = 0;

        [StringLength(150, ErrorMessage = "El texto alternativo no puede exceder 150 caracteres")]
        public string? ImgAlt { get; set; }
    }

    public class ActualizarProductoImagenDto
    {
        [Required(ErrorMessage = "El ID del producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de producto inválido")]
        public int ProId { get; set; }

        [Required(ErrorMessage = "La URL de la imagen es requerida")]
        [StringLength(255, ErrorMessage = "La URL no puede exceder 255 caracteres")]
        public string ImgUrl { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "El orden debe ser mayor o igual a 0")]
        public int ImgOrden { get; set; } = 0;

        [StringLength(150, ErrorMessage = "El texto alternativo no puede exceder 150 caracteres")]
        public string? ImgAlt { get; set; }
    }

    public class ActualizarOrdenImagenDto
    {
        [Required(ErrorMessage = "El nuevo orden es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El orden debe ser mayor o igual a 0")]
        public int NuevoOrden { get; set; }
    }

    public class EstablecerImagenPrincipalDto
    {
        [Required(ErrorMessage = "El ID del producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de producto inválido")]
        public int ProId { get; set; }
    }

    public class ReordenarImagenesDto
    {
        [Required(ErrorMessage = "La lista de orden de imágenes es requerida")]
        public List<int> OrdenImagenes { get; set; } = new List<int>();
    }

    public class ProductoImagenRespuestaDto
    {
        public int ImgId { get; set; }
        public int ProId { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public int? ImgOrden { get; set; }
        public string? ImgAlt { get; set; }
        public DateTime? ImgFechaCreacion { get; set; }
    }

    public class OperacionProductoImagenRespuestaDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public object? Datos { get; set; }
    }

    // DTOs para ProductoTallaColor
    public class CrearProductoTallaColorDto
    {
        [Required(ErrorMessage = "El ID del producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de producto inválido")]
        public int ProId { get; set; }

        [Required(ErrorMessage = "El ID de la talla es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de talla inválido")]
        public int TallaId { get; set; }

        [Required(ErrorMessage = "El ID del color es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de color inválido")]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        public int Stock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio de oferta debe ser mayor o igual a 0")]
        public decimal? PrecioOferta { get; set; }

        [StringLength(50, ErrorMessage = "El SKU no puede exceder 50 caracteres")]
        public string? Sku { get; set; }
    }

    public class ActualizarProductoTallaColorDto
    {
        [Required(ErrorMessage = "El ID del producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de producto inválido")]
        public int ProId { get; set; }

        [Required(ErrorMessage = "El ID de la talla es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de talla inválido")]
        public int TallaId { get; set; }

        [Required(ErrorMessage = "El ID del color es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de color inválido")]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        public int Stock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio de oferta debe ser mayor o igual a 0")]
        public decimal? PrecioOferta { get; set; }

        [StringLength(50, ErrorMessage = "El SKU no puede exceder 50 caracteres")]
        public string? Sku { get; set; }
    }

    public class ActualizarStockProductoTallaColorDto
    {
        [Required(ErrorMessage = "El nuevo stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        public int NuevoStock { get; set; }
    }

    public class ActualizarPrecioOfertaProductoTallaColorDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "El precio de oferta debe ser mayor o igual a 0")]
        public decimal? PrecioOferta { get; set; }
    }

    public class ReducirStockProductoTallaColorDto
    {
        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }

    public class AumentarStockProductoTallaColorDto
    {
        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }

    public class ProductoTallaColorRespuestaDto
    {
        public int ProductoTallaColorId { get; set; }
        public int ProId { get; set; }
        public int TallaId { get; set; }
        public int ColorId { get; set; }
        public int Stock { get; set; }
        public decimal? PrecioOferta { get; set; }
        public string? Sku { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class OperacionProductoTallaColorRespuestaDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public object? Datos { get; set; }
    }

    public class EstadisticasStockRespuestaDto
    {
        public int TotalCombinaciones { get; set; }
        public int CombinacionesConStock { get; set; }
        public int CombinacionesSinStock { get; set; }
        public int StockTotal { get; set; }
        public decimal StockPromedio { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
    }

    // DTOs para Promoción
    public class CrearPromocionDTO
    {
        [Required(ErrorMessage = "El nombre de la promoción es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La descripción no puede exceder 255 caracteres")]
        public string? Descripcion { get; set; }

        [StringLength(20, ErrorMessage = "El tipo de descuento no puede exceder 20 caracteres")]
        public string? TipoDescuento { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El valor del descuento debe ser mayor o igual a 0")]
        public decimal? ValorDescuento { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool Activo { get; set; } = true;
    }

    public class ActualizarPromocionDTO
    {
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede exceder 255 caracteres")]
        public string? Descripcion { get; set; }

        [StringLength(20, ErrorMessage = "El tipo de descuento no puede exceder 20 caracteres")]
        public string? TipoDescuento { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El valor del descuento debe ser mayor o igual a 0")]
        public decimal? ValorDescuento { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool? Activo { get; set; }
    }

    public class PromocionRespuestaDTO
    {
        public int PromocionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? TipoDescuento { get; set; }
        public decimal? ValorDescuento { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Activo { get; set; }
        public bool EsVigente { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalProductos { get; set; }
    }

    public class PromocionResumenDTO
    {
        public int PromocionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? TipoDescuento { get; set; }
        public decimal? ValorDescuento { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Activo { get; set; }
        public bool EsVigente { get; set; }
    }

    public class PromocionOperacionDTO
    {
        public int PromocionId { get; set; }
        public string Operacion { get; set; } = string.Empty;
        public DateTime FechaOperacion { get; set; }
        public string? Detalles { get; set; }
    }

    public class PromocionEstadisticasDTO
    {
        public int TotalPromociones { get; set; }
        public int PromocionesActivas { get; set; }
        public int PromocionesVigentes { get; set; }
        public int PromocionesExpiradas { get; set; }
        public decimal DescuentoPromedio { get; set; }
    }

    // DTOs para Talla
    public class CrearTallaDTO
    {
        [Required(ErrorMessage = "El nombre de la talla es requerido")]
        [StringLength(10, ErrorMessage = "El nombre no puede exceder 10 caracteres")]
        public string TalNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El género es requerido")]
        [StringLength(20, ErrorMessage = "El género no puede exceder 20 caracteres")]
        public string TalGenero { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "El orden de visualización debe ser mayor o igual a 0")]
        public int? TalOrdenVisualizacion { get; set; }
    }

    public class ActualizarTallaDTO
    {
        [StringLength(10, ErrorMessage = "El nombre no puede exceder 10 caracteres")]
        public string? TalNombre { get; set; }

        [StringLength(20, ErrorMessage = "El género no puede exceder 20 caracteres")]
        public string? TalGenero { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El orden de visualización debe ser mayor o igual a 0")]
        public int? TalOrdenVisualizacion { get; set; }
    }

    public class TallaRespuestaDTO
    {
        public int TallaId { get; set; }
        public string TalNombre { get; set; } = string.Empty;
        public string TalGenero { get; set; } = string.Empty;
        public int? TalOrdenVisualizacion { get; set; }
        public int TotalProductos { get; set; }
        public bool EsUtilizada { get; set; }
    }

    public class TallaResumenDTO
    {
        public int TallaId { get; set; }
        public string TalNombre { get; set; } = string.Empty;
        public string TalGenero { get; set; } = string.Empty;
        public int? TalOrdenVisualizacion { get; set; }
    }

    public class TallaOperacionDTO
    {
        public int TallaId { get; set; }
        public string Operacion { get; set; } = string.Empty;
        public DateTime FechaOperacion { get; set; }
        public string? Detalles { get; set; }
    }

    public class TallaEstadisticasDTO
    {
        public int TotalTallas { get; set; }
        public int TallasMasculinas { get; set; }
        public int TallasFemeninas { get; set; }
        public int TallasUnisex { get; set; }
        public int TallasUtilizadas { get; set; }
        public int TallasSinUso { get; set; }
    }

    // DTOs para Temporada
    public class CrearTemporadaDTO
    {
        [Required(ErrorMessage = "El nombre de la temporada es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La descripción no puede exceder 255 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }

        public bool Activo { get; set; } = true;
    }

    public class ActualizarTemporadaDTO
    {
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede exceder 255 caracteres")]
        public string? Descripcion { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool? Activo { get; set; }
    }

    public class TemporadaRespuestaDTO
    {
        public int TemporadaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool EsVigente { get; set; }
        public int DiasRestantes { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class TemporadaResumenDTO
    {
        public int TemporadaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool? Activo { get; set; }
        public bool EsVigente { get; set; }
    }

    public class TemporadaOperacionDTO
    {
        public int TemporadaId { get; set; }
        public string Operacion { get; set; } = string.Empty;
        public DateTime FechaOperacion { get; set; }
        public string? Detalles { get; set; }
    }

    public class TemporadaEstadisticasDTO
    {
        public int TotalTemporadas { get; set; }
        public int TemporadasActivas { get; set; }
        public int TemporadasVigentes { get; set; }
        public int TemporadasExpiradas { get; set; }
        public int TemporadasFuturas { get; set; }
        public decimal DuracionPromedio { get; set; }
    }

    // DTOs para Usuario
    public class CrearUsuarioDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string UsuNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string UsuApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string UsuEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255, ErrorMessage = "La contraseña no puede exceder 255 caracteres")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string UsuPassword { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? UsuTelefono { get; set; }

        [StringLength(20, ErrorMessage = "El género no puede exceder 20 caracteres")]
        public string? UsuGenero { get; set; }

        public DateOnly? UsuFechaNacimiento { get; set; }

        [StringLength(255, ErrorMessage = "La URL de la imagen no puede exceder 255 caracteres")]
        public string? UsuImagenPerfil { get; set; }

        [StringLength(50, ErrorMessage = "El estado de la cuenta no puede exceder 50 caracteres")]
        public string? UsuEstadoCuenta { get; set; } = "Activo";

        [StringLength(100, ErrorMessage = "El proveedor de autenticación no puede exceder 100 caracteres")]
        public string? UsuProveedorAutenticacion { get; set; }
    }

    public class ActualizarUsuarioDTO
    {
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? UsuNombre { get; set; }

        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string? UsuApellido { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string? UsuEmail { get; set; }

        [StringLength(255, ErrorMessage = "La contraseña no puede exceder 255 caracteres")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string? UsuPassword { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? UsuTelefono { get; set; }

        [StringLength(20, ErrorMessage = "El género no puede exceder 20 caracteres")]
        public string? UsuGenero { get; set; }

        public DateOnly? UsuFechaNacimiento { get; set; }

        [StringLength(255, ErrorMessage = "La URL de la imagen no puede exceder 255 caracteres")]
        public string? UsuImagenPerfil { get; set; }

        [StringLength(50, ErrorMessage = "El estado de la cuenta no puede exceder 50 caracteres")]
        public string? UsuEstadoCuenta { get; set; }

        [StringLength(100, ErrorMessage = "El proveedor de autenticación no puede exceder 100 caracteres")]
        public string? UsuProveedorAutenticacion { get; set; }
    }

    public class UsuarioRespuestaDTO
    {
        public int UsuId { get; set; }
        public string UsuNombre { get; set; } = string.Empty;
        public string UsuApellido { get; set; } = string.Empty;
        public string UsuEmail { get; set; } = string.Empty;
        public string? UsuTelefono { get; set; }
        public string? UsuGenero { get; set; }
        public DateOnly? UsuFechaNacimiento { get; set; }
        public string? UsuImagenPerfil { get; set; }
        public string? UsuEstadoCuenta { get; set; }
        public DateTime? UsuFechaRegistro { get; set; }
        public DateTime? UsuFechaUltimoLogin { get; set; }
        public string? UsuProveedorAutenticacion { get; set; }
        public DateTime? UsuFechaActualizacion { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public int TotalPedidos { get; set; }
        public int TotalDirecciones { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class UsuarioResumenDTO
    {
        public int UsuId { get; set; }
        public string UsuNombre { get; set; } = string.Empty;
        public string UsuApellido { get; set; } = string.Empty;
        public string UsuEmail { get; set; } = string.Empty;
        public string? UsuEstadoCuenta { get; set; }
        public DateTime? UsuFechaRegistro { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
    }

    public class UsuarioOperacionDTO
    {
        public int UsuId { get; set; }
        public string Operacion { get; set; } = string.Empty;
        public DateTime FechaOperacion { get; set; }
        public string? Detalles { get; set; }
    }

    public class UsuarioEstadisticasDTO
    {
        public int TotalUsuarios { get; set; }
        public int UsuariosActivos { get; set; }
        public int UsuariosInactivos { get; set; }
        public int UsuariosNuevosEsteMes { get; set; }
        public int UsuariosConPedidos { get; set; }
        public decimal PromedioPedidosPorUsuario { get; set; }
    }

    public class CambiarPasswordDTO
    {
        [Required(ErrorMessage = "La contraseña actual es requerida")]
        public string PasswordActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres")]
        public string NuevaPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de la contraseña es requerida")]
        [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }

    public class LoginUsuarioDTO
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRespuestaDTO
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public UsuarioResumenDTO? Usuario { get; set; }
    }

    public class RefreshTokenDTO
    {
        [Required(ErrorMessage = "El token es requerido")]
        public string Token { get; set; } = string.Empty;
    }

    public class RefreshTokenRespuestaDTO
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }

    // DTOs para UsuarioRole
    public class CrearUsuarioRoleDTO
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
        public int UsuId { get; set; }

        [Required(ErrorMessage = "El ID del rol es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser mayor a 0")]
        public int RolId { get; set; }
    }

    public class ActualizarUsuarioRoleDTO
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
        public int UsuId { get; set; }

        [Required(ErrorMessage = "El ID del rol es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser mayor a 0")]
        public int RolId { get; set; }
    }

    public class UsuarioRoleRespuestaDTO
    {
        public int UsuarioRolId { get; set; }
        public int UsuId { get; set; }
        public int RolId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string ApellidoUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
        public string NombreCompletoUsuario { get; set; } = string.Empty;
    }

    public class UsuarioRoleResumenDTO
    {
        public int UsuarioRolId { get; set; }
        public int UsuId { get; set; }
        public int RolId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
    }

    public class UsuarioRoleOperacionDTO
    {
        public int UsuarioRolId { get; set; }
        public string Operacion { get; set; } = string.Empty;
        public DateTime FechaOperacion { get; set; }
        public string? Detalles { get; set; }
    }

    public class UsuarioRoleEstadisticasDTO
    {
        public int TotalAsignaciones { get; set; }
        public int UsuariosConRoles { get; set; }
        public int UsuariosSinRoles { get; set; }
        public Dictionary<string, int> RolesMasAsignados { get; set; } = new Dictionary<string, int>();
        public decimal PromedioRolesPorUsuario { get; set; }
    }

    public class AsignarRolAUsuarioDTO
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
        public int UsuId { get; set; }

        [Required(ErrorMessage = "El ID del rol es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser mayor a 0")]
        public int RolId { get; set; }
    }

    public class AsignarRolRespuestaDTO
    {
        public bool Asignado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int? UsuarioRolId { get; set; }
    }

    public class RemoverRolDeUsuarioDTO
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
        public int UsuId { get; set; }

        [Required(ErrorMessage = "El ID del rol es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser mayor a 0")]
        public int RolId { get; set; }
    }

    public class RemoverRolRespuestaDTO
    {
        public bool Removido { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
} 