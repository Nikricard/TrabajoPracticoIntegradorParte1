using BE;
using DAL;
using System.Text;
using System.Security.Cryptography;
using BLL_;
using SE;
namespace BLL
{
    public class UsuarioBLL
    {
        //Empieza singleton
        private static UsuarioBLL? _instancia = null;

        public static UsuarioBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new UsuarioBLL();
                return _instancia;
            }
        }
        //termina singleton

        public Usuario? UsuarioActivo { get; private set; } = null;

        private readonly UsuarioDAL usuarioDAL;

        public UsuarioBLL()
        {
            usuarioDAL = new UsuarioDAL();
        }

        // Construye un snapshot del estado ACTUAL del usuario, releyendo
        // nombre y permisos desde la base. Se usa ANTES de cada operación
        // para poder registrarlo en la auditoría.
        private UsuarioSnapshot TomarSnapshot(int idUsuario, string nombre)
        {
            return new UsuarioSnapshot
            {
                Id = idUsuario,
                Nombre = nombre,
                Permisos = usuarioDAL.GetPermisosCodigos(idUsuario)
            };
        }

        public Usuario Add(Usuario usuario, string contraseñaPlana)
        {
            try
            {
                if (usuario == null)
                    throw new Exception("El usuario no puede ser nulo");
                if (!usuario.EsValido(out string mensaje))
                    throw new Exception(mensaje);
                if (string.IsNullOrEmpty(contraseñaPlana))
                    throw new Exception("El usuario debe tener una contraseña");

                usuario.Contraseña = Encriptado.HashContrasena(contraseñaPlana);
                usuario = usuarioDAL.Add(usuario);

                // Alta: no hay estado anterior que preservar como snapshot
                BitacoraBLL.Instancia.RegistrarAddUsuario(usuario);

                IntegridadBLL.Instancia.RecalcularDVHDeUsuario(usuario.Id, usuario.Nombre);
                IntegridadBLL.Instancia.RecalcularDVVUsuarios();

                return usuario;
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("Alta de usuario", ex);
                throw;
            }
        }

        public Usuario Modify(Usuario anterior, Usuario nuevo)
        {
            try
            {
                if (nuevo == null)
                    throw new Exception("El usuario no puede ser nulo");
                if (!nuevo.EsValido(out string mensaje))
                    throw new Exception(mensaje);

                // Toma snapshot ANTES de aplicar el cambio
                var snapshot = TomarSnapshot(anterior.Id, anterior.Nombre);

                nuevo = usuarioDAL.Modify(nuevo);
                BitacoraBLL.Instancia.RegistrarModifyUsuario(anterior, nuevo, snapshot);

                IntegridadBLL.Instancia.RecalcularDVHDeUsuario(nuevo.Id, nuevo.Nombre);
                IntegridadBLL.Instancia.RecalcularDVVUsuarios();

                return nuevo;
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("Modificación de usuario", ex);
                throw;
            }
        }

        public Usuario Login(string nombre, string contraseñaPlana)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseñaPlana))
                    throw new Exception("Nombre y contraseña son obligatorios");

                string hash = Encriptado.HashContrasena(contraseñaPlana);
                Usuario? usuario = usuarioDAL.Login(nombre, hash);
                if (usuario == null)
                    throw new Exception("Usuario o contraseña incorrectos");

                UsuarioActivo = usuario;
                PerfilBLL.Instancia.CargarPerfilDeUsuario(usuario.Id);
                BitacoraBLL.Instancia.RegistrarLogin(nombre);
                return UsuarioActivo;
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("Inicio de sesión", ex);
                throw;
            }
        }

        public Usuario Delete(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    throw new Exception("El usuario no puede ser nulo");
                if (!usuario.EsValido(out string mensaje))
                    throw new Exception(mensaje);

                // Toma snapshot ANTES de borrar
                var snapshot = TomarSnapshot(usuario.Id, usuario.Nombre);

                usuario = usuarioDAL.Delete(usuario);
                BitacoraBLL.Instancia.RegistrarDeleteUsuario(usuario, snapshot);

                IntegridadBLL.Instancia.RecalcularDVVUsuarios();
                return usuario;
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("Baja de usuario", ex);
                throw;
            }
        }

        public List<Usuario> GetAll()
        {
            try
            {
                return usuarioDAL.GetAll();
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("Listado de usuarios", ex);
                throw;
            }
        }


        // Restaura un usuario a un estado anterior guardado en un snapshot
        // de auditoría. Aplica Nombre + Permisos (la contraseña no se toca).
        // El rollback en sí queda registrado como una nueva entrada de
        // auditoría, con snapshot del estado ANTES del rollback, para
        // poder deshacerlo si hace falta.
        public void Rollback(UsuarioSnapshot destino, System.DateTime fechaSnapshotDestino)
        {
            try
            {
                if (destino == null)
                    throw new Exception("El snapshot es inválido.");

                if (!usuarioDAL.Existe(destino.Id))
                    throw new Exception(
                        $"El usuario Id {destino.Id} ya no existe en la base. " +
                        "No se puede aplicar el rollback (el usuario fue eliminado " +
                        "posteriormente).");

                // 1) Snapshot del estado ACTUAL, antes de aplicar el rollback
                //    (para dejarlo registrado y poder deshacer el rollback).
                var actual = TomarSnapshot(destino.Id, ""); // el nombre lo relee el DAL abajo
                var usuariosActuales = usuarioDAL.GetAll();
                var usuActual = usuariosActuales.Find(u => u.Id == destino.Id);
                if (usuActual != null) actual.Nombre = usuActual.Nombre;

                // 2) Aplicar el rollback
                usuarioDAL.Restaurar(destino.Id, destino.Nombre, destino.Permisos);

                // 3) Registrar en bitácora / auditoría
                BitacoraBLL.Instancia.RegistrarRollbackUsuario(
                    destino.Id, actual, destino, fechaSnapshotDestino);

                // 4) Recalcular dígitos verificadores
                IntegridadBLL.Instancia.RecalcularDVHDeUsuario(destino.Id, destino.Nombre);
                IntegridadBLL.Instancia.RecalcularDVVUsuarios();
            }
            catch (Exception ex)
            {
                BitacoraBLL.Instancia.RegistrarError("Rollback de usuario", ex);
                throw;
            }
        }


        public void Logout()
        {
            BitacoraBLL.Instancia.RegistrarLogout(UsuarioActivo?.Nombre ?? "");
            PerfilBLL.Instancia.LimpiarPerfil();
            UsuarioActivo = null;
        }
    }
}
