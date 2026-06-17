using ABS;
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

        // Observador de conjuntos
        // PerfilBLL avisa a los formularios suscriptos cuando se
        // crea, modifica o elimina un conjunto, para que se refresquen las listas.
        private readonly List<IObservadorConjuntos> _observadores
            = new List<IObservadorConjuntos>();

        //suscribe conjuntos al observador de arriba
        public void SuscribirConjuntos(IObservadorConjuntos obs)
        {
            if (!_observadores.Contains(obs))
                _observadores.Add(obs);
        }

        //desuscribe conjuntos del observador de arriba
        public void DesuscribirConjuntos(IObservadorConjuntos obs)
            => _observadores.Remove(obs);

        // Notifica a los formularios suscriptos que actualicen las listas de conjuntos.
        private void NotificarConjuntos()
        {
            foreach (var obs in _observadores)
                obs.ActualizarConjuntos();
        }

        // Permisos del usuario activo 
        public Perfil PerfilActivo { get; private set; }

        // Carga todos los permisos del usuario en un compuesto raíz.
        // Se llama desde el Login
        public void CargarPerfilDeUsuario(int idUsuario)
        {
            //creamos un perfil con el id del usuario, un nombre genérico y el compuesto raíz que contiene todos los permisos del usuario
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

        // Proceso de asignacion de permisos a usuarios 

        // Compuesto con los permisos actuales de un usuario (para el árbol)
        public PermisoCompuesto GetPermisosDeUsuario(int idUsuario)
            => _dal.GetPermisosDeUsuario(idUsuario);

        //Códigos asignados a un usuario (para marcar checkboxes)
        public List<string> GetCodigosDeUsuario(int idUsuario)
            => _dal.GetCodigosDeUsuario(idUsuario);

        // Guarda la lista completa de permisos de un usuario y lo registra.
        public void GuardarPermisosDeUsuario(int idUsuario, string nombreUsuario, List<string> codigos)
        {
            // Capturar los permisos actuales antes de sobrescribir
            List<string> anteriores = _dal.GetCodigosDeUsuario(idUsuario);
            string textoAnterior = anteriores.Count > 0
                ? string.Join(", ", anteriores)
                : "Sin permisos";
            // Guardar los nuevos permisos
            _dal.GuardarPermisosDeUsuario(idUsuario, codigos);

            string textoNuevo = codigos.Count > 0
                ? string.Join(", ", codigos)
                : "Sin permisos";
            // Registrar el cambio en la bitácora
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

        public List<PermisoBase> GetSeleccionablesParaConjunto(string codigoExcluir)
            => _dal.GetSeleccionablesParaConjunto(codigoExcluir);

        public List<PermisoBase> GetConjuntos()
            => _dal.GetConjuntos();

        public void CrearConjunto(string nombre, List<string> codigos)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El conjunto debe tener un nombre.");
            //verificaciones
            if (codigos == null || codigos.Count == 0)
                throw new Exception("Seleccione al menos un permiso.");

            try
            {
                // El código del conjunto se genera automáticamente en el DAL, por eso no se recibe como parámetro.
                _dal.CrearConjunto(nombre, codigos);
                //registro en bitacora
                BitacoraBLL.Instancia.RegistrarCreacionConjunto(
                    nombre, string.Join(", ", codigos));
                NotificarConjuntos();   // avisa a los formularios suscriptos
            }
            catch (Exception ex)
            {
                //registro del error en bitacora
                BitacoraBLL.Instancia.RegistrarError("CREAR_CONJUNTO", ex);
                throw;
            }
        }

        public void ActualizarConjunto(string codigo, string nombre, List<string> codigos)
        {
            //verificaciones
            if (string.IsNullOrEmpty(codigo))
                throw new Exception("Seleccione un conjunto para actualizar.");
            if (EsProtegido(nombre))
                throw new Exception($"El conjunto '{nombre}' es del sistema y no puede modificarse.");
            if (codigos == null || codigos.Count == 0)
                throw new Exception("Seleccione al menos un permiso.");
            //llamamos a actualizar conjunto del DAL
            _dal.ActualizarConjunto(codigo, nombre, codigos);
            NotificarConjuntos();   // avisa a los formularios suscriptos
        }

        public void EliminarConjunto(string codigo, string nombre)
        {
            //verificaciones
            if (EsProtegido(nombre))
                throw new Exception($"El conjunto '{nombre}' es del sistema y no puede eliminarse.");
            if (string.IsNullOrEmpty(codigo))
                throw new Exception("No se pudo determinar el código del conjunto.");
            //llamamos a eliminar conjunto del DAL
            _dal.EliminarConjunto(codigo);
            NotificarConjuntos();   // avisa a los formularios suscriptos
        }

        // Conjuntos del sistema que no se pueden modificar ni eliminar
        private bool EsProtegido(string nombre)
        {
            //array de nombres protegidos para que no se puedan modificar ni eliminar
            string[] protegidos = { "Administrador", "Operador", "Traductor", "Auditor" };
            //verificamos si el nombre del conjunto está en el array de protegidos
            return Array.Exists(protegidos, p => p == nombre);
        }

        // Constantes de permisos
        public static class Permisos
        {
            // Atómicos
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
            public const string Operador      = "GE010";
            public const string Traductor     = "GE020";
            public const string Auditor       = "GE030";
            public const string Administrador = "GE040";
        }
    }
}