-- ============================================================================
--  09 — Ampliación de columnas Operacion en auditorías
-- ============================================================================
--  Fix: la columna Operacion en AuditoriaIdioma y AuditoriaUsuario estaba
--  definida como VARCHAR(20), pero los nombres legibles introducidos en la
--  v2.7 exceden ese tamaño (por ejemplo "Modificación de traducción" = 25
--  caracteres, o "Asignación de perfil" también supera el límite en algún
--  caso). Al intentar registrar la operación SQL Server devuelve el error:
--
--    String or binary data would be truncated in table
--    'Usuarios.dbo.AuditoriaIdioma', column 'Operacion'.
--
--  Se amplían ambas columnas a VARCHAR(50).
--
--  Es IDEMPOTENTE: SQL Server permite ampliar una columna VARCHAR sin borrar
--  datos existentes.
--
--  ORDEN DE EJECUCIÓN: 9° (después de 08).
-- ============================================================================

USE Usuarios;
GO

ALTER TABLE AuditoriaIdioma
    ALTER COLUMN Operacion VARCHAR(50) NOT NULL;
GO

ALTER TABLE AuditoriaUsuario
    ALTER COLUMN Operacion VARCHAR(50) NOT NULL;
GO

-- Verificación
SELECT 'AuditoriaIdioma.Operacion' AS Columna,
       CHARACTER_MAXIMUM_LENGTH AS Tamanio
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'AuditoriaIdioma' AND COLUMN_NAME = 'Operacion'
UNION ALL
SELECT 'AuditoriaUsuario.Operacion',
       CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'AuditoriaUsuario' AND COLUMN_NAME = 'Operacion';
GO

PRINT '09 OK — Columnas Operacion ampliadas a VARCHAR(50).';
GO
