using System;
using System.Security.Cryptography;
using System.Text;

namespace SE
{
    // Servicios de hashing
    // Se usa para:
    //   - Hash de contraseñas (HashContrasena).
    //   - Cálculo de dígitos verificadores DVH y DVV (HashDeCampos).
    public static class Encriptado
    {
        public static string HashContrasena(string texto)
        {
            if (texto == null) texto = "";
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // Hash combinado de una lista de campos.
        // Concatena los campos con un separador "|" (que no puede aparecer en
        // los datos normales) para evitar colisiones — por ejemplo, que
        // (Nombre="ab", Apellido="c") y (Nombre="a", Apellido="bc") produzcan
        // el mismo hash.
        // Se usa para DVH (por fila) y DVV (por tabla).
        public static string HashDeCampos(params string[] campos)
        {
            // Si no hay campos, se devuelve el hash de la cadena vacía.
            if (campos == null || campos.Length == 0)
                return HashContrasena("");

            var sb = new StringBuilder();
            // Concatenar los campos con un separador "|"
            for (int i = 0; i < campos.Length; i++)
            {
                // Agregar el separador solo entre campos, no al final.
                if (i > 0) sb.Append('|');
                // Si el campo es null, se trata como cadena vacía.
                sb.Append(campos[i] ?? "");
            }
            // Devolver el hash de la cadena concatenada.
            return HashContrasena(sb.ToString());
        }
    }
}
