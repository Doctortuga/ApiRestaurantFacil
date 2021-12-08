using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace WebApplication.Models
{
    public class DaoProveedor
    {
        OracleConnection conexion = BD.getConexion();
        public List<Proveedor> BuscarProveedor(int pid_proveedor)// LISTO-------------
        {
            List<Proveedor> lista = new List<Proveedor>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_PROVEEDOR", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_proveedor", pid_proveedor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_proveedor = int.Parse(dr.GetInt32(0).ToString());
                var nombre = dr.GetString(1).ToString();


                Proveedor prov = new Proveedor() { Id_Proveedor = id_proveedor, Nombre = nombre };
                lista.Add(prov);
            }
            conexion.Close();
            return lista;
        }

        public List<Proveedor> listarProveedor() // LISTO-------------
        {
            List<Proveedor> lista = new List<Proveedor>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_PROVEEDOR", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_proveedor = int.Parse(dr.GetInt32(0).ToString());
                var nombre = dr.GetString(1).ToString();


                Proveedor prov = new Proveedor() { Id_Proveedor = id_proveedor, Nombre = nombre };
                lista.Add(prov);
            }
            conexion.Close();
            return lista;
        }

        public int agregarProveedor(Proveedor prov)
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_PROVEEDOR", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_Proveedor", prov.Id_Proveedor);
            cmd.Parameters.Add(":pnombre", prov.Nombre);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarProveedor(int pid_proveedor)// LISTO-------------
        {
            var respuesta = false;
            DaoProveedor dao = new DaoProveedor();
            var existe = dao.existeProveedor(pid_proveedor);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_PROVEEDOR", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_proveedor", pid_proveedor);
                cmd.Parameters["@pid_proveedor"].Value = (pid_proveedor);
                ora.Open();
                OracleDataReader rdr = cmd.ExecuteReader();
                respuesta = true;
            }
            else
            {
                respuesta = false;

            }
            return respuesta;
        }

        public bool existeProveedor(int pid_proveedor)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_proveedor FROM proveedor WHERE Id_proveedor LIKE :pid_proveedor";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_PROVEEDOR", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_proveedor",pid_proveedor);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_proveedor", pid_proveedor);
            output.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            if (dr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public static bool ActualizarProveedor(Proveedor prov)// LISTO------------
        {
            int pid_proveedor = 0;
            string pnombre = "";
            pid_proveedor = prov.Id_Proveedor;
            pnombre = prov.Nombre;


            var respuesta = false;
            DaoProveedor dao = new DaoProveedor();
            var existe = dao.existeProveedor(prov.Id_Proveedor);
            if (existe == true)
            {
                if (pid_proveedor != 0 && pnombre != "")
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_PROVEEDOR", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_proveedor", pid_proveedor);
                    cmd.Parameters.Add("@pnombre", pnombre);
                    ora.Open();
                    OracleDataReader rdr = cmd.ExecuteReader();
                    respuesta = true;
                }
            }
            else
            {
                respuesta = false;

            }
            return respuesta;
        }
    }
}