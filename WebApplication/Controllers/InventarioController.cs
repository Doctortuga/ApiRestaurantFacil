using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class InventarioController : ApiController
    {
        public bool ctrlRegistro(Inventario inv)
        {
            DaoInventario modelo = new DaoInventario();
            var respuesta = false;

            if ((0 == (inv.Id_Inventario)) || (0 == (inv.Stock)) || (0 == (inv.Registro_Ped_Prov_Prod_Id_Prod)) || (0 == (inv.Registro_Ped_Prov_Id_Reg_Prov)) || string.IsNullOrEmpty(inv.Usuario_rut) || string.IsNullOrEmpty(inv.Id_Producto))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeInventario(inv.Id_Inventario))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarInventario(inv);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Inventario> Get(int Id_Inventario)
        {
            return new DaoInventario().BuscarInventario(Id_Inventario);
        }

        public IEnumerable<Inventario> GetAll()
        {
            return new DaoInventario().listarInventario();
        }

        public bool Put(Inventario inv)
        {

            return DaoInventario.ActualizarInventario(inv);
        }

        public bool Delete(int Id_Inventario)
        {
            return DaoInventario.EliminarInventario(Id_Inventario);
        }
    }
}
