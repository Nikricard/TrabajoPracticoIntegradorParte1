-- Tabla Idioma
CREATE TABLE Idioma (
    IdIdioma  INT PRIMARY KEY IDENTITY(1,1),
    Nombre    VARCHAR(50)  NOT NULL UNIQUE,
    defecto   BIT          NOT NULL DEFAULT 0
);

-- Tabla Palabra 
CREATE TABLE Palabra (
    IdPalabra INT PRIMARY KEY IDENTITY(1,1),
    Texto     VARCHAR(100) NOT NULL UNIQUE
);

-- Tabla Traduccion  (PK compuesta IdIdioma + IdPalabra)
CREATE TABLE Traduccion (
    IdIdioma   INT NOT NULL REFERENCES Idioma(IdIdioma),
    IdPalabra  INT NOT NULL REFERENCES Palabra(IdPalabra),
    Traduccion VARCHAR(200) NOT NULL,
    PRIMARY KEY (IdIdioma, IdPalabra)
);

--Datos iniciales
INSERT INTO Idioma   (Nombre,    defecto) VALUES ('Español', 1);
INSERT INTO Idioma   (Nombre,    defecto) VALUES ('Inglés',  0);

INSERT INTO Palabra  (Texto) VALUES ('btnIngresar');
INSERT INTO Palabra  (Texto) VALUES ('btnRegistrar');
INSERT INTO Palabra  (Texto) VALUES ('btnSalir');
INSERT INTO Palabra  (Texto) VALUES ('btnModificar');
INSERT INTO Palabra  (Texto) VALUES ('btnEliminar');
INSERT INTO Palabra  (Texto) VALUES ('lblNombre');
INSERT INTO Palabra  (Texto) VALUES ('lblContrasena');
INSERT INTO Palabra  (Texto) VALUES ('lblId');
INSERT INTO Palabra  (Texto) VALUES ('menuGestionar');
INSERT INTO Palabra  (Texto) VALUES ('menuListar');
INSERT INTO Palabra  (Texto) VALUES ('menuIdioma');
INSERT INTO Palabra  (Texto) VALUES ('menuSalir');
INSERT INTO Palabra  (Texto) VALUES ('menuAgregar');

-- Traducciones Español (IdIdioma=1)
INSERT INTO Traduccion VALUES (1, 1,  'Ingresar');
INSERT INTO Traduccion VALUES (1, 2,  'Registrar');
INSERT INTO Traduccion VALUES (1, 3,  'Salir');
INSERT INTO Traduccion VALUES (1, 4,  'Modificar');
INSERT INTO Traduccion VALUES (1, 5,  'Eliminar');
INSERT INTO Traduccion VALUES (1, 6,  'Nombre');
INSERT INTO Traduccion VALUES (1, 7,  'Contraseña');
INSERT INTO Traduccion VALUES (1, 8,  'ID');
INSERT INTO Traduccion VALUES (1, 9,  'Gestionar Usuarios');
INSERT INTO Traduccion VALUES (1, 10, 'Listar');
INSERT INTO Traduccion VALUES (1, 11, 'Idioma');
INSERT INTO Traduccion VALUES (1, 12, 'Salir');
INSERT INTO Traduccion VALUES (1, 13, 'Agregar idioma...');

-- Traducciones Inglés (IdIdioma=2)
INSERT INTO Traduccion VALUES (2, 1,  'Login');
INSERT INTO Traduccion VALUES (2, 2,  'Register');
INSERT INTO Traduccion VALUES (2, 3,  'Exit');
INSERT INTO Traduccion VALUES (2, 4,  'Modify');
INSERT INTO Traduccion VALUES (2, 5,  'Delete');
INSERT INTO Traduccion VALUES (2, 6,  'Name');
INSERT INTO Traduccion VALUES (2, 7,  'Password');
INSERT INTO Traduccion VALUES (2, 8,  'ID');
INSERT INTO Traduccion VALUES (2, 9,  'User Management');
INSERT INTO Traduccion VALUES (2, 10, 'List');
INSERT INTO Traduccion VALUES (2, 11, 'Language');
INSERT INTO Traduccion VALUES (2, 12, 'Exit');
INSERT INTO Traduccion VALUES (2, 13, 'Add language...');

select * from Idioma

select * from Traduccion
select * from Palabra order by Texto
