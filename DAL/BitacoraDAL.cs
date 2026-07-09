using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class BitacoraDAL
    {
        private readonly string cs =
            "Server=DESKTOP-FD6Q6GG\\SQLEXPRESS;Database=Usuarios;Integrated Security=True";


        // Registro de eventos (bitácora) — sin cambios respecto de la versión anterior
        public void RegistrarEvento(RegistroBitacora r)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO Bitacora
                        (Fecha, Usuario, Actividad, TipoEvento,
                         Descripcion, Entidad, ValorAnterior, ValorNuevo)
                    VALUES
                        (@Fecha, @Usuario, @Actividad, @Tipo,
                         @Desc, @Entidad, @Ant, @Nue)", con);
                cmd.Parameters.AddWithValue("@Fecha",    r.Fecha);
                cmd.Parameters.AddWithValue("@Usuario",  r.Usuario ?? "");
                cmd.Parameters.AddWithValue("@Actividad", r.Actividad ?? "");
                cmd.Parameters.AddWithValue("@Tipo",     r.TipoEvento.ToString());
                cmd.Parameters.AddWithValue("@Desc",     (object)r.Descripcion   ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Entidad",  (object)r.Entidad       ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ant",      (object)r.ValorAnterior ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nue",      (object)r.ValorNuevo    ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }


        // Auditoría de usuarios — ahora persiste SnapshotJson
        public void RegistrarAuditoriaUsuario(AuditoriaUsuario a)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO AuditoriaUsuario
                        (Fecha, UsuarioAccion, Operacion, IdUsuario,
                         NombreAnterior, NombreNuevo, SnapshotJson)
                    VALUES
                        (@Fecha, @UsAcc, @Op, @IdUs,
                         @Ant, @Nue, @Snap)", con);
                cmd.Parameters.AddWithValue("@Fecha",  a.Fecha);
                cmd.Parameters.AddWithValue("@UsAcc",  a.UsuarioAccion ?? "");
                cmd.Parameters.AddWithValue("@Op",     a.Operacion ?? "");
                cmd.Parameters.AddWithValue("@IdUs",   a.IdUsuario);
                cmd.Parameters.AddWithValue("@Ant",    (object)a.NombreAnterior ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nue",    (object)a.NombreNuevo    ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Snap",   (object)a.SnapshotJson   ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }


        // Auditoría de idiomas — sin cambios
        public void RegistrarAuditoriaIdioma(AuditoriaIdioma a)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO AuditoriaIdioma
                        (Fecha, UsuarioAccion, Operacion, IdIdioma,
                         NombreAnterior, NombreNuevo,
                         ClaveTraduccion, ValorAnterior, ValorNuevo)
                    VALUES
                        (@Fecha, @UsAcc, @Op, @IdId,
                         @NAnt, @NNue,
                         @Clave, @VAnt, @VNue)", con);
                cmd.Parameters.AddWithValue("@Fecha",  a.Fecha);
                cmd.Parameters.AddWithValue("@UsAcc",  a.UsuarioAccion ?? "");
                cmd.Parameters.AddWithValue("@Op",     a.Operacion ?? "");
                cmd.Parameters.AddWithValue("@IdId",   a.IdIdioma);
                cmd.Parameters.AddWithValue("@NAnt",   (object)a.NombreAnterior  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NNue",   (object)a.NombreNuevo     ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Clave",  (object)a.ClaveTraduccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@VAnt",   (object)a.ValorAnterior   ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@VNue",   (object)a.ValorNuevo      ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }


        // Consultas de bitácora — sin cambios respecto de la versión anterior
        public List<RegistroBitacora> Buscar(
            DateTime? desde, DateTime? hasta,
            string usuario, string actividad, string tipoEvento)
        {
            var lista = new List<RegistroBitacora>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    SELECT IdBitacora, Fecha, Usuario, Actividad, TipoEvento,
                           Descripcion, Entidad, ValorAnterior, ValorNuevo
                    FROM Bitacora
                    WHERE (@Desde IS NULL OR Fecha >= @Desde)
                      AND (@Hasta IS NULL OR Fecha <  DATEADD(DAY, 1, @Hasta))
                      AND (@Us    IS NULL OR Usuario   = @Us)
                      AND (@Act   IS NULL OR Actividad LIKE '%' + @Act + '%')
                      AND (@Tipo  IS NULL OR TipoEvento = @Tipo)
                    ORDER BY Fecha DESC", con);
                cmd.Parameters.AddWithValue("@Desde", (object)desde ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Hasta", (object)hasta ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Us",    string.IsNullOrEmpty(usuario)   ? (object)DBNull.Value : usuario);
                cmd.Parameters.AddWithValue("@Act",   string.IsNullOrEmpty(actividad) ? (object)DBNull.Value : actividad);
                cmd.Parameters.AddWithValue("@Tipo",  string.IsNullOrEmpty(tipoEvento)? (object)DBNull.Value : tipoEvento);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Enum.TryParse(rdr.GetString(4), out TipoEvento tipo);
                    lista.Add(new RegistroBitacora
                    {
                        IdBitacora    = rdr.GetInt32(0),
                        Fecha         = rdr.GetDateTime(1),
                        Usuario       = rdr.GetString(2),
                        Actividad     = rdr.GetString(3),
                        TipoEvento    = tipo,
                        Descripcion   = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                        Entidad       = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                        ValorAnterior = rdr.IsDBNull(7) ? null : rdr.GetString(7),
                        ValorNuevo    = rdr.IsDBNull(8) ? null : rdr.GetString(8)
                    });
                }
            }
            return lista;
        }


        // Auditoría de usuarios — trae también el SnapshotJson
        public List<AuditoriaUsuario> GetAuditoriaUsuario(int? idUsuario = null)
        {
            var lista = new List<AuditoriaUsuario>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    SELECT IdAuditoria, Fecha, UsuarioAccion, Operacion, IdUsuario,
                           NombreAnterior, NombreNuevo, SnapshotJson
                    FROM AuditoriaUsuario
                    WHERE (@Id IS NULL OR IdUsuario = @Id)
                    ORDER BY Fecha DESC", con);
                cmd.Parameters.AddWithValue("@Id", (object)idUsuario ?? DBNull.Value);
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
                        NombreNuevo    = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                        SnapshotJson   = rdr.IsDBNull(7) ? null : rdr.GetString(7)
                    });
                }
            }
            return lista;
        }


        public List<AuditoriaIdioma> GetAuditoriaIdioma(int? idIdioma = null)
        {
            var lista = new List<AuditoriaIdioma>();
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    SELECT IdAuditoria, Fecha, UsuarioAccion, Operacion, IdIdioma,
                           NombreAnterior, NombreNuevo,
                           ClaveTraduccion, ValorAnterior, ValorNuevo
                    FROM AuditoriaIdioma
                    WHERE (@Id IS NULL OR IdIdioma = @Id)
                    ORDER BY Fecha DESC", con);
                cmd.Parameters.AddWithValue("@Id", (object)idIdioma ?? DBNull.Value);
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
