using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class PedidoProv
    {
        public int Id_ped_prov { get; set; }
        public int Total { get; set; }
        public string Fecha { get; set; }
        public string Responsable { get; set; }
        public string Usuario_rut { get; set; }
        public int Proveedor_id_proveedor { get; set; }
    }
}