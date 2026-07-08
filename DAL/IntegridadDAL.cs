using BE;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class IntegridadDAL
    {
        private readonly string cs =
            "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=Usuarios;Integrated Security=True";

        // Devuelve todos los usuarios con los campos relevantes + DVH guardado.
        public List<(int Id, string Nombre, string Contrasena, string DVH)> GetUsuariosParaIntegridad()
        {
            var lista = new List<(int, string, string, string)>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Nombre, Contrasena, ISNULL(DVH, '') FROM Usuarios ORDER BY Id", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    lista.Add((
                        rdr.GetInt32(0),
                        rdr.GetString(1),
                        rdr.GetString(2),
                        rdr.GetString(3)
                    ));
            }
            return lista;
        }

        // Devuelve los campos de un usuario por Id.
        public (int Id, string Nombre, string Contrasena)? GetUsuarioParaHash(int idUsuario)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Nombre, Contrasena FROM Usuarios WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", idUsuario);
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    return (rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2));
                return null;
            }
        }

        // Devuelve los campos de un usuario por Nombre.
        // Se usa como fallback cuando el DAL de Add no devuelve el Id
        // correcto del recién creado (Id queda en 0).
        public (int Id, string Nombre, string Contrasena)? GetUsuarioParaHashPorNombre(string nombre)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Nombre, Contrasena FROM Usuarios WHERE Nombre = @Nombre", con);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    return (rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2));
                return null;
            }
        }

        // Cuenta cuántos usuarios tienen el DVH sin calcular (NULL o vacío).
        public int ContarUsuariosSinDVH()
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM Usuarios WHERE DVH IS NULL OR DVH = ''", con);
                return System.Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Guarda el DVH de un usuario específico.
        public void GuardarDVH(int idUsuario, string dvh)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "UPDATE Usuarios SET DVH = @DVH WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@DVH", dvh);
                cmd.Parameters.AddWithValue("@Id",  idUsuario);
                cmd.ExecuteNonQuery();
            }
        }

        // Lee el DVV guardado para la tabla Usuarios.
        public string GetDVV()
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT DVV FROM DigitoVerificadorTabla WHERE NombreTabla = 'Usuarios'", con);
                var resultado = cmd.ExecuteScalar();
                return resultado?.ToString() ?? "";
            }
        }

        // Inserta o actualiza el DVV de la tabla Usuarios.
        public void GuardarDVV(string dvv)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    IF EXISTS (SELECT 1 FROM DigitoVerificadorTabla WHERE NombreTabla = 'Usuarios')
                        UPDATE DigitoVerificadorTabla
                        SET DVV = @DVV, FechaCalculo = GETDATE()
                        WHERE NombreTabla = 'Usuarios'
                    ELSE
                        INSERT INTO DigitoVerificadorTabla (NombreTabla, DVV, FechaCalculo)
                        VALUES ('Usuarios', @DVV, GETDATE())", con);
                cmd.Parameters.AddWithValue("@DVV", dvv);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
