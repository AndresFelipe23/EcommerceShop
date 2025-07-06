-- =========================================
-- BASE DE DATOS
-- =========================================
CREATE DATABASE EcommerceRopa;
GO
USE EcommerceRopa;
GO

-- =========================================
-- TABLAS PRINCIPALES DE USUARIOS Y ROLES
-- =========================================
CREATE TABLE Roles (
    RolId INT PRIMARY KEY IDENTITY,
    RolNombre NVARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE Usuarios (
    UsuId INT PRIMARY KEY IDENTITY,
    UsuNombre NVARCHAR(100) NOT NULL,
    UsuApellido NVARCHAR(100) NOT NULL,
    UsuEmail NVARCHAR(100) UNIQUE NOT NULL,
    UsuPasswordHash NVARCHAR(255) NOT NULL,
    UsuTelefono NVARCHAR(20),
    UsuGenero NVARCHAR(20),
    UsuFechaNacimiento DATE,
    UsuImagenPerfil NVARCHAR(255),
    UsuEstadoCuenta NVARCHAR(50) DEFAULT 'Activo',
    UsuFechaRegistro DATETIME DEFAULT GETDATE(),
    UsuFechaUltimoLogin DATETIME,
    UsuFechaActualizacion DATETIME,
    UsuProveedorAutenticacion NVARCHAR(100)
);


CREATE TABLE UsuarioRoles (
    UsuarioRolId INT PRIMARY KEY IDENTITY,
    UsuId INT NOT NULL,
    RolId INT NOT NULL,
    FOREIGN KEY (UsuId) REFERENCES Usuarios(UsuId) ON DELETE CASCADE,
    FOREIGN KEY (RolId) REFERENCES Roles(RolId) ON DELETE CASCADE
);

-- =========================================
-- TABLA DE DIRECCIONES
-- =========================================
CREATE TABLE Direcciones (
    DirId INT PRIMARY KEY IDENTITY,
    UsuId INT NOT NULL,
    DirTitulo NVARCHAR(100),
    DirNombre NVARCHAR(100) NOT NULL,
    DirTelefono NVARCHAR(20),
    DirPais NVARCHAR(100) NOT NULL,
    DirDepartamento NVARCHAR(100),
    DirCiudad NVARCHAR(100) NOT NULL,
    DirCodigoPostal NVARCHAR(20),
    DirLinea1 NVARCHAR(255) NOT NULL,
    DirLinea2 NVARCHAR(255),
    DirReferencia NVARCHAR(255),
    DirEsPrincipal BIT DEFAULT 0,
    DirFechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuId) REFERENCES Usuarios(UsuId) ON DELETE CASCADE
);

-- =========================================
-- CATEGORÍAS Y SUBCATEGORÍAS
-- =========================================
CREATE TABLE Categorias (
    CatId INT PRIMARY KEY IDENTITY,
    CatNombre NVARCHAR(100) NOT NULL,
    CatDescripcion NVARCHAR(255),
    CatPadreId INT NULL,
    CatOrden INT DEFAULT 0,
    CatActivo BIT DEFAULT 1,
    CatFechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CatPadreId) REFERENCES Categorias(CatId)
);

-- =========================================
-- PRODUCTOS Y SUS RELACIONES
-- =========================================
CREATE TABLE Productos (
    ProId INT PRIMARY KEY IDENTITY,
    ProNombre NVARCHAR(150) NOT NULL,
    ProDescripcion NVARCHAR(MAX),
    ProPrecio DECIMAL(10, 2) NOT NULL,
    ProImagenPrincipal NVARCHAR(MAX),
    ProGenero NVARCHAR(50),
    ProCategoriaId INT NOT NULL,
    ProActivo BIT DEFAULT 1,
    ProFechaCreacion DATETIME DEFAULT GETDATE(),
    ProFechaActualizacion DATETIME,
    FOREIGN KEY (ProCategoriaId) REFERENCES Categorias(CatId)
);

CREATE TABLE ProductoImagenes (
    ImgId INT PRIMARY KEY IDENTITY,
    ProId INT NOT NULL,
    ImgUrl NVARCHAR(255) NOT NULL,
    ImgOrden INT DEFAULT 0,
    ImgAlt NVARCHAR(150),
    ImgFechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProId) REFERENCES Productos(ProId) ON DELETE CASCADE
);

CREATE TABLE Tallas (
    TallaId INT PRIMARY KEY IDENTITY,
    TalNombre NVARCHAR(10) NOT NULL,
    TalGenero NVARCHAR(20) NOT NULL,
    TalOrdenVisualizacion INT DEFAULT 0
);

CREATE TABLE Colores (
    ColorId INT PRIMARY KEY IDENTITY,
    ColNombre NVARCHAR(50) NOT NULL,
    ColCodigoHex NVARCHAR(10)
);

CREATE TABLE ProductoTallaColor (
    ProductoTallaColorId INT PRIMARY KEY IDENTITY,
    ProId INT NOT NULL,
    TallaId INT NOT NULL,
    ColorId INT NOT NULL,
    Stock INT NOT NULL,
    PrecioOferta DECIMAL(10, 2),
    SKU NVARCHAR(50),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME,
    FOREIGN KEY (ProId) REFERENCES Productos(ProId) ON DELETE CASCADE,
    FOREIGN KEY (TallaId) REFERENCES Tallas(TallaId),
    FOREIGN KEY (ColorId) REFERENCES Colores(ColorId)
);

-- =========================================
-- CARRITO DE COMPRAS
-- =========================================
CREATE TABLE Carrito (
    CarritoId INT PRIMARY KEY IDENTITY,
    UsuId INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuId) REFERENCES Usuarios(UsuId) ON DELETE CASCADE
);

CREATE TABLE CarritoItem (
    CarritoItemId INT PRIMARY KEY IDENTITY,
    CarritoId INT NOT NULL,
    ProductoTallaColorId INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    FechaAgregado DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CarritoId) REFERENCES Carrito(CarritoId) ON DELETE CASCADE,
    FOREIGN KEY (ProductoTallaColorId) REFERENCES ProductoTallaColor(ProductoTallaColorId)
);

-- =========================================
-- PEDIDOS Y DETALLES
-- =========================================
CREATE TABLE Pedidos (
    PedidoId INT PRIMARY KEY IDENTITY,
    UsuId INT NOT NULL,
    DireccionEnvioId INT NOT NULL,
    Estado NVARCHAR(50) DEFAULT 'Pendiente',
    MetodoPago NVARCHAR(50),
    Total DECIMAL(10,2) NOT NULL,
    Observaciones NVARCHAR(500),
    FechaPedido DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME,
    FOREIGN KEY (UsuId) REFERENCES Usuarios(UsuId),
    FOREIGN KEY (DireccionEnvioId) REFERENCES Direcciones(DirId)
);

CREATE TABLE PedidoDetalle (
    PedidoDetalleId INT PRIMARY KEY IDENTITY,
    PedidoId INT NOT NULL,
    ProductoTallaColorId INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(PedidoId) ON DELETE CASCADE,
    FOREIGN KEY (ProductoTallaColorId) REFERENCES ProductoTallaColor(ProductoTallaColorId)
);

-- =========================================
-- CUPONES Y RELACIÓN CON USUARIOS
-- =========================================
CREATE TABLE Cupones (
    CuponId INT PRIMARY KEY IDENTITY,
    Codigo NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255),
    TipoDescuento NVARCHAR(20),
    ValorDescuento DECIMAL(10, 2),
    MontoMinimo DECIMAL(10, 2),
    FechaInicio DATETIME,
    FechaFin DATETIME,
    LimiteUso INT,
    UsosRealizados INT DEFAULT 0,
    Activo BIT DEFAULT 1
);

CREATE TABLE CuponesUsuarios (
    CuponUsuarioId INT PRIMARY KEY IDENTITY,
    CuponId INT NOT NULL,
    UsuId INT NOT NULL,
    Usado BIT DEFAULT 0,
    FechaUso DATETIME,
    FOREIGN KEY (CuponId) REFERENCES Cupones(CuponId),
    FOREIGN KEY (UsuId) REFERENCES Usuarios(UsuId)
);

-- =========================================
-- PROMOCIONES Y RELACIONES
-- =========================================
CREATE TABLE Promociones (
    PromocionId INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    TipoDescuento NVARCHAR(20),
    ValorDescuento DECIMAL(10, 2),
    FechaInicio DATETIME,
    FechaFin DATETIME,
    Activo BIT DEFAULT 1
);

CREATE TABLE PromocionProducto (
    PromocionProductoId INT PRIMARY KEY IDENTITY,
    PromocionId INT,
    ProId INT,
    FOREIGN KEY (PromocionId) REFERENCES Promociones(PromocionId),
    FOREIGN KEY (ProId) REFERENCES Productos(ProId)
);

CREATE TABLE PromocionCategoria (
    PromocionCategoriaId INT PRIMARY KEY IDENTITY,
    PromocionId INT,
    CatId INT,
    FOREIGN KEY (PromocionId) REFERENCES Promociones(PromocionId),
    FOREIGN KEY (CatId) REFERENCES Categorias(CatId)
);

-- =========================================
-- TEMPORADAS
-- =========================================
CREATE TABLE Temporadas (
    TemporadaId INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- =========================================
-- LOGS DEL SISTEMA
-- =========================================
CREATE TABLE Logs (
    LogId INT PRIMARY KEY IDENTITY,
    UsuId INT NULL,
    Tipo NVARCHAR(50) NOT NULL,
    Mensaje NVARCHAR(1000) NOT NULL,
    Origen NVARCHAR(100),
    Metodo NVARCHAR(100),
    DatosEntrada NVARCHAR(MAX),
    DatosSalida NVARCHAR(MAX),
    Fecha DATETIME DEFAULT GETDATE(),
    IP NVARCHAR(45),
    Navegador NVARCHAR(255),
    FOREIGN KEY (UsuId) REFERENCES Usuarios(UsuId)
);

-- =========================================
-- INVENTARIO / MOVIMIENTOS DE STOCK
-- =========================================
CREATE TABLE Inventario (
    InventarioId INT PRIMARY KEY IDENTITY,
    ProductoTallaColorId INT NOT NULL,
    TipoMovimiento NVARCHAR(50) NOT NULL,    -- Entrada, Salida, Ajuste, Venta
    Cantidad INT NOT NULL,
    Descripcion NVARCHAR(255),
    FechaMovimiento DATETIME DEFAULT GETDATE(),
    UsuId INT NULL,
    FOREIGN KEY (ProductoTallaColorId) REFERENCES ProductoTallaColor(ProductoTallaColorId),
    FOREIGN KEY (UsuId) REFERENCES Usuarios(UsuId)
);


