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
    public class DaoPago
    {
        OracleConnection conexion = BD.getConexion();
        public List<Pago> BuscarPago(int pid_pago)
        {
            List<Pago> lista = new List<Pago>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_PAGO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_pago", pid_pago);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_pago = int.Parse(dr.GetInt32(0).ToString());
                var medio_pago = dr.GetString(1);
                var total = int.Parse(dr.GetInt32(2).ToString());
                var fecha = DateTime.Parse(dr.GetDateTime(3).ToString());
                var pedido_id_pedido = int.Parse(dr.GetInt32(4).ToString());


                Pago pag = new Pago() { Id_pago = id_pago, Medio_pago = medio_pago, Total = total, Fecha = fecha, Pedido_id_pedido = pedido_id_pedido };
                lista.Add(pag);
            }
            conexion.Close();
            return lista;
        }

        public List<Pago> listarPago() // LISTO-------------
        {
            List<Pago> lista = new List<Pago>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_PAGO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_pago = int.Parse(dr.GetInt32(0).ToString());
                var medio_pago = dr.GetString(1);
                var total = int.Parse(dr.GetInt32(2).ToString());
                var fecha = DateTime.Parse(dr.GetDateTime(3).ToString());
                var pedido_id_pedido = int.Parse(dr.GetInt32(4).ToString());


                Pago pag = new Pago() { Id_pago = id_pago, Medio_pago = medio_pago, Total = total, Fecha = fecha, Pedido_id_pedido = pedido_id_pedido };
                lista.Add(pag);
            }
            conexion.Close();
            return lista;
        }

        public int agregarPago(Pago pag)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_PAGO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pia_pago", pag.Id_pago);
            cmd.Parameters.Add(":pmedio_pago", pag.Medio_pago);
            cmd.Parameters.Add(":ptotal", pag.Total);
            cmd.Parameters.Add(":pfecha", pag.Fecha);
            cmd.Parameters.Add(":ppedido_id_pedido", pag.Pedido_id_pedido);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarPago(int pid_pago)// LISTO-------------
        {
            var respuesta = false;
            DaoPago dao = new DaoPago();
            var existe = dao.existePago(pid_pago);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_PAGO", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_pago", pid_pago);
                cmd.Parameters["@pid_pago"].Value = (pid_pago);
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

        public bool existePago(int pid_pago)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_pago FROM pago WHERE Id_pago LIKE :pid_pago";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_PAGO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_pago",pid_pago);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_pago", pid_pago);
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

        public static bool ActualizarPago(Pago pag)// LISTO------------
        {
            int pid_pago = 0;
            string pmedio_pago = "";
            int ptotal = 0;
            DateTime pfecha = DateTime.Now;
            int ppedido_id_pedido = 0;

            pid_pago = pag.Id_pago;
            pmedio_pago = pag.Medio_pago;
            ptotal = pag.Total;
            pfecha = pag.Fecha;
            ppedido_id_pedido = pag.Pedido_id_pedido;

            var respuesta = false;
            DaoPago dao = new DaoPago();
            var existe = dao.existePago(pid_pago);
            if (existe == true)
            {
                if (pid_pago != 0 && pmedio_pago != "" && ptotal != 0 && pfecha != DateTime.Now && ppedido_id_pedido != 0)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_MENU", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_pago", pid_pago);
                    cmd.Parameters.Add("@pmedio_pago", pmedio_pago);
                    cmd.Parameters.Add("@ptotal", ptotal);
                    cmd.Parameters.Add("@pfecha", pfecha);
                    cmd.Parameters.Add("@ppedido_id_pedido", ppedido_id_pedido);

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