using System;

namespace BE
{
    // Registro de cambio sobre un usuario. Guarda quién, cuándo, qué operación
    // y — desde la v2.7 — un snapshot JSON del estado ANTERIOR al cambio.
    // El snapshot permite el rollback selectivo desde la grilla de auditoría.
    public class AuditoriaUsuario
    {
        public int IdAuditoria { get; set; }
        public DateTime Fecha { get; set; }
        public string UsuarioAccion { get; set; }
        public string Operacion { get; set; }
        public int IdUsuario { get; set; }
        public string NombreAnterior { get; set; }
        public string NombreNuevo { get; set; }

        // Snapshot serializado (JSON) del estado anterior al cambio.
        // NULL en registros históricos previos a la v2.7 y en las
        // operaciones donde no hay estado anterior (Alta).
        public string SnapshotJson { get; set; }
    }
}
