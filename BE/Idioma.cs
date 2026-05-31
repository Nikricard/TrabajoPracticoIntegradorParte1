namespace BE
{
    public class Idioma
    {
        public int IdIdioma { get; set; }
        public string Nombre { get; set; }
        public bool Defecto { get; set; }

        public Dictionary<string, string> Traducciones { get; set; }
            = new Dictionary<string, string>();

        // Devuelve la traducción de una clave.
        // Si la clave no existe O su traducción está vacía,
        // devuelve la clave misma para no dejar el control en blanco.
        public string Traducir(string clave)
        {
            if (string.IsNullOrEmpty(clave)) return clave;

            if (Traducciones.TryGetValue(clave, out string valor)
                && !string.IsNullOrEmpty(valor))
                return valor;

            return clave;
        }
    }


    public class Palabra
    {
        public int    IdPalabra { get; set; }
        public string Texto     { get; set; }   // El tag de control, ej: "btnIngresar"
    }
}
