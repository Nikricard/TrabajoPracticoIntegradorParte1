-- ============================================================================
--  08 — Control de cambios: snapshot JSON y permiso de rollback
-- ============================================================================
--  Agrega:
--   1) Columna SnapshotJson en AuditoriaUsuario.
--      Guarda el estado ANTERIOR al cambio (Nombre + Permisos, en JSON)
--      para poder hacer rollback selectivo desde el dgv de auditoría.
--   2) Permiso atómico ADM002 (Restaurar a estado anterior de auditoría),
--      asignado al perfil GE040 Administrador.
--   3) Tags de traducción: btnRestaurar (botón nuevo en frmBitacora).
--
--  Sobre la contraseña: el snapshot NO la guarda. Por decisión de diseño,
--  la contraseña de un usuario es inalterable a lo largo del sistema y
--  el rollback no la restaura.
--
--  Es IDEMPOTENTE.
--
--  ORDEN DE EJECUCIÓN: 8° (último, después de 07).
-- ============================================================================

USE Usuarios;
GO

-- 1) Columna SnapshotJson en AuditoriaUsuario
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'AuditoriaUsuario' AND COLUMN_NAME = 'SnapshotJson')
BEGIN
    ALTER TABLE AuditoriaUsuario ADD SnapshotJson NVARCHAR(MAX) NULL;
    PRINT 'Columna SnapshotJson agregada a AuditoriaUsuario.';
END
ELSE
    PRINT 'Columna SnapshotJson ya existía.';
GO

-- 2) Permiso atómico ADM002
IF NOT EXISTS (SELECT 1 FROM Permiso WHERE Codigo = 'ADM002')
    INSERT INTO Permiso (Codigo, Nombre, EsCompuesto)
    VALUES ('ADM002', 'Restaurar a estado anterior de auditoría', 0);
GO

-- 3) Asignar ADM002 al perfil Administrador (GE040)
IF NOT EXISTS (
    SELECT 1 FROM PermisoHijo
    WHERE CodigoPadre = 'GE040' AND CodigoHijo = 'ADM002')
    INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo)
    VALUES ('GE040', 'ADM002');
GO

-- 4) Tag y traducción para el nuevo botón de frmBitacora
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'btnRestaurar')
    INSERT INTO Palabra (Texto) VALUES ('btnRestaurar');
GO

DECLARE @res INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'btnRestaurar');

DELETE FROM Traduccion
WHERE IdPalabra = @res AND IdIdioma IN (1, 2);

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion) VALUES
(1, @res, 'Restaurar a este estado'),
(2, @res, 'Restore to this state');
GO

-- 5) Verificación
SELECT 'Columna SnapshotJson' AS Item,
       CASE WHEN EXISTS (
            SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME='AuditoriaUsuario' AND COLUMN_NAME='SnapshotJson')
            THEN 'OK' ELSE 'FALTA' END AS Estado
UNION ALL
SELECT 'Permiso ADM002',
       CASE WHEN EXISTS (SELECT 1 FROM Permiso WHERE Codigo='ADM002')
            THEN 'OK' ELSE 'FALTA' END
UNION ALL
SELECT 'GE040 -> ADM002',
       CASE WHEN EXISTS (
            SELECT 1 FROM PermisoHijo
            WHERE CodigoPadre='GE040' AND CodigoHijo='ADM002')
            THEN 'OK' ELSE 'FALTA' END
UNION ALL
SELECT 'Traducción btnRestaurar',
       CASE WHEN (SELECT COUNT(*) FROM Traduccion t
                  JOIN Palabra p ON p.IdPalabra=t.IdPalabra
                  WHERE p.Texto='btnRestaurar') = 2
            THEN 'OK (2)' ELSE 'FALTA' END;
GO

PRINT '08 OK — Control de cambios con snapshot JSON y permiso ADM002.';
GO
