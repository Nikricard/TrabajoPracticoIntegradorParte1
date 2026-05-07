namespace BE
{
    public class Idioma
    {
        public int    IdIdioma { get; set; }
        public string Nombre   { get; set; }
        public bool   Defecto  { get; set; }

        // Diccionario en memoria: Tag del control --> traducción
        public Dictionary<string, string> Traducciones { get; set; }
            = new Dictionary<string, string>();

        // Busca la traducción de una clave.
        public string Traducir(string clave)
        {
            if (string.IsNullOrEmpty(clave)) return clave;
            return Traducciones.TryGetValue(clave, out string valor) ? valor : clave;
        }
        public override string ToString()
        {
            if (Nombre != null)
            {
                return Nombre;
            }
            else
            {
                return "Idioma sin nombre";
            }
        }
    }

    public class Palabra
    {
        public int    IdPalabra { get; set; }
        public string Texto     { get; set; }   // El tag de control, ej: "btnIngresar"
    }
}
