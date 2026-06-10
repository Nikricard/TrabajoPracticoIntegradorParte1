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


        // Devuelve un permiso compuesto "raíz virtual" que agrupa todos
        // los permisos asignados al usuario (atómicos y compuestos).
        // Sirve para login (TienePermiso) y para el TreeView.

        public PermisoCompuesto GetPermisosDeUsuario(int idUsuario)
        {
            // Cargar todos los permisos atómicos y compuestos en un diccionario.
            Dictionary<string, IPermiso> todos = CargarTodosLosPermisos();
            // Construir el árbol de permisos compuesto a partir de las relaciones padre-hijo.
            ConstruirArbol(todos);

            var raiz = new PermisoCompuesto
            {
                Codigo = "ROOT",
                Nombre = "Permisos del usuario"
            };

            using (var con = new SqlConnection(cs))
            {
                con.Open();
                // Leer los códigos de permisos asignados al usuario y agregarlos a la raíz.
                var cmd = new SqlCommand("SELECT Codigo FROM UsuarioPermiso WHERE IdUsuario = @Id", con);
                cmd.Parameters.AddWithValue("@Id", idUsuario);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string codigo = rdr.GetString(0);
                    // Si el código existe en el diccionario, agregar el permiso correspondiente a la raíz.
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
                var cmd = new SqlCommand("SELECT Codigo FROM UsuarioPermiso WHERE IdUsuario = @Id", con);
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
                        var del = new SqlCommand("DELETE FROM UsuarioPermiso WHERE IdUsuario = @Id", con, tr);
                        del.Parameters.AddWithValue("@Id", idUsuario);
                        del.ExecuteNonQuery();

                        // Insertar los nuevos
                        foreach (string codigo in codigos)
                        {
                            var ins = new SqlCommand("INSERT INTO UsuarioPermiso (IdUsuario, Codigo) VALUES (@Id, @C)",con, tr);
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
                var cmd = new SqlCommand("SELECT Codigo, Nombre FROM Permiso WHERE EsCompuesto = 0 ORDER BY Codigo", con);
                var rdr = cmd.ExecuteReader();
                // Agregar solo los permisos atómicos a la lista.
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
            // Cargar todos los permisos en un diccionario y construir el árbol para que los compuestos tengan sus hijos.
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
                // Verificar que no exista un permiso atómico con el mismo nombre
                var check = new SqlCommand("SELECT COUNT(1) FROM Permiso WHERE Nombre = @N AND EsCompuesto = 1", con);
                check.Parameters.AddWithValue("@N", nombre);
                // Verificar que no exista otro conjunto con el mismo nombre
                if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    // Si ya existe un conjunto con ese nombre, lanzar una excepción
                    throw new Exception($"Ya existe un conjunto llamado '{nombre}'.");
                // Generar código único para el nuevo conjunto
                string codigo = GenerarCodigoCompuesto(con);

                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // Insertar el nuevo permiso compuesto
                        var cmdP = new SqlCommand("INSERT INTO Permiso (Codigo, Nombre, EsCompuesto) VALUES (@C, @N, 1)",con, tr);
                        cmdP.Parameters.AddWithValue("@C", codigo);
                        cmdP.Parameters.AddWithValue("@N", nombre);
                        cmdP.ExecuteNonQuery();

                        // Insertar las relaciones con los hijos seleccionados
                        foreach (string hijo in codigosHijos)
                        {
                            var cmdH = new SqlCommand("INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES (@P, @H)",con, tr);
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
                        var cmdN = new SqlCommand("UPDATE Permiso SET Nombre = @N WHERE Codigo = @C", con, tr);
                        cmdN.Parameters.AddWithValue("@N", nuevoNombre);
                        cmdN.Parameters.AddWithValue("@C", codigo);
                        cmdN.ExecuteNonQuery();

                        // Reemplazar hijos
                        var cmdDel = new SqlCommand("DELETE FROM PermisoHijo WHERE CodigoPadre = @C", con, tr);
                        cmdDel.Parameters.AddWithValue("@C", codigo);
                        cmdDel.ExecuteNonQuery();

                        foreach (string hijo in codigosHijos)
                        {
                            var cmdH = new SqlCommand("INSERT INTO PermisoHijo (CodigoPadre, CodigoHijo) VALUES (@P, @H)",con, tr);
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

        // Elimina un conjunto: lo elimina de los usuarios, de otros conjuntos que lo contengan, sus hijos y el permiso.
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
                        var cmdU = new SqlCommand("DELETE FROM UsuarioPermiso WHERE Codigo = @C", con, tr);
                        cmdU.Parameters.AddWithValue("@C", codigo);
                        cmdU.ExecuteNonQuery();

                        // Eliminarlo como hijo de otros conjuntos y sus propios hijos
                        var cmdH = new SqlCommand("DELETE FROM PermisoHijo WHERE CodigoPadre = @C OR CodigoHijo = @C",con, tr);
                        cmdH.Parameters.AddWithValue("@C", codigo);
                        cmdH.ExecuteNonQuery();

                        // Borrar el permiso compuesto
                        var cmdP = new SqlCommand("DELETE FROM Permiso WHERE Codigo = @C", con, tr);
                        cmdP.Parameters.AddWithValue("@C", codigo);
                        cmdP.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Genera un código único para un nuevo permiso compuesto,
        // basado en el máximo número usado entre los códigos que empiezan con "GE".
        private string GenerarCodigoCompuesto(SqlConnection con)
        {
            //el max es 40 porque yo tengo 4 permisos del sistema con codigos 10,20,30,40
            int max = 40;
            // Buscar el máximo número entre los códigos que empiezan con "GE" para generar un nuevo código único.
            var cmd = new SqlCommand("SELECT Codigo FROM Permiso WHERE Codigo LIKE 'GE%'", con);
            var rdr = cmd.ExecuteReader();
            // Recorrer los códigos encontrados, extraer el número y actualizar el máximo si es mayor.
            while (rdr.Read())
            {
                string cod = rdr.GetString(0);
                // Si el código tiene el formato esperado (GE seguido de un número), extraer el número y comparar con el máximo.
                if (cod.Length > 2 && int.TryParse(cod.Substring(2), out int num))
                    // Si el número extraído es mayor que el máximo actual, actualizar el máximo.
                    if (num > max) max = num;
            }
            rdr.Close();
            //retornar el nuevo codigo generado sumando 10 al nuevo maximo
            return "GE" + (max + 10).ToString("D3");
        }

        // Para Composite
        // Carga todos los permisos atómicos y compuestos en un diccionario sin relaciones.
        private Dictionary<string, IPermiso> CargarTodosLosPermisos()
        {
            var todos = new Dictionary<string, IPermiso>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand("SELECT Codigo, Nombre, EsCompuesto FROM Permiso", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string codigo = rdr.GetString(0);
                    string nombre = rdr.GetString(1);
                    bool esCompuesto = rdr.GetBoolean(2);
                    // Crear un objeto PermisoCompuesto o PermisoAtomico según el valor de EsCompuesto y agregarlo al diccionario.
                    todos[codigo] = esCompuesto
                        ? (IPermiso)new PermisoCompuesto { Codigo = codigo, Nombre = nombre }
                        : (IPermiso)new PermisoAtomico { Codigo = codigo, Nombre = nombre };
                }
            }
            return todos;
        }

        // Construye el árbol de permisos compuesto a partir de las relaciones padre-hijo.
        private void ConstruirArbol(Dictionary<string, IPermiso> todos)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                // Leer las relaciones padre-hijo y armar el árbol de permisos compuesto.
                var cmd = new SqlCommand("SELECT CodigoPadre, CodigoHijo FROM PermisoHijo", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string padre = rdr.GetString(0);
                    string hijo = rdr.GetString(1);
                    // Si el padre es un compuesto y el hijo existe, agregarlo al compuesto.
                    if (todos.ContainsKey(padre) && todos[padre] is PermisoCompuesto compuesto
                        && todos.ContainsKey(hijo))
                        compuesto.Agregar(todos[hijo]);
                }
            }
        }
    }

}