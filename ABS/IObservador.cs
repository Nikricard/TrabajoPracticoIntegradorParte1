using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS
{
    //Interfaz Observer
    public interface IObservadorIdioma
    {
        void ActualizarIdioma(Idioma idioma);
    }
}
