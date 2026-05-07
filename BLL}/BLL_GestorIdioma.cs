using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL_
{
    //Interfaz Observer
    public interface IObservadorIdioma
    {
        void ActualizarIdioma(Idioma idioma);
    }

    //Gestor Idioma
    public class GestorIdioma
    {
        // Uso Singleton
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

        //El idioma activo en el sistema
        public Idioma IdiomaActivo { get; private set; }

        //lista de idiomas cargados desde la db
        public List<Idioma> IdiomasDisponibles { get; private set; }

        //patron observer

        public void Suscribir(IObservadorIdioma obs)
        {
            if (!_observadores.Contains(obs))
            {
                _observadores.Add(obs);
            }
        }

        public void Desuscribir(IObservadorIdioma obs)
        {
            _observadores.Remove(obs);
        }

        private void Notificar()
        {
            foreach (var obs in _observadores)
            {
                obs.ActualizarIdioma(IdiomaActivo);
            }
        }

        //carga de los idiomas desde la base de datos
        public void CargarIdiomas()
        {
            IdiomasDisponibles = _dal.GetAllIdiomas();

            // Si no hay activo, usar el marcado como defecto
            if (IdiomaActivo == null)
            {
                IdiomaActivo = IdiomasDisponibles.Find(i => i.Defecto);

                if (IdiomaActivo == null)
                {
                    if (IdiomasDisponibles.Count > 0)
                    {
                        IdiomaActivo = IdiomasDisponibles[0];
                    }
                    else
                    {
                        IdiomaActivo = null;
                    }
                }
            }
        }

        // Cambia el idioma activo y notifica a todos los formularios registrados.
        public void CambiarIdioma(Idioma idioma)
        {
            IdiomaActivo = idioma;
            Notificar();
        }

        //Agrega un idioma nuevo y recarga la lista.
        public Idioma AgregarIdioma(string nombre, bool defecto,
            List<(string clave, string valor)> traducciones)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El idioma debe tener un nombre.");

            int idIdioma = _dal.AddIdioma(nombre, defecto);

            foreach (var (clave, valor) in traducciones)
            {
                if (string.IsNullOrEmpty(clave) || string.IsNullOrEmpty(valor)) continue;
                int idPalabra = _dal.AddPalabra(clave);
                _dal.SaveTraduccion(idIdioma, idPalabra, valor);
            }

            CargarIdiomas();
            return IdiomasDisponibles.Find(i => i.IdIdioma == idIdioma);
        }

        //Elimina un idioma y recarga la lista
        public void EliminarIdioma(int idIdioma)
        {
            _dal.DeleteIdioma(idIdioma);
            CargarIdiomas();

            // Si se eliminó el activo, cambiar al primero disponible
            if (IdiomaActivo != null && IdiomaActivo.IdIdioma == idIdioma)
            {
                if (IdiomasDisponibles.Count > 0)
                {
                    IdiomaActivo = IdiomasDisponibles[0];
                }
                else
                {
                    IdiomaActivo = null;
                }

                if (IdiomaActivo != null)
                {
                    Notificar();
                }
            }
        }

        //Guarda o actualiza una traducción individual
        public void GuardarTraduccion(int idIdioma, string clave, string valor)
        {
            int idPalabra = _dal.AddPalabra(clave);
            _dal.SaveTraduccion(idIdioma, idPalabra, valor);
            CargarIdiomas(); // refresca el diccionario en memoria
            if (IdiomaActivo?.IdIdioma == idIdioma)
            {
                Notificar();
            }
        }

        //Devuelve todas las palabras disponibles
        public List<Palabra> GetPalabras() => _dal.GetAllPalabras();
    }
}
