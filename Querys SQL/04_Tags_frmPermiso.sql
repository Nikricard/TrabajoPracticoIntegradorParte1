--  04 — Tags de frmPermiso
--  Es IDEMPOTENTE: se puede ejecutar varias veces sin duplicar.
--  ORDEN DE EJECUCIÓN: 4°

USE Usuarios;
GO

-- 1) Crear las claves si no existen
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'lblAtomicos')
    INSERT INTO Palabra (Texto) VALUES ('lblAtomicos');
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'lblConjuntosDisp')
    INSERT INTO Palabra (Texto) VALUES ('lblConjuntosDisp');
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'lblArbolConjunto')
    INSERT INTO Palabra (Texto) VALUES ('lblArbolConjunto');
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'btnActualizar')
    INSERT INTO Palabra (Texto) VALUES ('btnActualizar');
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto = 'lblGestionPermisos')
    INSERT INTO Palabra (Texto) VALUES ('lblGestionPermisos');
GO

-- 2) Traducciones ES/EN (limpia y carga; idempotente)
DECLARE @atom INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'lblAtomicos');
DECLARE @disp INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'lblConjuntosDisp');
DECLARE @arb  INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'lblArbolConjunto');
DECLARE @act  INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'btnActualizar');
DECLARE @ges  INT = (SELECT IdPalabra FROM Palabra WHERE Texto = 'lblGestionPermisos');

DELETE FROM Traduccion
WHERE IdPalabra IN (@atom, @disp, @arb, @act, @ges) AND IdIdioma IN (1, 2);

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion) VALUES
(1, @atom, 'Permisos atómicos'),
(2, @atom, 'Atomic permissions'),
(1, @disp, 'Conjuntos disponibles'),
(2, @disp, 'Available sets'),
(1, @arb,  'Detalle del conjunto'),
(2, @arb,  'Set details'),
(1, @act,  'Actualizar'),
(2, @act,  'Update'),
(1, @ges,  'Gestión de conjuntos de permisos'),
(2, @ges,  'Permission sets management');
GO

-- 3) Verificación
DECLARE @n INT = (
    SELECT COUNT(*) FROM Traduccion t
    JOIN Palabra p ON p.IdPalabra = t.IdPalabra
    WHERE p.Texto IN ('lblAtomicos','lblConjuntosDisp','lblArbolConjunto',
                      'btnActualizar','lblGestionPermisos'));
SELECT 'Traducciones frmPermiso' AS Item,
       CASE WHEN @n = 10 THEN 'OK (10)' ELSE CONCAT('FALTAN (', 10 - @n, ')') END AS Estado;
GO

PRINT '04 OK — Tags de frmPermiso cargadas.';
GO
