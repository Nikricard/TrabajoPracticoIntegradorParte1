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

        // leemos los perfiles y el arbol

        public List<Perfil> GetAllPerfiles()
        {
            Dictionary<string, IPermiso> todos = CargarTodosLosPermisos();
            ConstruirArbol(todos);

            List<Perfil> perfiles = new List<Perfil>();
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

        // ABM de permisos y conjuntos

        // Devuelve todos los permisos atómicos (las funcionalidades base)
        // para mostrarlos en el CheckedListBox de selección.
        public List<PermisoAtomico> GetPermisosAtomicos()
        {
            var lista = new List<PermisoAtomico>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Codigo, Nombre FROM Permiso WHERE EsCompuesto = 0 ORDER BY Codigo", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    lista.Add(new PermisoAtomico
                    {
                        Codigo = rdr.GetString(0),
                        Nombre = rdr.GetString(1)
                    });
            }
            return lista;
        }

        // Crea un conjunto: un permiso compuesto (categoría) + su perfil
        // asignable, con los permisos hijos seleccionados.
        public void CrearConjunto(string nombre, List<string> codigosHijos)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // Verificar nombre único
                var check = new SqlCommand(
                    "SELECT COUNT(1) FROM Perfil WHERE Nombre = @N", con);
                check.Parameters.AddWithValue("@N", nombre);
                if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    throw new Exception($"Ya existe un conjunto llamado '{nombre}'.");

                // Generar código único
                string codigo = GenerarCodigoCompuesto(con);

                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        //Permiso compuesto (categoría)
                        var cmdP = new SqlCommand(
                            "INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES (@C, @N, 1)",
                            con, tr);
                        cmdP.Parameters.AddWithValue("@C", codigo);
                        cmdP.Parameters.AddWithValue("@N", nombre);
                        cmdP.ExecuteNonQuery();

                        //Hijos del compuesto
                        foreach (string hijo in codigosHijos)
                        {
                            var cmdH = new SqlCommand(
                                "INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES (@P, @H)",
                                con, tr);
                            cmdH.Parameters.AddWithValue("@P", codigo);
                            cmdH.Parameters.AddWithValue("@H", hijo);
                            cmdH.ExecuteNonQuery();
                        }

                        //Perfil asignable que apunta al compuesto
                        var cmdPer = new SqlCommand(
                            "INSERT INTO Perfil (Nombre, Codigo) VALUES (@N, @C)",
                            con, tr);
                        cmdPer.Parameters.AddWithValue("@N", nombre);
                        cmdPer.Parameters.AddWithValue("@C", codigo);
                        cmdPer.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Elimina un conjunto: quita el perfil a sus usuarios, y borra
        // el perfil, sus relaciones hijo y el permiso compuesto.

        public void EliminarConjunto(int idPerfil, string codigoCompuesto)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        //Le saca el perfil a los usuarios que lo tengan
                        var cmdU = new SqlCommand(
                            "UPDATE Usuarios SET IdPerfil = NULL WHERE IdPerfil = @Id", con, tr);
                        cmdU.Parameters.AddWithValue("@Id", idPerfil);
                        cmdU.ExecuteNonQuery();

                        //Borra el perfil
                        var cmdPer = new SqlCommand(
                            "DELETE FROM Perfil WHERE IdPerfil = @Id", con, tr);
                        cmdPer.Parameters.AddWithValue("@Id", idPerfil);
                        cmdPer.ExecuteNonQuery();

                        //Borra las relaciones hijo del compuesto
                        var cmdH = new SqlCommand(
                            "DELETE FROM PermisoHijo WHERE CodigoPadre = @C", con, tr);
                        cmdH.Parameters.AddWithValue("@C", codigoCompuesto);
                        cmdH.ExecuteNonQuery();

                        //Borra el permiso compuesto
                        var cmdP = new SqlCommand(
                            "DELETE FROM Permiso WHERE Codigo = @C", con, tr);
                        cmdP.Parameters.AddWithValue("@C", codigoCompuesto);
                        cmdP.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Genera un código compuesto único
        // buscando el mayor número existente y sumando 10.
        private string GenerarCodigoCompuesto(SqlConnection con)
        {
            int max = 40; // el primero generado será GE050
            var cmd = new SqlCommand(
                "SELECT Codigo FROM Permiso WHERE Codigo LIKE 'GE%'", con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string cod = rdr.GetString(0);
                if (cod.Length > 2 && int.TryParse(cod.Substring(2), out int num))
                    if (num > max) max = num;
            }
            rdr.Close();
            return "GE" + (max + 10).ToString("D3");
        }

        // Helpers de composite

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