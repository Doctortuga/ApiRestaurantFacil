using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Reserva
    {
        public int Id_reserva { get; set; }
        public DateTime Fecha_reserva { get; set; }
        public string Observacion { get; set; }
        public string Cliente_rut { get; set; }
    }
}