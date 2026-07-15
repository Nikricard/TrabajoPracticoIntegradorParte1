-- 04 · Dígito Verificador — tabla del DVV por tabla protegida (el DVH es columna de Usuarios).
-- Inicializar con "Recalcular dígitos verificadores" en el primer login del admin.

USE Usuarios;
GO

CREATE TABLE DigitoVerificadorTabla (
    NombreTabla  VARCHAR(50) PRIMARY KEY,
    DVV          VARCHAR(64) NOT NULL,
    FechaCalculo DATETIME    NOT NULL DEFAULT GETDATE()
);
GO
