--  00 — Limpieza completa de la base
--  Borra todas las tablas en orden inverso al de las claves foráneas.
--  Sirve para recrear toda la estructura desde cero.
--
--  ORDEN DE EJECUCIÓN: PRIMERO (antes de 01).
--  USAR SOLO si querés rehacer la base.

USE Usuarios;
GO

IF OBJECT_ID('DigitoVerificadorTabla', 'U') IS NOT NULL DROP TABLE DigitoVerificadorTabla;
IF OBJECT_ID('AuditoriaIdioma',        'U') IS NOT NULL DROP TABLE AuditoriaIdioma;
IF OBJECT_ID('AuditoriaUsuario',       'U') IS NOT NULL DROP TABLE AuditoriaUsuario;
IF OBJECT_ID('Bitacora',               'U') IS NOT NULL DROP TABLE Bitacora;
IF OBJECT_ID('Traduccion',             'U') IS NOT NULL DROP TABLE Traduccion;
IF OBJECT_ID('Palabra',                'U') IS NOT NULL DROP TABLE Palabra;
IF OBJECT_ID('Idioma',                 'U') IS NOT NULL DROP TABLE Idioma;
IF OBJECT_ID('UsuarioPermiso',         'U') IS NOT NULL DROP TABLE UsuarioPermiso;
IF OBJECT_ID('Usuarios',               'U') IS NOT NULL DROP TABLE Usuarios;
IF OBJECT_ID('PermisoHijo',            'U') IS NOT NULL DROP TABLE PermisoHijo;
IF OBJECT_ID('Permiso',                'U') IS NOT NULL DROP TABLE Permiso;
GO

PRINT '00 OK — Base limpiada. Ejecutar ahora 01, 02, 03, 05 y 07 en orden.';
GO
