-- 02 · Idiomas — estructura, idiomas ES/EN y todas las traducciones de la UI.

USE Usuarios;
GO

CREATE TABLE Idioma (
    IdIdioma INT         PRIMARY KEY IDENTITY(1,1),
    Nombre   VARCHAR(50) NOT NULL UNIQUE,
    defecto  BIT         NOT NULL DEFAULT 0
);

CREATE TABLE Palabra (
    IdPalabra INT          PRIMARY KEY IDENTITY(1,1),
    Texto     VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Traduccion (
    IdIdioma   INT          NOT NULL REFERENCES Idioma(IdIdioma),
    IdPalabra  INT          NOT NULL REFERENCES Palabra(IdPalabra),
    Traduccion VARCHAR(200) NOT NULL,
    PRIMARY KEY (IdIdioma, IdPalabra)
);
GO

INSERT INTO Idioma (Nombre, defecto) VALUES ('Español', 1), ('Inglés', 0);
GO

-- Fuente única: cada tag con su traducción ES e EN.
CREATE TABLE #T (Tag VARCHAR(100), Es VARCHAR(200), En VARCHAR(200));
INSERT INTO #T (Tag, Es, En) VALUES
('btnIngresar',       'Ingresar',                          'Login'),
('btnRegistrar',      'Registrar',                         'Register'),
('btnSalir',          'Salir',                             'Exit'),
('btnModificar',      'Modificar',                         'Modify'),
('btnEliminar',       'Eliminar',                          'Delete'),
('btnBuscar',         'Buscar',                            'Search'),
('btnLimpiar',        'Limpiar Filtros',                   'Clean Filters'),
('btnAsignar',        'Asignar',                           'Assign'),
('btnCrear',          'Crear',                             'Create'),
('btnActualizar',     'Actualizar',                        'Update'),
('btnRestaurar',      'Restaurar a este estado',           'Restore to this state'),
('lblNombre',         'Nombre',                            'Name'),
('lblContrasena',     'Contraseña',                        'Password'),
('lblId',             'ID',                                'ID'),
('lblActividad',      'Actividad',                         'Activity'),
('lblTipoDeEvento',   'Tipo de Evento',                    'Event Type'),
('lblDesde',          'Desde',                             'Since'),
('lblHasta',          'Hasta',                             'To'),
('lblUsuario',        'Usuario',                           'User'),
('lblPerfil',         'Perfil',                            'Profile'),
('lblPermisos',       'Permisos',                          'Permissions'),
('lblConjuntos',      'Conjuntos',                         'Sets'),
('lblConjuntosDisp',  'Conjuntos disponibles',             'Available sets'),
('lblPermisosDisp',   'Permisos disponibles',              'Available permissions'),
('lblAtomicos',       'Permisos atómicos',                 'Atomic permissions'),
('lblArbolConjunto',  'Detalle del conjunto',              'Set details'),
('lblGestionPermisos','Gestión de conjuntos de permisos',  'Permission sets management'),
('lblClave',          'Clave',                             'Key'),
('lblValor',          'Valor',                             'Value'),
('lblLogeado',        'Usted se logeó como:',              'Logged in as:'),
('menuGestionar',     'Gestionar Usuarios',                'User Management'),
('menuListar',        'Listar',                            'List'),
('menuIdioma',        'Idioma',                            'Language'),
('menuAgregar',       'Agregar idioma...',                 'Add language...'),
('menuBitacora',      'Bitácora',                          'Logbook'),
('menuPerfiles',      'Perfiles',                          'Profiles'),
('menuPermisos',      'Permisos',                          'Permissions'),
('menuCerrar',        'Cerrar Sesión',                     'Logout'),
('menuBaseDatos',     'Base de datos',                     'Database'),
('menuBackup',        'Hacer backup...',                   'Create backup...'),
('menuRestore',       'Restaurar backup...',               'Restore backup...'),
('menuRecalcularDV',  'Recalcular dígitos verificadores',  'Recalculate check digits');

INSERT INTO Palabra (Texto) SELECT Tag FROM #T;

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
SELECT 1, P.IdPalabra, T.Es FROM Palabra P JOIN #T T ON P.Texto = T.Tag;

INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
SELECT 2, P.IdPalabra, T.En FROM Palabra P JOIN #T T ON P.Texto = T.Tag;

DROP TABLE #T;
GO
