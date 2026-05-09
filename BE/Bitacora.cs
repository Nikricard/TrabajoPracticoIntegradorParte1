using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum TipoEvento
    {
        Exito,
        Error,
        Excepcion

    }
    //Constructores para cada clase de auditoria 
    public class RegistroBitacora
    {
        public int IdBitacora { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Actividad { get; set; }
        public TipoEvento TipoEvento { get; set; }
        public string Descripcion { get; set; }
        public string Entidad { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
    }

    public class AuditoriaUsuario
    {
        public int IdAuditoria { get; set; }
        public DateTime Fecha { get; set; }
        public string UsuarioAccion { get; set; }
        public string Operacion { get; set; }
        public int IdUsuario { get; set; }
        public string NombreAnterior { get; set; }
        public string NombreNuevo { get; set; }
    }

    public class AuditoriaIdioma
    {
        public int IdAuditoria { get; set; }
        public DateTime Fecha { get; set; }
        public string UsuarioAccion { get; set; }
        public string Operacion { get; set; }
        public int IdIdioma { get; set; }
        public string NombreAnterior { get; set; }
        public string NombreNuevo { get; set; }
        public string ClaveTraduccion { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
    }
}
