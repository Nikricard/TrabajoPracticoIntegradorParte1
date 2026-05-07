Create table Usuarios (Id int Primary Key Identity (1,1), Nombre nvarchar(50) unique, Contrasena VARCHAR(64) NOT NULL DEFAULT '' );
select * from Usuarios
delete * from Usuarios
