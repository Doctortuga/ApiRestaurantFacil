using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Login
    {
        public bool PuedeIngresar { get; set; }
        public string mensaje { get; set; }
        public int rol { get; set; }
        public string rut { get; set; }
    }
}