using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TableroOrdenController : ApiController
    {
        public bool ctrlRegistro(TableroOrden tabor)
        {
            DaoTableroOrden modelo = new DaoTableroOrden();
            var respuesta = false;

            if ((0 == (tabor.Id_tablero)) || (0 == (tabor.Reserva_id_reserva)) || (0 == (tabor.Detalle_pedido_id_det_ped)))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeTableroOrden(tabor.Id_tablero))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarTableroOrden(tabor);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<TableroOrden> Get(int Id_tablero)
        {
            return new DaoTableroOrden().BuscarTableroOrden(Id_tablero);
        }

        public IEnumerable<TableroOrden> GetAll()
        {
            return new DaoTableroOrden().listarTableroOrden();
        }

        public bool Put(TableroOrden tabor)
        {

            return DaoTableroOrden.ActualizarTableroOrden(tabor);
        }

        public bool Delete(int Id_tablero)
        {
            return DaoTableroOrden.EliminarTableroOrden(Id_tablero);
        }
    }
}
