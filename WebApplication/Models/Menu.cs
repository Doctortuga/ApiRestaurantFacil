using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Menu
    {
        public int Id_menu { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public int Receta_id_receta { get; set; }
        public string Categoria { get; set; }
    }
}