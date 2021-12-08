using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class UsuarioController : ApiController
    {
        // POST api/values
        DaoUsuario modelo = new DaoUsuario();
        public bool ctrlRegistro(Usuario usuario)
        {

            var respuesta = false;

            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Rut) || string.IsNullOrEmpty(usuario.Contraseña) || 0 == usuario.Id_rol)
            {
                respuesta = false;
            }
            else
            {
                if (usuario.Contraseña == usuario.Contraseña)
                {
                    if (modelo.existeUsuario(usuario.Rut))
                    {
                        respuesta = false;
                    }
                    else
                    {
                        usuario.Contraseña = modelo.generarSHA1(usuario.Contraseña);
                        modelo.agregarUsuario(usuario);
                        respuesta = true;
                    }
                }
                else
                {
                    respuesta = false;
                }
            }
            return respuesta;

        }
        public IEnumerable<Usuario> Get(string rut)
        {
            return new DaoUsuario().BuscarUsuario(rut);
        }
        public IEnumerable<Usuario> GetAll()
        {
            return new DaoUsuario().listarUsuario();
        }
        public bool Put(Usuario usu)
        {

            return DaoUsuario.ActualizarUsuario(usu);
        }

        public bool Delete(string Rut)
        {     
            return DaoUsuario.EliminarUsuario(Rut);           
        }
    }
}