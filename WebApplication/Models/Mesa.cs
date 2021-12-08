using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Mesa
    {
        public int Nro_mesa { get; set; }
        public string Disponibilidad { get; set; }
        public int Id_reserva { get; set; }
        public int Capacidad { get; set; }


    }
}