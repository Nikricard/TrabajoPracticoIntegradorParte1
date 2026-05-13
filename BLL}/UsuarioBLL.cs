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
        //El singleton debe estar en la capa de servicios
        //El encriptado debe estar en la capa de servicios
        //Las verificaciones para los usuarios deben estar en la BE
        //utilizando algun metodo que luego se llame desde la BLL
        //donde solo lo agrego enviando a la DAL
        
        //Empieza singleton
        private static UsuarioBLL? _instancia = null; // El ? indica que la variable puede ser nula, lo cual es necesario para el patrón singleton

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

        public Usuario? UsuarioActivo { get; private set; } = null; // sesión actual
        //El ? indica que la variable puede ser nula, lo cual es necesario para representar la ausencia de un usuario activo (sesión cerrada)
        //variable para guardar la sesión del usuario logueado, se asigna en el login y se limpia en el logout
        
        private readonly UsuarioDAL usuarioDAL; //instancia de un usuario tipo DAL

        public UsuarioBLL()
        {
            usuarioDAL = new UsuarioDAL(); //inicializamos la instancia en el constructor
        }

        public Usuario Add(Usuario usuario,string contraseñaPlana)
        {
            try
            {
                if (usuario == null)
                {
                    throw new Exception("El usuario no puede ser nulo");
                }   //verificamos que el usuario no sea nulo

                if (!usuario.EsValido(out string mensaje))
                {//ahora la BE hace sus propias validaciones
                    throw new Exception(mensaje);
                }

                if (string.IsNullOrEmpty(contraseñaPlana))
                {//verificacion para que el usuario ingrese una contraseña
                    throw new Exception("El usuario debe tener una contraseña");
                }
                usuario.Contraseña = Encriptado.HashContrasena(contraseñaPlana); // hasheamos antes de persistir
                usuario = usuarioDAL.Add(usuario); //enviamos el usuario en la lista a la DAL
                
                BitacoraBLL.Instancia.RegistrarAddUsuario(usuario);//registramos la acción en la bitácora

                return usuario; //planeamos retornar el usuario ingresado a la DAL para que lo persista en la base
            }
            catch (Exception ex)
            {
                //en caso de haber algun problema, agarramos la ex y frenamos la ejecucion
                BitacoraBLL.Instancia.RegistrarError("ADD_USUARIO", ex);
                throw;
            }
        }

        public Usuario Modify(Usuario anterior, Usuario nuevo)
        {
            try
            {
                if (nuevo == null)
                {
                    throw new Exception("El usuario no puede ser nulo");
                }   //verificamos que el usuario no sea null

                if (!nuevo.EsValido(out string mensaje))
                {//ahora la BE hace sus propias validaciones
                    throw new Exception(mensaje);
                }

                nuevo = usuarioDAL.Modify(nuevo); //enviamos el usuario en la lista a la DAL

                BitacoraBLL.Instancia.RegistrarModifyUsuario(anterior, nuevo); //registramos la acción en la bitácora, enviando el estado anterior y el nuevo del usuario

                return nuevo; //planeamos retornar el usuario ingresado a la DAL para que lo persista en la base
            }
            catch (Exception ex)
            {
                //en caso de haber algun problema, agarramos la ex y frenamos la ejecucion
                BitacoraBLL.Instancia.RegistrarError("MODIFY_USUARIO", ex);
                throw;
            }
        }

        public Usuario Login(string nombre, string contraseñaPlana)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseñaPlana))
                {
                    throw new Exception("Nombre y contraseña son obligatorios");
                }
                //guardamos la contraseña hasheada en el atributo del usuario para enviarla a la DAL
                string hash = Encriptado.HashContrasena(contraseñaPlana);
                Usuario? usuario = usuarioDAL.Login(nombre, hash);

                if (usuario == null)
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                }
                UsuarioActivo = usuario; // enviamos la sesión al singleton

                //carga el perfil del usuario ingresado desde la bll
                PerfilBLL.Instancia.CargarPerfilDeUsuario(usuario.Id);

                BitacoraBLL.Instancia.RegistrarLogin(nombre); //registramos la acción en la bitácora, enviando el nombre del usuario que hizo login
                //crea instancia de bitacora y registra el login con el nombre
                return UsuarioActivo; // retornamos el usuario activo para mostrar su nombre en la barra de navegación
            }
            catch (Exception ex)
            {
                //en caso de haber algun problema, agarramos la ex y frenamos la ejecucion
                BitacoraBLL.Instancia.RegistrarError("LOGIN", ex);
                throw;
            }

        }

        public Usuario Delete(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    throw new Exception("El usuario no puede ser nulo");
                }   //verificamos que el usuario no sea null

                if (!usuario.EsValido(out string mensaje))
                {//ahora la BE hace sus propias validaciones
                    throw new Exception(mensaje);
                }

                usuario = usuarioDAL.Delete(usuario); //enviamos el usuario en la lista a la DAL

                BitacoraBLL.Instancia.RegistrarDeleteUsuario(usuario); //registramos la acción en la bitácora, enviando el estado del usuario que se eliminó

                return usuario; //retornamos el usuario ingresado a la DAL para que lo borre de la base

            }
            catch (Exception ex)
            {
                //en caso de haber algun problema, agarramos la ex y frenamos la ejecucion
                BitacoraBLL.Instancia.RegistrarError("DELETE_USUARIO", ex);
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
                //en caso de haber algun problema, agarramos la ex y frenamos la ejecucion
                BitacoraBLL.Instancia.RegistrarError("GETALL_USUARIO", ex);
                throw;
            }
        }


        public void Logout()
        {
            BitacoraBLL.Instancia.RegistrarLogout(UsuarioActivo?.Nombre ?? "");
            PerfilBLL.Instancia.LimpiarPerfil();  // limpia el perfil activo
            UsuarioActivo = null; // limpiamos la sesión
        }

    }
}
