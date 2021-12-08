using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class MesaController : ApiController
    {
        public IEnumerable<Mesa> GetAll()
        {
            return new DaoMesa().listarMesa();
        }
        public IEnumerable<MesasTotales> GetMesasTotales(int a)
        {
            return new DaoMesa().MesasTotales();
        }
        public bool AddMesa(Mesa mes)
        {
            DaoMesa modelo = new DaoMesa();

            var respuesta = false;

            if (0 == mes.Nro_mesa || string.IsNullOrEmpty(mes.Disponibilidad) || 0 == mes.Id_reserva)
            {
                respuesta = false;
            }
            else
            {
              
                    if (modelo.existeMesa(mes.Nro_mesa))
                    {
                        respuesta = false;
                    }
                    else
                    {
                        
                        modelo.agregarMesa(mes);
                        respuesta = true;
                    }
       
            }
            return respuesta;

        }

        public bool Put(Mesa mes)
        {

            return DaoMesa.ActualizarMESA(mes);
        }

        public bool Delete(int nro_mesa)
        {
            return DaoMesa.EliminarMesa(nro_mesa);
        }
    }
}
