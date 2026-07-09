using System.Collections.Generic;
using System.Text.Json;

namespace BE
{
    // Snapshot del estado de un Usuario en un momento determinado.
    // Se usa para el control de cambios (auditoría) y el rollback:
    // cada entrada de AuditoriaUsuario guarda un snapshot serializado
    // con el estado ANTERIOR al cambio, para poder restaurarlo después.
    //
    // NOTA: la contraseña NO se incluye porque el enunciado la define
    // como inalterable a lo largo del programa. El rollback restaura
    // solo Nombre y Permisos.
    public class UsuarioSnapshot
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<string> Permisos { get; set; } = new List<string>();

        // Serializa a JSON con formato indentado (más legible en la BD
        // si alguien lo consulta directo desde SSMS).
        public string ToJson()
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(this, opts);
        }

        public static UsuarioSnapshot FromJson(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            return JsonSerializer.Deserialize<UsuarioSnapshot>(json);
        }

        // Descripción amigable para mostrar en el diálogo de confirmación.
        public string DescripcionCorta()
        {
            string permisos = Permisos == null || Permisos.Count == 0
                ? "(sin permisos)"
                : string.Join(", ", Permisos);
            return $"Nombre: '{Nombre}'\nPermisos: {permisos}";
        }
    }
}
