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

        private BitacoraBLL() { _dal = new BitacoraDAL(); }
        private readonly BitacoraDAL _dal;

        
        private string UsuarioActual =>
            UsuarioBLL.Instancia.UsuarioActivo?.Nombre ?? "Sistema";

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
        }

        

        public void RegistrarLogin(string nombreUsuario)
        {
            Registrar("LOGIN", TipoEvento.Exito,
                $"El usuario '{nombreUsuario}' inició sesión.");
        }

        public void RegistrarLogout(string nombreUsuario)
        {
            Registrar("LOGOUT", TipoEvento.Exito,
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
            Registrar("ADD_USUARIO", TipoEvento.Exito,
                $"Nuevo usuario: '{u.Nombre}'", "Usuario",
                null, $"Id:{u.Id} Nombre:{u.Nombre}");

            _dal.RegistrarAuditoriaUsuario(new AuditoriaUsuario
            {
                Fecha          = DateTime.Now,
                UsuarioAccion  = UsuarioActual,
                Operacion      = "ADD",
                IdUsuario      = u.Id,
                NombreAnterior = null,
                NombreNuevo    = u.Nombre
            });
        }

        public void RegistrarModifyUsuario(Usuario anterior, Usuario nuevo)
        {
            Registrar("MODIFY_USUARIO", TipoEvento.Exito,
                $"Usuario Id:{nuevo.Id} modificado.", "Usuario",
                $"Nombre:{anterior.Nombre}",
                $"Nombre:{nuevo.Nombre}");

            _dal.RegistrarAuditoriaUsuario(new AuditoriaUsuario
            {
                Fecha          = DateTime.Now,
                UsuarioAccion  = UsuarioActual,
                Operacion      = "MODIFY",
                IdUsuario      = nuevo.Id,
                NombreAnterior = anterior.Nombre,
                NombreNuevo    = nuevo.Nombre
            });
        }

        public void RegistrarDeleteUsuario(Usuario u)
        {
            Registrar("DELETE_USUARIO", TipoEvento.Exito,
                $"Usuario '{u.Nombre}' eliminado.", "Usuario",
                $"Id:{u.Id} Nombre:{u.Nombre}", null);

            _dal.RegistrarAuditoriaUsuario(new AuditoriaUsuario
            {
                Fecha          = DateTime.Now,
                UsuarioAccion  = UsuarioActual,
                Operacion      = "DELETE",
                IdUsuario      = u.Id,
                NombreAnterior = u.Nombre,
                NombreNuevo    = null
            });
        }

        //Idiomas

        public void RegistrarAddIdioma(int idIdioma, string nombre)
        {
            Registrar("ADD_IDIOMA", TipoEvento.Exito,
                $"Nuevo idioma: '{nombre}'", "Idioma",
                null, $"Id:{idIdioma} Nombre:{nombre}");

            _dal.RegistrarAuditoriaIdioma(new AuditoriaIdioma
            {
                Fecha         = DateTime.Now,
                UsuarioAccion = UsuarioActual,
                Operacion     = "ADD",
                IdIdioma      = idIdioma,
                NombreNuevo   = nombre
            });
        }

        public void RegistrarDeleteIdioma(int idIdioma, string nombre)
        {
            Registrar("DELETE_IDIOMA", TipoEvento.Exito,
                $"Idioma '{nombre}' eliminado.", "Idioma",
                $"Id:{idIdioma} Nombre:{nombre}", null);

            _dal.RegistrarAuditoriaIdioma(new AuditoriaIdioma
            {
                Fecha          = DateTime.Now,
                UsuarioAccion  = UsuarioActual,
                Operacion      = "DELETE",
                IdIdioma       = idIdioma,
                NombreAnterior = nombre
            });
        }

        public void RegistrarTraduccion(int idIdioma, string clave,
            string valorAnterior, string valorNuevo)
        {
            Registrar("TRADUCCION", TipoEvento.Exito,
                $"Traducción '{clave}' modificada.", "Idioma",
                valorAnterior, valorNuevo);

            _dal.RegistrarAuditoriaIdioma(new AuditoriaIdioma
            {
                Fecha           = DateTime.Now,
                UsuarioAccion   = UsuarioActual,
                Operacion       = "ADD_TRADUCCION",
                IdIdioma        = idIdioma,
                ClaveTraduccion = clave,
                ValorAnterior   = valorAnterior,
                ValorNuevo      = valorNuevo
            });
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
