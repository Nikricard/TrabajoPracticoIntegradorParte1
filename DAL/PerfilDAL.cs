using BE;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace DAL
{
    public class PerfilDAL
    {
        private readonly string cs =
            "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=Usuarios;Integrated Security=True";

        public List<Perfil> GetAllPerfiles()  // metodo para obtener perfiles de la db
        {
            Dictionary<string, IPermiso> todos = CargarTodosLosPermisos();
            ConstruirArbol(todos); // diccionario con todos los permisos atómicos y compuestos, con sus relaciones padre-hijo ya establecidas

            List<Perfil> perfiles = new List<Perfil>(); //lista de perfiles que se va a llenar con los datos de la db y luego se devuelve

            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT IdPerfil, Nombre, Codigo FROM Perfil ORDER BY IdPerfil", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string codigo = rdr.GetString(2);
                    perfiles.Add(new Perfil
                    {
                        IdPerfil = rdr.GetInt32(0),
                        Nombre = rdr.GetString(1),
                        Permiso = todos.ContainsKey(codigo) ? todos[codigo] : null
                    });
                }
            }
            return perfiles;
        }

        public Perfil GetPerfilDeUsuario(int idUsuario)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    SELECT p.IdPerfil, p.Nombre, p.Codigo
                    FROM Perfil p
                    INNER JOIN Usuarios u ON u.IdPerfil = p.IdPerfil
                    WHERE u.Id = @IdUsuario", con);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                var rdr = cmd.ExecuteReader();
                if (!rdr.Read()) return null;

                string codigo = rdr.GetString(2);
                int id = rdr.GetInt32(0);
                string nombre = rdr.GetString(1);
                rdr.Close();

                Dictionary<string, IPermiso> todos = CargarTodosLosPermisos();
                ConstruirArbol(todos);
                return new Perfil
                {
                    IdPerfil = id,
                    Nombre = nombre,
                    Permiso = todos.ContainsKey(codigo) ? todos[codigo] : null 
                    // el perfil del usuario se construye con el permiso raíz que
                    // se obtiene de la db, y ese permiso ya tiene toda su estructura
                    // de hijos gracias a la construcción del árbol

                };
            }
        }

        public void AsignarPerfil(int idUsuario, int idPerfil)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(
                            "UPDATE Usuarios SET IdPerfil=@IdPerfil WHERE Id=@IdUsuario",
                            con, tr);
                        cmd.Parameters.AddWithValue("@IdPerfil", idPerfil);
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Carga todos los permisos atómicos y compuestos en un diccionario.

        private Dictionary<string, IPermiso> CargarTodosLosPermisos()
        {
            var todos = new Dictionary<string, IPermiso>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Codigo, Nombre, EsCompuesto FROM Permiso", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string codigo = rdr.GetString(0);
                    string nombre = rdr.GetString(1);
                    bool esCompuesto = rdr.GetBoolean(2);

                    todos[codigo] = esCompuesto
                        ? (IPermiso)new PermisoCompuesto { Codigo = codigo, Nombre = nombre }
                        : (IPermiso)new PermisoAtomico { Codigo = codigo, Nombre = nombre };
                }
            }
            return todos;
        }

        // Construye el árbol leyendo la tabla PermisoHijo.
        // Como es muchos-a-muchos, un mismo nodo puede aparecer
        // bajo múltiples padres (ej: GE010 es hijo de GE040 Y de GE030).

        private void ConstruirArbol(Dictionary<string, IPermiso> todos)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT CodigoPadre, CodigoHijo FROM PermisoHijo", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string padre = rdr.GetString(0);
                    string hijo = rdr.GetString(1);

                    if (todos.ContainsKey(padre) && todos[padre] is PermisoCompuesto compuesto
                        && todos.ContainsKey(hijo))
                        compuesto.Agregar(todos[hijo]);
                }
            }
        }
    }
}