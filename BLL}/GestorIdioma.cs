using BE;
using DAL;
using ABS;
using System;
using System.Collections.Generic;

namespace BLL_
{
    //Gestor Idioma
    public class GestorIdioma
    {
        // Singleton 
        private static GestorIdioma _instancia;
        public static GestorIdioma Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new GestorIdioma();
                return _instancia;
            }
        }

        private GestorIdioma()
        {
            _dal = new IdiomaDAL();
            CargarIdiomas();
        }

        private readonly IdiomaDAL _dal;
        private readonly List<IObservadorIdioma> _observadores
            = new List<IObservadorIdioma>();

        public Idioma IdiomaActivo { get; private set; }
        public List<Idioma> IdiomasDisponibles { get; private set; }



        // Observer 
        public void Suscribir(IObservadorIdioma obs)
        {
            if (!_observadores.Contains(obs))
                _observadores.Add(obs);
        }

        public void Desuscribir(IObservadorIdioma obs)
        {
            _observadores.Remove(obs);
        }

        private void Notificar()
        {
            foreach (var obs in _observadores)
                obs.ActualizarIdioma(IdiomaActivo);
        }


        //carga de los idiomas desde la base de datos
        public void CargarIdiomas()
        {
            IdiomasDisponibles = _dal.GetAllIdiomas();

            if (IdiomaActivo == null)
            {
                IdiomaActivo = IdiomasDisponibles.Find(i => i.Defecto);
                if (IdiomaActivo == null && IdiomasDisponibles.Count > 0)
                    IdiomaActivo = IdiomasDisponibles[0];
            }
            else
            {
                // Refrescar el idioma activo con los datos recién cargados
                IdiomaActivo = IdiomasDisponibles.Find(i => i.IdIdioma == IdiomaActivo.IdIdioma)
                               ?? IdiomaActivo;
            }
        }

        public void CambiarIdioma(Idioma idioma)
        {
            IdiomaActivo = idioma;
            Notificar();
        }


        //ABM de Idiomas
        // Ahora cuando se crea un idioma se le asigna automaticamente todas las tags
        // existentes con traducción vacía, listas para completar desde form
        public Idioma AgregarIdioma(string nombre, bool defecto)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El idioma debe tener un nombre.");

            int idIdioma = _dal.AddIdioma(nombre, defecto);

            // Asignación automática de todas las tags
            _dal.AsignarTodasLasPalabrasAIdioma(idIdioma);

            CargarIdiomas();
            return IdiomasDisponibles.Find(i => i.IdIdioma == idIdioma);
        }

        public void EliminarIdioma(int idIdioma)
        {
            _dal.DeleteIdioma(idIdioma);
            CargarIdiomas();

            if (IdiomaActivo != null && IdiomaActivo.IdIdioma == idIdioma)
            {
                IdiomaActivo = IdiomasDisponibles.Count > 0 ? IdiomasDisponibles[0] : null;
                if (IdiomaActivo != null) Notificar();
            }
        }


        //  Traducciones 

        public void GuardarTraduccion(int idIdioma, string clave, string valor)
        {
            int idPalabra = _dal.AddPalabra(clave); // devuelve la existente si ya está
            _dal.SaveTraduccion(idIdioma, idPalabra, valor);
            CargarIdiomas();
            if (IdiomaActivo?.IdIdioma == idIdioma) Notificar();
        }


        //ABM de Tag 

        public List<Palabra> GetPalabras() => _dal.GetAllPalabras();

        // ahora al crear una tag se la asigna automaticamente a todos los idiomas
        // existentes con traducción vacía.
        public void AgregarPalabra(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                throw new Exception("La tag no puede estar vacía.");

            int idPalabra = _dal.AddPalabra(texto);

            // Asignación automática a todos los idiomas
            _dal.AsignarPalabraATodosLosIdiomas(idPalabra);

            CargarIdiomas(); // refresca diccionarios en memoria
        }

        public void EliminarPalabra(int idPalabra)
        {
            _dal.DeletePalabra(idPalabra);
            CargarIdiomas();
            // Si el idioma activo cambió su set de claves, notificar
            if (IdiomaActivo != null) Notificar();
        }


    }
}
