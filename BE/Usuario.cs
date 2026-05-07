namespace BE
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Contraseña { get; set; }


        public bool EsValido(out string mensaje)
        {
            // Valida que el usuario tenga un formato correcto.
            // Verifica que el nombre no sea nulo, vacío ni un número puro.

            if (string.IsNullOrEmpty(Nombre))
            {
                mensaje = "El usuario debe tener un nombre.";
                return false;
            }

            if (int.TryParse(Nombre, out _))
            {
                mensaje = "El nombre de usuario no puede ser un número.";
                return false;
            }

            mensaje = string.Empty;
            return true;
        }


    }
}
