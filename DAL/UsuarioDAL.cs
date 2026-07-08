using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UsuarioDAL
    {
        private readonly string conexion = "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=Usuarios;Integrated Security=True";

        public Usuario Add(Usuario usuario)
        {   //se recibe un usuario en la lista, hasheado previamente en BLL, con datos ya validados en BE
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();

                // SELECT SCOPE_IDENTITY() recupera el Id IDENTITY
                // que SQL Server genera. Esto es lo que permite que
                // IntegridadBLL pueda leer el usuario recién creado para
                // calcular su DVH sin depender del fallback por nombre.
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Usuarios(Nombre, Contrasena) VALUES(@Nombre, @Contrasena); " +
                    "SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Contrasena", usuario.Contraseña);

                // ExecuteScalar devuelve el primer valor de la primera fila:
                // el Id que acaba de generar SQL Server.
                object resultado = cmd.ExecuteScalar();
                if (resultado == null || resultado == DBNull.Value)
                {
                    throw new Exception("No se pudo agregar el usuario");
                }

                usuario.Id = Convert.ToInt32(resultado);
                return usuario;
            }
        }

        public Usuario Login(string nombre, string contraseñaHasheada)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT Id, Nombre, Contrasena FROM Usuarios " +
                    "WHERE Nombre = @Nombre AND Contrasena = @Contrasena", conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Contrasena", contraseñaHasheada);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Contraseña = reader.GetString(2)
                    };
                    return usuario;
                }
                return null;
            }
        }

        public List<Usuario> GetAll()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT Id, Nombre, Contrasena FROM Usuarios ORDER BY Id", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Contraseña = reader.GetString(2)
                    });
                }
            }
            return lista;
        }

        public Usuario Modify(Usuario usuario)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();

                // solo se actualiza el Nombre.
                // La contraseña no se toca .
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Usuarios SET Nombre = @Nombre WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Id", usuario.Id);

                int filas = cmd.ExecuteNonQuery();
                if (filas == 0)
                {
                    throw new Exception("No se pudo modificar el usuario");
                }
                return usuario;
            }
        }

        public Usuario Delete(Usuario usuario)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();

                // Se borran en transacción: primero los permisos asignados al
                // usuario (para no romper la FK en UsuarioPermiso), después
                // el usuario. Sin este paso previo, SQL Server tira error 547
                // ("FOREIGN KEY constraint conflict") cuando el usuario tiene
                // permisos asignados.
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmdPermisos = new SqlCommand(
                            "DELETE FROM UsuarioPermiso WHERE IdUsuario = @Id", conn, tr);
                        cmdPermisos.Parameters.AddWithValue("@Id", usuario.Id);
                        cmdPermisos.ExecuteNonQuery();

                        SqlCommand cmdUsuario = new SqlCommand(
                            "DELETE FROM Usuarios WHERE Id = @Id", conn, tr);
                        cmdUsuario.Parameters.AddWithValue("@Id", usuario.Id);
                        int filas = cmdUsuario.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            tr.Rollback();
                            throw new Exception("No se pudo eliminar el usuario");
                        }

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                return usuario;
            }
        }
    }
}
