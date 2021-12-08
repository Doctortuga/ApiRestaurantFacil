using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class RecetaController : ApiController
    {
        public bool ctrlRegistro(Receta rec)
        {
            DaoReceta modelo = new DaoReceta();
            var respuesta = false;

            if ((0 == (rec.Id_receta)) || (null == (rec.Tiempo_preparacion)))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeReceta(rec.Id_receta))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarReceta(rec);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Receta> Get(int Id_receta)
        {
            return new DaoReceta().BuscarReceta(Id_receta);
        }

        public IEnumerable<Receta> GetAll()
        {
            return new DaoReceta().listarReceta();
        }

        public bool Put(Receta rec)
        {

            return DaoReceta.ActualizarReceta(rec);
        }

        public bool Delete(int Id_receta)
        {
            return DaoReceta.EliminarReceta(Id_receta);
        }
    }
}
