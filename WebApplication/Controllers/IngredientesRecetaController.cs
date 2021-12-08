using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class IngredientesRecetaController : ApiController
    {
        public bool ctrlRegistro(IngredientesReceta ingrec)
        {
            DaoIngredientesReceta modelo = new DaoIngredientesReceta();
            var respuesta = false;

            if ((0 == (ingrec.Id_ingredientes)) || (0 == (ingrec.Receta_id_receta)) || (0 == (ingrec.Cantidad)) || 0 == (ingrec.Producto_id_producto))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeIngredientesReceta(ingrec.Id_ingredientes))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarIngredientesReceta(ingrec);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<IngredientesReceta> Get(int Id_ingredientes)
        {
            return new DaoIngredientesReceta().BuscarIngredientesReceta(Id_ingredientes);
        }

        public IEnumerable<IngredientesReceta> GetAll()
        {
            return new DaoIngredientesReceta().listarIngredientesReceta();
        }

        public bool Put(IngredientesReceta ingrec)
        {

            return DaoIngredientesReceta.ActualizarIngredientesReceta(ingrec);
        }

        public bool Delete(int Id_ingredientes)
        {
            return DaoIngredientesReceta.EliminarIngredientesReceta(Id_ingredientes);
        }
    }
}
