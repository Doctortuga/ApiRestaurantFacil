using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class PedidoController : ApiController
    {
        public bool ctrlRegistro(Pedido ped)
        {
            DaoPedido modelo = new DaoPedido();
            var respuesta = false;

            if ((0 == (ped.Id_pedido)) || (null == (ped.Fecha)) || (0 == (ped.Total)) || (0 == (ped.Mesa_nro_mesa)) || (0 == (ped.Garzon_id_garzon)))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existePedido(ped.Id_pedido))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarPedido(ped);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Pedido> Get(int Id_pedido)
        {
            return new DaoPedido().BuscarPedido(Id_pedido);
        }

        public IEnumerable<Pedido> GetAll()
        {
            return new DaoPedido().listarPedido();
        }

        public bool Put(Pedido ped)
        {

            return DaoPedido.ActualizarPedido(ped);
        }

        public bool Delete(int Id_pedido)
        {
            return DaoPedido.EliminarPedido(Id_pedido);
        }
    }
}
