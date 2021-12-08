using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Producto
    {
        public int Id_Producto { get; set; }
        public string Nombre { get; set;}
        public int Precio { get; set; }
        public string Categoria { get; set; }
        public string Formato { get; set; }
        public string Imagen { get; set; }



    }
}