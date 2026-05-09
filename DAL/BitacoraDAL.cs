using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BitacoraDAL
    {
        private readonly string cs =
            "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=Usuarios;Integrated Security=True";

        public void RegistrarEvento(RegistroBitacora r)
        {
            try
            {
                using (var con = new SqlConnection(cs))
                {
                    con.Open();
                    var cmd = new SqlCommand(@"
                        INSERT INTO Bitacora
                            (Fecha, Usuario, Actividad, TipoEvento,
                             Descripcion, Entidad, ValorAnterior, ValorNuevo)
                        VALUES
                            (@Fecha, @Usuario, @Actividad, @TipoEvento,
                             @Descripcion, @Entidad, @ValorAnterior, @ValorNuevo)",
                        con);
                    cmd.Parameters.AddWithValue("@Fecha",         r.Fecha);
                    cmd.Parameters.AddWithValue("@Usuario",        r.Usuario       ?? "");  // si el usuario es null, guardamos cadena vacía para evitar errores de null en la BD
                    cmd.Parameters.AddWithValue("@Actividad",      r.Actividad     ?? "");
                    cmd.Parameters.AddWithValue("@TipoEvento",     r.TipoEvento.ToString());
                    cmd.Parameters.AddWithValue("@Descripcion",    r.Descripcion   ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Entidad",        r.Entidad       ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ValorAnterior",  r.ValorAnterior ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ValorNuevo",     r.ValorNuevo    ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        public List<RegistroBitacora> Buscar(
            DateTime? desde, DateTime? hasta,
            string usuario, string actividad, string tipoEvento)
        {
            var lista = new List<RegistroBitacora>();

            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var sql = @"
                    SELECT IdBitacora, Fecha, Usuario, Actividad, TipoEvento,
                           Descripcion, Entidad, ValorAnterior, ValorNuevo
                    FROM Bitacora
                    WHERE 1=1";

                if (desde.HasValue)      sql += " AND Fecha >= @Desde";
                if (hasta.HasValue)      sql += " AND Fecha <= @Hasta";
                if (!string.IsNullOrEmpty(usuario))    sql += " AND Usuario LIKE @Usuario";
                if (!string.IsNullOrEmpty(actividad))  sql += " AND Actividad LIKE @Actividad";
                if (!string.IsNullOrEmpty(tipoEvento)) sql += " AND TipoEvento = @TipoEvento";
                sql += " ORDER BY Fecha DESC";

                var cmd = new SqlCommand(sql, con);
                if (desde.HasValue)      cmd.Parameters.AddWithValue("@Desde",      desde.Value);
                if (hasta.HasValue)      cmd.Parameters.AddWithValue("@Hasta",      hasta.Value.AddDays(1));
                if (!string.IsNullOrEmpty(usuario))    cmd.Parameters.AddWithValue("@Usuario",    $"%{usuario}%");
                if (!string.IsNullOrEmpty(actividad))  cmd.Parameters.AddWithValue("@Actividad",  $"%{actividad}%");
                if (!string.IsNullOrEmpty(tipoEvento)) cmd.Parameters.AddWithValue("@TipoEvento", tipoEvento);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lista.Add(new RegistroBitacora // mapeamos cada registro a un objeto RegistroBitacora
                    {
                        IdBitacora    = rdr.GetInt32(0),
                        Fecha         = rdr.GetDateTime(1),
                        Usuario       = rdr.GetString(2),
                        Actividad     = rdr.GetString(3),
                        TipoEvento    = (TipoEvento)Enum.Parse(typeof(TipoEvento), rdr.GetString(4)),
                        Descripcion   = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                        Entidad       = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                        ValorAnterior = rdr.IsDBNull(7) ? null : rdr.GetString(7),
                        ValorNuevo    = rdr.IsDBNull(8) ? null : rdr.GetString(8)
                    });
                }
            }
            return lista;
        }

        //Auditoría Usuario

        public void RegistrarAuditoriaUsuario(AuditoriaUsuario a)
        {
            try
            {
                using (var con = new SqlConnection(cs))
                {
                    con.Open();
                    var cmd = new SqlCommand(@"
                        INSERT INTO AuditoriaUsuario
                            (Fecha, UsuarioAccion, Operacion, IdUsuario,
                             NombreAnterior, NombreNuevo)
                        VALUES
                            (@Fecha, @UsuarioAccion, @Operacion, @IdUsuario,
                             @NombreAnterior, @NombreNuevo)", con);
                    cmd.Parameters.AddWithValue("@Fecha",          a.Fecha);
                    cmd.Parameters.AddWithValue("@UsuarioAccion",   a.UsuarioAccion  ?? "");
                    cmd.Parameters.AddWithValue("@Operacion",       a.Operacion      ?? "");
                    cmd.Parameters.AddWithValue("@IdUsuario",       a.IdUsuario);
                    cmd.Parameters.AddWithValue("@NombreAnterior",  a.NombreAnterior ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreNuevo",     a.NombreNuevo    ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        public List<AuditoriaUsuario> GetAuditoriaUsuario(int? idUsuario = null)
        {
            var lista = new List<AuditoriaUsuario>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var sql = @"
                    SELECT IdAuditoria, Fecha, UsuarioAccion, Operacion,
                           IdUsuario, NombreAnterior, NombreNuevo
                    FROM AuditoriaUsuario";
                if (idUsuario.HasValue) sql += " WHERE IdUsuario = @IdUsuario";
                sql += " ORDER BY Fecha DESC";

                var cmd = new SqlCommand(sql, con);
                if (idUsuario.HasValue)
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario.Value);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lista.Add(new AuditoriaUsuario
                    {
                        IdAuditoria    = rdr.GetInt32(0),
                        Fecha          = rdr.GetDateTime(1),
                        UsuarioAccion  = rdr.GetString(2),
                        Operacion      = rdr.GetString(3),
                        IdUsuario      = rdr.GetInt32(4),
                        NombreAnterior = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                        NombreNuevo    = rdr.IsDBNull(6) ? null : rdr.GetString(6)
                    });
                }
            }
            return lista;
        }

        //Auditoría Idioma

        public void RegistrarAuditoriaIdioma(AuditoriaIdioma a)
        {
            try
            {
                using (var con = new SqlConnection(cs))
                {
                    con.Open();
                    var cmd = new SqlCommand(@"
                        INSERT INTO AuditoriaIdioma
                            (Fecha, UsuarioAccion, Operacion, IdIdioma,
                             NombreAnterior, NombreNuevo, ClaveTraduccion,
                             ValorAnterior, ValorNuevo)
                        VALUES
                            (@Fecha, @UsuarioAccion, @Operacion, @IdIdioma,
                             @NombreAnterior, @NombreNuevo, @ClaveTraduccion,
                             @ValorAnterior, @ValorNuevo)", con);
                    cmd.Parameters.AddWithValue("@Fecha",           a.Fecha);
                    cmd.Parameters.AddWithValue("@UsuarioAccion",    a.UsuarioAccion   ?? "");
                    cmd.Parameters.AddWithValue("@Operacion",        a.Operacion       ?? "");
                    cmd.Parameters.AddWithValue("@IdIdioma",         a.IdIdioma);
                    cmd.Parameters.AddWithValue("@NombreAnterior",   a.NombreAnterior  ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreNuevo",      a.NombreNuevo     ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClaveTraduccion",  a.ClaveTraduccion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ValorAnterior",    a.ValorAnterior   ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ValorNuevo",       a.ValorNuevo      ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        public List<AuditoriaIdioma> GetAuditoriaIdioma(int? idIdioma = null)
        {
            var lista = new List<AuditoriaIdioma>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var sql = @"
                    SELECT IdAuditoria, Fecha, UsuarioAccion, Operacion, IdIdioma,
                           NombreAnterior, NombreNuevo, ClaveTraduccion,
                           ValorAnterior, ValorNuevo
                    FROM AuditoriaIdioma";
                if (idIdioma.HasValue) sql += " WHERE IdIdioma = @IdIdioma";
                sql += " ORDER BY Fecha DESC";

                var cmd = new SqlCommand(sql, con);
                if (idIdioma.HasValue)
                    cmd.Parameters.AddWithValue("@IdIdioma", idIdioma.Value);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lista.Add(new AuditoriaIdioma
                    {
                        IdAuditoria     = rdr.GetInt32(0),
                        Fecha           = rdr.GetDateTime(1),
                        UsuarioAccion   = rdr.GetString(2),
                        Operacion       = rdr.GetString(3),
                        IdIdioma        = rdr.GetInt32(4),
                        NombreAnterior  = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                        NombreNuevo     = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                        ClaveTraduccion = rdr.IsDBNull(7) ? null : rdr.GetString(7),
                        ValorAnterior   = rdr.IsDBNull(8) ? null : rdr.GetString(8),
                        ValorNuevo      = rdr.IsDBNull(9) ? null : rdr.GetString(9)
                    });
                }
            }
            return lista;
        }
    }
}
