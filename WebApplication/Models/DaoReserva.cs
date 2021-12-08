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
    public class DaoReserva
    {
        OracleConnection conexion = BD.getConexion();
        public List<Reserva> BuscarReserva(int pid_reserva)
        {
            List<Reserva> lista = new List<Reserva>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_RESERVA", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_reserva", pid_reserva);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_reserva = int.Parse(dr.GetInt32(0).ToString());
                var fecha_reserva = DateTime.Parse(dr.GetDateTime(1).ToString());
                var observacion = dr.GetString(2);
                var cliente_rut = dr.GetString(3);


                Reserva res = new Reserva() { Id_reserva = id_reserva, Fecha_reserva = fecha_reserva, Observacion = observacion, Cliente_rut = cliente_rut };
                lista.Add(res);
            }
            conexion.Close();
            return lista;
        }

        public List<Reserva> listarReserva() // LISTO-------------
        {
            List<Reserva> lista = new List<Reserva>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_RESERVA", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_reserva = int.Parse(dr.GetInt32(0).ToString());
                var fecha_reserva = DateTime.Parse(dr.GetDateTime(1).ToString());
                var observacion = dr.GetString(2);
                var cliente_rut = dr.GetString(3);


                Reserva res = new Reserva() { Id_reserva = id_reserva, Fecha_reserva = fecha_reserva, Observacion = observacion, Cliente_rut = cliente_rut };
                lista.Add(res);
            }
            conexion.Close();
            return lista;
        }

        public int agregarReserva(Reserva res)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_RESERVA", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_reserva", res.Id_reserva);
            cmd.Parameters.Add(":pfecha_reserva", res.Fecha_reserva);
            cmd.Parameters.Add(":pobservacion", res.Observacion);
            cmd.Parameters.Add(":pcliente_rut", res.Cliente_rut);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarReserva(int pid_reserva)// LISTO-------------
        {
            var respuesta = false;
            DaoReserva dao = new DaoReserva();
            var existe = dao.existeReserva(pid_reserva);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_RESERVA", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_reserva", pid_reserva);
                cmd.Parameters["@pid_reserva"].Value = (pid_reserva);
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

        public bool existeReserva(int pid_reserva)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_reserva FROM reserva WHERE Id_reserva LIKE :pid_reserva";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_RESERVA", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_reserva",pid_reserva);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_reserva", pid_reserva);
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

        public static bool ActualizarReserva(Reserva res)// LISTO------------
        {
            int pid_reserva = 0;
            DateTime pfecha_reserva = DateTime.Now;
            string pobservacion = "";
            string pcliente_rut = "";

            pid_reserva = res.Id_reserva;
            pfecha_reserva = res.Fecha_reserva;
            pobservacion = res.Observacion;
            pcliente_rut = res.Cliente_rut;

            var respuesta = false;
            DaoReserva dao = new DaoReserva();
            var existe = dao.existeReserva(pid_reserva);
            if (existe == true)
            {
                if (pid_reserva != 0 && pfecha_reserva != DateTime.Now && pobservacion != "" && pcliente_rut != "")
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_RESERVA", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_reserva", pid_reserva);
                    cmd.Parameters.Add("@pfecha_reserva", pfecha_reserva);
                    cmd.Parameters.Add("@pobservacion", pobservacion);
                    cmd.Parameters.Add("@pcliente_rut", pcliente_rut);

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