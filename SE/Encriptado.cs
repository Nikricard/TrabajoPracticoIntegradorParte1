using System.Security.Cryptography;
using System.Text;

namespace SE
{
    public class Encriptado
    {
        public static string HashContrasena(string contraseña)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                return Convert.ToHexString(bytes).ToLower(); // devuelve 64 chars hex
            }
        }
    }
}
