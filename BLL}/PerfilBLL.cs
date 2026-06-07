using BE;
using DAL;
using System;
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

        private PerfilBLL() { _dal = new PerfilDAL(); }
        private readonly PerfilDAL _dal;

        // Permisos del usuario activo 
        // PerfilActivo.Permiso es un compuesto virtual con TODOS
        // los permisos asignados al usuario. TienePermiso lo recorre.
        public Perfil PerfilActivo { get; private set; }

        // Carga todos los permisos del usuario en un compuesto raíz.
        // Se llama desde el Login (mismo nombre que antes, no rompe nada).
        public void CargarPerfilDeUsuario(int idUsuario)
        {
            PermisoCompuesto raiz = _dal.GetPermisosDeUsuario(idUsuario);
            PerfilActivo = new Perfil
            {
                IdPerfil = idUsuario,
                Nombre   = "Permisos del usuario",
                Permiso  = raiz
            };
        }

        public void LimpiarPerfil() => PerfilActivo = null;

        public bool TienePermiso(string codigo)
            => PerfilActivo?.TienePermiso(codigo) ?? false;

        // Asignación de permisos a usuarios 

        // Compuesto con los permisos actuales de un usuario (para el árbol)
        public PermisoCompuesto GetPermisosDeUsuario(int idUsuario)
            => _dal.GetPermisosDeUsuario(idUsuario);

        //Códigos asignados a un usuario (para marcar checkboxes)
        public List<string> GetCodigosDeUsuario(int idUsuario)
            => _dal.GetCodigosDeUsuario(idUsuario);

        // Guarda la lista completa de permisos de un usuario y lo registra.
        public void GuardarPermisosDeUsuario(int idUsuario, string nombreUsuario,
            List<string> codigos)
        {
            // Capturar los permisos actuales ANTES de sobrescribir
            List<string> anteriores = _dal.GetCodigosDeUsuario(idUsuario);
            string textoAnterior = anteriores.Count > 0
                ? string.Join(", ", anteriores)
                : "Sin permisos";

            _dal.GuardarPermisosDeUsuario(idUsuario, codigos);

            string textoNuevo = codigos.Count > 0
                ? string.Join(", ", codigos)
                : "Sin permisos";

            BitacoraBLL.Instancia.RegistrarAsignacionPerfil(
                nombreUsuarioAfectado: nombreUsuario,
                idUsuarioAfectado:     idUsuario,
                perfilAnterior:        textoAnterior,
                perfilNuevo:           textoNuevo
            );
        }

        // ABM de conjuntos

        public List<PermisoAtomico> GetPermisosAtomicos()
            => _dal.GetPermisosAtomicos();

        public List<IPermiso> GetSeleccionablesParaConjunto(string codigoExcluir)
            => _dal.GetSeleccionablesParaConjunto(codigoExcluir);

        public List<IPermiso> GetConjuntos()
            => _dal.GetConjuntos();

        public void CrearConjunto(string nombre, List<string> codigos)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El conjunto debe tener un nombre.");
            if (codigos == null || codigos.Count == 0)
                throw new Exception("Seleccione al menos un permiso.");

            try
            {
                _dal.CrearConjunto(nombre, codigos);
                BitacoraBLL.Instancia.RegistrarCreacionConjunto(
                    nombre, string.Join(", ", codigos));
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("CREAR_CONJUNTO", ex);
                throw;
            }
        }

        public void ActualizarConjunto(string codigo, string nombre, List<string> codigos)
        {
            if (string.IsNullOrEmpty(codigo))
                throw new Exception("Seleccione un conjunto para actualizar.");
            if (EsProtegido(nombre))
                throw new Exception($"El conjunto '{nombre}' es del sistema y no puede modificarse.");
            if (codigos == null || codigos.Count == 0)
                throw new Exception("Seleccione al menos un permiso.");

            _dal.ActualizarConjunto(codigo, nombre, codigos);
        }

        public void EliminarConjunto(string codigo, string nombre)
        {
            if (EsProtegido(nombre))
                throw new Exception($"El conjunto '{nombre}' es del sistema y no puede eliminarse.");
            if (string.IsNullOrEmpty(codigo))
                throw new Exception("No se pudo determinar el código del conjunto.");

            _dal.EliminarConjunto(codigo);
        }

        // Conjuntos del sistema que no se pueden tocar
        private bool EsProtegido(string nombre)
        {
            string[] protegidos =
                { "Administrador", "Operador", "Gestión de usuarios", "Gestión de idiomas" };
            return Array.Exists(protegidos, p => p == nombre);
        }

        // Constantes de permisos
        public static class Permisos
        {
            public const string CrearUsuario          = "USR001";
            public const string ModificarUsuario      = "USR002";
            public const string EliminarUsuario       = "USR003";
            public const string ListarUsuarios        = "USR004";
            public const string AgregarIdioma         = "IDM001";
            public const string EliminarIdioma        = "IDM002";
            public const string GestionarTraducciones = "IDM003";
            public const string AgregarTags           = "TAG001";
            public const string EliminarTags          = "TAG002";
            public const string VerBitacora           = "BIT001";
            public const string GestionarPerfiles     = "PRF001";
            public const string GestionUsuarios       = "GE010";
            public const string GestionIdiomas        = "GE020";
            public const string Administrador         = "GE030";
            public const string Operador              = "GE040";
        }
    }
}
