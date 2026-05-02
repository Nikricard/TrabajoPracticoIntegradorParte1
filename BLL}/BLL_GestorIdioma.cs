// ════════════════════════════════════════════════════════
// BLL_ — GestorIdioma.cs   (reemplaza al anterior)
// Patrón Observer (Subject) + Singleton
// ════════════════════════════════════════════════════════
using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL_
{
    // ── Interfaz Observer ─────────────────────────────────
    // Simple y directa: solo lo necesario para el patrón
    public interface IObservadorIdioma
    {
        void ActualizarIdioma(Idioma idioma);
    }

    // ── GestorIdioma (Subject) ────────────────────────────
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

        /// <summary>Idioma activo en el sistema.</summary>
        public Idioma IdiomaActivo { get; private set; }

        /// <summary>Lista de idiomas disponibles (cargada desde la DB).</summary>
        public List<Idioma> IdiomasDisponibles { get; private set; }

        // ── Observer ──────────────────────────────────────

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

        // ── Idioma ────────────────────────────────────────

        /// <summary>Carga o recarga los idiomas desde la base de datos.</summary>
        public void CargarIdiomas()
        {
            IdiomasDisponibles = _dal.GetAllIdiomas();

            // Si no hay activo, usar el marcado como defecto
            if (IdiomaActivo == null)
                IdiomaActivo = IdiomasDisponibles.Find(i => i.Defecto)
                               ?? (IdiomasDisponibles.Count > 0
                                   ? IdiomasDisponibles[0] : null);
        }

        /// <summary>
        /// Cambia el idioma activo y notifica a todos los formularios registrados.
        /// </summary>
        public void CambiarIdioma(Idioma idioma)
        {
            IdiomaActivo = idioma;
            Notificar();
        }

        /// <summary>Agrega un idioma nuevo y recarga la lista.</summary>
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

        /// <summary>Elimina un idioma y recarga la lista.</summary>
        public void EliminarIdioma(int idIdioma)
        {
            _dal.DeleteIdioma(idIdioma);
            CargarIdiomas();

            // Si se eliminó el activo, cambiar al primero disponible
            if (IdiomaActivo?.IdIdioma == idIdioma)
            {
                IdiomaActivo = IdiomasDisponibles.Count > 0
                    ? IdiomasDisponibles[0] : null;
                if (IdiomaActivo != null) Notificar();
            }
        }

        /// <summary>Guarda o actualiza una traducción individual.</summary>
        public void GuardarTraduccion(int idIdioma, string clave, string valor)
        {
            int idPalabra = _dal.AddPalabra(clave);
            _dal.SaveTraduccion(idIdioma, idPalabra, valor);
            CargarIdiomas(); // refresca el diccionario en memoria
            if (IdiomaActivo?.IdIdioma == idIdioma) Notificar();
        }

        /// <summary>Devuelve todas las palabras (claves) disponibles.</summary>
        public List<Palabra> GetPalabras() => _dal.GetAllPalabras();
    }
}
