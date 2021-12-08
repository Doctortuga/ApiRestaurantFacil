using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class DetallePedido
    {
        public int Id_detalle_pedido { get; set; }
        public int Menu_id_menu { get; set; }
        public string Realizado { get; set; }
        public DateTime Tiempo_inicio { get; set; }
        public DateTime Tiempo_termino { get; set; }
        public int Pedido_id_pedido { get; set; }
    }
}