using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class TableroOrden
    {
        public int Id_tablero { get; set; }
        public int Reserva_id_reserva { get; set; }
        public int Detalle_pedido_id_det_ped { get; set; }
    }
}