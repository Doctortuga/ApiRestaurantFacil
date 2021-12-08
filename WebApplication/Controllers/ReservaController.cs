using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ReservaController : ApiController
    {
        public bool ctrlRegistro(Reserva res)
        {
            DaoReserva modelo = new DaoReserva();
            var respuesta = false;

            if ((0 == (res.Id_reserva)) || (null == (res.Fecha_reserva)) || string.IsNullOrEmpty(res.Observacion) || string.IsNullOrEmpty(res.Cliente_rut))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeReserva(res.Id_reserva))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarReserva(res);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Reserva> Get(int Id_reserva)
        {
            return new DaoReserva().BuscarReserva(Id_reserva);
        }

        public IEnumerable<Reserva> GetAll()
        {
            return new DaoReserva().listarReserva();
        }

        public bool Put(Reserva res)
        {

            return DaoReserva.ActualizarReserva(res);
        }

        public bool Delete(int Id_reserva)
        {
            return DaoReserva.EliminarReserva(Id_reserva);
        }
    }
}
