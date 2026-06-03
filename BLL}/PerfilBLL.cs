using BE;
using DAL;
using System.Collections.Generic;

namespace BLL
{
    public class PerfilBLL
    {
        // Singleton
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

        public Perfil GetPerfilDeUsuario(int idUsuario)
            => _dal.GetPerfilDeUsuario(idUsuario);

        private PerfilBLL() { _dal = new PerfilDAL(); }
        private readonly PerfilDAL _dal;

        // Perfil del usuario activo 
        public Perfil PerfilActivo { get; private set; }

        public void CargarPerfilDeUsuario(int idUsuario)
            => PerfilActivo = _dal.GetPerfilDeUsuario(idUsuario);

        public void LimpiarPerfil()
            => PerfilActivo = null;

        public bool TienePermiso(string codigo)
            => PerfilActivo?.TienePermiso(codigo) ?? false;

        // Gestión de perfiles
        public List<Perfil> GetAllPerfiles()
            => _dal.GetAllPerfiles();

        public void AsignarPerfil(int idUsuario, string nombreUsuario,
            int idPerfil, string nombrePerfilNuevo)
        {
            Perfil perfilAnterior = _dal.GetPerfilDeUsuario(idUsuario);
            string nombreAnterior = perfilAnterior?.Nombre;

            _dal.AsignarPerfil(idUsuario, idPerfil);

            BitacoraBLL.Instancia.RegistrarAsignacionPerfil(
                nombreUsuarioAfectado: nombreUsuario,
                idUsuarioAfectado: idUsuario,
                perfilAnterior: nombreAnterior,
                perfilNuevo: nombrePerfilNuevo
            );
        }

        // ABM de conjuntos de permisos
        //Devuelve los permisos atómicos para seleccionar
        public List<PermisoAtomico> GetPermisosAtomicos()
            => _dal.GetPermisosAtomicos();

        // Crea un conjunto de permisos  --> categoría + perfil asignable 
        public void CrearConjunto(string nombre, List<string> codigos)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El conjunto debe tener un nombre.");
            if (codigos == null || codigos.Count == 0)
                throw new Exception("Seleccione al menos un permiso.");

            try
            {
                _dal.CrearConjunto(nombre, codigos);

                //Registro en bitácora: qué conjunto se creó y con qué permisos
                BitacoraBLL.Instancia.RegistrarCreacionConjunto(
                    nombre, string.Join(", ", codigos));
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("CREAR_CONJUNTO", ex);
                throw;
            }

        }

        // Elimina un conjunto creado
        public void EliminarConjunto(int idPerfil, string nombre, string codigoCompuesto)
        {
            string[] protegidos = { "Administrador", "Operador", "Usuario" };
            if (Array.Exists(protegidos, p => p == nombre))
                throw new Exception(
                    $"El perfil '{nombre}' es del sistema y no puede eliminarse.");

            if (string.IsNullOrEmpty(codigoCompuesto))
                throw new Exception("No se pudo determinar el código del conjunto.");

            _dal.EliminarConjunto(idPerfil, codigoCompuesto);
        }

        // Permisos constantes
        public static class Permisos
        {
            public const string CrearUsuario = "USR001";
            public const string ModificarUsuario = "USR002";
            public const string EliminarUsuario = "USR003";
            public const string ListarUsuarios = "USR004";
            public const string AgregarIdioma = "IDM001";
            public const string EliminarIdioma = "IDM002";
            public const string GestionarTraducciones = "IDM003";
            public const string AgregarTags = "TAG001"; 
            public const string EliminarTags = "TAG002"; 
            public const string VerBitacora = "BIT001";
            public const string GestionarPerfiles = "PRF001";
            public const string GestionUsuarios = "GE010";
            public const string GestionIdiomas = "GE020";
            public const string Administrador = "GE030";
            public const string Operador = "GE040";
        }
    }

}