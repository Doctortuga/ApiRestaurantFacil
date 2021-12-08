using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Usuario
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public int Id_rol { get; set; }
      
    }
}