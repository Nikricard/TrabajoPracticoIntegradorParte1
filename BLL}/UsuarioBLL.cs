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

                BitacoraBLL.Instancia.RegistrarAddUsuario(usuario);

                // Dígitos verificadores: se pasa Id y Nombre.
                // Si el DAL.Add no devolvió el Id IDENTITY (queda en 0),
                // IntegridadBLL busca por nombre como fallback.
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

                nuevo = usuarioDAL.Modify(nuevo);

                BitacoraBLL.Instancia.RegistrarModifyUsuario(anterior, nuevo);

                // El DVH se recalcula leyendo los datos actuales desde la BD.
                // Se pasa nombre como fallback por si el Id no viene bien.
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

                usuario = usuarioDAL.Delete(usuario);

                BitacoraBLL.Instancia.RegistrarDeleteUsuario(usuario);

                // El usuario ya no existe, solo hay que recalcular el DVV.
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


        public void Logout()
        {
            BitacoraBLL.Instancia.RegistrarLogout(UsuarioActivo?.Nombre ?? "");
            PerfilBLL.Instancia.LimpiarPerfil();
            UsuarioActivo = null;
        }

    }
}
