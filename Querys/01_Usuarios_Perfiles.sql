-- ════════════════════════════════════════════════════════
-- 01 — USUARIOS, PERMISOS y PERFILES   (T01 + T04)
-- Patrón Composite para permisos.
-- Ejecutar PRIMERO (las demás tablas no dependen de esta,
-- pero Usuarios necesita Perfil que se crea acá).
-- ════════════════════════════════════════════════════════
USE Usuarios;
GO

-- ── Estructura ────────────────────────────────────────

-- Permisos (atómicos y compuestos en la misma tabla)
CREATE TABLE Permiso (
    Codigo      VARCHAR(10)  PRIMARY KEY,
    Nombre      VARCHAR(100) NOT NULL,
    EsCompuesto BIT          NOT NULL DEFAULT 0
);
GO

-- Relación padre-hijo (muchos-a-muchos: un permiso puede
-- estar bajo varios padres). Es el corazón del Composite.
CREATE TABLE PermisoHijo (
    CodigoPadre VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    CodigoHijo  VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    PRIMARY KEY (CodigoPadre, CodigoHijo)
);
GO

-- Un perfil ES un permiso (normalmente compuesto) asignable
CREATE TABLE Perfil (
    IdPerfil INT         PRIMARY KEY IDENTITY(1,1),
    Nombre   VARCHAR(50) NOT NULL UNIQUE,
    Codigo   VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo)
);
GO

-- Usuarios con FK al perfil
CREATE TABLE Usuarios (
    Id         INT          PRIMARY KEY IDENTITY(1,1),
    Nombre     VARCHAR(100) NOT NULL UNIQUE,
    Contrasena VARCHAR(200) NOT NULL,
    IdPerfil   INT          NULL REFERENCES Perfil(IdPerfil)
);
GO

-- ── Permisos atómicos (las funcionalidades base) ──────
INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES
('USR001', 'Crear usuario',          0),
('USR002', 'Modificar usuario',      0),
('USR003', 'Eliminar usuario',       0),
('USR004', 'Listar usuarios',        0),
('IDM001', 'Agregar idioma',         0),
('IDM002', 'Eliminar idioma',        0),
('IDM003', 'Agregar traducciones',   0),
('TAG001', 'Agregar tags',           0),
('TAG002', 'Eliminar tags',          0),
('BIT001', 'Ver bitácora',           0),
('PRF001', 'Gestionar perfiles',     0);
GO

-- ── Permisos compuestos (categorías) ──────────────────
INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES
('GE010', 'Gestión de usuarios', 1),
('GE020', 'Gestión de idiomas',  1),
('GE040', 'Operador',            1),
('GE030', 'Administrador',       1);
GO

-- ── Árbol de permisos ─────────────────────────────────
-- GE010 → usuarios
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE010','USR001'),('GE010','USR002'),('GE010','USR003'),('GE010','USR004');

-- GE020 → idiomas + tags + traducciones
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE020','IDM001'),('GE020','IDM002'),('GE020','IDM003'),
('GE020','TAG001'),('GE020','TAG002');

-- GE040 (Operador) → usuarios + idiomas
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE040','GE010'),('GE040','GE020');

-- GE030 (Administrador) → todo
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE030','GE010'),('GE030','GE020'),('GE030','BIT001'),('GE030','PRF001');
GO

-- ── Perfiles fijos del sistema ────────────────────────
INSERT INTO Perfil (Nombre, Codigo) VALUES
('Administrador', 'GE030'),   -- IdPerfil = 1
('Operador',      'GE040'),   -- IdPerfil = 2
('Usuario',       'USR004');  -- IdPerfil = 3
GO

-- ── Usuario admin inicial ─────────────────────────────
-- Usuario: admin   Contraseña: admin   (SHA-256, minúsculas)
INSERT INTO Usuarios (Nombre, Contrasena, IdPerfil) VALUES
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 1);
GO

PRINT '01 OK — Usuarios, permisos y perfiles creados.';
GO
