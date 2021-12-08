using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class StockPendienteController : ApiController
    {
        public bool ctrlRegistro(StockPendiente stopen)
        {
            DaoStockPendiente modelo = new DaoStockPendiente();
            var respuesta = false;

            if ((0 == (stopen.Id_stock_pend)) || string.IsNullOrEmpty(stopen.Disponibilidad)|| (0 == (stopen.Producto_id_producto)) || (0 == (stopen.Precio)) || (0 == (stopen.Cantidad)) || (null == (stopen.Fecha_ingreso)) || (null == (stopen.Fecha_vencimiento)) || (0 == (stopen.Stock_id_reg_prov)))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeStockPendiente(stopen.Id_stock_pend))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarStockPendiente(stopen);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<StockPendiente> Get(int Id_stock_pend)
        {
            return new DaoStockPendiente().BuscarStockPendiente(Id_stock_pend);
        }

        public IEnumerable<StockPendiente> GetAll()
        {
            return new DaoStockPendiente().listarStockPendiente();
        }

        public bool Put(StockPendiente stopen)
        {

            return DaoStockPendiente.ActualizarStockPendiente(stopen);
        }

        public bool Delete(int Id_stock_pend)
        {
            return DaoStockPendiente.EliminarStockPendiente(Id_stock_pend);
        }
    }
}
