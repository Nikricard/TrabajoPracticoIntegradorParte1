-- ════════════════════════════════════════════════════════
-- ABM de Permisos — Agregar permisos de tags + soporte form
-- Ejecutar sobre la base de datos: Usuarios
-- Seguro de re-ejecutar (usa IF NOT EXISTS)
-- ════════════════════════════════════════════════════════

-- ── Permisos atómicos nuevos para tags ────────────────
IF NOT EXISTS (SELECT 1 FROM Permiso WHERE Codigo = 'TAG001')
    INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES ('TAG001', 'Agregar tags', 0);

IF NOT EXISTS (SELECT 1 FROM Permiso WHERE Codigo = 'TAG002')
    INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES ('TAG002', 'Eliminar tags', 0);

-- Renombrar IDM003 para que coincida con "Agregar traducciones"
UPDATE Permiso SET Nombre = 'Agregar traducciones' WHERE Codigo = 'IDM003';

-- ── Asignar tags al compuesto de idiomas (GE020) ──────
-- Así Operador y Admin (que incluyen GE020) heredan los permisos de tags
IF NOT EXISTS (SELECT 1 FROM PermisoHijo WHERE CodigoPadre='GE020' AND CodigoHijo='TAG001')
    INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES ('GE020','TAG001');

IF NOT EXISTS (SELECT 1 FROM PermisoHijo WHERE CodigoPadre='GE020' AND CodigoHijo='TAG002')
    INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES ('GE020','TAG002');

-- ── Palabras (tags) para el formulario frmPermiso ─────
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto='btnCrear')
    INSERT INTO Palabra (Texto) VALUES ('btnCrear');
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto='lblConjuntos')
    INSERT INTO Palabra (Texto) VALUES ('lblConjuntos');
IF NOT EXISTS (SELECT 1 FROM Palabra WHERE Texto='lblPermisosDisp')
    INSERT INTO Palabra (Texto) VALUES ('lblPermisosDisp');

-- Traducciones de esas palabras (a todos los idiomas con valor por idioma)
-- Español
INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
SELECT 1, p.IdPalabra, 'Crear' FROM Palabra p WHERE p.Texto='btnCrear'
AND NOT EXISTS (SELECT 1 FROM Traduccion t WHERE t.IdIdioma=1 AND t.IdPalabra=p.IdPalabra);

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
SELECT 1, p.IdPalabra, 'Conjuntos' FROM Palabra p WHERE p.Texto='lblConjuntos'
AND NOT EXISTS (SELECT 1 FROM Traduccion t WHERE t.IdIdioma=1 AND t.IdPalabra=p.IdPalabra);

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
SELECT 1, p.IdPalabra, 'Permisos disponibles' FROM Palabra p WHERE p.Texto='lblPermisosDisp'
AND NOT EXISTS (SELECT 1 FROM Traduccion t WHERE t.IdIdioma=1 AND t.IdPalabra=p.IdPalabra);

-- Inglés (IdIdioma=2 si existe)
INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
SELECT 2, p.IdPalabra, 'Create' FROM Palabra p WHERE p.Texto='btnCrear'
AND EXISTS (SELECT 1 FROM Idioma WHERE IdIdioma=2)
AND NOT EXISTS (SELECT 1 FROM Traduccion t WHERE t.IdIdioma=2 AND t.IdPalabra=p.IdPalabra);

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
SELECT 2, p.IdPalabra, 'Sets' FROM Palabra p WHERE p.Texto='lblConjuntos'
AND EXISTS (SELECT 1 FROM Idioma WHERE IdIdioma=2)
AND NOT EXISTS (SELECT 1 FROM Traduccion t WHERE t.IdIdioma=2 AND t.IdPalabra=p.IdPalabra);

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
SELECT 2, p.IdPalabra, 'Available permissions' FROM Palabra p WHERE p.Texto='lblPermisosDisp'
AND EXISTS (SELECT 1 FROM Idioma WHERE IdIdioma=2)
AND NOT EXISTS (SELECT 1 FROM Traduccion t WHERE t.IdIdioma=2 AND t.IdPalabra=p.IdPalabra);

-- ── Verificación ──────────────────────────────────────
SELECT Codigo, Nombre, EsCompuesto FROM Permiso WHERE EsCompuesto = 0 ORDER BY Codigo;
