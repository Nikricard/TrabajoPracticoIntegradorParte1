--  05 — Permiso de Backup (ADM001) y tags del menú "Base de datos"
--  Agrega:
--   1) El permiso atómico ADM001 ("Backup y restore de base de datos").
--   2) El vínculo GE040 → ADM001 (Administrador hereda backup por defecto).
--   3) Las cuatro tags de traducción del menú: menuBaseDatos, menuBackup,
--      menuRestore, menuRecalcularDV, con sus traducciones ES/EN.
--
--  Decisión: ADM001 es atómico para que pueda asignarse opcionalmente a
--  cualquier otro conjunto de permisos que se cree en el futuro, no solo al
--  Administrador. Cubre backup, restore y recálculo de dígitos verificadores
--  (operaciones de gestión de base de datos).

--  ORDEN DE EJECUCIÓN: 5° 


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
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'menuRecalcularDV')
    INSERT INTO Palabra (Texto) VALUES ('menuRecalcularDV');
GO

-- 4) Traducciones ES/EN (limpia y carga; idempotente)
DECLARE @mbd INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuBaseDatos');
DECLARE @mbk INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuBackup');
DECLARE @mre INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuRestore');
DECLARE @rdv INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuRecalcularDV');

DELETE FROM Traduccion
WHERE IdPalabra IN (@mbd, @mbk, @mre, @rdv) AND IdIdioma IN (1, 2);

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion) VALUES
(1, @mbd, 'Base de datos'),
(2, @mbd, 'Database'),
(1, @mbk, 'Hacer backup...'),
(2, @mbk, 'Create backup...'),
(1, @mre, 'Restaurar backup...'),
(2, @mre, 'Restore backup...'),
(1, @rdv, 'Recalcular dígitos verificadores'),
(2, @rdv, 'Recalculate check digits');
GO

-- 5) Verificación
DECLARE @mbd2 INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuBaseDatos');
DECLARE @mbk2 INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuBackup');
DECLARE @mre2 INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuRestore');
DECLARE @rdv2 INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'menuRecalcularDV');

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
                  WHERE IdPalabra IN (@mbd2, @mbk2, @mre2, @rdv2)) = 8
            THEN 'OK (8)' ELSE 'FALTA' END;
GO

PRINT '05 OK — Permiso ADM001 creado y tags del menú cargadas.';
GO
