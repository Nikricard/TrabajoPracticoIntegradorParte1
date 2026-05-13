using System.Collections.Generic;

namespace BE
{
    // Componente: interfaz común para permisos atómicos y compuestos
    public interface IPermiso
    {
        string Codigo { get; }
        string Nombre { get; }
        bool TienePermiso(string codigo);
    }

    //Permiso atómico --> Leaf
    public class PermisoAtomico : IPermiso
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        // Un permiso atómico coincide solo si el código es exactamente el mismo.

        public bool TienePermiso(string codigo)
            => Codigo == codigo;
    }

    //Composite: permiso compuesto
    public class PermisoCompuesto : IPermiso
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public List<IPermiso> Hijos { get; set; } = new List<IPermiso>();

        public void Agregar(IPermiso permiso) => Hijos.Add(permiso);
        public void Quitar(IPermiso permiso) => Hijos.Remove(permiso);

        // Recorre recursivamente todos los hijos buscando el código.
        // Un compuesto tiene el permiso si al menos uno de sus hijos lo tiene.

        public bool TienePermiso(string codigo)
        {
            if (Codigo == codigo) return true;
            foreach (IPermiso hijo in Hijos)
                if (hijo.TienePermiso(codigo))
                    return true;
            return false;
        }
    }

    //Perfil
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public IPermiso Permiso { get; set; }  // raíz del árbol composite

        // Delega la verificación al árbol de permisos.

        public bool TienePermiso(string codigo)
            => Permiso?.TienePermiso(codigo) ?? false;
    }
}