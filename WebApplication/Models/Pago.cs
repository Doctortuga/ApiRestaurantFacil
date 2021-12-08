using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Pago
    {
        public int Id_pago { get; set; }
        public string Medio_pago { get; set; }
        public int Total { get; set; }
        public DateTime Fecha { get; set; }
        public int Pedido_id_pedido { get; set; }
    }
}