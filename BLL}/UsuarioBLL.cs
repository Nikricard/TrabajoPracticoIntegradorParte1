using BE;
using DAL;
using System.Text;
using System.Security.Cryptography;
using BLL_;
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

        private string HashContrasena(string contraseña)    //metodo para hashear la contraseña usando System.Security.Cryptography
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                return Convert.ToHexString(bytes).ToLower(); // devuelve 64 chars hexa
            }
        }

        public Usuario Add(Usuario usuario,string contraseñaPlana)
        {
            try
            {
                if (usuario == null)
                {
                    throw new Exception("El usuario no puede ser nulo");
                }   //verificamos que el usuario no sea nulo

                if (string.IsNullOrEmpty(usuario.Nombre))
                {
                    throw new Exception("El usuario debe tener un nombre");
                }//verificamos que el usuario tenga un nombre

                if (int.TryParse(usuario.Nombre, out int resultadoNumero))
                {
                    // en caso de que el usuario escriba numeros en vez de su nombre
                    throw new Exception("El nombre de usuario no puede ser un número");
                }

                if (string.IsNullOrEmpty(contraseñaPlana))
                {//verificacion para que el usuario ingrese una contraseña
                    throw new Exception("El usuario debe tener una contraseña");
                }
                usuario.Contraseña = HashContrasena(contraseñaPlana); // hasheamos antes de persistir
                usuario = usuarioDAL.Add(usuario); //enviamos el usuario en la lista a la DAL

                return usuario; //planeamos retornar el usuario ingresado a la DAL para que lo persista en la base
            }
            catch (Exception)
            {
                //en caso de haber algun problema, agarramos la ex y frenamos la ejecucion
                throw;
            }
        }

        public Usuario Modify(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    throw new Exception("El usuario no puede ser nulo");
                }   //verificamos que el usuario no sea null

                if (int.TryParse(usuario.Nombre, out int resultadoNumero))
                {
                    // en caso de que el usuario escriba números en vez de nombre
                    throw new Exception("El nombre de usuario no puede ser un número");
                }

                if (string.IsNullOrEmpty(usuario.Nombre))
                {
                    throw new Exception("El usuario debe tener un nombre");
                }//verificamos que el usuario tenga un nombre
                usuario = usuarioDAL.Modify(usuario); //enviamos el usuario en la lista a la DAL
                return usuario; //planeamos retornar el usuario ingresado a la DAL para que lo persista en la base
            }
            catch (Exception)
            {
                //en caso de haber algun problema, agarramos la ex y frenamos la ejec
                throw;
            }
        }

        public Usuario Login(string nombre, string contraseñaPlana)
        {
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contraseñaPlana))
            {
                throw new Exception("Nombre y contraseña son obligatorios");
            }
            //guardamos la contraseña hasheada en el atributo del usuario para enviarla a la DAL
            string hash = HashContrasena(contraseñaPlana);
            Usuario? usuario = usuarioDAL.Login(nombre, hash);

            if (usuario == null)
            {
                throw new Exception("Usuario o contraseña incorrectos");
            }
            UsuarioActivo = usuario; // enviamos la sesión al singleton
            return UsuarioActivo; // retornamos el usuario activo para mostrar su nombre en la barra de navegación
        }

        public Usuario Delete(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    throw new Exception("El usuario no puede ser nulo");
                }   //verificamos que el usuario no sea null
                if (string.IsNullOrEmpty(usuario.Nombre))
                {
                    throw new Exception("El usuario debe tener un nombre");
                }//verificamos que el usuario tenga un nombre

                usuario = usuarioDAL.Delete(usuario); //enviamos el usuario en la lista a la DAL
                return usuario; //retornamos el usuario ingresado a la DAL para que lo borre de la base

            }
            catch (Exception)
            {
                //en caso de haber algun problema, agarramos la ex y frenamos la ejecucion
                throw;
            }
        }

        public List<Usuario> GetAll()
        {
            try
            {
                return usuarioDAL.GetAll();
            }
            catch (Exception)
            {
                //en caso de haber algun problema, agarramos la ex y frenamos la ejecucion
                throw;
            }
        }


        public void Logout()
        {
            UsuarioActivo = null; // limpiamos la sesión
        }

    }
}
