--  07 — Dígitos verificadores
--  Crea la tabla DigitoVerificadorTabla, que almacena el DVV (dígito
--  verificador vertical) de cada tabla protegida.
--  DVV = hash SHA-256 de la concatenación de todos los DVH de la tabla,
--  ordenados por Id. 
--  Detecta altas, bajas y reordenamientos externos.
--
--  Después de correr este script:
--   - La primera vez que arranque el sistema, VerificarIntegridad() pasará
--     porque no hay DVV guardado (se considera "primera ejecución").
--   - El admin debe hacer login normal y elegir "Recalcular" cuando el
--     sistema se lo pregunte, para inicializar los DVH y el DVV.
--   - Desde ese momento, cualquier UPDATE/INSERT/DELETE por afuera del
--     sistema será detectado al próximo inicio.

--  ORDEN DE EJECUCIÓN: 7° 

USE Usuarios;
GO

-- Tabla DigitoVerificadorTabla (DVV por tabla)
CREATE TABLE DigitoVerificadorTabla (
    NombreTabla  VARCHAR(50)  PRIMARY KEY,
    DVV          VARCHAR(64)  NOT NULL,
    FechaCalculo DATETIME     NOT NULL DEFAULT GETDATE()
);
GO

PRINT '07 OK — Tabla DigitoVerificadorTabla creada.';
PRINT 'IMPORTANTE: Iniciar sesión como admin y elegir "Recalcular" la primera vez.';
GO
