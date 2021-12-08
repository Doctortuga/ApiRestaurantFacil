using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class IngredientesReceta
    {
        public int Id_ingredientes { get; set; }
        public int Receta_id_receta { get; set; }
        public int Cantidad { get; set; }
        public int Producto_id_producto { get; set; }
    }
}