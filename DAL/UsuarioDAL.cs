using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class UsuarioDAL
    {
        private readonly string conexion =
            "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=Usuarios;Integrated Security=True";

        public Usuario Add(Usuario usuario)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();

                // SCOPE_IDENTITY() para recuperar el Id IDENTITY recién generado.
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Usuarios(Nombre, Contrasena) VALUES(@Nombre, @Contrasena); " +
                    "SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Contrasena", usuario.Contraseña);

                object resultado = cmd.ExecuteScalar();
                if (resultado == null || resultado == DBNull.Value)
                    throw new Exception("No se pudo agregar el usuario");

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
                    return new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Contraseña = reader.GetString(2)
                    };
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
                // Solo actualiza el Nombre — la contraseña no se toca desde frmModificar.
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Usuarios SET Nombre = @Nombre WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Id", usuario.Id);

                int filas = cmd.ExecuteNonQuery();
                if (filas == 0)
                    throw new Exception("No se pudo modificar el usuario");
                return usuario;
            }
        }

        public Usuario Delete(Usuario usuario)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();
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


        // Métodos para snapshots y rollback (v2.7)

        // Devuelve la lista de códigos de permiso asignados a un usuario.
        // Se usa al construir un snapshot ANTES de una modificación.
        public List<string> GetPermisosCodigos(int idUsuario)
        {
            var codigos = new List<string>();
            using (var con = new SqlConnection(conexion))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Codigo FROM UsuarioPermiso WHERE IdUsuario = @Id", con);
                cmd.Parameters.AddWithValue("@Id", idUsuario);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    codigos.Add(rdr.GetString(0));
            }
            return codigos;
        }

        // Verifica que el usuario del snapshot todavía exista en la base.
        public bool Existe(int idUsuario)
        {
            using (var con = new SqlConnection(conexion))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM Usuarios WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", idUsuario);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // Aplica un snapshot al usuario: restaura Nombre y reemplaza sus
        // permisos por los que estaban vigentes en el snapshot.
        // No toca la contraseña (inalterable por decisión de diseño).
        // Todo en una transacción.
        public void Restaurar(int idUsuario, string nombreSnapshot, List<string> permisosSnapshot)
        {
            using (var con = new SqlConnection(conexion))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // 1) Restaurar el nombre
                        var cmdNombre = new SqlCommand(
                            "UPDATE Usuarios SET Nombre = @Nombre WHERE Id = @Id", con, tr);
                        cmdNombre.Parameters.AddWithValue("@Nombre", nombreSnapshot);
                        cmdNombre.Parameters.AddWithValue("@Id", idUsuario);
                        int filas = cmdNombre.ExecuteNonQuery();
                        if (filas == 0)
                        {
                            tr.Rollback();
                            throw new Exception(
                                $"El usuario Id {idUsuario} ya no existe. " +
                                "No se puede aplicar el rollback.");
                        }

                        // 2) Borrar todos los permisos actuales del usuario
                        var cmdDelPerm = new SqlCommand(
                            "DELETE FROM UsuarioPermiso WHERE IdUsuario = @Id", con, tr);
                        cmdDelPerm.Parameters.AddWithValue("@Id", idUsuario);
                        cmdDelPerm.ExecuteNonQuery();

                        // 3) Insertar los permisos del snapshot
                        if (permisosSnapshot != null)
                        {
                            foreach (string codigo in permisosSnapshot)
                            {
                                var cmdIns = new SqlCommand(
                                    "INSERT INTO UsuarioPermiso (IdUsuario, Codigo) " +
                                    "VALUES (@Id, @Codigo)", con, tr);
                                cmdIns.Parameters.AddWithValue("@Id", idUsuario);
                                cmdIns.Parameters.AddWithValue("@Codigo", codigo);
                                cmdIns.ExecuteNonQuery();
                            }
                        }

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
