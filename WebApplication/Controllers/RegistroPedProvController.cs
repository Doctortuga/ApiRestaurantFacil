using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class RegistroPedProvController : ApiController
    {
        public bool ctrlRegistro(RegistroPedProv regpedprov)
        {
            DaoRegistroPedProv modelo = new DaoRegistroPedProv();
            var respuesta = false;

            if ((0 == (regpedprov.Id_reg_prov)) || (0 == (regpedprov.Producto_id_producto)) || (0 == (regpedprov.Precio_compra)) || (0 == (regpedprov.Cantidad)) || (null == (regpedprov.Fecha_ingreso)) || (null == (regpedprov.Fecha_Vencimiento)) || (0 == (regpedprov.Pedido_prov_id_ped_prov)))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeRegistroPedProv(regpedprov.Id_reg_prov))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarRegistroPedProv(regpedprov);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<RegistroPedProv> Get(int Id_reg_prov)
        {
            return new DaoRegistroPedProv().BuscarRegistroPedProv(Id_reg_prov);
        }

        public IEnumerable<RegistroPedProv> GetAll()
        {
            return new DaoRegistroPedProv().listarRegistroPedProv();
        }

        public bool Put(RegistroPedProv regpedprov)
        {

            return DaoRegistroPedProv.ActualizarRegistroPedProv(regpedprov);
        }

        public bool Delete(int Id_reg_prov)
        {
            return DaoRegistroPedProv.EliminarRegistroPedProv(Id_reg_prov);
        }
    }
}
