using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;


namespace WebApplication.Controllers
{
    public class ProveedorController : ApiController
    {
        public bool ctrlRegistro(Proveedor prov)
        {
            DaoProveedor modelo = new DaoProveedor();
            var respuesta = false;

            if ((0 == (prov.Id_Proveedor)) || string.IsNullOrEmpty(prov.Nombre))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeProveedor(prov.Id_Proveedor))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarProveedor(prov);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Proveedor> Get(int Id_Proveedor)
        {
            return new DaoProveedor().BuscarProveedor(Id_Proveedor);
        }

        public IEnumerable<Proveedor> GetAll()
        {
            return new DaoProveedor().listarProveedor();
        }

        public bool Put(Proveedor role)
        {

            return DaoProveedor.ActualizarProveedor(role);
        }

        public bool Delete(int id_Rol)
        {
            return DaoProveedor.EliminarProveedor(id_Rol);
        }
    }
}
