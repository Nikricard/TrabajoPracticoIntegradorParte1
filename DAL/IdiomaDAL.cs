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
            "Server=.;Database=Usuarios;Integrated Security=True";
        //Server facultad = .
        //Server casa = DESKTOP-FD6Q6GG\SQLEXPRESS

        // Devuelve todos los idiomas con sus traducciones cargadas.
        public List<Idioma> GetAllIdiomas()
        {
            var idiomas = new List<Idioma>();

            using (var con = new SqlConnection(cs))
            {
                con.Open();
                {

                    // Trae todos los idiomas
                    var cmd = new SqlCommand("SELECT * FROM Idioma ORDER BY IdIdioma", con);
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        idiomas.Add(new Idioma
                        {
                            IdIdioma = rdr.GetInt32(0),
                            Nombre = rdr.GetString(1),
                            Defecto = rdr.GetBoolean(2)
                        });
                    }
                    rdr.Close();

                    // Carga traducciones de cada idioma
                    foreach (var idioma in idiomas)
                    {
                        var cmdT = new SqlCommand(@"SELECT p.Texto, t.Traduccion FROM Traduccion t INNER JOIN Palabra p ON t.IdPalabra = p.IdPalabra WHERE t.IdIdioma = @IdIdioma", con);
                        cmdT.Parameters.AddWithValue("@IdIdioma", idioma.IdIdioma);

                        var rdrT = cmdT.ExecuteReader();
                        while (rdrT.Read())
                            idioma.Traducciones[rdrT.GetString(0)] = rdrT.GetString(1);
                        rdrT.Close();
                    }
                }
                return idiomas;
            }
        }

        //Inserta un nuevo idioma. Devuelve el IdIdioma asignado.

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
                        var cmd = new SqlCommand("INSERT INTO Idioma (Nombre, defecto) OUTPUT INSERTED.IdIdioma " + "VALUES (@Nombre, @Defecto)", con, tr);
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

        // Elimina un idioma y sus traducciones.
        public void DeleteIdioma(int idIdioma)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // Borrar traducciones primero usando la FK
                        var cmdT = new SqlCommand("DELETE FROM Traduccion WHERE IdIdioma = @Id", con, tr);
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

        //obtener palabras
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

        // Inserta una Palabra si no existe --> Devuelve su IdPalabra.
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
                        var cmd = new SqlCommand( "INSERT INTO Palabra (Texto) OUTPUT INSERTED.IdPalabra " + "VALUES (@Texto)", con, tr);
                        cmd.Parameters.AddWithValue("@Texto", texto);
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        tr.Commit();
                        return id;
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Inserta o actualiza una traducción para un idioma y palabra.
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

        //Elimina una tag y todas sus traducciones
        public void DeletePalabra(int idPalabra)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        // Primero las traducciones (FK)
                        var cmdT = new SqlCommand(
                            "DELETE FROM Traduccion WHERE IdPalabra = @Id", con, tr);
                        cmdT.Parameters.AddWithValue("@Id", idPalabra);
                        cmdT.ExecuteNonQuery();

                        var cmdP = new SqlCommand(
                            "DELETE FROM Palabra WHERE IdPalabra = @Id", con, tr);
                        cmdP.Parameters.AddWithValue("@Id", idPalabra);
                        cmdP.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Asigna todas las palabras existentes a un idioma,
        // con traducción vacía. Usa NOT EXISTS para no duplicar.
        public void AsignarTodasLasPalabrasAIdioma(int idIdioma)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(@"
                            INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
                            SELECT @IdIdioma, p.IdPalabra, ''
                            FROM Palabra p
                            WHERE NOT EXISTS (
                                SELECT 1 FROM Traduccion t
                                WHERE t.IdIdioma = @IdIdioma AND t.IdPalabra = p.IdPalabra
                            )", con, tr);
                        cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

        // Asigna una tag a todos los idiomas existentes,
        // con traducción vacía. Usa NOT EXISTS para no duplicar.
        public void AsignarPalabraATodosLosIdiomas(int idPalabra)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(@"
                            INSERT INTO Traduccion (IdIdioma, IdPalabra, Traduccion)
                            SELECT i.IdIdioma, @IdPalabra, ''
                            FROM Idioma i
                            WHERE NOT EXISTS (
                                SELECT 1 FROM Traduccion t
                                WHERE t.IdIdioma = i.IdIdioma AND t.IdPalabra = @IdPalabra
                            )", con, tr);
                        cmd.Parameters.AddWithValue("@IdPalabra", idPalabra);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch { tr.Rollback(); throw; }
                }
            }
        }

    }
}
