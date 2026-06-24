using ABS;
using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class BitacoraBLL
    {
        //singleton
        private static BitacoraBLL _instancia;
        public static BitacoraBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new BitacoraBLL();
                return _instancia;
            }
        }

        private BitacoraBLL() 
        { 
            _dal = new BitacoraDAL(); 
        }
        
        private readonly BitacoraDAL _dal;


        // Observer de bitácora
        // Lista de formularios que escuchan los registros y se refrescan solos.
        private readonly List<IObservadorBitacora> _observadores
            = new List<IObservadorBitacora>();

        public void Suscribir(IObservadorBitacora obs)
        {
            if (!_observadores.Contains(obs))
                _observadores.Add(obs);
        }

        public void Desuscribir(IObservadorBitacora obs)
            => _observadores.Remove(obs);

        private void Notificar()
        {
            foreach (var obs in _observadores)
                obs.ActualizarBitacora();
        }


        private string UsuarioActual =>
            UsuarioBLL.Instancia.UsuarioActivo?.Nombre ?? "Sistema";

        // Método central: persiste y notifica.
        // Todos los métodos públicos de registro pasan por acá.
        private void Registrar(string actividad, TipoEvento tipo,
            string descripcion = null, string entidad = null,
            string anterior = null, string nuevo = null)
        {
            _dal.RegistrarEvento(new RegistroBitacora
            {
                Fecha         = DateTime.Now,
                Usuario       = UsuarioActual,
                Actividad     = actividad,
                TipoEvento    = tipo,
                Descripcion   = descripcion,
                Entidad       = entidad,
                ValorAnterior = anterior,
                ValorNuevo    = nuevo
            });

            // Avisar a los formularios suscriptos para que se refresquen
            Notificar();
        }

        

        public void RegistrarLogin(string nombreUsuario)
        {
            Registrar("Inicio de sesión", TipoEvento.Exito,
                $"El usuario '{nombreUsuario}' inició sesión.");
        }

        public void RegistrarLogout(string nombreUsuario)
        {
            Registrar("Cierre de sesión", TipoEvento.Exito,
                $"El usuario '{nombreUsuario}' cerró sesión.");
        }

        public void RegistrarError(string actividad, Exception ex)
        {
            Registrar(actividad, TipoEvento.Error,
                $"Error: {ex.Message}",
                entidad: ex.GetType().Name);
        }

        public void RegistrarExcepcion(string actividad, Exception ex)
        {
            Registrar(actividad, TipoEvento.Excepcion,
                $"Excepción: {ex.Message}\n{ex.StackTrace}",
                entidad: ex.GetType().Name);
        }

        //Usuarios

        public void RegistrarAddUsuario(Usuario u)
        {
            Registrar("Alta de usuario", TipoEvento.Exito,
                $"Nuevo usuario: '{u.Nombre}'", "Usuario",
                null, $"Id:{u.Id} Nombre:{u.Nombre}");

            _dal.RegistrarAuditoriaUsuario(new AuditoriaUsuario
            {
                Fecha          = DateTime.Now,
                UsuarioAccion  = UsuarioActual,
                Operacion      = "Alta",
                IdUsuario      = u.Id,
                NombreAnterior = null,
                NombreNuevo    = u.Nombre
            });

            Notificar();
        }

        public void RegistrarModifyUsuario(Usuario anterior, Usuario nuevo)
        {
            Registrar("Modificación de usuario", TipoEvento.Exito,
                $"Usuario Id:{nuevo.Id} modificado.", "Usuario",
                $"Nombre:{anterior.Nombre}",
                $"Nombre:{nuevo.Nombre}");

            _dal.RegistrarAuditoriaUsuario(new AuditoriaUsuario
            {
                Fecha          = DateTime.Now,
                UsuarioAccion  = UsuarioActual,
                Operacion      = "Modificación",
                IdUsuario      = nuevo.Id,
                NombreAnterior = anterior.Nombre,
                NombreNuevo    = nuevo.Nombre
            });

            Notificar();
        }

        public void RegistrarDeleteUsuario(Usuario u)
        {
            Registrar("Baja de usuario", TipoEvento.Exito,
                $"Usuario '{u.Nombre}' eliminado.", "Usuario",
                $"Id:{u.Id} Nombre:{u.Nombre}", null);

            _dal.RegistrarAuditoriaUsuario(new AuditoriaUsuario
            {
                Fecha          = DateTime.Now,
                UsuarioAccion  = UsuarioActual,
                Operacion      = "Baja",
                IdUsuario      = u.Id,
                NombreAnterior = u.Nombre,
                NombreNuevo    = null
            });

            Notificar();
        }

        //Idiomas

        public void RegistrarAddIdioma(int idIdioma, string nombre)
        {
            Registrar("Alta de idioma", TipoEvento.Exito,
                $"Nuevo idioma: '{nombre}'", "Idioma",
                null, $"Id:{idIdioma} Nombre:{nombre}");

            _dal.RegistrarAuditoriaIdioma(new AuditoriaIdioma
            {
                Fecha         = DateTime.Now,
                UsuarioAccion = UsuarioActual,
                Operacion     = "Alta",
                IdIdioma      = idIdioma,
                NombreNuevo   = nombre
            });

            Notificar();
        }

        public void RegistrarDeleteIdioma(int idIdioma, string nombre)
        {
            Registrar("Baja de idioma", TipoEvento.Exito,
                $"Idioma '{nombre}' eliminado.", "Idioma",
                $"Id:{idIdioma} Nombre:{nombre}", null);

            _dal.RegistrarAuditoriaIdioma(new AuditoriaIdioma
            {
                Fecha          = DateTime.Now,
                UsuarioAccion  = UsuarioActual,
                Operacion      = "Baja",
                IdIdioma       = idIdioma,
                NombreAnterior = nombre
            });

            Notificar();
        }

        public void RegistrarTraduccion(int idIdioma, string clave,
            string valorAnterior, string valorNuevo)
        {
            Registrar("Modificación de traducción", TipoEvento.Exito,
                $"Traducción '{clave}' modificada.", "Idioma",
                valorAnterior, valorNuevo);

            _dal.RegistrarAuditoriaIdioma(new AuditoriaIdioma
            {
                Fecha           = DateTime.Now,
                UsuarioAccion   = UsuarioActual,
                Operacion       = "Modificación de traducción",
                IdIdioma        = idIdioma,
                ClaveTraduccion = clave,
                ValorAnterior   = valorAnterior,
                ValorNuevo      = valorNuevo
            });

            Notificar();
        }

        public void RegistrarAsignacionPerfil(string nombreUsuarioAfectado, int idUsuarioAfectado,
            string perfilAnterior, string perfilNuevo)
        {
            Registrar(
                actividad: "Asignación de perfil",
                tipo: TipoEvento.Exito,
                descripcion: $"Perfil asignado a '{nombreUsuarioAfectado}' " +
                             $"(Id: {idUsuarioAfectado}). " +
                             $"Anterior: '{perfilAnterior ?? "Sin perfil"}' → " +
                             $"Nuevo: '{perfilNuevo}'.",
                entidad: "Perfil",
                anterior: perfilAnterior,
                nuevo: perfilNuevo
            );

            _dal.RegistrarAuditoriaUsuario(new AuditoriaUsuario
            {
                Fecha = DateTime.Now,
                UsuarioAccion = UsuarioActual,
                Operacion = "Asignación de perfil",
                IdUsuario = idUsuarioAfectado,
                NombreAnterior = perfilAnterior ?? "Sin perfil",
                NombreNuevo = perfilNuevo
            });

            Notificar();
        }

        public void RegistrarCreacionConjunto(string nombreConjunto, string permisosIncluidos)
        {
            Registrar(
                actividad: "Alta de conjunto",
                tipo: TipoEvento.Exito,
                descripcion: $"Conjunto de permisos '{nombreConjunto}' creado.",
                entidad: "Perfil",
                anterior: null,
                nuevo: $"Permisos: {permisosIncluidos}"
            );
        }

        public void RegistrarBackup(string rutaArchivo)
        {
            Registrar(
                actividad: "Backup de base de datos",
                tipo: TipoEvento.Exito,
                descripcion: $"Backup de la base ejecutado por {UsuarioActual}.",
                entidad: "Base de datos",
                anterior: null,
                nuevo: rutaArchivo
            );
        }

        public void RegistrarRestore(string rutaArchivo)
        {
            Registrar(
                actividad: "Restore de base de datos",
                tipo: TipoEvento.Exito,
                descripcion: $"Restore de la base ejecutado por {UsuarioActual}.",
                entidad: "Base de datos",
                anterior: null,
                nuevo: rutaArchivo
            );
        }

        //Consultas

        public List<RegistroBitacora> Buscar(
            DateTime? desde, DateTime? hasta,
            string usuario, string actividad, string tipoEvento)
            => _dal.Buscar(desde, hasta, usuario, actividad, tipoEvento);

        public List<AuditoriaUsuario> GetAuditoriaUsuario(int? idUsuario = null)
            => _dal.GetAuditoriaUsuario(idUsuario);

        public List<AuditoriaIdioma> GetAuditoriaIdioma(int? idIdioma = null)
            => _dal.GetAuditoriaIdioma(idIdioma);
    }
}
