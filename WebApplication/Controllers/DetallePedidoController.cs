using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class DetallePedidoController : ApiController
    {
        public bool ctrlRegistro(DetallePedido dp)
        {
            DaoDetallePedido modelo = new DaoDetallePedido();
            var respuesta = false;

            if ((0 == (dp.Id_detalle_pedido)) || (0 == (dp.Menu_id_menu)) || string.IsNullOrEmpty(dp.Realizado) || (null == (dp.Tiempo_inicio)) || (null == (dp.Tiempo_termino)) || 0 == (dp.Pedido_id_pedido))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeDetallePedido(dp.Id_detalle_pedido))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarDetallePedido(dp);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<DetallePedido> Get(int Id_detalle_pedido)
        {
            return new DaoDetallePedido().BuscarDetallePedido(Id_detalle_pedido);
        }

        public IEnumerable<DetallePedido> GetAll()
        {
            return new DaoDetallePedido().listarDetallePedido();
        }

        public bool Put(DetallePedido dp)
        {

            return DaoDetallePedido.ActualizarDetallePedido(dp);
        }

        public bool Delete(int Id_detalle_pedido)
        {
            return DaoDetallePedido.EliminarDetallePedido(Id_detalle_pedido);
        }
    }
}
