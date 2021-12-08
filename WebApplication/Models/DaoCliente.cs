
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
    public class DaoCliente
    {
        OracleConnection conexion = BD.getConexion();
        public List<Cliente> BuscarCliente(string prut)// LISTO-------------
        {
            List<Cliente> lista = new List<Cliente>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_CLIENTES", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":prut", prut);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var rut = dr.GetString(0);
                var nombre = dr.GetString(1);
                var telefono = dr.GetString(2);
                var correo = dr.GetString(3);
                var id_rol = int.Parse(dr.GetInt32(4).ToString());

                Cliente cli = new Cliente() { Rut = rut, Nombre = nombre, Telefono = telefono, Correo = correo, Id_rol = id_rol };
                lista.Add(cli);
            }
            conexion.Close();
            return lista;
        }


        public List<Cliente> listarCliente() // LISTO-------------
        {
            List<Cliente> lista = new List<Cliente>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_CLIENTES", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var rut = dr.GetString(0).ToString();
                var nombre = dr.GetString(1);
                var telefono = dr.GetString(2);
                var correo = dr.GetString(4);
                var id_rol = int.Parse(dr.GetInt32(3).ToString());

                Cliente cli = new Cliente() { Rut = rut, Nombre = nombre, Telefono = telefono, Correo = correo, Id_rol = id_rol };
                lista.Add(cli);
            }
            conexion.Close();
            return lista;
        }

        public int agregarCliente(Cliente cli)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_CLIENTE", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":prut", cli.Rut);
            cmd.Parameters.Add(":pnombre", cli.Nombre);
            cmd.Parameters.Add(":ptelefono", cli.Telefono);
            cmd.Parameters.Add(":pid_rol", cli.Id_rol);
            cmd.Parameters.Add(":pcorreo", cli.Correo);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarCliente(string prut)// LISTO-------------
        {
            var respuesta = false;
            DaoCliente dao = new DaoCliente();
            var existe = dao.existeCliente(prut);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_CLIENTE", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@prut", prut);
                cmd.Parameters["@prut"].Value = (prut);
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

        public bool existeCliente(string prut)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Rut FROM Cliente WHERE Rut LIKE :prut";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_CLIENTE", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":prut",prut);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":prut", prut);
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

        public static bool ActualizarCliente(Cliente cli)// LISTO------------
        {
            string prut = "";
            string pnombre = "";
            string ptelefono = "";
            int pid_rol = 0;
            string pcorreo = "";
            prut = cli.Rut;
            pnombre = cli.Nombre;
            ptelefono = cli.Telefono;
            pid_rol = cli.Id_rol;


            var respuesta = false;
            DaoCliente dao = new DaoCliente();
            var existe = dao.existeCliente(cli.Rut);
            if (existe == true)
            {
                if (prut != "" && pnombre != "" && ptelefono != "" && pid_rol != 0 && pcorreo != "")
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_CLIENTE", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@prut", prut);
                    cmd.Parameters.Add("@pnombre", pnombre);
                    cmd.Parameters.Add("@ptelefono", ptelefono);
                    cmd.Parameters.Add("@pid_rol", pid_rol);
                    cmd.Parameters.Add("@pcorreo", pcorreo);
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