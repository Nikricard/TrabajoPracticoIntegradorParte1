--  06 — Normalización de nombres de actividad (OPCIONAL)
--  Reemplaza los códigos en mayúsculas que estaban en uso por  nuevos nombres que ahora genera BitacoraBLL,
--  tanto en la tabla Bitacora como en las dos tablas de auditoría.

--  ORDEN DE EJECUCIÓN: 6° (opcional)

USE Usuarios;
GO

-- Bitacora.Actividad
UPDATE Bitacora SET Actividad = 'Inicio de sesión'             WHERE Actividad = 'LOGIN';
UPDATE Bitacora SET Actividad = 'Cierre de sesión'             WHERE Actividad = 'LOGOUT';
UPDATE Bitacora SET Actividad = 'Alta de usuario'              WHERE Actividad = 'ADD_USUARIO';
UPDATE Bitacora SET Actividad = 'Modificación de usuario'      WHERE Actividad = 'MODIFY_USUARIO';
UPDATE Bitacora SET Actividad = 'Baja de usuario'              WHERE Actividad = 'DELETE_USUARIO';
UPDATE Bitacora SET Actividad = 'Alta de idioma'               WHERE Actividad = 'ADD_IDIOMA';
UPDATE Bitacora SET Actividad = 'Baja de idioma'               WHERE Actividad = 'DELETE_IDIOMA';
UPDATE Bitacora SET Actividad = 'Modificación de traducción'   WHERE Actividad = 'TRADUCCION';
UPDATE Bitacora SET Actividad = 'Asignación de perfil'         WHERE Actividad = 'ASIGNAR_PERFIL';
UPDATE Bitacora SET Actividad = 'Alta de conjunto'             WHERE Actividad = 'CREAR_CONJUNTO';
UPDATE Bitacora SET Actividad = 'Backup de base de datos'      WHERE Actividad = 'BACKUP_DB';
UPDATE Bitacora SET Actividad = 'Restore de base de datos'     WHERE Actividad = 'RESTORE_DB';
GO

-- AuditoriaUsuario.Operacion
UPDATE AuditoriaUsuario SET Operacion = 'Alta'                  WHERE Operacion = 'ADD';
UPDATE AuditoriaUsuario SET Operacion = 'Modificación'          WHERE Operacion = 'MODIFY';
UPDATE AuditoriaUsuario SET Operacion = 'Baja'                  WHERE Operacion = 'DELETE';
UPDATE AuditoriaUsuario SET Operacion = 'Asignación de perfil'  WHERE Operacion = 'ASIGNAR_PERFIL';
GO

-- AuditoriaIdioma.Operacion
UPDATE AuditoriaIdioma SET Operacion = 'Alta'                       WHERE Operacion = 'ADD';
UPDATE AuditoriaIdioma SET Operacion = 'Baja'                       WHERE Operacion = 'DELETE';
UPDATE AuditoriaIdioma SET Operacion = 'Modificación de traducción' WHERE Operacion = 'ADD_TRADUCCION';
GO

PRINT '06 OK — Registros históricos normalizados al nuevo formato legible.';
GO
