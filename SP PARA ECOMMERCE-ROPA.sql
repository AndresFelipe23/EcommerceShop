-- ========================================
-- SP: Usuario_Insert
-- ========================================
ALTER PROCEDURE Usuario_Insert
    @UsuNombre NVARCHAR(100),
    @UsuApellido NVARCHAR(100),
    @UsuEmail NVARCHAR(100),
    @UsuPasswordHash NVARCHAR(255),
    @UsuTelefono NVARCHAR(20) = NULL,
    @UsuGenero NVARCHAR(20) = NULL,
    @UsuFechaNacimiento DATE = NULL,
    @UsuImagenPerfil NVARCHAR(255) = NULL,
    @UsuEstadoCuenta NVARCHAR(50) = 'Activo',
    @UsuProveedorAutenticacion NVARCHAR(100) = 'Local',
    @NewUsuId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Usuarios (
        UsuNombre, UsuApellido, UsuEmail, UsuPasswordHash,
        UsuTelefono, UsuGenero, UsuFechaNacimiento, UsuImagenPerfil,
        UsuEstadoCuenta, UsuProveedorAutenticacion, UsuFechaRegistro
    )
    VALUES (
        @UsuNombre, @UsuApellido, @UsuEmail, @UsuPasswordHash,
        @UsuTelefono, @UsuGenero, @UsuFechaNacimiento, @UsuImagenPerfil,
        @UsuEstadoCuenta, @UsuProveedorAutenticacion, 
        DATEADD(HOUR, -5, GETUTCDATE()) -- Hora local de Colombia (UTC-5)
    );
    
    SET @NewUsuId = SCOPE_IDENTITY();
END;
GO

-- ========================================
-- SP: Usuario_List
-- ========================================
CREATE PROCEDURE Usuario_List
AS
BEGIN
    SELECT * FROM Usuarios;
END;
GO

-- ========================================
-- SP: Usuario_ListPorId
-- ========================================
CREATE PROCEDURE Usuario_ListPorId
    @UsuId INT
AS
BEGIN
    SELECT * FROM Usuarios WHERE UsuId = @UsuId;
END;
GO

-- ========================================
-- SP: Usuario_Update
-- ========================================
CREATE PROCEDURE Usuario_Update
    @UsuId INT,
    @UsuNombre NVARCHAR(100),
    @UsuApellido NVARCHAR(100),
    @UsuTelefono NVARCHAR(20),
    @UsuGenero NVARCHAR(20),
    @UsuFechaNacimiento DATE,
    @UsuImagenPerfil NVARCHAR(255),
    @UsuEstadoCuenta NVARCHAR(50)
AS
BEGIN
    UPDATE Usuarios
    SET UsuNombre = @UsuNombre,
        UsuApellido = @UsuApellido,
        UsuTelefono = @UsuTelefono,
        UsuGenero = @UsuGenero,
        UsuFechaNacimiento = @UsuFechaNacimiento,
        UsuImagenPerfil = @UsuImagenPerfil,
        UsuEstadoCuenta = @UsuEstadoCuenta,
        UsuFechaActualizacion = GETDATE()
    WHERE UsuId = @UsuId;
END;
GO

-- ========================================
-- SP: Usuario_Delete
-- ========================================
CREATE PROCEDURE Usuario_Delete
    @UsuId INT
AS
BEGIN
    DELETE FROM Usuarios WHERE UsuId = @UsuId;
END;
GO


-- ========================================
-- SP: Log_Insert
-- Descripción: Inserta un registro en la tabla Logs
-- ========================================
CREATE PROCEDURE Log_Insert
    @UsuId INT = NULL,                    -- Usuario relacionado (opcional)
    @Tipo NVARCHAR(50),                  -- Info, Warning, Error, Excepcion, Accion
    @Mensaje NVARCHAR(1000),            -- Descripción del evento
    @Origen NVARCHAR(100),              -- Módulo o clase
    @Metodo NVARCHAR(100),              -- Endpoint o función
    @DatosEntrada NVARCHAR(MAX) = NULL, -- Parámetros recibidos
    @DatosSalida NVARCHAR(MAX) = NULL,  -- Resultado devuelto
    @IP NVARCHAR(45) = NULL,            -- IP del cliente
    @Navegador NVARCHAR(255) = NULL     -- Info del navegador o agente
AS
BEGIN
    INSERT INTO Logs (
        UsuId, Tipo, Mensaje, Origen, Metodo,
        DatosEntrada, DatosSalida, IP, Navegador, Fecha
    )
    VALUES (
        @UsuId, @Tipo, @Mensaje, @Origen, @Metodo,
        @DatosEntrada, @DatosSalida, @IP, @Navegador, GETDATE()
    );
END;
GO


-- ========================================
-- SP: Log_List
-- Descripción: Lista de logs
-- =======================================
CREATE PROCEDURE Log_List
AS
BEGIN
    SELECT TOP 100 *
    FROM Logs
    ORDER BY Fecha DESC;
END;
GO


-- ========================================
-- SP: Categoria_Insert
-- ========================================
CREATE PROCEDURE Categoria_Insert
    @CatNombre NVARCHAR(100),
    @CatDescripcion NVARCHAR(255) = NULL,
    @CatPadreId INT = NULL,
    @CatOrden INT = 0,
    @CatActivo BIT = 1
AS
BEGIN
    INSERT INTO Categorias (
        CatNombre, CatDescripcion, CatPadreId, CatOrden, CatActivo, CatFechaCreacion
    )
    VALUES (
        @CatNombre, @CatDescripcion, @CatPadreId, @CatOrden, @CatActivo, GETDATE()
    );
END;
GO

-- ========================================
-- SP: Categoria_List
-- ========================================
CREATE PROCEDURE Categoria_List
AS
BEGIN
    SELECT *
    FROM Categorias
    ORDER BY CatOrden ASC, CatNombre ASC;
END;
GO

-- ========================================
-- SP: Categoria_ListPorId
-- ========================================
CREATE PROCEDURE Categoria_ListPorId
    @CatId INT
AS
BEGIN
    SELECT *
    FROM Categorias
    WHERE CatId = @CatId;
END;
GO

-- ========================================
-- SP: Categoria_Update
-- ========================================
CREATE PROCEDURE Categoria_Update
    @CatId INT,
    @CatNombre NVARCHAR(100),
    @CatDescripcion NVARCHAR(255) = NULL,
    @CatPadreId INT = NULL,
    @CatOrden INT = 0,
    @CatActivo BIT = 1
AS
BEGIN
    UPDATE Categorias
    SET
        CatNombre = @CatNombre,
        CatDescripcion = @CatDescripcion,
        CatPadreId = @CatPadreId,
        CatOrden = @CatOrden,
        CatActivo = @CatActivo
    WHERE CatId = @CatId;
END;
GO

-- ========================================
-- SP: Categoria_Delete
-- ========================================
CREATE PROCEDURE Categoria_Delete
    @CatId INT
AS
BEGIN
    DELETE FROM Categorias
    WHERE CatId = @CatId;
END;
GO

-- ========================================
-- SP: Color_Insert
-- ========================================
CREATE PROCEDURE Color_Insert
    @ColNombre NVARCHAR(50),
    @ColCodigoHex NVARCHAR(10)
AS
BEGIN
    INSERT INTO Colores (ColNombre, ColCodigoHex)
    VALUES (@ColNombre, @ColCodigoHex);
END;
GO

-- ========================================
-- SP: Color_List
-- ========================================
CREATE PROCEDURE Color_List
AS
BEGIN
    SELECT *
    FROM Colores
    ORDER BY ColNombre ASC;
END;
GO

-- ========================================
-- SP: Color_ListPorId
-- ========================================
CREATE PROCEDURE Color_ListPorId
    @ColorId INT
AS
BEGIN
    SELECT *
    FROM Colores
    WHERE ColorId = @ColorId;
END;
GO

-- ========================================
-- SP: Color_Update
-- ========================================
CREATE PROCEDURE Color_Update
    @ColorId INT,
    @ColNombre NVARCHAR(50),
    @ColCodigoHex NVARCHAR(10)
AS
BEGIN
    UPDATE Colores
    SET ColNombre = @ColNombre,
        ColCodigoHex = @ColCodigoHex
    WHERE ColorId = @ColorId;
END;
GO

-- ========================================
-- SP: Color_Delete
-- ========================================
CREATE PROCEDURE Color_Delete
    @ColorId INT
AS
BEGIN
    DELETE FROM Colores
    WHERE ColorId = @ColorId;
END;
GO

-- ========================================
-- SP: Cupon_Insert
-- ========================================
CREATE PROCEDURE Cupon_Insert
    @Codigo NVARCHAR(50),
    @Descripcion NVARCHAR(255) = NULL,
    @TipoDescuento NVARCHAR(20),
    @ValorDescuento DECIMAL(10, 2),
    @MontoMinimo DECIMAL(10, 2),
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @LimiteUso INT,
    @Activo BIT = 1
AS
BEGIN
    INSERT INTO Cupones (
        Codigo, Descripcion, TipoDescuento, ValorDescuento, MontoMinimo,
        FechaInicio, FechaFin, LimiteUso, Activo
    )
    VALUES (
        @Codigo, @Descripcion, @TipoDescuento, @ValorDescuento, @MontoMinimo,
        @FechaInicio, @FechaFin, @LimiteUso, @Activo
    );
END;
GO

-- ========================================
-- SP: Cupon_List
-- ========================================
CREATE PROCEDURE Cupon_List
AS
BEGIN
    SELECT *
    FROM Cupones
    ORDER BY FechaInicio DESC;
END;
GO

-- ========================================
-- SP: Cupon_ListPorId
-- ========================================
CREATE PROCEDURE Cupon_ListPorId
    @CuponId INT
AS
BEGIN
    SELECT *
    FROM Cupones
    WHERE CuponId = @CuponId;
END;
GO

-- ========================================
-- SP: Cupon_Update
-- ========================================
CREATE PROCEDURE Cupon_Update
    @CuponId INT,
    @Codigo NVARCHAR(50),
    @Descripcion NVARCHAR(255) = NULL,
    @TipoDescuento NVARCHAR(20),
    @ValorDescuento DECIMAL(10, 2),
    @MontoMinimo DECIMAL(10, 2),
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @LimiteUso INT,
    @Activo BIT
AS
BEGIN
    UPDATE Cupones
    SET
        Codigo = @Codigo,
        Descripcion = @Descripcion,
        TipoDescuento = @TipoDescuento,
        ValorDescuento = @ValorDescuento,
        MontoMinimo = @MontoMinimo,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        LimiteUso = @LimiteUso,
        Activo = @Activo
    WHERE CuponId = @CuponId;
END;
GO

-- ========================================
-- SP: Cupon_Delete
-- ========================================
CREATE PROCEDURE Cupon_Delete
    @CuponId INT
AS
BEGIN
    DELETE FROM Cupones
    WHERE CuponId = @CuponId;
END;
GO

-- ========================================
-- SP: CuponUsuario_Insert
-- ========================================
CREATE PROCEDURE CuponUsuario_Insert
    @UsuId INT,
    @CuponId INT,
    @FechaUso DATETIME = NULL
AS
BEGIN
    INSERT INTO CuponesUsuarios (UsuId, CuponId, FechaUso)
    VALUES (@UsuId, @CuponId, ISNULL(@FechaUso, GETDATE()));
END;
GO

-- ========================================
-- SP: CuponUsuario_List
-- ========================================
CREATE PROCEDURE CuponUsuario_List
AS
BEGIN
    SELECT cuu.CuponUsuarioId, cuu.UsuId, cuu.CuponId, cuu.FechaUso,
           u.UsuNombre, c.Codigo AS CuponCodigo
    FROM CuponesUsuarios cuu
    INNER JOIN Usuarios u ON cuu.UsuId = u.UsuId
    INNER JOIN Cupones c ON cuu.CuponId = c.CuponId
    ORDER BY cuu.FechaUso DESC;
END;
GO

-- ========================================
-- SP: CuponUsuario_ListPorUsuario
-- ========================================
CREATE PROCEDURE CuponUsuario_ListPorUsuario
    @UsuId INT
AS
BEGIN
    SELECT cuu.CuponUsuarioId, cuu.CuponId, cuu.FechaUso,
           c.Codigo AS CuponCodigo, c.Descripcion
    FROM CuponesUsuarios cuu
    INNER JOIN Cupones c ON cuu.CuponId = c.CuponId
    WHERE cuu.UsuId = @UsuId
    ORDER BY cuu.FechaUso DESC;
END;
GO

-- ========================================
-- SP: CuponUsuario_Existe
-- Valida si un usuario ya usó un cupón específico
-- ========================================
CREATE PROCEDURE CuponUsuario_Existe
    @UsuId INT,
    @CuponId INT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM CuponesUsuarios
        WHERE UsuId = @UsuId AND CuponId = @CuponId
    )
        SELECT 1 AS YaUsado;
    ELSE
        SELECT 0 AS YaUsado;
END;
GO

-- ========================================
-- SP: Direccion_Insert
-- ========================================
CREATE PROCEDURE Direccion_Insert
    @UsuId INT,
    @DirTitulo NVARCHAR(100),
    @DirNombre NVARCHAR(100),
    @DirTelefono NVARCHAR(20),
    @DirPais NVARCHAR(100),
    @DirDepartamento NVARCHAR(100),
    @DirCiudad NVARCHAR(100),
    @DirCodigoPostal NVARCHAR(20),
    @DirLinea1 NVARCHAR(255),
    @DirLinea2 NVARCHAR(255),
    @DirReferencia NVARCHAR(255),
    @DirEsPrincipal BIT
AS
BEGIN
    INSERT INTO Direcciones (
        UsuId, DirTitulo, DirNombre, DirTelefono, DirPais, DirDepartamento,
        DirCiudad, DirCodigoPostal, DirLinea1, DirLinea2, DirReferencia,
        DirEsPrincipal, DirFechaCreacion
    )
    VALUES (
        @UsuId, @DirTitulo, @DirNombre, @DirTelefono, @DirPais, @DirDepartamento,
        @DirCiudad, @DirCodigoPostal, @DirLinea1, @DirLinea2, @DirReferencia,
        @DirEsPrincipal, GETDATE()
    );
END;
GO

-- ========================================
-- SP: Direccion_List
-- ========================================
CREATE PROCEDURE Direccion_List
AS
BEGIN
    SELECT * FROM Direcciones ORDER BY DirFechaCreacion DESC;
END;
GO

-- ========================================
-- SP: Direccion_ListPorUsuario
-- ========================================
CREATE PROCEDURE Direccion_ListPorUsuario
    @UsuId INT
AS
BEGIN
    SELECT * FROM Direcciones
    WHERE UsuId = @UsuId
    ORDER BY DirEsPrincipal DESC, DirFechaCreacion DESC;
END;
GO

-- ========================================
-- SP: Direccion_Update
-- ========================================
CREATE PROCEDURE Direccion_Update
    @DirId INT,
    @DirTitulo NVARCHAR(100),
    @DirNombre NVARCHAR(100),
    @DirTelefono NVARCHAR(20),
    @DirPais NVARCHAR(100),
    @DirDepartamento NVARCHAR(100),
    @DirCiudad NVARCHAR(100),
    @DirCodigoPostal NVARCHAR(20),
    @DirLinea1 NVARCHAR(255),
    @DirLinea2 NVARCHAR(255),
    @DirReferencia NVARCHAR(255),
    @DirEsPrincipal BIT
AS
BEGIN
    UPDATE Direcciones
    SET
        DirTitulo = @DirTitulo,
        DirNombre = @DirNombre,
        DirTelefono = @DirTelefono,
        DirPais = @DirPais,
        DirDepartamento = @DirDepartamento,
        DirCiudad = @DirCiudad,
        DirCodigoPostal = @DirCodigoPostal,
        DirLinea1 = @DirLinea1,
        DirLinea2 = @DirLinea2,
        DirReferencia = @DirReferencia,
        DirEsPrincipal = @DirEsPrincipal
    WHERE DirId = @DirId;
END;
GO

-- ========================================
-- SP: Direccion_Delete
-- ========================================
CREATE PROCEDURE Direccion_Delete
    @DirId INT
AS
BEGIN
    DELETE FROM Direcciones WHERE DirId = @DirId;
END;
GO

-- ========================================
-- SP: Inventario_Insert
-- ========================================
CREATE PROCEDURE Inventario_Insert
    @ProductoTallaColorId INT,
    @TipoMovimiento NVARCHAR(50),
    @Cantidad INT,
    @Descripcion NVARCHAR(255),
    @UsuId INT = NULL
AS
BEGIN
    INSERT INTO Inventario (
        ProductoTallaColorId, TipoMovimiento, Cantidad, Descripcion, UsuId
    )
    VALUES (
        @ProductoTallaColorId, @TipoMovimiento, @Cantidad, @Descripcion, @UsuId
    );
END;
GO

-- ========================================
-- SP: Inventario_List
-- ========================================
CREATE PROCEDURE Inventario_List
AS
BEGIN
    SELECT 
        i.InventarioId,
        i.ProductoTallaColorId,
        p.ProNombre,
        t.TalNombre,
        c.ColNombre,
        i.TipoMovimiento,
        i.Cantidad,
        i.Descripcion,
        i.FechaMovimiento,
        u.UsuNombre
    FROM Inventario i
    INNER JOIN ProductoTallaColor ptc ON i.ProductoTallaColorId = ptc.ProductoTallaColorId
    INNER JOIN Productos p ON ptc.ProId = p.ProId
    INNER JOIN Tallas t ON ptc.TallaId = t.TallaId
    INNER JOIN Colores c ON ptc.ColorId = c.ColorId
    LEFT JOIN Usuarios u ON i.UsuId = u.UsuId
    ORDER BY i.FechaMovimiento DESC;
END;
GO

-- ========================================
-- SP: Inventario_ListPorProducto
-- ========================================
CREATE PROCEDURE Inventario_ListPorProducto
    @ProId INT
AS
BEGIN
    SELECT 
        i.InventarioId,
        i.TipoMovimiento,
        i.Cantidad,
        i.Descripcion,
        i.FechaMovimiento,
        t.TalNombre,
        c.ColNombre,
        u.UsuNombre
    FROM Inventario i
    INNER JOIN ProductoTallaColor ptc ON i.ProductoTallaColorId = ptc.ProductoTallaColorId
    INNER JOIN Tallas t ON ptc.TallaId = t.TallaId
    INNER JOIN Colores c ON ptc.ColorId = c.ColorId
    INNER JOIN Productos p ON ptc.ProId = p.ProId
    LEFT JOIN Usuarios u ON i.UsuId = u.UsuId
    WHERE p.ProId = @ProId
    ORDER BY i.FechaMovimiento DESC;
END;
GO


-- ========================================
-- SP: Inventario_Update
-- ========================================
CREATE PROCEDURE Inventario_Update
    @InventarioId INT,
    @Cantidad INT
AS
BEGIN
    UPDATE Inventario
    SET Cantidad = @Cantidad
    WHERE InventarioId = @InventarioId;
END;
GO

-- ========================================
-- SP: Inventario_Delete
-- ========================================
CREATE PROCEDURE Inventario_Delete
    @InventarioId INT
AS
BEGIN
    DELETE FROM Inventario
    WHERE InventarioId = @InventarioId;
END;
GO

-- ========================================
-- SP: Tallas_Insert
-- ========================================
CREATE PROCEDURE Tallas_Insert
    @TalNombre NVARCHAR(10),
    @TalGenero NVARCHAR(20),
    @TalOrdenVisualizacion INT = 0
AS
BEGIN
    INSERT INTO Tallas (TalNombre, TalGenero, TalOrdenVisualizacion)
    VALUES (@TalNombre, @TalGenero, @TalOrdenVisualizacion);
END;
GO

-- ========================================
-- SP: Tallas_List
-- ========================================
CREATE PROCEDURE Tallas_List
AS
BEGIN
    SELECT 
        TallaId,
        TalNombre,
        TalGenero,
        TalOrdenVisualizacion
    FROM Tallas
    ORDER BY TalGenero, TalOrdenVisualizacion;
END;
GO

-- SP: Tallas_ListPorId
-- Obtener una talla específica por ID
CREATE PROCEDURE Tallas_ListPorId
    @TallaId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        TallaId,
        TalNombre,
        TalGenero,
        TalOrdenVisualizacion
    FROM Talla
    WHERE TallaId = @TallaId;
END
GO

-- ========================================
-- SP: Tallas_Update
-- ========================================
CREATE PROCEDURE Tallas_Update
    @TallaId INT,
    @TalNombre NVARCHAR(10),
    @TalGenero NVARCHAR(20),
    @TalOrdenVisualizacion INT
AS
BEGIN
    UPDATE Tallas
    SET TalNombre = @TalNombre,
        TalGenero = @TalGenero,
        TalOrdenVisualizacion = @TalOrdenVisualizacion
    WHERE TallaId = @TallaId;
END;
GO

-- ========================================
-- SP: Tallas_Delete
-- ========================================
CREATE PROCEDURE Tallas_Delete
    @TallaId INT
AS
BEGIN
    DELETE FROM Tallas
    WHERE TallaId = @TallaId;
END;
GO


-- ========================================
-- SP: Roles_Insert
-- ========================================
CREATE PROCEDURE Roles_Insert
    @RolNombre NVARCHAR(50)
AS
BEGIN
    INSERT INTO Roles (RolNombre)
    VALUES (@RolNombre);
END;
GO

-- ========================================
-- SP: Roles_List
-- ========================================
CREATE PROCEDURE Roles_List
AS
BEGIN
    SELECT 
        RolId,
        RolNombre
    FROM Roles
    ORDER BY RolNombre;
END;
GO

-- ========================================
-- SP: Roles_ListPorId
-- ========================================
CREATE PROCEDURE Roles_ListPorId
    @RolId INT
AS
BEGIN
    SELECT 
        RolId,
        RolNombre
    FROM Roles
    WHERE RolId = @RolId;
END;
GO

-- ========================================
-- SP: Roles_Update
-- ========================================
CREATE PROCEDURE Roles_Update
    @RolId INT,
    @RolNombre NVARCHAR(50)
AS
BEGIN
    UPDATE Roles
    SET RolNombre = @RolNombre
    WHERE RolId = @RolId;
END;
GO

-- ========================================
-- SP: Roles_Delete
-- ========================================
CREATE PROCEDURE Roles_Delete
    @RolId INT
AS
BEGIN
    DELETE FROM Roles
    WHERE RolId = @RolId;
END;
GO

-- ========================================
-- SP: Temporadas_Insert
-- ========================================
CREATE PROCEDURE Temporadas_Insert
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @Activo BIT = 1
AS
BEGIN
    INSERT INTO Temporadas (Nombre, Descripcion, FechaInicio, FechaFin, Activo)
    VALUES (@Nombre, @Descripcion, @FechaInicio, @FechaFin, @Activo);
END;
GO

-- ========================================
-- SP: Temporadas_List
-- ========================================
CREATE PROCEDURE Temporadas_List
AS
BEGIN
    SELECT 
        TemporadaId,
        Nombre,
        Descripcion,
        FechaInicio,
        FechaFin,
        Activo,
        FechaCreacion
    FROM Temporadas
    ORDER BY FechaInicio DESC;
END;
GO

-- ========================================
-- SP: Temporadas_Update
-- ========================================
CREATE PROCEDURE Temporadas_Update
    @TemporadaId INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @Activo BIT
AS
BEGIN
    UPDATE Temporadas
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        Activo = @Activo
    WHERE TemporadaId = @TemporadaId;
END;
GO

-- ========================================
-- SP: Temporadas_Delete
-- ========================================
CREATE PROCEDURE Temporadas_Delete
    @TemporadaId INT
AS
BEGIN
    DELETE FROM Temporadas
    WHERE TemporadaId = @TemporadaId;
END;
GO

-- ========================================
-- SP: UsuarioRoles_Insert
-- ========================================
CREATE PROCEDURE UsuarioRoles_Insert
    @UsuId INT,
    @RolId INT
AS
BEGIN
    INSERT INTO UsuarioRoles (UsuId, RolId)
    VALUES (@UsuId, @RolId);
END;
GO

-- ========================================
-- SP: UsuarioRoles_List
-- ========================================
CREATE PROCEDURE UsuarioRoles_List
AS
BEGIN
    SELECT 
        ur.UsuarioRolId,
        ur.UsuId,
        u.UsuNombre,
        ur.RolId,
        r.RolNombre
    FROM UsuarioRoles ur
    INNER JOIN Usuarios u ON ur.UsuId = u.UsuId
    INNER JOIN Roles r ON ur.RolId = r.RolId
    ORDER BY ur.UsuarioRolId;
END;
GO

-- ========================================
-- SP: UsuarioRoles_Update
-- ========================================
CREATE PROCEDURE UsuarioRoles_Update
    @UsuarioRolId INT,
    @UsuId INT,
    @RolId INT
AS
BEGIN
    UPDATE UsuarioRoles
    SET UsuId = @UsuId,
        RolId = @RolId
    WHERE UsuarioRolId = @UsuarioRolId;
END;
GO

-- ========================================
-- SP: UsuarioRoles_Delete
-- ========================================
CREATE PROCEDURE UsuarioRoles_Delete
    @UsuarioRolId INT
AS
BEGIN
    DELETE FROM UsuarioRoles
    WHERE UsuarioRolId = @UsuarioRolId;
END;
GO


-- ========================================
-- SP: Pedidos_Insert
-- ========================================
CREATE PROCEDURE Pedidos_Insert
    @UsuId INT,
    @DireccionEnvioId INT,
    @Estado NVARCHAR(50) = 'Pendiente',
    @MetodoPago NVARCHAR(50),
    @Total DECIMAL(10,2),
    @Observaciones NVARCHAR(500)
AS
BEGIN
    INSERT INTO Pedidos (UsuId, DireccionEnvioId, Estado, MetodoPago, Total, Observaciones)
    VALUES (@UsuId, @DireccionEnvioId, @Estado, @MetodoPago, @Total, @Observaciones);
END;
GO

-- ========================================
-- SP: Pedidos_List
-- ========================================
CREATE PROCEDURE Pedidos_List
AS
BEGIN
    SELECT 
        p.PedidoId,
        p.UsuId,
        u.UsuNombre,
        p.DireccionEnvioId,
        d.DirNombre,
        d.DirTelefono,
        d.DirPais,
        d.DirDepartamento,
        d.DirCiudad,
        d.DirCodigoPostal,
        d.DirLinea1,
        d.DirLinea2,
        d.DirReferencia,
        p.Estado,
        p.MetodoPago,
        p.Total,
        p.Observaciones,
        p.FechaPedido,
        p.FechaActualizacion
    FROM Pedidos p
    INNER JOIN Usuarios u ON p.UsuId = u.UsuId
    INNER JOIN Direcciones d ON p.DireccionEnvioId = d.DirId
    ORDER BY p.FechaPedido DESC;
END;
GO


-- ========================================
-- SP: Pedidos_Update
-- ========================================
CREATE PROCEDURE Pedidos_Update
    @PedidoId INT,
    @Estado NVARCHAR(50),
    @MetodoPago NVARCHAR(50),
    @Total DECIMAL(10,2),
    @Observaciones NVARCHAR(500)
AS
BEGIN
    UPDATE Pedidos
    SET Estado = @Estado,
        MetodoPago = @MetodoPago,
        Total = @Total,
        Observaciones = @Observaciones,
        FechaActualizacion = GETDATE()
    WHERE PedidoId = @PedidoId;
END;
GO

-- ========================================
-- SP: Pedidos_Delete
-- ========================================
CREATE PROCEDURE Pedidos_Delete
    @PedidoId INT
AS
BEGIN
    DELETE FROM Pedidos
    WHERE PedidoId = @PedidoId;
END;
GO


CREATE PROCEDURE PedidoDetalle_Insert
    @PedidoId INT,
    @ProductoTallaColorId INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    INSERT INTO PedidoDetalle (PedidoId, ProductoTallaColorId, Cantidad, PrecioUnitario)
    VALUES (@PedidoId, @ProductoTallaColorId, @Cantidad, @PrecioUnitario);
END;
GO

CREATE PROCEDURE PedidoDetalle_List
AS
BEGIN
    SELECT 
        pd.PedidoDetalleId,
        pd.PedidoId,
        pd.ProductoTallaColorId,
        ptc.ProId,
        ptc.TallaId,
        ptc.ColorId,
        pd.Cantidad,
        pd.PrecioUnitario,
        pd.Subtotal
    FROM PedidoDetalle pd
    INNER JOIN ProductoTallaColor ptc ON pd.ProductoTallaColorId = ptc.ProductoTallaColorId
    ORDER BY pd.PedidoDetalleId DESC;
END;
GO

CREATE PROCEDURE PedidoDetalle_ListPorPedidoId
    @PedidoId INT
AS
BEGIN
    SELECT 
        pd.PedidoDetalleId,
        pd.PedidoId,
        pd.ProductoTallaColorId,
        ptc.ProId,
        ptc.TallaId,
        ptc.ColorId,
        p.ProNombre,
        t.TalNombre AS Talla,
        c.ColNombre AS Color,
        pd.Cantidad,
        pd.PrecioUnitario,
        pd.Subtotal
    FROM PedidoDetalle pd
    INNER JOIN ProductoTallaColor ptc ON pd.ProductoTallaColorId = ptc.ProductoTallaColorId
    INNER JOIN Productos p ON ptc.ProId = p.ProId
    INNER JOIN Tallas t ON ptc.TallaId = t.TallaId
    INNER JOIN Colores c ON ptc.ColorId = c.ColorId
    WHERE pd.PedidoId = @PedidoId
    ORDER BY pd.PedidoDetalleId;
END;
GO



-- =============================================
-- Nombre: Productos_Insert
-- Descripción: Inserta un nuevo producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Productos_Insert
    @ProNombre NVARCHAR(150),
    @ProDescripcion NVARCHAR(MAX),
    @ProPrecio DECIMAL(10, 2),
    @ProImagenPrincipal NVARCHAR(MAX),
    @ProGenero NVARCHAR(50),
    @ProCategoriaId INT
AS
BEGIN
    INSERT INTO Productos (
        ProNombre, ProDescripcion, ProPrecio,
        ProImagenPrincipal, ProGenero, ProCategoriaId
    )
    VALUES (
        @ProNombre, @ProDescripcion, @ProPrecio,
        @ProImagenPrincipal, @ProGenero, @ProCategoriaId
    );
END;
GO

-- =============================================
-- Nombre: Productos_Update
-- Descripción: Actualiza un producto existente
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Productos_Update
    @ProId INT,
    @ProNombre NVARCHAR(150),
    @ProDescripcion NVARCHAR(MAX),
    @ProPrecio DECIMAL(10, 2),
    @ProImagenPrincipal NVARCHAR(MAX),
    @ProGenero NVARCHAR(50),
    @ProCategoriaId INT,
    @ProActivo BIT
AS
BEGIN
    UPDATE Productos
    SET
        ProNombre = @ProNombre,
        ProDescripcion = @ProDescripcion,
        ProPrecio = @ProPrecio,
        ProImagenPrincipal = @ProImagenPrincipal,
        ProGenero = @ProGenero,
        ProCategoriaId = @ProCategoriaId,
        ProActivo = @ProActivo,
        ProFechaActualizacion = GETDATE()
    WHERE ProId = @ProId;
END;
GO


-- =============================================
-- Nombre: Productos_Delete
-- Descripción: Elimina un producto por ID
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Productos_Delete
    @ProId INT
AS
BEGIN
    DELETE FROM Productos
    WHERE ProId = @ProId;
END;
GO


-- =============================================
-- Nombre: Productos_List
-- Descripción: Lista todos los productos con su categoría
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Productos_List
AS
BEGIN
    SELECT 
        p.ProId,
        p.ProNombre,
        p.ProDescripcion,
        p.ProPrecio,
        p.ProImagenPrincipal,
        p.ProGenero,
        p.ProActivo,
        p.ProFechaCreacion,
        p.ProFechaActualizacion,
        c.CatId,
        c.CatNombre AS Categoria
    FROM Productos p
    INNER JOIN Categorias c ON p.ProCategoriaId = c.CatId
    ORDER BY p.ProFechaCreacion DESC;
END;
GO


-- =============================================
-- Nombre: Productos_ListPorId
-- Descripción: Devuelve un producto por su ID
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Productos_ListPorId
    @ProId INT
AS
BEGIN
    SELECT 
        p.ProId,
        p.ProNombre,
        p.ProDescripcion,
        p.ProPrecio,
        p.ProImagenPrincipal,
        p.ProGenero,
        p.ProActivo,
        p.ProFechaCreacion,
        p.ProFechaActualizacion,
        c.CatId,
        c.CatNombre AS Categoria
    FROM Productos p
    INNER JOIN Categorias c ON p.ProCategoriaId = c.CatId
    WHERE p.ProId = @ProId;
END;
GO


-- =============================================
-- Nombre: ProductoImagenes_Insert
-- Descripción: Inserta una nueva imagen secundaria para un producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoImagenes_Insert
    @ProId INT,
    @ImgUrl NVARCHAR(255),
    @ImgOrden INT = 0,
    @ImgAlt NVARCHAR(150) = NULL
AS
BEGIN
    INSERT INTO ProductoImagenes (
        ProId, ImgUrl, ImgOrden, ImgAlt
    )
    VALUES (
        @ProId, @ImgUrl, @ImgOrden, @ImgAlt
    );
END;
GO


-- =============================================
-- Nombre: ProductoImagenes_Update
-- Descripción: Actualiza una imagen secundaria de un producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoImagenes_Update
    @ImgId INT,
    @ImgUrl NVARCHAR(255),
    @ImgOrden INT,
    @ImgAlt NVARCHAR(150)
AS
BEGIN
    UPDATE ProductoImagenes
    SET
        ImgUrl = @ImgUrl,
        ImgOrden = @ImgOrden,
        ImgAlt = @ImgAlt
    WHERE ImgId = @ImgId;
END;
GO


-- =============================================
-- Nombre: ProductoImagenes_Delete
-- Descripción: Elimina una imagen secundaria por su ID
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoImagenes_Delete
    @ImgId INT
AS
BEGIN
    DELETE FROM ProductoImagenes
    WHERE ImgId = @ImgId;
END;
GO

-- =============================================
-- Nombre: ProductoImagenes_List
-- Descripción: Lista todas las imágenes secundarias de todos los productos
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoImagenes_List
AS
BEGIN
    SELECT 
        ImgId,
        ProId,
        ImgUrl,
        ImgOrden,
        ImgAlt,
        ImgFechaCreacion
    FROM ProductoImagenes
    ORDER BY ProId, ImgOrden;
END;
GO


-- =============================================
-- Nombre: ProductoImagenes_ListPorProducto
-- Descripción: Lista todas las imágenes secundarias de un producto específico
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoImagenes_ListPorProducto
    @ProId INT
AS
BEGIN
    SELECT 
        ImgId,
        ProId,
        ImgUrl,
        ImgOrden,
        ImgAlt,
        ImgFechaCreacion
    FROM ProductoImagenes
    WHERE ProId = @ProId
    ORDER BY ImgOrden;
END;
GO



-- =============================================
-- Nombre: ProductoImagenes_Insert
-- Descripción: Inserta una nueva imagen secundaria para un producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoImagenes_Insert
    @ProId INT,
    @ImgUrl NVARCHAR(255),
    @ImgOrden INT = 0,
    @ImgAlt NVARCHAR(150) = NULL
AS
BEGIN
    INSERT INTO ProductoImagenes (
        ProId, ImgUrl, ImgOrden, ImgAlt
    )
    VALUES (
        @ProId, @ImgUrl, @ImgOrden, @ImgAlt
    );
END;
GO


-- =============================================
-- Nombre: ProductoImagenes_Update
-- Descripción: Actualiza una imagen secundaria de un producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoImagenes_Update
    @ImgId INT,
    @ImgUrl NVARCHAR(255),
    @ImgOrden INT,
    @ImgAlt NVARCHAR(150)
AS
BEGIN
    UPDATE ProductoImagenes
    SET
        ImgUrl = @ImgUrl,
        ImgOrden = @ImgOrden,
        ImgAlt = @ImgAlt
    WHERE ImgId = @ImgId;
END;
GO


-- =============================================
-- Nombre: ProductoTallaColor_Insert
-- Descripción: Inserta una nueva combinación de producto, talla y color
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoTallaColor_Insert
    @ProId INT,
    @TallaId INT,
    @ColorId INT,
    @Stock INT,
    @PrecioOferta DECIMAL(10,2) = NULL,
    @SKU NVARCHAR(50) = NULL
AS
BEGIN
    INSERT INTO ProductoTallaColor (
        ProId, TallaId, ColorId, Stock, PrecioOferta, SKU
    )
    VALUES (
        @ProId, @TallaId, @ColorId, @Stock, @PrecioOferta, @SKU
    );
END;
GO


-- =============================================
-- Nombre: ProductoTallaColor_Update
-- Descripción: Actualiza una combinación de producto, talla y color
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoTallaColor_Update
    @ProductoTallaColorId INT,
    @Stock INT,
    @PrecioOferta DECIMAL(10,2),
    @SKU NVARCHAR(50)
AS
BEGIN
    UPDATE ProductoTallaColor
    SET
        Stock = @Stock,
        PrecioOferta = @PrecioOferta,
        SKU = @SKU,
        FechaActualizacion = GETDATE()
    WHERE ProductoTallaColorId = @ProductoTallaColorId;
END;
GO


-- =============================================
-- Nombre: ProductoTallaColor_Update
-- Descripción: Actualiza una combinación de producto, talla y color
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoTallaColor_Update
    @ProductoTallaColorId INT,
    @Stock INT,
    @PrecioOferta DECIMAL(10,2),
    @SKU NVARCHAR(50)
AS
BEGIN
    UPDATE ProductoTallaColor
    SET
        Stock = @Stock,
        PrecioOferta = @PrecioOferta,
        SKU = @SKU,
        FechaActualizacion = GETDATE()
    WHERE ProductoTallaColorId = @ProductoTallaColorId;
END;
GO


-- =============================================
-- Nombre: ProductoTallaColor_Delete
-- Descripción: Elimina un registro de combinación de producto, talla y color
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoTallaColor_Delete
    @ProductoTallaColorId INT
AS
BEGIN
    DELETE FROM ProductoTallaColor
    WHERE ProductoTallaColorId = @ProductoTallaColorId;
END;
GO


-- =============================================
-- Nombre: ProductoTallaColor_List
-- Descripción: Lista todas las combinaciones producto-talla-color
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoTallaColor_List
AS
BEGIN
    SELECT 
        ptc.ProductoTallaColorId,
        ptc.ProId,
        p.ProNombre,
        ptc.TallaId,
        t.TalNombre,
        ptc.ColorId,
        c.ColNombre,
        ptc.Stock,
        ptc.PrecioOferta,
        ptc.SKU,
        ptc.FechaCreacion,
        ptc.FechaActualizacion
    FROM ProductoTallaColor ptc
    INNER JOIN Productos p ON ptc.ProId = p.ProId
    INNER JOIN Tallas t ON ptc.TallaId = t.TallaId
    INNER JOIN Colores c ON ptc.ColorId = c.ColorId;
END;
GO


-- =============================================
-- Nombre: ProductoTallaColor_ListPorProducto
-- Descripción: Lista combinaciones de tallas y colores por producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE ProductoTallaColor_ListPorProducto
    @ProId INT
AS
BEGIN
    SELECT 
        ptc.ProductoTallaColorId,
        ptc.ProId,
        ptc.TallaId,
        t.TalNombre,
        ptc.ColorId,
        c.ColNombre,
        ptc.Stock,
        ptc.PrecioOferta,
        ptc.SKU,
        ptc.FechaCreacion,
        ptc.FechaActualizacion
    FROM ProductoTallaColor ptc
    INNER JOIN Tallas t ON ptc.TallaId = t.TallaId
    INNER JOIN Colores c ON ptc.ColorId = c.ColorId
    WHERE ptc.ProId = @ProId;
END;
GO


-- =============================================
-- Nombre: Promociones_Insert
-- Descripción: Inserta una nueva promoción
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Promociones_Insert
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255) = NULL,
    @TipoDescuento NVARCHAR(20),
    @ValorDescuento DECIMAL(10,2),
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @Activo BIT = 1
AS
BEGIN
    INSERT INTO Promociones (
        Nombre, Descripcion, TipoDescuento, ValorDescuento,
        FechaInicio, FechaFin, Activo
    )
    VALUES (
        @Nombre, @Descripcion, @TipoDescuento, @ValorDescuento,
        @FechaInicio, @FechaFin, @Activo
    );
END;
GO

-- =============================================
-- Nombre: Promociones_Update
-- Descripción: Actualiza los datos de una promoción
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Promociones_Update
    @PromocionId INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255) = NULL,
    @TipoDescuento NVARCHAR(20),
    @ValorDescuento DECIMAL(10,2),
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @Activo BIT
AS
BEGIN
    UPDATE Promociones
    SET 
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        TipoDescuento = @TipoDescuento,
        ValorDescuento = @ValorDescuento,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        Activo = @Activo
    WHERE PromocionId = @PromocionId;
END;
GO


-- =============================================
-- Nombre: Promociones_Delete
-- Descripción: Elimina una promoción por ID
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Promociones_Delete
    @PromocionId INT
AS
BEGIN
    DELETE FROM Promociones
    WHERE PromocionId = @PromocionId;
END;
GO


-- =============================================
-- Nombre: Promociones_List
-- Descripción: Lista todas las promociones
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Promociones_List
AS
BEGIN
    SELECT 
        PromocionId,
        Nombre,
        Descripcion,
        TipoDescuento,
        ValorDescuento,
        FechaInicio,
        FechaFin,
        Activo
    FROM Promociones;
END;
GO


-- =============================================
-- Nombre: Promociones_ListPorId
-- Descripción: Obtiene los datos de una promoción por su ID
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Promociones_ListPorId
    @PromocionId INT
AS
BEGIN
    SELECT 
        PromocionId,
        Nombre,
        Descripcion,
        TipoDescuento,
        ValorDescuento,
        FechaInicio,
        FechaFin,
        Activo
    FROM Promociones
    WHERE PromocionId = @PromocionId;
END;
GO


-- =============================================
-- Nombre: PromocionProducto_Insert
-- Descripción: Asocia un producto a una promoción
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionProducto_Insert
    @PromocionId INT,
    @ProId INT
AS
BEGIN
    INSERT INTO PromocionProducto (PromocionId, ProId)
    VALUES (@PromocionId, @ProId);
END;
GO


-- =============================================
-- Nombre: PromocionProducto_Update
-- Descripción: Actualiza la asociación entre promoción y producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionProducto_Update
    @PromocionProductoId INT,
    @PromocionId INT,
    @ProId INT
AS
BEGIN
    UPDATE PromocionProducto
    SET 
        PromocionId = @PromocionId,
        ProId = @ProId
    WHERE PromocionProductoId = @PromocionProductoId;
END;
GO


-- =============================================
-- Nombre: PromocionProducto_Delete
-- Descripción: Elimina una asociación entre promoción y producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionProducto_Delete
    @PromocionProductoId INT
AS
BEGIN
    DELETE FROM PromocionProducto
    WHERE PromocionProductoId = @PromocionProductoId;
END;
GO


-- =============================================
-- Nombre: PromocionProducto_List
-- Descripción: Lista todas las asociaciones entre promociones y productos
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionProducto_List
AS
BEGIN
    SELECT 
        pp.PromocionProductoId,
        pp.PromocionId,
        p.Nombre AS NombrePromocion,
        pp.ProId,
        pr.ProNombre AS NombreProducto
    FROM PromocionProducto pp
    INNER JOIN Promociones p ON pp.PromocionId = p.PromocionId
    INNER JOIN Productos pr ON pp.ProId = pr.ProId;
END;
GO


-- =============================================
-- Nombre: PromocionProducto_ListPorPromocion
-- Descripción: Lista los productos asociados a una promoción específica
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionProducto_ListPorPromocion
    @PromocionId INT
AS
BEGIN
    SELECT 
        pp.PromocionProductoId,
        pp.ProId,
        pr.ProNombre
    FROM PromocionProducto pp
    INNER JOIN Productos pr ON pp.ProId = pr.ProId
    WHERE pp.PromocionId = @PromocionId;
END;
GO


-- =============================================
-- Nombre: PromocionCategoria_Insert
-- Descripción: Asocia una promoción a una categoría
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionCategoria_Insert
    @PromocionId INT,
    @CatId INT
AS
BEGIN
    INSERT INTO PromocionCategoria (PromocionId, CatId)
    VALUES (@PromocionId, @CatId);
END;
GO


-- =============================================
-- Nombre: PromocionCategoria_Update
-- Descripción: Actualiza la relación entre promoción y categoría
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionCategoria_Update
    @PromocionCategoriaId INT,
    @PromocionId INT,
    @CatId INT
AS
BEGIN
    UPDATE PromocionCategoria
    SET 
        PromocionId = @PromocionId,
        CatId = @CatId
    WHERE PromocionCategoriaId = @PromocionCategoriaId;
END;
GO


-- =============================================
-- Nombre: PromocionCategoria_Delete
-- Descripción: Elimina una relación entre promoción y categoría
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionCategoria_Delete
    @PromocionCategoriaId INT
AS
BEGIN
    DELETE FROM PromocionCategoria
    WHERE PromocionCategoriaId = @PromocionCategoriaId;
END;
GO


-- =============================================
-- Nombre: PromocionCategoria_List
-- Descripción: Lista todas las promociones asociadas a categorías
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionCategoria_List
AS
BEGIN
    SELECT 
        pc.PromocionCategoriaId,
        pc.PromocionId,
        p.Nombre AS NombrePromocion,
        pc.CatId,
        c.CatNombre AS NombreCategoria
    FROM PromocionCategoria pc
    INNER JOIN Promociones p ON pc.PromocionId = p.PromocionId
    INNER JOIN Categorias c ON pc.CatId = c.CatId;
END;
GO


-- =============================================
-- Nombre: PromocionCategoria_ListPorPromocion
-- Descripción: Lista las categorías asociadas a una promoción específica
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE PromocionCategoria_ListPorPromocion
    @PromocionId INT
AS
BEGIN
    SELECT 
        pc.PromocionCategoriaId,
        pc.CatId,
        c.CatNombre
    FROM PromocionCategoria pc
    INNER JOIN Categorias c ON pc.CatId = c.CatId
    WHERE pc.PromocionId = @PromocionId;
END;
GO


-- =============================================
-- Nombre: Carrito_Insert
-- Descripción: Crea un carrito para un usuario
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Carrito_Insert
    @UsuId INT
AS
BEGIN
    INSERT INTO Carrito (UsuId)
    VALUES (@UsuId);
END;
GO


-- =============================================
-- Nombre: Carrito_Update
-- Descripción: Actualiza el campo de usuario en un carrito
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Carrito_Update
    @CarritoId INT,
    @UsuId INT
AS
BEGIN
    UPDATE Carrito
    SET UsuId = @UsuId
    WHERE CarritoId = @CarritoId;
END;
GO

-- =============================================
-- Nombre: Carrito_Delete
-- Descripción: Elimina un carrito por su ID
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Carrito_Delete
    @CarritoId INT
AS
BEGIN
    DELETE FROM Carrito
    WHERE CarritoId = @CarritoId;
END;
GO


-- =============================================
-- Nombre: Carrito_List
-- Descripción: Lista todos los carritos con información del usuario
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Carrito_List
AS
BEGIN
    SELECT 
        c.CarritoId,
        c.UsuId,
        u.UsuNombre,
        u.UsuEmail,
        c.FechaCreacion
    FROM Carrito c
    INNER JOIN Usuarios u ON c.UsuId = u.UsuId;
END;
GO


-- =============================================
-- Nombre: Carrito_ListPorUsuario
-- Descripción: Lista el carrito de un usuario específico
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE Carrito_ListPorUsuario
    @UsuId INT
AS
BEGIN
    SELECT 
        CarritoId,
        UsuId,
        FechaCreacion
    FROM Carrito
    WHERE UsuId = @UsuId;
END;
GO

-- =============================================
-- Nombre: CarritoItem_Insert
-- Descripción: Inserta un nuevo producto en el carrito
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE CarritoItem_Insert
    @CarritoId INT,
    @ProductoTallaColorId INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    INSERT INTO CarritoItem (CarritoId, ProductoTallaColorId, Cantidad, PrecioUnitario)
    VALUES (@CarritoId, @ProductoTallaColorId, @Cantidad, @PrecioUnitario);
END;
GO


-- =============================================
-- Nombre: CarritoItem_Update
-- Descripción: Actualiza la cantidad y el precio de un ítem del carrito
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE CarritoItem_Update
    @CarritoItemId INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    UPDATE CarritoItem
    SET Cantidad = @Cantidad,
        PrecioUnitario = @PrecioUnitario
    WHERE CarritoItemId = @CarritoItemId;
END;
GO


-- =============================================
-- Nombre: CarritoItem_Delete
-- Descripción: Elimina un ítem del carrito por su ID
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE CarritoItem_Delete
    @CarritoItemId INT
AS
BEGIN
    DELETE FROM CarritoItem
    WHERE CarritoItemId = @CarritoItemId;
END;
GO


-- =============================================
-- Nombre: CarritoItem_List
-- Descripción: Lista todos los ítems del carrito con detalles del producto
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE CarritoItem_List
AS
BEGIN
    SELECT 
        ci.CarritoItemId,
        ci.CarritoId,
        ci.ProductoTallaColorId,
        p.ProNombre,
        t.TalNombre AS Talla,
        c.ColNombre AS Color,
        ci.Cantidad,
        ci.PrecioUnitario,
        ci.FechaAgregado
    FROM CarritoItem ci
    INNER JOIN ProductoTallaColor ptc ON ci.ProductoTallaColorId = ptc.ProductoTallaColorId
    INNER JOIN Productos p ON ptc.ProId = p.ProId
    INNER JOIN Tallas t ON ptc.TallaId = t.TallaId
    INNER JOIN Colores c ON ptc.ColorId = c.ColorId;
END;
GO


-- =============================================
-- Nombre: CarritoItem_ListPorCarrito
-- Descripción: Lista los ítems de un carrito específico
-- Fecha de creación: 2025-07-04
-- Autor: Andres Espitia
-- =============================================
CREATE PROCEDURE CarritoItem_ListPorCarrito
    @CarritoId INT
AS
BEGIN
    SELECT 
        ci.CarritoItemId,
        ci.CarritoId,
        ci.ProductoTallaColorId,
        p.ProNombre,
        t.TalNombre AS Talla,
        c.ColNombre AS Color,
        ci.Cantidad,
        ci.PrecioUnitario,
        ci.FechaAgregado
    FROM CarritoItem ci
    INNER JOIN ProductoTallaColor ptc ON ci.ProductoTallaColorId = ptc.ProductoTallaColorId
    INNER JOIN Productos p ON ptc.ProId = p.ProId
    INNER JOIN Tallas t ON ptc.TallaId = t.TallaId
    INNER JOIN Colores c ON ptc.ColorId = c.ColorId
    WHERE ci.CarritoId = @CarritoId;
END;
GO
