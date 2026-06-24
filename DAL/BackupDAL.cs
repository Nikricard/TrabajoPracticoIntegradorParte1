using System;
using System.Data.SqlClient;

namespace DAL
{
    // DAL para backup y restore de la base.
    // CRITICO: las operaciones BACKUP/RESTORE no pueden ejecutarse mientras
    // estás conectado a la base que querés respaldar/restaurar. Por eso esta
    // clase usa una cadena de conexión que apunta a la base "master".
    public class BackupDAL
    {
        // Conexión a master (no a Usuarios) — necesario para que SQL Server
        // pueda obtener acceso exclusivo sobre la base "Usuarios".
        private readonly string csMaster =
            "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=master;Integrated Security=True";

        // Nombre de la base que se respalda / restaura.
        private const string DB = "Usuarios";

        // Hace un backup completo de la base en el archivo indicado.
        // Si el archivo ya existe, lo sobrescribe (WITH INIT).
        public void HacerBackup(string rutaArchivo)
        {
            using (var con = new SqlConnection(csMaster))
            {
                con.Open();
                string sql = $"BACKUP DATABASE [{DB}] TO DISK = @Ruta WITH FORMAT, INIT, COPY_ONLY";
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Ruta", rutaArchivo);
                cmd.CommandTimeout = 300;  // hasta 5 min para bases grandes
                cmd.ExecuteNonQuery();
            }
        }

        // Restaura la base desde un archivo .bak.
        // Pasos: (1) cierra todas las conexiones poniendo la base en SINGLE_USER
        //        (2) RESTORE DATABASE con REPLACE
        //        (3) vuelve a MULTI_USER
        // Si el RESTORE falla, igual intenta devolver la base a MULTI_USER.
        public void RestaurarBackup(string rutaArchivo)
        {
            using (var con = new SqlConnection(csMaster))
            {
                con.Open();

                // (1) Echar a todos los usuarios conectados a la base
                var cmdSingle = new SqlCommand(
                    $"ALTER DATABASE [{DB}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", con);
                cmdSingle.CommandTimeout = 60;
                cmdSingle.ExecuteNonQuery();

                try
                {
                    // (2) Restaurar (REPLACE = sobrescribe la base actual)
                    var cmdRestore = new SqlCommand(
                        $"RESTORE DATABASE [{DB}] FROM DISK = @Ruta WITH REPLACE", con);
                    cmdRestore.Parameters.AddWithValue("@Ruta", rutaArchivo);
                    cmdRestore.CommandTimeout = 300;
                    cmdRestore.ExecuteNonQuery();
                }
                finally
                {
                    // (3) Devolver la base a multi-usuario, falle o no el restore
                    var cmdMulti = new SqlCommand(
                        $"ALTER DATABASE [{DB}] SET MULTI_USER", con);
                    cmdMulti.CommandTimeout = 60;
                    cmdMulti.ExecuteNonQuery();
                }
            }
        }
    }
}
