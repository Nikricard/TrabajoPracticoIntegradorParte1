-- ════════════════════════════════════════════════════════
-- Idiomas y Traducciones
-- ════════════════════════════════════════════════════════

-- Tabla Idioma
CREATE TABLE Idioma (
    IdIdioma  INT          PRIMARY KEY IDENTITY(1,1),
    Nombre    VARCHAR(50)  NOT NULL UNIQUE,
    defecto   BIT          NOT NULL DEFAULT 0
);

-- Tabla Palabra
CREATE TABLE Palabra (
    IdPalabra INT          PRIMARY KEY IDENTITY(1,1),
    Texto     VARCHAR(100) NOT NULL UNIQUE
);

-- Tabla Traduccion (PK compuesta IdIdioma + IdPalabra)
CREATE TABLE Traduccion (
    IdIdioma   INT          NOT NULL REFERENCES Idioma(IdIdioma),
    IdPalabra  INT          NOT NULL REFERENCES Palabra(IdPalabra),
    Traduccion VARCHAR(200) NOT NULL,
    PRIMARY KEY (IdIdioma, IdPalabra)
);

-- ── Idiomas iniciales ─────────────────────────────────
INSERT INTO Idioma (Nombre, defecto) VALUES ('Español', 1);  -- IdIdioma = 1
INSERT INTO Idioma (Nombre, defecto) VALUES ('Inglés',  0);  -- IdIdioma = 2

-- ── Palabras (en orden, los IdPalabra se asignan 1..N) ──
INSERT INTO Palabra (Texto) VALUES ('btnIngresar');     -- 1
INSERT INTO Palabra (Texto) VALUES ('btnRegistrar');    -- 2
INSERT INTO Palabra (Texto) VALUES ('btnSalir');        -- 3
INSERT INTO Palabra (Texto) VALUES ('btnModificar');    -- 4
INSERT INTO Palabra (Texto) VALUES ('btnEliminar');     -- 5
INSERT INTO Palabra (Texto) VALUES ('btnBuscar');       -- 6
INSERT INTO Palabra (Texto) VALUES ('btnLimpiar');      -- 7
INSERT INTO Palabra (Texto) VALUES ('lblNombre');       -- 8
INSERT INTO Palabra (Texto) VALUES ('lblContrasena');   -- 9
INSERT INTO Palabra (Texto) VALUES ('lblId');           -- 10
INSERT INTO Palabra (Texto) VALUES ('lblActividad');    -- 11
INSERT INTO Palabra (Texto) VALUES ('lblTipoDeEvento'); -- 12
INSERT INTO Palabra (Texto) VALUES ('lblDesde');        -- 13
INSERT INTO Palabra (Texto) VALUES ('lblHasta');        -- 14
INSERT INTO Palabra (Texto) VALUES ('lblUsuario');      -- 15
INSERT INTO Palabra (Texto) VALUES ('menuGestionar');   -- 16
INSERT INTO Palabra (Texto) VALUES ('menuListar');      -- 17
INSERT INTO Palabra (Texto) VALUES ('menuIdioma');      -- 18
INSERT INTO Palabra (Texto) VALUES ('menuSalir');       -- 19
INSERT INTO Palabra (Texto) VALUES ('menuAgregar');     -- 20
INSERT INTO Palabra (Texto) VALUES ('menuBitacora');    -- 21
INSERT INTO Palabra (Texto) VALUES ('lblLogeado');    -- 22
INSERT INTO Palabra (Texto) VALUES ('menuPerfiles');    -- 23
INSERT INTO Palabra (Texto) VALUES ('lblPerfil');    -- 24
INSERT INTO Palabra (Texto) VALUES ('lblPermisos');    -- 25
INSERT INTO Palabra (Texto) VALUES ('btnAsignar');    -- 26

-- ── Traducciones Español (IdIdioma = 1) ──────────────
INSERT INTO Traduccion VALUES (1,  1,  'Ingresar');
INSERT INTO Traduccion VALUES (1,  2,  'Registrar');
INSERT INTO Traduccion VALUES (1,  3,  'Salir');
INSERT INTO Traduccion VALUES (1,  4,  'Modificar');
INSERT INTO Traduccion VALUES (1,  5,  'Eliminar');
INSERT INTO Traduccion VALUES (1,  6,  'Buscar');
INSERT INTO Traduccion VALUES (1,  7,  'Limpiar Filtros');
INSERT INTO Traduccion VALUES (1,  8,  'Nombre');
INSERT INTO Traduccion VALUES (1,  9,  'Contraseña');
INSERT INTO Traduccion VALUES (1,  10, 'ID');
INSERT INTO Traduccion VALUES (1,  11, 'Actividad');
INSERT INTO Traduccion VALUES (1,  12, 'Tipo de Evento');
INSERT INTO Traduccion VALUES (1,  13, 'Desde');
INSERT INTO Traduccion VALUES (1,  14, 'Hasta');
INSERT INTO Traduccion VALUES (1,  15, 'Usuario');
INSERT INTO Traduccion VALUES (1,  16, 'Gestionar Usuarios');
INSERT INTO Traduccion VALUES (1,  17, 'Listar');
INSERT INTO Traduccion VALUES (1,  18, 'Idioma');
INSERT INTO Traduccion VALUES (1,  19, 'Salir');
INSERT INTO Traduccion VALUES (1,  20, 'Agregar idioma...');
INSERT INTO Traduccion VALUES (1,  21, 'Bitácora');
INSERT INTO Traduccion VALUES (1,  23, 'Perfiles');
INSERT INTO Traduccion VALUES (1,  24, 'Perfil');
INSERT INTO Traduccion VALUES (1,  25, 'Permisos');
INSERT INTO Traduccion VALUES (1,  26, 'Asignar');

-- ── Traducciones Inglés (IdIdioma = 2) ───────────────
INSERT INTO Traduccion VALUES (2,  1,  'Login');
INSERT INTO Traduccion VALUES (2,  2,  'Register');
INSERT INTO Traduccion VALUES (2,  3,  'Exit');
INSERT INTO Traduccion VALUES (2,  4,  'Modify');
INSERT INTO Traduccion VALUES (2,  5,  'Delete');
INSERT INTO Traduccion VALUES (2,  6,  'Search');
INSERT INTO Traduccion VALUES (2,  7,  'Clean Filters');
INSERT INTO Traduccion VALUES (2,  8,  'Name');
INSERT INTO Traduccion VALUES (2,  9,  'Password');
INSERT INTO Traduccion VALUES (2,  10, 'ID');
INSERT INTO Traduccion VALUES (2,  11, 'Activity');
INSERT INTO Traduccion VALUES (2,  12, 'Event Type');
INSERT INTO Traduccion VALUES (2,  13, 'Since');
INSERT INTO Traduccion VALUES (2,  14, 'To');
INSERT INTO Traduccion VALUES (2,  15, 'User');
INSERT INTO Traduccion VALUES (2,  16, 'User Management');
INSERT INTO Traduccion VALUES (2,  17, 'List');
INSERT INTO Traduccion VALUES (2,  18, 'Language');
INSERT INTO Traduccion VALUES (2,  19, 'Exit');
INSERT INTO Traduccion VALUES (2,  20, 'Add language...');
INSERT INTO Traduccion VALUES (2,  21, 'Logbook');
INSERT INTO Traduccion VALUES (2,  23, 'Profiles');
INSERT INTO Traduccion VALUES (2,  24, 'Profile');
INSERT INTO Traduccion VALUES (2,  25, 'Permissions');
INSERT INTO Traduccion VALUES (2,  26, 'Assign');

-- ── Verificación ──────────────────────────────────────
SELECT * FROM Idioma;
SELECT p.IdPalabra, p.Texto,
       t1.Traduccion AS Español,
       t2.Traduccion AS Inglés
FROM Palabra p
LEFT JOIN Traduccion t1 ON t1.IdPalabra = p.IdPalabra AND t1.IdIdioma = 1
LEFT JOIN Traduccion t2 ON t2.IdPalabra = p.IdPalabra AND t2.IdIdioma = 2
ORDER BY p.IdPalabra;

delete from Traduccion where IdPalabra = 22 and IdIdioma = 2
delete from Traduccion where IdPalabra = 24
delete from Traduccion where IdPalabra = 25