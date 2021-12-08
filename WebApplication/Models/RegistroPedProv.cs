using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class RegistroPedProv
    {
        public int Id_reg_prov { get; set; }
        public int Producto_id_producto { get; set; }
        public int Precio_compra { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha_ingreso { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public int Pedido_prov_id_ped_prov { get; set; }
    }
}