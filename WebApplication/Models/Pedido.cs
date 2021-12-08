using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Pedido
    {
        public int Id_pedido { get; set; }
        public DateTime Fecha { get; set; }
        public int Total { get; set; }
        public int Mesa_nro_mesa { get; set; }
        public int Garzon_id_garzon { get; set; }
    }
}