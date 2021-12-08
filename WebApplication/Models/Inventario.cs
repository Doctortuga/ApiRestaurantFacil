using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Inventario
    {
        public int Id_Inventario { get; set; }
        public int Stock { get; set; }
        public int Registro_Ped_Prov_Prod_Id_Prod { get; set; }
        public int Registro_Ped_Prov_Id_Reg_Prov { get; set; }
        public string Usuario_rut { get; set; }
        public string Id_Producto { get; set; }
    }
}