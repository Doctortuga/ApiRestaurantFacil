using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class GarzonController : ApiController
    {
        public bool ctrlRegistro(Garzon gar)
        {
            DaoGarzon modelo = new DaoGarzon();
            var respuesta = false;

            if ((0 == (gar.Id_Garzon)) || string.IsNullOrEmpty(gar.Nombre) || string.IsNullOrEmpty(gar.Usuario_Rut))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeGarzon(gar.Id_Garzon))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarGarzon(gar);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Garzon> Get(int Id_Garzon)
        {
            return new DaoGarzon().BuscarGarzon(Id_Garzon);
        }

        public IEnumerable<Garzon> GetAll()
        {
            return new DaoGarzon().listarGarzon();
        }

        public bool Put(Garzon gar)
        {

            return DaoGarzon.ActualizarGarzon(gar);
        }

        public bool Delete(int Id_Garzon)
        {
            return DaoGarzon.EliminarGarzon(Id_Garzon);
        }
    }
}
