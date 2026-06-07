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


        // Devuelve un permiso compuesto "raíz virtual" que agrupa TODOS
        // los permisos asignados al usuario (atómicos y compuestos).
        // Sirve para login (TienePermiso) y para el TreeView.

        public PermisoCompuesto GetPermisosDeUsuario(int idUsuario)
        {
            Dictionary<string, IPermiso> todos = CargarTodosLosPermisos();
            ConstruirArbol(todos);

            var raiz = new PermisoCompuesto
            {
                Codigo = "ROOT",
                Nombre = "Permisos del usuario"
            };

            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Codigo FROM UsuarioPermiso WHERE IdUsuario = @Id", con);
                cmd.Parameters.AddWithValue("@Id", idUsuario);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string codigo = rdr.GetString(0);
                    if (todos.ContainsKey(codigo))
                        raiz.Agregar(todos[codigo]);
                }
            }
            return raiz;
        }

        // Devuelve solo los códigos asignados a un usuario
        // (para marcar los checkboxes en frmPerfil).

        public List<string> GetCodigosDeUsuario(int idUsuario)
        {
            var lista = new List<string>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT Codigo FROM UsuarioPermiso WHERE IdUsuario = @Id", con);
                cmd.Parameters.AddWithValue("@Id", idUsuario);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    lista.Add(rdr.GetString(0));
            }
            return lista;
        }

        // Reemplaza los permisos de un usuario por los seleccionados.
        public void GuardarPermisosDeUsuario(int idUsuario, List<string> codigos)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // Borrar los actuales
                        var del = new SqlCommand(
                            "DELETE FROM UsuarioPermiso WHERE IdUsuario = @Id", con, tr);
                        del.Parameters.AddWithValue("@Id", idUsuario);
                        del.ExecuteNonQuery();

                        // Insertar los nuevos
                        foreach (string codigo in codigos)
                        {
                            var ins = new SqlCommand(
                                "INSERT INTO UsuarioPermiso (IdUsuario, Codigo) VALUES (@Id, @C)",
                                con, tr);
                            ins.Parameters.AddWithValue("@Id", idUsuario);
                            ins.Parameters.AddWithValue("@C", codigo);
                            ins.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Lectura para listas y árboles
        // Solo los permisos atómicos.
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
        
        // Permisos seleccionables para armar un conjunto: atómicos + compuestos,
        // excluye el propio conjunto que se está editando (evita auto-referencia).
        public List<IPermiso> GetSeleccionablesParaConjunto(string codigoExcluir)
        {
            Dictionary<string, IPermiso> todos = CargarTodosLosPermisos();
            ConstruirArbol(todos);

            var lista = new List<IPermiso>();
            foreach (var kv in todos)
                if (kv.Key != codigoExcluir)
                    lista.Add(kv.Value);
            return lista;
        }

        // Devuelve solo los permisos compuestos para el grid.

        public List<IPermiso> GetConjuntos()
        {
            Dictionary<string, IPermiso> todos = CargarTodosLosPermisos();
            ConstruirArbol(todos);

            var lista = new List<IPermiso>();
            foreach (var p in todos.Values)
                if (p is PermisoCompuesto) lista.Add(p);
            return lista;
        }

        // ABM de permisos compuestos

        public void CrearConjunto(string nombre, List<string> codigosHijos)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                var check = new SqlCommand(
                    "SELECT COUNT(1) FROM Permiso WHERE Nombre = @N AND EsCompuesto = 1", con);
                check.Parameters.AddWithValue("@N", nombre);
                if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    throw new Exception($"Ya existe un conjunto llamado '{nombre}'.");

                string codigo = GenerarCodigoCompuesto(con);

                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        var cmdP = new SqlCommand(
                            "INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES (@C, @N, 1)",
                            con, tr);
                        cmdP.Parameters.AddWithValue("@C", codigo);
                        cmdP.Parameters.AddWithValue("@N", nombre);
                        cmdP.ExecuteNonQuery();

                        foreach (string hijo in codigosHijos)
                        {
                            var cmdH = new SqlCommand(
                                "INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES (@P, @H)",
                                con, tr);
                            cmdH.Parameters.AddWithValue("@P", codigo);
                            cmdH.Parameters.AddWithValue("@H", hijo);
                            cmdH.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Actualiza un conjunto --> cambia su nombre y reemplaza sus hijos.
        public void ActualizarConjunto(string codigo, string nuevoNombre, List<string> codigosHijos)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // Actualizar nombre
                        var cmdN = new SqlCommand(
                            "UPDATE Permiso SET Nombre = @N WHERE Codigo = @C", con, tr);
                        cmdN.Parameters.AddWithValue("@N", nuevoNombre);
                        cmdN.Parameters.AddWithValue("@C", codigo);
                        cmdN.ExecuteNonQuery();

                        // Reemplazar hijos
                        var cmdDel = new SqlCommand(
                            "DELETE FROM PermisoHijo WHERE CodigoPadre = @C", con, tr);
                        cmdDel.Parameters.AddWithValue("@C", codigo);
                        cmdDel.ExecuteNonQuery();

                        foreach (string hijo in codigosHijos)
                        {
                            var cmdH = new SqlCommand(
                                "INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES (@P, @H)",
                                con, tr);
                            cmdH.Parameters.AddWithValue("@P", codigo);
                            cmdH.Parameters.AddWithValue("@H", hijo);
                            cmdH.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Elimina un conjunto: lo elimina de los usuarios, de otros conjuntos
        // que lo contengan, sus hijos y el permiso.
        public void EliminarConjunto(string codigo)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // Eliminarlo de los usuarios
                        var cmdU = new SqlCommand(
                            "DELETE FROM UsuarioPermiso WHERE Codigo = @C", con, tr);
                        cmdU.Parameters.AddWithValue("@C", codigo);
                        cmdU.ExecuteNonQuery();

                        // Eliminarlo como hijo de otros conjuntos y sus propios hijos
                        var cmdH = new SqlCommand(
                            "DELETE FROM PermisoHijo WHERE CodigoPadre = @C OR CodigoHijo = @C",
                            con, tr);
                        cmdH.Parameters.AddWithValue("@C", codigo);
                        cmdH.ExecuteNonQuery();

                        // Borrar el permiso compuesto
                        var cmdP = new SqlCommand(
                            "DELETE FROM Permiso WHERE Codigo = @C", con, tr);
                        cmdP.Parameters.AddWithValue("@C", codigo);
                        cmdP.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        private string GenerarCodigoCompuesto(SqlConnection con)
        {
            int max = 40;
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

        // Para Composite

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