using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApplication.Models;
using System.Security.Cryptography;



namespace WebApplication.Controllers
{
    public class LoginController : ApiController
    {
        // POST api/values
        DaoUsuario modelo = new DaoUsuario();

        public Login Login(string rut,string clave)
        {
            //string usuario = "Miguel";
            //string password = "123";

            bool respuesta = false;
            int flag = 0;
            Usuario datosUsuario = null;
            Login loginclase = new Login();


            if (string.IsNullOrEmpty(rut) || string.IsNullOrEmpty(clave))
            {
                respuesta = false;
                flag = 0;

            }
            else
            {
                datosUsuario = modelo.porUsuario(rut);

                if (datosUsuario == null)
                {
                    respuesta = false;

                }
                else
                {
                    if (datosUsuario.Contraseña != modelo.generarSHA1(clave))
                    {
                        respuesta = false;
                        loginclase.mensaje = "no Puede ingresar";
                    }
                    else
                    {
                        Session.id = datosUsuario.Rut;
                        Session.usuario = rut;
                        Session.nombre = datosUsuario.Nombre;
                        Session.id_rol = datosUsuario.Id_rol;
                        loginclase.rut = datosUsuario.Rut;
                        loginclase.rol = datosUsuario.Id_rol;
                        loginclase.PuedeIngresar = true;
                        loginclase.mensaje = "Puede ingresar";


                        respuesta = true;

                    }
                }

            }
            return loginclase;
            
        }


    }
}