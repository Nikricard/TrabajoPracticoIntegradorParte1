// ════════════════════════════════════════════════════════
// BE — Entidades de Idioma
// Archivo: Idioma.cs  (agregar al proyecto BE)
// ════════════════════════════════════════════════════════
namespace BE
{
    public class Idioma
    {
        public int    IdIdioma { get; set; }
        public string Nombre   { get; set; }
        public bool   Defecto  { get; set; }

        // Diccionario en memoria: clave (Tag del control) → traducción
        public Dictionary<string, string> Traducciones { get; set; }
            = new Dictionary<string, string>();

        /// <summary>
        /// Busca la traducción de una clave.
        /// Si no existe devuelve la clave misma para no romper la UI.
        /// </summary>
        public string Traducir(string clave)
        {
            if (string.IsNullOrEmpty(clave)) return clave;
            return Traducciones.TryGetValue(clave, out string valor) ? valor : clave;
        }
        public override string ToString()
        {
            return Nombre ?? "Idioma sin nombre";
        }
    }

    public class Palabra
    {
        public int    IdPalabra { get; set; }
        public string Texto     { get; set; }   // la clave, ej: "btnIngresar"
    }
}
