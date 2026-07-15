-- 03 · Bitácora y Auditoría — log de eventos y control de cambios de usuarios/idiomas.
-- Las auditorías no tienen FK a Usuarios/Idioma para preservar el historial ante bajas.

USE Usuarios;
GO

CREATE TABLE Bitacora (
    IdBitacora    INT          PRIMARY KEY IDENTITY(1,1),
    Fecha         DATETIME     NOT NULL DEFAULT GETDATE(),
    Usuario       VARCHAR(100) NOT NULL,
    Actividad     VARCHAR(100) NOT NULL,
    TipoEvento    VARCHAR(20)  NOT NULL,
    Descripcion   VARCHAR(500) NULL,
    Entidad       VARCHAR(50)  NULL,
    ValorAnterior VARCHAR(500) NULL,
    ValorNuevo    VARCHAR(500) NULL
);

CREATE TABLE AuditoriaUsuario (
    IdAuditoria    INT           PRIMARY KEY IDENTITY(1,1),
    Fecha          DATETIME      NOT NULL DEFAULT GETDATE(),
    UsuarioAccion  VARCHAR(100)  NOT NULL,
    Operacion      VARCHAR(50)   NOT NULL,
    IdUsuario      INT           NOT NULL,
    NombreAnterior VARCHAR(500)  NULL,
    NombreNuevo    VARCHAR(500)  NULL,
    SnapshotJson   NVARCHAR(MAX) NULL
);

CREATE TABLE AuditoriaIdioma (
    IdAuditoria     INT          PRIMARY KEY IDENTITY(1,1),
    Fecha           DATETIME     NOT NULL DEFAULT GETDATE(),
    UsuarioAccion   VARCHAR(100) NOT NULL,
    Operacion       VARCHAR(50)  NOT NULL,
    IdIdioma        INT          NOT NULL,
    NombreAnterior  VARCHAR(100) NULL,
    NombreNuevo     VARCHAR(100) NULL,
    ClaveTraduccion VARCHAR(100) NULL,
    ValorAnterior   VARCHAR(200) NULL,
    ValorNuevo      VARCHAR(200) NULL
);
GO
