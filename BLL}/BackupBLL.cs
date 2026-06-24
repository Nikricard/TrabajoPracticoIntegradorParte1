using DAL;
using System;
using System.IO;

namespace BLL
{
    // Lógica de negocio para backup/restore.
    // Valida la ruta, delega la persistencia a BackupDAL y registra en bitácora.
    public class BackupBLL
    {
        // Singleton
        private static BackupBLL _instancia;
        public static BackupBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new BackupBLL();
                return _instancia;
            }
        }

        private BackupBLL() { _dal = new BackupDAL(); }
        private readonly BackupDAL _dal;


        public void HacerBackup(string rutaArchivo)
        {
            if (string.IsNullOrEmpty(rutaArchivo))
                throw new Exception("Debe indicar una ruta de destino para el backup.");

            // Verifica que la carpeta destino exista
            string carpeta = Path.GetDirectoryName(rutaArchivo);
            if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
                throw new Exception($"La carpeta de destino no existe:\n{carpeta}");

            try
            {
                _dal.HacerBackup(rutaArchivo);
                BitacoraBLL.Instancia.RegistrarBackup(rutaArchivo);
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("BACKUP_DB", ex);
                throw;
            }
        }

        public void RestaurarBackup(string rutaArchivo)
        {
            if (string.IsNullOrEmpty(rutaArchivo))
                throw new Exception("Debe indicar el archivo .bak a restaurar.");
            if (!File.Exists(rutaArchivo))
                throw new Exception($"No se encontró el archivo:\n{rutaArchivo}");

            try
            {
                _dal.RestaurarBackup(rutaArchivo);
                BitacoraBLL.Instancia.RegistrarRestore(rutaArchivo);
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("RESTORE_DB", ex);
                throw;
            }
        }
    }
}
