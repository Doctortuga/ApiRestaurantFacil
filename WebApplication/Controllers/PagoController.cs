using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class PagoController : ApiController
    {
        public bool ctrlRegistro(Pago pag)
        {
            DaoPago modelo = new DaoPago();
            var respuesta = false;

            if ((0 == (pag.Id_pago)) || string.IsNullOrEmpty(pag.Medio_pago) || (0 == (pag.Total)) || (null == (pag.Fecha)) || (0 == (pag.Pedido_id_pedido)))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existePago(pag.Id_pago))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarPago(pag);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Pago> Get(int Id_pago)
        {
            return new DaoPago().BuscarPago(Id_pago);
        }

        public IEnumerable<Pago> GetAll()
        {
            return new DaoPago().listarPago();
        }

        public bool Put(Pago pag)
        {

            return DaoPago.ActualizarPago(pag);
        }

        public bool Delete(int Id_pago)
        {
            return DaoPago.EliminarPago(Id_pago);
        }
    }
}
