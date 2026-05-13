using BE;
using DAL;
using System.Collections.Generic;

namespace BLL
{
    public class PerfilBLL
    {
        //Singleton
        private static PerfilBLL _instancia;
        public static PerfilBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new PerfilBLL();
                return _instancia;
            }
        }

        private PerfilBLL() { _dal = new PerfilDAL(); } // Constructor privado para controlar la creación de instancias
        private readonly PerfilDAL _dal; // DAL para acceder a datos de perfiles

        // Perfil del usuario activo
        public Perfil PerfilActivo { get; private set; }

        public void CargarPerfilDeUsuario(int idUsuario)
        {
            PerfilActivo = _dal.GetPerfilDeUsuario(idUsuario); //desde la bll le pedimos a la dal que 
        }                                                      // cargue el perfil del usuario activo según su id

        public void LimpiarPerfil()
        {
            PerfilActivo = null;
        }

        // Verificación de permisos 
        public bool TienePermiso(string codigo)         // delega la verificación al perfil activo, que a su vez delega al árbol de permisos
            => PerfilActivo?.TienePermiso(codigo) ?? false;

        // Gestión de perfiles 
        public List<Perfil> GetAllPerfiles() // creamos uan lista que devuelva todos los perfiles de la db
            => _dal.GetAllPerfiles();
 
        // Asigna un perfil a un usuario y registra el otorgamiento
        // en la bitácora con el estado anterior y el nuevo.

        public void AsignarPerfil(int idUsuario, string nombreUsuario,
            int idPerfil, string nombrePerfilNuevo)
        {
            // Obtenemos el perfil actual ANTES de cambiar para registrar el anterior
            Perfil perfilAnterior = _dal.GetPerfilDeUsuario(idUsuario);
            string nombreAnterior = perfilAnterior?.Nombre;

            // Persiste el cambio
            _dal.AsignarPerfil(idUsuario, idPerfil);

            // Registra en bitácora: quién, cuándo, qué cambió
            BitacoraBLL.Instancia.RegistrarAsignacionPerfil(
                nombreUsuarioAfectado: nombreUsuario,
                idUsuarioAfectado: idUsuario,   //enviamos todos los parametros
                perfilAnterior: nombreAnterior,
                perfilNuevo: nombrePerfilNuevo
            );
        }

        // Constantes de permisos
        public static class Permisos
        { //hardcodeamos los codigos de permisos para usarlos, importante que coincidan con la db
            public const string CrearUsuario = "USR001";
            public const string ModificarUsuario = "USR002";
            public const string EliminarUsuario = "USR003";
            public const string ListarUsuarios = "USR004";
            public const string AgregarIdioma = "IDM001";
            public const string EliminarIdioma = "IDM002";
            public const string GestionarTraducciones = "IDM003";
            public const string VerBitacora = "BIT001";
            public const string GestionarPerfiles = "PRF001";
            public const string GestionUsuarios = "GE010";
            public const string GestionIdiomas = "GE020";
            public const string Administrador = "GE030";
            public const string Operador = "GE040";
        }
    }
}