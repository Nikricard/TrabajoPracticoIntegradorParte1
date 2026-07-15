-- 01 · Usuarios y Permisos — estructura, permisos del sistema y usuario admin.

USE Usuarios;
GO

CREATE TABLE Permiso (
    Codigo      VARCHAR(10)  PRIMARY KEY,
    Nombre      VARCHAR(100) NOT NULL,
    EsCompuesto BIT          NOT NULL DEFAULT 0
);

CREATE TABLE PermisoHijo (
    CodigoPadre VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    CodigoHijo  VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    PRIMARY KEY (CodigoPadre, CodigoHijo)
);

CREATE TABLE Usuarios (
    Id         INT          PRIMARY KEY IDENTITY(1,1),
    Nombre     VARCHAR(100) NOT NULL UNIQUE,
    Contrasena VARCHAR(200) NOT NULL,
    DVH        VARCHAR(64)  NULL
);

CREATE TABLE UsuarioPermiso (
    IdUsuario INT         NOT NULL REFERENCES Usuarios(Id),
    Codigo    VARCHAR(10) NOT NULL REFERENCES Permiso(Codigo),
    PRIMARY KEY (IdUsuario, Codigo)
);
GO

INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES
('USR001', 'Crear usuario',                          0),
('USR002', 'Modificar usuario',                      0),
('USR003', 'Eliminar usuario',                       0),
('USR004', 'Listar usuarios',                        0),
('IDM001', 'Agregar idioma',                         0),
('IDM002', 'Eliminar idioma',                        0),
('IDM003', 'Agregar traducciones',                   0),
('TAG001', 'Agregar tags',                           0),
('TAG002', 'Eliminar tags',                          0),
('BIT001', 'Ver bitácora',                           0),
('PRF001', 'Gestionar perfiles',                     0),
('ADM001', 'Backup y restore de base de datos',      0),
('ADM002', 'Restaurar a estado anterior de auditoría', 0),
('GE010',  'Operador',                               1),
('GE020',  'Traductor',                              1),
('GE030',  'Auditor',                                1),
('GE040',  'Administrador',                          1);
GO

INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES
('GE010','USR001'),('GE010','USR002'),('GE010','USR003'),('GE010','USR004'),
('GE020','IDM001'),('GE020','IDM002'),('GE020','IDM003'),('GE020','TAG001'),('GE020','TAG002'),
('GE030','BIT001'),
('GE040','GE010'),('GE040','GE020'),('GE040','GE030'),
('GE040','PRF001'),('GE040','ADM001'),('GE040','ADM002');
GO

-- admin / admin (hash SHA-256)
INSERT INTO Usuarios (Nombre, Contrasena) VALUES
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918');

INSERT INTO UsuarioPermiso (IdUsuario, Codigo)
SELECT Id, 'GE040' FROM Usuarios WHERE Nombre = 'admin';
GO
