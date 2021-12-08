using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class RolController : ApiController
    {
        public bool ctrlRegistro(Rol role)
        {
            DaoRol modelo = new DaoRol();
            var respuesta = false;

            if ((0 == (role.Id_Rol)) || string.IsNullOrEmpty(role.Nombre))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeRol(role.Id_Rol))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarRol(role);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Rol> Get(int Id_Rol)
        {
            return new DaoRol().BuscarRol(Id_Rol);
        }

        public IEnumerable<Rol> GetAll()
        {
            return new DaoRol().listarRol();
        }

        public bool Put(Rol role)
        {

            return DaoRol.ActualizarRol(role);
        }

        public bool Delete(int id_Rol)
        {
            return DaoRol.EliminarRol(id_Rol);
        }
    }
}
