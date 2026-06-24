-- 05 — Permiso atómico ADM001 (Backup/Restore de la base)
-- Se asigna como hijo del compuesto Administrador (GE040) y se cargan
-- las tags de traducción de los nuevos ítems del menú.
-- Es idempotente: se puede ejecutar más de una vez sin duplicar.
-- Ejecutar después de 01_Usuarios_Permisos.sql.

USE Usuarios;
GO

-- 1) Permiso atómico ADM001
IF NOT EXISTS (SELECT 1 FROM Permiso WHERE Codigo = 'ADM001')
    INSERT INTO Permiso (Codigo, Nombre, EsCompuesto)
    VALUES ('ADM001', 'Backup y restore de base de datos', 0);
GO

-- 2) Asignarlo al perfil Administrador (GE040)
IF NOT EXISTS (
    SELECT 1 FROM PermisoHijo
    WHERE CodigoPadre = 'GE040' AND CodigoHijo = 'ADM001')
    INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo)
    VALUES ('GE040', 'ADM001');
GO

-- 3) Tags de traducción de los ítems del menú
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'menuBaseDatos')
    INSERT INTO Palabra (Texto) VALUES ('menuBaseDatos');
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'menuBackup')
    INSERT INTO Palabra (Texto) VALUES ('menuBackup');
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'menuRestore')
    INSERT INTO Palabra (Texto) VALUES ('menuRestore');
GO

-- 4) Traducciones ES/EN (limpia y carga; idempotente)
DECLARE @mbd INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuBaseDatos');
DECLARE @mbk INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuBackup');
DECLARE @mre INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuRestore');

DELETE FROM Traduccion
WHERE IdPalabra IN (@mbd, @mbk, @mre) AND IdIdioma IN (1, 2);

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion) VALUES
(1, @mbd, 'Base de datos'),
(2, @mbd, 'Database'),
(1, @mbk, 'Hacer backup...'),
(2, @mbk, 'Create backup...'),
(1, @mre, 'Restaurar backup...'),
(2, @mre, 'Restore backup...');
GO

-- 5) Verificación
SELECT 'Permiso ADM001' AS Item,
       CASE WHEN EXISTS (SELECT 1 FROM Permiso WHERE Codigo='ADM001')
            THEN 'OK' ELSE 'FALTA' END AS Estado
UNION ALL
SELECT 'GE040 -> ADM001',
       CASE WHEN EXISTS (
            SELECT 1 FROM PermisoHijo
            WHERE CodigoPadre='GE040' AND CodigoHijo='ADM001')
            THEN 'OK' ELSE 'FALTA' END
UNION ALL
SELECT 'Traducciones menú',
       CASE WHEN (SELECT COUNT(*) FROM Traduccion
                  WHERE IdPalabra IN (@mbd, @mbk, @mre)) = 6
            THEN 'OK (6)' ELSE 'FALTA' END;
GO

PRINT '05 OK — Permiso ADM001 creado y asignado al Administrador.';
GO
