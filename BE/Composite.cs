using System.Collections.Generic;

namespace BE
{
    // Clase abstracta común para permisos atómicos y compuestos
    public abstract class PermisoBase
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public abstract bool TienePermiso(string codigo);
        //declaracion de los metodos en la clase abstracta base
        public abstract void Agregar(PermisoBase permiso);  // Agrega un permiso hijo al compuesto
        public abstract void Quitar(PermisoBase permiso);  // Elimina un permiso hijo del compuesto

    }

    // Permiso atómico --> Leaf
    public class PermisoAtomico : PermisoBase
    {
        // Un permiso atómico coincide solo si el código es exactamente el mismo.
        public override bool TienePermiso(string codigo)
            => Codigo == codigo;

        //Implementacion de los metodos declarados en la clase asbtracta base,
        //aunque no los usamos porque las hojas no tienen hijos 
        public override void Agregar(PermisoBase permiso) { }  
        public override void Quitar(PermisoBase permiso) { }
    }

    // Composite: permiso compuesto
    public class PermisoCompuesto : PermisoBase
    {
        public List<PermisoBase> Hijos { get; set; } = new List<PermisoBase>();   // lista de permisos hijos

        public override void Agregar(PermisoBase permiso) => Hijos.Add(permiso);    // Agrega un permiso hijo al compuesto
        public override void Quitar(PermisoBase permiso) => Hijos.Remove(permiso);  // Elimina un permiso hijo del compuesto

        // Recorre recursivamente todos los hijos buscando el código.
        // Un compuesto tiene el permiso si al menos uno de sus hijos lo tiene.
        public override bool TienePermiso(string codigo) // Verifica si el permiso compuesto o alguno de sus hijos tiene el código dado
        {
            if (Codigo == codigo) return true;
            foreach (PermisoBase hijo in Hijos)
                if (hijo.TienePermiso(codigo))
                    return true;
            return false;
        }
    }

    // Perfil
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public PermisoBase Permiso { get; set; }  // raíz del árbol composite

        // Delega la verificación al árbol de permisos.
        public bool TienePermiso(string codigo)
            => Permiso?.TienePermiso(codigo) ?? false;
    }
}