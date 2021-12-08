using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class StockPendiente
    {
        public int Id_stock_pend { get; set; }
        public string Disponibilidad { get; set; }
        public int Producto_id_producto { get; set; }
        public int Precio { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha_ingreso { get; set; }
        public DateTime Fecha_vencimiento { get; set; }
        public int Stock_id_reg_prov { get; set; }
    }
}