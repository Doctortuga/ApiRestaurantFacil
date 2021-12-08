using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class MenuController : ApiController
    {
        public bool ctrlRegistro(Menu men)
        {
            DaoMenu modelo = new DaoMenu();
            var respuesta = false;

            if ((0 == (men.Id_menu)) || string.IsNullOrEmpty(men.Nombre) || (0 == (men.Precio)) || (0 == (men.Receta_id_receta)) || string.IsNullOrEmpty(men.Categoria))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeMenu(men.Id_menu))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarMenu(men);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Menu> Get(int Id_menu)
        {
            return new DaoMenu().BuscarMenu(Id_menu);
        }

        public IEnumerable<Menu> GetAll()
        {
            return new DaoMenu().listarMenu();
        }

        public bool Put(Menu men)
        {

            return DaoMenu.ActualizarMenu(men);
        }

        public bool Delete(int Id_menu)
        {
            return DaoMenu.EliminarMenu(Id_menu);
        }
    }
}
