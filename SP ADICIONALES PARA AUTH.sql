-- ========================================
-- PROCEDIMIENTOS ALMACENADOS ADICIONALES PARA AUTH
-- ========================================

-- ========================================
-- SP: Usuario_Login
-- Descripción: Autenticación de usuario por email y contraseña
-- ========================================
CREATE PROCEDURE Usuario_Login
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SELECT 
        UsuId,
        UsuNombre,
        UsuApellido,
        UsuEmail,
        UsuTelefono,
        UsuGenero,
        UsuFechaNacimiento,
        UsuImagenPerfil,
        UsuEstadoCuenta,
        UsuFechaRegistro,
        UsuFechaUltimoLogin,
        UsuProveedorAutenticacion,
        UsuFechaActualizacion
    FROM Usuarios 
    WHERE UsuEmail = @Email 
    AND UsuPasswordHash = @PasswordHash
    AND UsuEstadoCuenta = 'Activo';
END;
GO

-- ========================================
-- SP: Usuario_UpdateLastLogin
-- Descripción: Actualiza la fecha del último login
-- ========================================
CREATE PROCEDURE Usuario_UpdateLastLogin
    @UsuId INT
AS
BEGIN
    UPDATE Usuarios
    SET UsuFechaUltimoLogin = GETDATE()
    WHERE UsuId = @UsuId;
END;
GO

-- ========================================
-- SP: Usuario_CheckEmailExists
-- Descripción: Verifica si un email ya existe
-- ========================================
CREATE PROCEDURE Usuario_CheckEmailExists
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT COUNT(*) as Existe
    FROM Usuarios 
    WHERE UsuEmail = @Email;
END;
GO

-- ========================================
-- SP: Usuario_ChangePassword
-- Descripción: Cambia la contraseña de un usuario
-- ========================================
CREATE PROCEDURE Usuario_ChangePassword
    @UsuId INT,
    @NewPasswordHash NVARCHAR(255)
AS
BEGIN
    UPDATE Usuarios
    SET 
        UsuPasswordHash = @NewPasswordHash,
        UsuFechaActualizacion = GETDATE()
    WHERE UsuId = @UsuId;
END;
GO

-- ========================================
-- SP: Usuario_GetByEmail
-- Descripción: Obtiene usuario por email
-- ========================================
CREATE PROCEDURE Usuario_GetByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT 
        UsuId,
        UsuNombre,
        UsuApellido,
        UsuEmail,
        UsuTelefono,
        UsuGenero,
        UsuFechaNacimiento,
        UsuImagenPerfil,
        UsuEstadoCuenta,
        UsuFechaRegistro,
        UsuFechaUltimoLogin,
        UsuProveedorAutenticacion,
        UsuFechaActualizacion
    FROM Usuarios 
    WHERE UsuEmail = @Email;
END;
GO

-- ========================================
-- SP: Usuario_GetWithRoles
-- Descripción: Obtiene usuario con sus roles
-- ========================================
CREATE PROCEDURE Usuario_GetWithRoles
    @UsuId INT
AS
BEGIN
    SELECT 
        u.UsuId,
        u.UsuNombre,
        u.UsuApellido,
        u.UsuEmail,
        u.UsuTelefono,
        u.UsuGenero,
        u.UsuFechaNacimiento,
        u.UsuImagenPerfil,
        u.UsuEstadoCuenta,
        u.UsuFechaRegistro,
        u.UsuFechaUltimoLogin,
        u.UsuProveedorAutenticacion,
        u.UsuFechaActualizacion,
        r.RolId,
        r.RolNombre
    FROM Usuarios u
    LEFT JOIN UsuarioRoles ur ON u.UsuId = ur.UsuId
    LEFT JOIN Roles r ON ur.RolId = r.RolId
    WHERE u.UsuId = @UsuId;
END;
GO

-- ========================================
-- SP: Usuario_GetStatistics
-- Descripción: Obtiene estadísticas de usuarios
-- ========================================
CREATE PROCEDURE Usuario_GetStatistics
AS
BEGIN
    SELECT 
        COUNT(*) as TotalUsuarios,
        COUNT(CASE WHEN UsuEstadoCuenta = 'Activo' THEN 1 END) as UsuariosActivos,
        COUNT(CASE WHEN UsuEstadoCuenta = 'Inactivo' THEN 1 END) as UsuariosInactivos,
        COUNT(CASE WHEN UsuFechaRegistro >= DATEADD(MONTH, -1, GETDATE()) THEN 1 END) as UsuariosNuevosEsteMes,
        COUNT(CASE WHEN UsuFechaUltimoLogin >= DATEADD(DAY, -7, GETDATE()) THEN 1 END) as UsuariosActivosEstaSemana
    FROM Usuarios;
END;
GO

-- ========================================
-- SP: Usuario_GetActiveUsers
-- Descripción: Obtiene usuarios activos
-- ========================================
CREATE PROCEDURE Usuario_GetActiveUsers
AS
BEGIN
    SELECT 
        UsuId,
        UsuNombre,
        UsuApellido,
        UsuEmail,
        UsuTelefono,
        UsuGenero,
        UsuFechaNacimiento,
        UsuImagenPerfil,
        UsuEstadoCuenta,
        UsuFechaRegistro,
        UsuFechaUltimoLogin,
        UsuProveedorAutenticacion,
        UsuFechaActualizacion
    FROM Usuarios 
    WHERE UsuEstadoCuenta = 'Activo'
    ORDER BY UsuFechaRegistro DESC;
END;
GO

-- ========================================
-- SP: Usuario_DeactivateAccount
-- Descripción: Desactiva una cuenta de usuario
-- ========================================
CREATE PROCEDURE Usuario_DeactivateAccount
    @UsuId INT
AS
BEGIN
    UPDATE Usuarios
    SET 
        UsuEstadoCuenta = 'Inactivo',
        UsuFechaActualizacion = GETDATE()
    WHERE UsuId = @UsuId;
END;
GO

-- ========================================
-- SP: Usuario_ActivateAccount
-- Descripción: Activa una cuenta de usuario
-- ========================================
CREATE PROCEDURE Usuario_ActivateAccount
    @UsuId INT
AS
BEGIN
    UPDATE Usuarios
    SET 
        UsuEstadoCuenta = 'Activo',
        UsuFechaActualizacion = GETDATE()
    WHERE UsuId = @UsuId;
END;
GO

-- ========================================
-- SP: Usuario_GetLoginHistory
-- Descripción: Obtiene historial de logins de un usuario
-- ========================================
CREATE PROCEDURE Usuario_GetLoginHistory
    @UsuId INT,
    @Days INT = 30
AS
BEGIN
    SELECT 
        Tipo,
        Mensaje,
        Origen,
        Metodo,
        DatosEntrada,
        DatosSalida,
        IP,
        Navegador,
        Fecha
    FROM Logs
    WHERE UsuId = @UsuId 
    AND Tipo IN ('Info', 'Error')
    AND Mensaje LIKE '%login%'
    AND Fecha >= DATEADD(DAY, -@Days, GETDATE())
    ORDER BY Fecha DESC;
END;
GO

-- ========================================
-- SP: Usuario_GetFailedLoginAttempts
-- Descripción: Obtiene intentos fallidos de login
-- ========================================
CREATE PROCEDURE Usuario_GetFailedLoginAttempts
    @Email NVARCHAR(100),
    @Hours INT = 24
AS
BEGIN
    SELECT 
        COUNT(*) as IntentosFallidos
    FROM Logs
    WHERE DatosEntrada LIKE '%' + @Email + '%'
    AND Tipo = 'Error'
    AND Mensaje LIKE '%login%'
    AND Fecha >= DATEADD(HOUR, -@Hours, GETDATE());
END;
GO

-- ========================================
-- SP: Usuario_ResetPassword
-- Descripción: Resetea la contraseña de un usuario
-- ========================================
CREATE PROCEDURE Usuario_ResetPassword
    @Email NVARCHAR(100),
    @NewPasswordHash NVARCHAR(255)
AS
BEGIN
    UPDATE Usuarios
    SET 
        UsuPasswordHash = @NewPasswordHash,
        UsuFechaActualizacion = GETDATE()
    WHERE UsuEmail = @Email;
END;
GO

-- ========================================
-- SP: Usuario_GetProfile
-- Descripción: Obtiene perfil completo de usuario
-- ========================================
CREATE PROCEDURE Usuario_GetProfile
    @UsuId INT
AS
BEGIN
    SELECT 
        u.UsuId,
        u.UsuNombre,
        u.UsuApellido,
        u.UsuEmail,
        u.UsuTelefono,
        u.UsuGenero,
        u.UsuFechaNacimiento,
        u.UsuImagenPerfil,
        u.UsuEstadoCuenta,
        u.UsuFechaRegistro,
        u.UsuFechaUltimoLogin,
        u.UsuProveedorAutenticacion,
        u.UsuFechaActualizacion,
        (SELECT COUNT(*) FROM Pedidos WHERE UsuId = u.UsuId) as TotalPedidos,
        (SELECT COUNT(*) FROM Direcciones WHERE UsuId = u.UsuId) as TotalDirecciones,
        (SELECT COUNT(*) FROM Carritos WHERE UsuId = u.UsuId) as TotalCarritos
    FROM Usuarios u
    WHERE u.UsuId = @UsuId;
END;
GO 