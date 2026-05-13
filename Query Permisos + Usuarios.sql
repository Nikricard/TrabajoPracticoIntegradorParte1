

USE Usuarios;
GO

-- ════════════════════════════════════════════════════════
-- PASO 1: Limpiar todo en orden (respetando FKs)
-- ════════════════════════════════════════════════════════
/*
IF OBJECT_ID('AuditoriaIdioma',  'U') IS NOT NULL DROP TABLE AuditoriaIdioma;
IF OBJECT_ID('AuditoriaUsuario', 'U') IS NOT NULL DROP TABLE AuditoriaUsuario;
IF OBJECT_ID('Bitacora',         'U') IS NOT NULL DROP TABLE Bitacora;
IF OBJECT_ID('PermisoHijo',      'U') IS NOT NULL DROP TABLE PermisoHijo;
IF OBJECT_ID('Perfil',           'U') IS NOT NULL DROP TABLE Perfil;
IF OBJECT_ID('Permiso',          'U') IS NOT NULL DROP TABLE Permiso;
IF OBJECT_ID('Traduccion',       'U') IS NOT NULL DROP TABLE Traduccion;
IF OBJECT_ID('Palabra',          'U') IS NOT NULL DROP TABLE Palabra;
IF OBJECT_ID('Idioma',           'U') IS NOT NULL DROP TABLE Idioma;
IF OBJECT_ID('Usuarios',         'U') IS NOT NULL DROP TABLE Usuarios;
GO*/

-- ════════════════════════════════════════════════════════
-- PASO 2: Tablas base
-- ════════════════════════════════════════════════════════

-- T01 — Usuarios
CREATE TABLE Usuarios (
    Id        INT          PRIMARY KEY IDENTITY(1,1),
    Nombre    VARCHAR(100) NOT NULL UNIQUE,
    Contrasena VARCHAR(200) NOT NULL,
    IdPerfil  INT          NULL   -- FK se agrega después de crear Perfil
);
GO

-- ════════════════════════════════════════════════════════
-- PASO 3: T04 — Permisos y Perfiles (Patrón Composite)
-- ════════════════════════════════════════════════════════

CREATE TABLE Permiso (
    Codigo      VARCHAR(10)  PRIMARY KEY,
    Nombre      VARCHAR(100) NOT NULL,
    EsCompuesto BIT          NOT NULL DEFAULT 0
);
GO

CREATE TABLE PermisoHijo (
    CodigoPadre VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    CodigoHijo  VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    PRIMARY KEY (CodigoPadre, CodigoHijo)
);
GO

CREATE TABLE Perfil (
    IdPerfil INT         PRIMARY KEY IDENTITY(1,1),
    Nombre   VARCHAR(50) NOT NULL UNIQUE,
    Codigo   VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo)
);
GO

-- FK de Usuarios → Perfil
ALTER TABLE Usuarios
    ADD CONSTRAINT FK_Usuarios_Perfil
    FOREIGN KEY (IdPerfil) REFERENCES Perfil(IdPerfil);
GO

-- ── Permisos atómicos
INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES
('USR001', 'Crear usuario',           0),
('USR002', 'Modificar usuario',       0),
('USR003', 'Eliminar usuario',        0),
('USR004', 'Listar usuarios',         0),
('IDM001', 'Agregar idioma',          0),
('IDM002', 'Eliminar idioma',         0),
('IDM003', 'Gestionar traducciones',  0),
('BIT001', 'Ver bitácora',            0),
('PRF001', 'Gestionar perfiles',      0);

-- ── Permisos compuestos
INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES
('GE010', 'Gestión de usuarios', 1),
('GE020', 'Gestión de idiomas',  1),
('GE040', 'Operador',            1),
('GE030', 'Administrador',       1);

-- ── Árbol: GE010 → usuarios
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE010','USR001'),('GE010','USR002'),
('GE010','USR003'),('GE010','USR004');

-- ── Árbol: GE020 → idiomas
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE020','IDM001'),('GE020','IDM002'),('GE020','IDM003');

-- ── Árbol: GE040 (Operador) → usuarios + idiomas
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE040','GE010'),('GE040','GE020');

-- ── Árbol: GE030 (Admin) → todo
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE030','GE010'),('GE030','GE020'),
('GE030','BIT001'),('GE030','PRF001');

-- ── Perfiles fijos
INSERT INTO Perfil (Nombre, Codigo) VALUES
('Administrador', 'GE030'),  -- IdPerfil = 1
('Operador',      'GE040'),  -- IdPerfil = 2
('Usuario',       'USR004'); -- IdPerfil = 3
GO


-- ════════════════════════════════════════════════════════
-- PASO 4: T06 — Bitácora y Auditoría
-- ════════════════════════════════════════════════════════

CREATE TABLE Bitacora (
    IdBitacora    INT          PRIMARY KEY IDENTITY(1,1),
    Fecha         DATETIME     NOT NULL DEFAULT GETDATE(),
    Usuario       VARCHAR(100) NOT NULL,
    Actividad     VARCHAR(100) NOT NULL,
    TipoEvento    VARCHAR(20)  NOT NULL,  -- 'Exito','Error','Excepcion'
    Descripcion   VARCHAR(500) NULL,
    Entidad       VARCHAR(50)  NULL,
    ValorAnterior VARCHAR(500) NULL,
    ValorNuevo    VARCHAR(500) NULL
);
GO

CREATE TABLE AuditoriaUsuario (
    IdAuditoria    INT          PRIMARY KEY IDENTITY(1,1),
    Fecha          DATETIME     NOT NULL DEFAULT GETDATE(),
    UsuarioAccion  VARCHAR(100) NOT NULL,
    Operacion      VARCHAR(20)  NOT NULL,  -- 'ADD','MODIFY','DELETE'
    IdUsuario      INT          NOT NULL,
    NombreAnterior VARCHAR(100) NULL,
    NombreNuevo    VARCHAR(100) NULL
);
GO

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

-- ════════════════════════════════════════════════════════
-- PASO 5: Usuario admin inicial
-- Contraseña: admin (SHA-256 en minúsculas)
-- ════════════════════════════════════════════════════════

INSERT INTO Usuarios (Nombre, Contrasena, IdPerfil)
VALUES (
    'admin',
    '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',
    1  -- Administrador
);
GO

-- ════════════════════════════════════════════════════════
-- VERIFICACIÓN
-- ════════════════════════════════════════════════════════

SELECT 'Usuarios'        AS Tabla, COUNT(*) AS Registros FROM Usuarios        UNION ALL
SELECT 'Permiso',                  COUNT(*)               FROM Permiso         UNION ALL
SELECT 'PermisoHijo',              COUNT(*)               FROM PermisoHijo     UNION ALL
SELECT 'Perfil',                   COUNT(*)               FROM Perfil          UNION ALL
SELECT 'Idioma',                   COUNT(*)               FROM Idioma          UNION ALL
SELECT 'Bitacora',                 COUNT(*)               FROM Bitacora        UNION ALL
SELECT 'AuditoriaUsuario',         COUNT(*)               FROM AuditoriaUsuario UNION ALL
SELECT 'AuditoriaIdioma',          COUNT(*)               FROM AuditoriaIdioma;
GO