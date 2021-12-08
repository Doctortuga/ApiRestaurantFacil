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
    public class DaoRol
    {
        OracleConnection conexion = BD.getConexion();
        public List<Rol> BuscarRol(int pid_rol)// LISTO-------------
        {
            List<Rol> lista = new List<Rol>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_ROL", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_rol", pid_rol);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_rol = int.Parse(dr.GetInt32(0).ToString());
                var nombre = dr.GetString(1).ToString();


                Rol role = new Rol() { Id_Rol = id_rol, Nombre = nombre };
                lista.Add(role);
            }
            conexion.Close();
            return lista;
        }

        public List<Rol> listarRol() // LISTO-------------
        {
            List<Rol> lista = new List<Rol>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_ROL", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_rol = int.Parse(dr.GetInt32(0).ToString());
                var nombre = dr.GetString(1).ToString();


                Rol role = new Rol() { Id_Rol = id_rol, Nombre = nombre };
                lista.Add(role);
            }
            conexion.Close();
            return lista;
        }

        public int agregarRol(Rol role)
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_ROL", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_rol", role.Id_Rol);
            cmd.Parameters.Add(":pnombre", role.Nombre);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarRol(int pid_rol)// LISTO-------------
        {
            var respuesta = false;
            DaoRol dao = new DaoRol();
            var existe = dao.existeRol(pid_rol);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_ROL", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_rol", pid_rol);
                cmd.Parameters["@pid_rol"].Value = (pid_rol);
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

        public bool existeRol(int pid_rol)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_rol FROM rol WHERE Id_rol LIKE :pid_rol";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_PRODUCTO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_rol",pid_rol);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_rol", pid_rol);
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

        public static bool ActualizarRol(Rol role)// LISTO------------
        {
            int pid_rol = 0;
            string pnombre = "";
            pid_rol = role.Id_Rol;
            pnombre = role.Nombre;


            var respuesta = false;
            DaoRol dao = new DaoRol();
            var existe = dao.existeRol(role.Id_Rol);
            if (existe == true)
            {
                if (pid_rol != 0 && pnombre != "")
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_ROL", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_rol", pid_rol);
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