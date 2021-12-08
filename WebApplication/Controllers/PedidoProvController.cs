using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class PedidoProvController : ApiController
    {
        public bool ctrlRegistro(PedidoProv pedprov)
        {
            DaoPedidoProv modelo = new DaoPedidoProv();
            var respuesta = false;

            //CONDICIONES//
            if (modelo.existePedidoProv(pedprov.Id_ped_prov))
            {
                respuesta = false;
            }
            else
            {
                var obj = modelo.agregarPedidoProv(pedprov);
                if (obj == null)
                {
                    respuesta = false;
                }
                else
                {
                    respuesta = true;
                }
            }


            return respuesta;

        }

        public int Get(int a)
        {
            return new DaoPedidoProv().GetIdPedido();
        }

        public IEnumerable<PedidoProv> GetAll()
        {
            return new DaoPedidoProv().listarPedidoProv();
        }

        public bool Put(PedidoProv pedprov)
        {

            return DaoPedidoProv.ActualizarPedidoProv(pedprov);
        }

        public bool Delete(int Id_ped_prov)
        {
            return DaoPedidoProv.EliminarPedidoProv(Id_ped_prov);
        }
    }
}
