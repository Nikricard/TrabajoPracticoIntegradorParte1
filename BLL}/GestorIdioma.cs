using ABS;
using BE;
using BLL;
using DAL;
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

            try
            {
                int idIdioma = _dal.AddIdioma(nombre, defecto);
                _dal.AsignarTodasLasPalabrasAIdioma(idIdioma);

                // Registro en bitácora
                BitacoraBLL.Instancia.RegistrarAddIdioma(idIdioma, nombre);

                CargarIdiomas();
                return IdiomasDisponibles.Find(i => i.IdIdioma == idIdioma);
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("ADD_IDIOMA", ex);
                throw;
            }

        }

        public void EliminarIdioma(int idIdioma)
        {
            try
            {
                // Buscamos el nombre antes de borrar para la bitácora
                Idioma idioma = IdiomasDisponibles.Find(i => i.IdIdioma == idIdioma);
                string nombre = idioma?.Nombre ?? $"Id {idIdioma}";

                _dal.DeleteIdioma(idIdioma);

                // Registro en bitácora
                BitacoraBLL.Instancia.RegistrarDeleteIdioma(idIdioma, nombre);

                CargarIdiomas();

                if (IdiomaActivo != null && IdiomaActivo.IdIdioma == idIdioma)
                {
                    IdiomaActivo = IdiomasDisponibles.Count > 0 ? IdiomasDisponibles[0] : null;
                    if (IdiomaActivo != null) Notificar();
                }
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("DELETE_IDIOMA", ex);
                throw;
            }

        }


        //  Traducciones 

        public void GuardarTraduccion(int idIdioma, string clave, string valor)
        {
            try
            {
                // Capturamos el valor anterior para la auditoría
                Idioma idioma = IdiomasDisponibles.Find(i => i.IdIdioma == idIdioma);
                string valorAnterior = null;
                idioma?.Traducciones.TryGetValue(clave, out valorAnterior);

                int idPalabra = _dal.AddPalabra(clave);
                _dal.SaveTraduccion(idIdioma, idPalabra, valor);

                //Registro en bitácora con valor anterior y nuevo
                BitacoraBLL.Instancia.RegistrarTraduccion(
                    idIdioma, clave, valorAnterior, valor);

                CargarIdiomas();
                if (IdiomaActivo?.IdIdioma == idIdioma) Notificar();
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("TRADUCCION", ex);
                throw;
            }

        }


        //ABM de Tag 

        public List<Palabra> GetPalabras() => _dal.GetAllPalabras();

        // ahora al crear una tag se la asigna automaticamente a todos los idiomas
        // existentes con traducción vacía.
        public void AgregarPalabra(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                throw new Exception("La tag no puede estar vacía.");

            try
            {
                int idPalabra = _dal.AddPalabra(texto);
                _dal.AsignarPalabraATodosLosIdiomas(idPalabra);

                //Registro en bitácora
                BitacoraBLL.Instancia.RegistrarTraduccion(0, texto, null,
                    $"Tag '{texto}' creada y asignada a todos los idiomas");

                CargarIdiomas();
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("ADD_TAG", ex);
                throw;
            }

        }

        public void EliminarPalabra(int idPalabra)
        {
            try
            {
                _dal.DeletePalabra(idPalabra);

                //Registro en bitácora
                BitacoraBLL.Instancia.RegistrarTraduccion(0, $"Id {idPalabra}",
                    "Tag existente", "Tag eliminada de todos los idiomas");

                CargarIdiomas();
                if (IdiomaActivo != null) Notificar();
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("DELETE_TAG", ex);
                throw;
            }

        }


    }
}
