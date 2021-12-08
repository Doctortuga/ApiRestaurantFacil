using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;

namespace WebApplication.Models
{
    public class BD
    {
        public static OracleConnection getConexion()
        {
                            string oradb = "Data Source=(DESCRIPTION="
                  + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))"
                  + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));"
                  + "User Id=ESTUDIANTE1;Password=123;";
            OracleConnection conexion = new OracleConnection(oradb);
            //OracleConnection conexion = new OracleConnection("User Id=MiguelBustos;Password=123;Data Source=Restaurapp"); 
            //MySqlConnection conexion = new MySqlConnection("Database=Portafolio;Data Source=localhost;User Id=root;Password=P4zt4n0ztr4h2.");
      
            return conexion;

        }
    }
}