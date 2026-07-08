--  03 — Bitácora y Auditoría (control de cambios)
--  Crea las tablas Bitacora (log general de eventos: login, logout, errores,
--  excepciones y operaciones de negocio) y las dos tablas de auditoría:
--  AuditoriaUsuario y AuditoriaIdioma.
--
--  Las tablas de auditoría guardan IdUsuario/IdIdioma como dato (referencia
--  lógica), SIN clave foránea. Si existiera la FK, no se podrían eliminar usuarios ni idiomas con historial. 
--  Sin FK, el historial sobrevive a las bajas.
--
--  ORDEN DE EJECUCIÓN: 3°

USE Usuarios;
GO

-- Bitácora general (todas las operaciones)
CREATE TABLE Bitacora (
    IdBitacora    INT          PRIMARY KEY IDENTITY(1,1),
    Fecha         DATETIME     NOT NULL DEFAULT GETDATE(),
    Usuario       VARCHAR(100) NOT NULL,
    Actividad     VARCHAR(100) NOT NULL,
    TipoEvento    VARCHAR(20)  NOT NULL,   -- Exito, Error, Excepcion
    Descripcion   VARCHAR(500) NULL,
    Entidad       VARCHAR(50)  NULL,
    ValorAnterior VARCHAR(500) NULL,
    ValorNuevo    VARCHAR(500) NULL
);
GO

-- Control de cambios de Usuario
CREATE TABLE AuditoriaUsuario (
    IdAuditoria    INT          PRIMARY KEY IDENTITY(1,1),
    Fecha          DATETIME     NOT NULL DEFAULT GETDATE(),
    UsuarioAccion  VARCHAR(100) NOT NULL,
    Operacion      VARCHAR(20)  NOT NULL,   -- 'ADD' / 'MODIFY' / 'DELETE' / 'ASIGNAR_PERFIL'
    IdUsuario      INT          NOT NULL,
    NombreAnterior VARCHAR(100) NULL,
    NombreNuevo    VARCHAR(100) NULL
);
GO

-- Control de cambios de Idioma
CREATE TABLE AuditoriaIdioma (
    IdAuditoria     INT          PRIMARY KEY IDENTITY(1,1),
    Fecha           DATETIME     NOT NULL DEFAULT GETDATE(),
    UsuarioAccion   VARCHAR(100) NOT NULL,
    Operacion       VARCHAR(20)  NOT NULL,
    IdIdioma        INT          NOT NULL,
    NombreAnterior  VARCHAR(100) NULL,
    NombreNuevo     VARCHAR(100) NULL,
    ClaveTraduccion VARCHAR(100) NULL,
    ValorAnterior   VARCHAR(200) NULL,
    ValorNuevo      VARCHAR(200) NULL
);
GO

PRINT '03 OK — Tablas de bitácora y auditoría creadas.';
GO

-- Verificación final del estado de la base hasta acá
SELECT 'Usuarios'         AS Tabla, COUNT(*) AS Registros FROM Usuarios         UNION ALL
SELECT 'Permiso',                   COUNT(*)               FROM Permiso          UNION ALL
SELECT 'PermisoHijo',               COUNT(*)               FROM PermisoHijo      UNION ALL
SELECT 'UsuarioPermiso',            COUNT(*)               FROM UsuarioPermiso   UNION ALL
SELECT 'Idioma',                    COUNT(*)               FROM Idioma           UNION ALL
SELECT 'Palabra',                   COUNT(*)               FROM Palabra          UNION ALL
SELECT 'Traduccion',                COUNT(*)               FROM Traduccion       UNION ALL
SELECT 'Bitacora',                  COUNT(*)               FROM Bitacora         UNION ALL
SELECT 'AuditoriaUsuario',          COUNT(*)               FROM AuditoriaUsuario UNION ALL
SELECT 'AuditoriaIdioma',           COUNT(*)               FROM AuditoriaIdioma;
GO
