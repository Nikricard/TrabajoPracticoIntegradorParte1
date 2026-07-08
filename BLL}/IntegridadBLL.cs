using DAL;
using SE;
using System;
using System.Collections.Generic;

namespace BLL
{
    // BLL para la verificación de integridad de la tabla Usuarios.
    // Implementa el patrón DVH + DVV:
    //   - DVH: hash de los campos de cada fila. Detecta cambios internos.
    //   - DVV: hash combinado de TODOS los DVH. Detecta altas/bajas/reordenamientos.
    public class IntegridadBLL
    {
        // Singleton
        private static IntegridadBLL _instancia;
        public static IntegridadBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new IntegridadBLL();
                return _instancia;
            }
        }

        private IntegridadBLL() { _dal = new IntegridadDAL(); }
        private readonly IntegridadDAL _dal;

        // Estado de la última verificación.
        public bool IntegridadOk { get; private set; } = true;
        public string UltimoMensaje { get; private set; } = "";
        public List<string> UltimasInconsistencias { get; private set; } = new List<string>();

        // Indica si la tabla Usuarios necesita inicialización de DVH/DVV.
        public bool NecesitaInicializacion()
            => _dal.ContarUsuariosSinDVH() > 0; // Si hay filas con DVH nulo, se necesita inicialización.

        public string CalcularDVH(int id, string nombre, string contrasena)
            => Encriptado.HashDeCampos(id.ToString(), nombre, contrasena ?? "");

        // Recalcula el DVH de un usuario específico leyendo los datos actuales de la base

        // Estrategia de búsqueda:
        //   1) Si el Id es válido (> 0), busca por Id.
        //   2) Si no encuentra (o Id = 0), busca por nombre.
        public void RecalcularDVHDeUsuario(int idUsuario, string nombre = null)
        {
            (int Id, string Nombre, string Contrasena)? datos = null;

            // Intento 1: por Id
            if (idUsuario > 0)
                datos = _dal.GetUsuarioParaHash(idUsuario);

            // Intento 2: por nombre
            if (datos == null && !string.IsNullOrEmpty(nombre))
                datos = _dal.GetUsuarioParaHashPorNombre(nombre);

            if (datos == null) return;  // el usuario realmente no existe

            // Recalcula el DVH y lo guarda en la base de datos.
            string dvh = CalcularDVH(datos.Value.Id, datos.Value.Nombre, datos.Value.Contrasena);
            _dal.GuardarDVH(datos.Value.Id, dvh);
        }

        // Recalcula el DVV concatenando los DVH actuales y guarda el resultado.
        public void RecalcularDVVUsuarios()
        {
            // Lee todos los DVH de la tabla Usuarios y calcula el DVV.
            var usuarios = _dal.GetUsuariosParaIntegridad();
            // Se guarda en la tabla DigitoVerificadorTabla.
            var dvhs = new List<string>();
            // Se concatenan todos los DVH y se calcula el hash combinado.
            foreach (var u in usuarios)
                dvhs.Add(u.DVH);

            string dvv = Encriptado.HashDeCampos(dvhs.ToArray());
            _dal.GuardarDVV(dvv);
        }

        public bool VerificarIntegridad()
        {
            try
            {
                var usuarios = _dal.GetUsuariosParaIntegridad();
                var dvhsGuardados = new List<string>();
                UltimasInconsistencias = new List<string>();

                // Verifica cada fila: calcula el DVH y lo compara con el guardado.
                foreach (var u in usuarios)
                {
                    string dvhCalculado = CalcularDVH(u.Id, u.Nombre, u.Contrasena);
                    dvhsGuardados.Add(u.DVH);

                    // Si el DVH guardado no coincide con el calculado, hay inconsistencia.
                    if (!string.IsNullOrEmpty(u.DVH) && dvhCalculado != u.DVH)
                        // Se reporta la inconsistencia con el nombre y el Id del usuario.
                        UltimasInconsistencias.Add(
                            $"Usuario '{u.Nombre}' (Id {u.Id}): DVH no coincide.");
                }

                string dvvCalculado = Encriptado.HashDeCampos(dvhsGuardados.ToArray());
                string dvvGuardado  = _dal.GetDVV();

                if (!string.IsNullOrEmpty(dvvGuardado) && dvvCalculado != dvvGuardado)
                    // Si el DVV guardado no coincide con el calculado, hay inconsistencia.
                    UltimasInconsistencias.Add(
                        "DVV de la tabla Usuarios no coincide: " +
                        "alguien agregó, eliminó o reordenó filas desde afuera.");

                if (UltimasInconsistencias.Count == 0)
                {
                    IntegridadOk = true;
                    UltimoMensaje = "Integridad verificada correctamente.";
                    return true;
                }


                IntegridadOk = false;
                // Se reportan todas las inconsistencias encontradas.
                UltimoMensaje = "Se detectaron inconsistencias en la tabla Usuarios:\n\n" +
                                string.Join("\n", UltimasInconsistencias);

                // Registrar el error en la bitácora para que quede constancia de la verificación fallida.
                BitacoraBLL.Instancia.RegistrarError(
                    "Verificación de integridad",
                    new Exception(UltimoMensaje));

                return false;
            }
            catch (Exception ex)
            {
                IntegridadOk = false;
                UltimoMensaje = $"Error al verificar integridad: {ex.Message}";
                return false;
            }
        }

        // Recalcula todos los DVH y el DVV de la tabla Usuarios.
        public void RecalcularTodo()
        {
            try
            {
                // Recalcula el DVH de cada usuario y lo guarda en la base de datos.
                var usuarios = _dal.GetUsuariosParaIntegridad();

                foreach (var u in usuarios)
                {
                    string dvh = CalcularDVH(u.Id, u.Nombre, u.Contrasena);
                    _dal.GuardarDVH(u.Id, dvh);
                }

                RecalcularDVVUsuarios();

                IntegridadOk = true;
                UltimoMensaje = "Dígitos verificadores recalculados correctamente.";
                UltimasInconsistencias = new List<string>();
            }
            catch (Exception ex)
            {
                IntegridadOk = false;
                UltimoMensaje = $"Error al recalcular: {ex.Message}";
                throw;
            }
        }
    }
}
