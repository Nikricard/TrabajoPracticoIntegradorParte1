-- ════════════════════════════════════════════════════════
-- 02 — IDIOMAS, PALABRAS y TRADUCCIONES   (T05)
-- Patrón Observer para el cambio de idioma.
-- Independiente de los otros módulos. Ejecutar en 2º lugar.
-- ════════════════════════════════════════════════════════
USE Usuarios;
GO

-- ── Estructura ────────────────────────────────────────
CREATE TABLE Idioma (
    IdIdioma INT          PRIMARY KEY IDENTITY(1,1),
    Nombre   VARCHAR(50)  NOT NULL UNIQUE,
    defecto  BIT          NOT NULL DEFAULT 0
);
GO

CREATE TABLE Palabra (
    IdPalabra INT          PRIMARY KEY IDENTITY(1,1),
    Texto     VARCHAR(100) NOT NULL UNIQUE
);
GO

CREATE TABLE Traduccion (
    IdIdioma   INT          NOT NULL REFERENCES Idioma(IdIdioma),
    IdPalabra  INT          NOT NULL REFERENCES Palabra(IdPalabra),
    Traduccion VARCHAR(200) NOT NULL,
    PRIMARY KEY (IdIdioma, IdPalabra)
);
GO

-- ── Idiomas iniciales ─────────────────────────────────
INSERT INTO Idioma (Nombre, defecto) VALUES ('Español', 1);  -- IdIdioma = 1
INSERT INTO Idioma (Nombre, defecto) VALUES ('Inglés',  0);  -- IdIdioma = 2
GO

-- ── Palabras (claves = Tag de cada control) ───────────
-- El orden define el IdPalabra (1..N). Ejecutar de corrido.
INSERT INTO Palabra (Texto) VALUES
('btnIngresar'),     -- 1
('btnRegistrar'),    -- 2
('btnSalir'),        -- 3
('btnModificar'),    -- 4
('btnEliminar'),     -- 5
('btnBuscar'),       -- 6
('btnLimpiar'),      -- 7
('btnAsignar'),      -- 8
('btnCrear'),        -- 9
('lblNombre'),       -- 10
('lblContrasena'),   -- 11
('lblId'),           -- 12
('lblActividad'),    -- 13
('lblTipoDeEvento'), -- 14
('lblDesde'),        -- 15
('lblHasta'),        -- 16
('lblUsuario'),      -- 17
('lblPerfil'),       -- 18
('lblPermisos'),     -- 19
('lblConjuntos'),    -- 20
('lblPermisosDisp'), -- 21
('lblClave'),        -- 22
('lblValor'),        -- 23
('lblLogeado'),      -- 24
('menuGestionar'),   -- 25
('menuListar'),      -- 26
('menuIdioma'),      -- 27
('menuAgregar'),     -- 28
('menuBitacora'),    -- 29
('menuPerfiles'),    -- 30
('menuPermisos'),    -- 31
('menuCerrar');      -- 32
GO

-- ── Traducciones Español (IdIdioma = 1) ───────────────
INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion) VALUES
(1, 1,  'Ingresar'),
(1, 2,  'Registrar'),
(1, 3,  'Salir'),
(1, 4,  'Modificar'),
(1, 5,  'Eliminar'),
(1, 6,  'Buscar'),
(1, 7,  'Limpiar Filtros'),
(1, 8,  'Asignar'),
(1, 9,  'Crear'),
(1, 10, 'Nombre'),
(1, 11, 'Contraseña'),
(1, 12, 'ID'),
(1, 13, 'Actividad'),
(1, 14, 'Tipo de Evento'),
(1, 15, 'Desde'),
(1, 16, 'Hasta'),
(1, 17, 'Usuario'),
(1, 18, 'Perfil'),
(1, 19, 'Permisos'),
(1, 20, 'Conjuntos'),
(1, 21, 'Permisos disponibles'),
(1, 22, 'Clave'),
(1, 23, 'Valor'),
(1, 24, 'Usted se logeó como:'),
(1, 25, 'Gestionar Usuarios'),
(1, 26, 'Listar'),
(1, 27, 'Idioma'),
(1, 28, 'Agregar idioma...'),
(1, 29, 'Bitácora'),
(1, 30, 'Perfiles'),
(1, 31, 'Permisos'),
(1, 32, 'Cerrar Sesión');
GO

-- ── Traducciones Inglés (IdIdioma = 2) ────────────────
INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion) VALUES
(2, 1,  'Login'),
(2, 2,  'Register'),
(2, 3,  'Exit'),
(2, 4,  'Modify'),
(2, 5,  'Delete'),
(2, 6,  'Search'),
(2, 7,  'Clean Filters'),
(2, 8,  'Assign'),
(2, 9,  'Create'),
(2, 10, 'Name'),
(2, 11, 'Password'),
(2, 12, 'ID'),
(2, 13, 'Activity'),
(2, 14, 'Event Type'),
(2, 15, 'Since'),
(2, 16, 'To'),
(2, 17, 'User'),
(2, 18, 'Profile'),
(2, 19, 'Permissions'),
(2, 20, 'Sets'),
(2, 21, 'Available permissions'),
(2, 22, 'Key'),
(2, 23, 'Value'),
(2, 24, 'Logged in as:'),
(2, 25, 'User Management'),
(2, 26, 'List'),
(2, 27, 'Language'),
(2, 28, 'Add language...'),
(2, 29, 'Logbook'),
(2, 30, 'Profiles'),
(2, 31, 'Permissions'),
(2, 32, 'Logout');
GO

PRINT '02 OK — Idiomas, palabras y traducciones creados.';
GO
