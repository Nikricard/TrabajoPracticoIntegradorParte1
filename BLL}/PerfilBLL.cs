using ABS;
using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private PerfilBLL()
        {
            _dal = new PerfilDAL();
        }

        private readonly PerfilDAL _dal;
        public Perfil PerfilActivo { get; private set; }


        // Observer de cambios de conjuntos
        private readonly List<IObservadorConjuntos> _observadoresConjuntos
            = new List<IObservadorConjuntos>();

        public void SuscribirConjuntos(IObservadorConjuntos obs)
        {
            if (!_observadoresConjuntos.Contains(obs))
                _observadoresConjuntos.Add(obs);
        }

        public void DesuscribirConjuntos(IObservadorConjuntos obs)
            => _observadoresConjuntos.Remove(obs);

        private void NotificarConjuntos()
        {
            foreach (var obs in _observadoresConjuntos.ToList())
                obs.ActualizarConjuntos();
        }


        // Códigos de permisos del sistema (constantes fuertemente tipadas)
        public static class Permisos
        {
            // Atómicos
            public const string CrearUsuario       = "USR001";
            public const string ModificarUsuario   = "USR002";
            public const string EliminarUsuario    = "USR003";
            public const string ListarUsuarios     = "USR004";
            public const string AgregarIdioma      = "IDM001";
            public const string EliminarIdioma     = "IDM002";
            public const string AgregarTraduccion  = "IDM003";
            public const string AgregarTag         = "TAG001";
            public const string EliminarTag        = "TAG002";
            // Alias en plural para compatibilidad con formularios existentes
            public const string AgregarTags        = "TAG001";
            public const string EliminarTags       = "TAG002";
            public const string VerBitacora        = "BIT001";
            public const string GestionarPerfiles  = "PRF001";
            public const string Backup             = "ADM001";
            public const string RestaurarAuditoria = "ADM002";

            // Compuestos del sistema
            public const string Operador      = "GE010";
            public const string Traductor     = "GE020";
            public const string Auditor       = "GE030";
            public const string Administrador = "GE040";
        }


        // Perfil activo
        public void CargarPerfilDeUsuario(int idUsuario)
        {
            PermisoBase raiz = _dal.GetPermisosDeUsuario(idUsuario);
            PerfilActivo = new Perfil
            {
                Nombre = "Perfil de usuario",
                Permiso = raiz
            };
        }

        public void LimpiarPerfil()
        {
            PerfilActivo = null;
        }

        public bool TienePermiso(string codigo)
            => PerfilActivo?.TienePermiso(codigo) ?? false;


        // ABM de conjuntos (sin cambios respecto de la versión anterior)
        public List<PermisoAtomico> GetPermisosAtomicos()
            => _dal.GetPermisosAtomicos();

        public List<PermisoBase> GetConjuntos()
            => _dal.GetConjuntos();

        public bool EsProtegido(string codigo)
        {
            return codigo == Permisos.Operador
                || codigo == Permisos.Traductor
                || codigo == Permisos.Auditor
                || codigo == Permisos.Administrador;
        }

        public void CrearConjunto(string nombre, List<string> codigosHijos)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El conjunto debe tener un nombre.");
            if (codigosHijos == null || codigosHijos.Count == 0)
                throw new Exception("Debe seleccionar al menos un permiso.");

            _dal.CrearConjunto(nombre.Trim(), codigosHijos);
            BitacoraBLL.Instancia.RegistrarCreacionConjunto(
                nombre.Trim(),
                string.Join(", ", codigosHijos));
            NotificarConjuntos();
        }

        public void ActualizarConjunto(string codigo, string nombre, List<string> codigosHijos)
        {
            if (EsProtegido(codigo))
                throw new Exception("Los perfiles del sistema no pueden modificarse.");
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El conjunto debe tener un nombre.");
            if (codigosHijos == null || codigosHijos.Count == 0)
                throw new Exception("Debe seleccionar al menos un permiso.");

            _dal.ActualizarConjunto(codigo, nombre.Trim(), codigosHijos);
            NotificarConjuntos();
        }

        public void EliminarConjunto(string codigo, string nombre)
        {
            if (EsProtegido(codigo))
                throw new Exception("Los perfiles del sistema no pueden eliminarse.");

            _dal.EliminarConjunto(codigo);
            NotificarConjuntos();
        }


        // v2.7 — Asignación de permisos con snapshot y transacción única

        // Reemplaza TODOS los permisos de un usuario en una sola operación:
        //   1) Toma snapshot del estado ACTUAL (nombre + permisos actuales).
        //   2) Reemplaza los permisos en transacción (delete + inserts).
        //   3) Registra UNA sola entrada de auditoría con el snapshot,
        //      para que el rollback pueda volver al estado anterior completo.
        //
        // Este método reemplaza al ciclo "asignar permiso 1, asignar permiso 2..."
        // que había en frmPerfil, que generaba múltiples entradas sin snapshot.
        public void ReemplazarPermisosDeUsuario(
            int idUsuario, string nombreUsuario, List<string> codigosNuevos)
        {
            if (codigosNuevos == null) codigosNuevos = new List<string>();

            var usuarioDAL = new UsuarioDAL();

            // 1) Snapshot del estado ACTUAL
            var snapshot = new UsuarioSnapshot
            {
                Id = idUsuario,
                Nombre = nombreUsuario,
                Permisos = usuarioDAL.GetPermisosCodigos(idUsuario)
            };

            // 2) Reemplazar los permisos usando UsuarioDAL.Restaurar
            //    (misma primitiva transaccional que usa el rollback).
            //    Para no cambiar el nombre del usuario, le pasamos su nombre actual.
            usuarioDAL.Restaurar(idUsuario, nombreUsuario, codigosNuevos);

            // 3) Registrar la asignación en auditoría, con snapshot del estado
            //    ANTERIOR al cambio.
            string descAnterior = snapshot.Permisos.Count == 0
                ? "Sin permisos"
                : string.Join(", ", snapshot.Permisos);
            string descNuevo = codigosNuevos.Count == 0
                ? "Sin permisos"
                : string.Join(", ", codigosNuevos);

            BitacoraBLL.Instancia.RegistrarAsignacionPerfil(
                nombreUsuario, idUsuario, descAnterior, descNuevo, snapshot);

            // 4) Recalcular dígitos verificadores
            IntegridadBLL.Instancia.RecalcularDVHDeUsuario(idUsuario, nombreUsuario);
            IntegridadBLL.Instancia.RecalcularDVVUsuarios();
        }
    }
}
