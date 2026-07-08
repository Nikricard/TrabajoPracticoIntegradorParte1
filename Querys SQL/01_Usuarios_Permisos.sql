--  01 — Usuarios y Permisos (estructura + datos base)
--  Crea las tablas de Usuarios, Permiso, PermisoHijo y UsuarioPermiso.
--  Inserta permisos atómicos (USR, IDM, TAG, BIT, PRF), compuestos del sistema
--  (GE010 Operador, GE020 Traductor, GE030 Auditor, GE040 Administrador) y el
--  usuario admin inicial con clave "admin".
--
--  Patrón Composite: la jerarquía de permisos vive en PermisoHijo,
--  que es una autorrelación de Permiso (CodigoPadre/CodigoHijo).
--
--  Incluye la columna DVH en Usuarios para el soporte de dígitos
--  verificadores (queda NULL hasta que el sistema los calcule por primera vez).

USE Usuarios;
GO

-- Estructura

-- Permisos (atómicos y compuestos en la misma tabla)
CREATE TABLE Permiso (
    Codigo      VARCHAR(10)  PRIMARY KEY,
    Nombre      VARCHAR(100) NOT NULL,
    EsCompuesto BIT          NOT NULL DEFAULT 0
);
GO

-- Relación padre-hijo: un compuesto contiene otros permisos atómicos o compuestos
CREATE TABLE PermisoHijo (
    CodigoPadre VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    CodigoHijo  VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    PRIMARY KEY (CodigoPadre, CodigoHijo)
);
GO

-- Usuarios (con columna DVH — dígito verificador horizontal por fila)
CREATE TABLE Usuarios (
    Id         INT          PRIMARY KEY IDENTITY(1,1),
    Nombre     VARCHAR(100) NOT NULL UNIQUE,
    Contrasena VARCHAR(200) NOT NULL,
    DVH        VARCHAR(64)  NULL   -- hash SHA-256 de Id + Nombre + Contrasena
);
GO

-- Permisos asignados a cada usuario (muchos-a-muchos)
CREATE TABLE UsuarioPermiso (
    IdUsuario INT         NOT NULL REFERENCES Usuarios(Id),
    Codigo    VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    PRIMARY KEY (IdUsuario, Codigo)
);
GO

-- Permisos atómicos (las funcionalidades base)
INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES
('USR001', 'Crear usuario',        0),
('USR002', 'Modificar usuario',    0),
('USR003', 'Eliminar usuario',     0),
('USR004', 'Listar usuarios',      0),
('IDM001', 'Agregar idioma',       0),
('IDM002', 'Eliminar idioma',      0),
('IDM003', 'Agregar traducciones', 0),
('TAG001', 'Agregar tags',         0),
('TAG002', 'Eliminar tags',        0),
('BIT001', 'Ver bitácora',         0),
('PRF001', 'Gestionar perfiles',   0);
GO

-- Permisos compuestos (perfiles del sistema)
INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES
('GE010', 'Operador',      1),
('GE020', 'Traductor',     1),
('GE030', 'Auditor',       1),
('GE040', 'Administrador', 1);
GO

-- Árbol de permisos
-- Operador → ABM de usuarios + listar
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE010','USR001'),('GE010','USR002'),('GE010','USR003'),('GE010','USR004');

-- Traductor → idiomas + traducciones + tags
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE020','IDM001'),('GE020','IDM002'),('GE020','IDM003'),
('GE020','TAG001'),('GE020','TAG002');

-- Auditor → acceso a bitácora
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE030','BIT001');

-- Administrador → Operador + Traductor + Auditor + gestionar perfiles
INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE040','GE010'),('GE040','GE020'),('GE040','GE030'),('GE040','PRF001');
GO

-- Usuario admin inicial
-- Usuario: admin   Contraseña: admin   (hash SHA-256)
INSERT INTO Usuarios (Nombre, Contrasena) VALUES
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918');
GO

-- admin recibe el permiso compuesto Administrador (GE040)
INSERT INTO UsuarioPermiso (IdUsuario, Codigo)
SELECT Id, 'GE040' FROM Usuarios WHERE Nombre = 'admin';
GO

PRINT '01 OK — Usuarios y permisos creados.';
GO
