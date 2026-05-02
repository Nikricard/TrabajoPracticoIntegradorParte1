// ════════════════════════════════════════════════════════
// DAL — IdiomaDAL.cs
// ════════════════════════════════════════════════════════
using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class IdiomaDAL
    {
        private readonly string cs =
            "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=Usuarios;Integrated Security=True";

        // ── Idioma ───────────────────────────────────────────

        /// <summary>
        /// Devuelve todos los idiomas con sus traducciones cargadas.
        /// </summary>
        public List<Idioma> GetAllIdiomas()
        {
            var idiomas = new List<Idioma>();

            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // 1. Traer todos los idiomas
                var cmd = new SqlCommand(
                    "SELECT IdIdioma, Nombre, defecto FROM Idioma ORDER BY IdIdioma", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    idiomas.Add(new Idioma
                    {
                        IdIdioma = rdr.GetInt32(0),
                        Nombre   = rdr.GetString(1),
                        Defecto  = rdr.GetBoolean(2)
                    });
                }
                rdr.Close();

                // 2. Cargar traducciones de cada idioma
                foreach (var idioma in idiomas)
                {
                    var cmdT = new SqlCommand(@"
                        SELECT p.Texto, t.Traduccion
                        FROM Traduccion t
                        INNER JOIN Palabra p ON t.IdPalabra = p.IdPalabra
                        WHERE t.IdIdioma = @IdIdioma", con);
                    cmdT.Parameters.AddWithValue("@IdIdioma", idioma.IdIdioma);

                    var rdrT = cmdT.ExecuteReader();
                    while (rdrT.Read())
                        idioma.Traducciones[rdrT.GetString(0)] = rdrT.GetString(1);
                    rdrT.Close();
                }
            }
            return idiomas;
        }

        /// <summary>
        /// Inserta un nuevo idioma. Devuelve el IdIdioma asignado.
        /// </summary>
        public int AddIdioma(string nombre, bool defecto)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // Verificar duplicado
                var check = new SqlCommand(
                    "SELECT COUNT(1) FROM Idioma WHERE Nombre = @Nombre", con);
                check.Parameters.AddWithValue("@Nombre", nombre);
                if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    throw new Exception($"El idioma '{nombre}' ya existe.");

                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(
                            "INSERT INTO Idioma (Nombre, defecto) OUTPUT INSERTED.IdIdioma " +
                            "VALUES (@Nombre, @Defecto)", con, tr);
                        cmd.Parameters.AddWithValue("@Nombre",   nombre);
                        cmd.Parameters.AddWithValue("@Defecto",  defecto ? 1 : 0);
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        tr.Commit();
                        return id;
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        /// <summary>
        /// Elimina un idioma y sus traducciones.
        /// </summary>
        public void DeleteIdioma(int idIdioma)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // Borrar traducciones primero (FK)
                        var cmdT = new SqlCommand(
                            "DELETE FROM Traduccion WHERE IdIdioma = @Id", con, tr);
                        cmdT.Parameters.AddWithValue("@Id", idIdioma);
                        cmdT.ExecuteNonQuery();

                        var cmdI = new SqlCommand(
                            "DELETE FROM Idioma WHERE IdIdioma = @Id", con, tr);
                        cmdI.Parameters.AddWithValue("@Id", idIdioma);
                        cmdI.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // ── Palabra ──────────────────────────────────────────

        public List<Palabra> GetAllPalabras()
        {
            var lista = new List<Palabra>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(
                    "SELECT IdPalabra, Texto FROM Palabra ORDER BY IdPalabra", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    lista.Add(new Palabra
                    {
                        IdPalabra = rdr.GetInt32(0),
                        Texto     = rdr.GetString(1)
                    });
            }
            return lista;
        }

        /// <summary>
        /// Inserta una Palabra si no existe. Devuelve su IdPalabra.
        /// </summary>
        public int AddPalabra(string texto)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // Si ya existe, devuelve el id existente
                var check = new SqlCommand(
                    "SELECT IdPalabra FROM Palabra WHERE Texto = @Texto", con);
                check.Parameters.AddWithValue("@Texto", texto);
                var existing = check.ExecuteScalar();
                if (existing != null) return Convert.ToInt32(existing);

                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(
                            "INSERT INTO Palabra (Texto) OUTPUT INSERTED.IdPalabra " +
                            "VALUES (@Texto)", con, tr);
                        cmd.Parameters.AddWithValue("@Texto", texto);
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        tr.Commit();
                        return id;
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // ── Traduccion ───────────────────────────────────────

        /// <summary>
        /// Inserta o actualiza una traducción para un idioma+palabra.
        /// </summary>
        public void SaveTraduccion(int idIdioma, int idPalabra, string traduccion)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // MERGE: inserta si no existe, actualiza si ya existe
                        var cmd = new SqlCommand(@"
                            IF EXISTS (SELECT 1 FROM Traduccion
                                       WHERE IdIdioma=@IdI AND IdPalabra=@IdP)
                                UPDATE Traduccion SET Traduccion=@Val
                                WHERE IdIdioma=@IdI AND IdPalabra=@IdP
                            ELSE
                                INSERT INTO Traduccion (IdIdioma,IdPalabra,Traduccion)
                                VALUES (@IdI,@IdP,@Val)", con, tr);
                        cmd.Parameters.AddWithValue("@IdI", idIdioma);
                        cmd.Parameters.AddWithValue("@IdP", idPalabra);
                        cmd.Parameters.AddWithValue("@Val", traduccion);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        public void DeleteTraduccion(int idIdioma, int idPalabra)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(
                            "DELETE FROM Traduccion WHERE IdIdioma=@IdI AND IdPalabra=@IdP",
                            con, tr);
                        cmd.Parameters.AddWithValue("@IdI", idIdioma);
                        cmd.Parameters.AddWithValue("@IdP", idPalabra);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }
    }
}
