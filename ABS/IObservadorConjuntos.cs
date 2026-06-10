using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS
{
    // Observer para los cambios en los conjuntos de permisos (permisos compuestos).
    // Lo implementan los formularios que muestran la lista de conjuntos y que deben
    // actualizarse cuando otro formulario crea, modifica o elimina un conjunto.
    public interface IObservadorConjuntos
    {
        void ActualizarConjuntos();
    }
}
