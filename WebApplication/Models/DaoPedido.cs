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
    public class DaoPedido
    {
        OracleConnection conexion = BD.getConexion();
        public List<Pedido> BuscarPedido(int pid_pedido)
        {
            List<Pedido> lista = new List<Pedido>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_PEDIDO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_pedido", pid_pedido);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_pedido = int.Parse(dr.GetInt32(0).ToString());
                var fecha = DateTime.Parse(dr.GetDateTime(1).ToString());
                var total = int.Parse(dr.GetInt32(2).ToString());
                var mesa_nro_mesa = int.Parse(dr.GetInt32(3).ToString());
                var garzon_id_garzon = int.Parse(dr.GetInt32(4).ToString());


                Pedido ped = new Pedido() { Id_pedido = id_pedido, Fecha = fecha, Total = total, Mesa_nro_mesa = mesa_nro_mesa, Garzon_id_garzon = garzon_id_garzon };
                lista.Add(ped);
            }
            conexion.Close();
            return lista;
        }

        public List<Pedido> listarPedido() // LISTO-------------
        {
            List<Pedido> lista = new List<Pedido>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_PEDIDO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_pedido = int.Parse(dr.GetInt32(0).ToString());
                var fecha = DateTime.Parse(dr.GetDateTime(1).ToString());
                var total = int.Parse(dr.GetInt32(2).ToString());
                var mesa_nro_mesa = int.Parse(dr.GetInt32(3).ToString());
                var garzon_id_garzon = int.Parse(dr.GetInt32(4).ToString());


                Pedido ped = new Pedido() { Id_pedido = id_pedido, Fecha = fecha, Total = total, Mesa_nro_mesa = mesa_nro_mesa, Garzon_id_garzon = garzon_id_garzon };
                lista.Add(ped);
            }
            conexion.Close();
            return lista;
        }

        public int agregarPedido(Pedido ped)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_PEDIDO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pia_pedido", ped.Id_pedido);
            cmd.Parameters.Add(":pfecha", ped.Fecha);
            cmd.Parameters.Add(":ptotal", ped.Total);
            cmd.Parameters.Add(":pmesa_nro_mesa", ped.Mesa_nro_mesa);
            cmd.Parameters.Add(":pgarzon_id_garzon", ped.Garzon_id_garzon);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarPedido(int pid_pedido)// LISTO-------------
        {
            var respuesta = false;
            DaoPedido dao = new DaoPedido();
            var existe = dao.existePedido(pid_pedido);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_PEDIDO", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_pedido", pid_pedido);
                cmd.Parameters["@pid_pedido"].Value = (pid_pedido);
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

        public bool existePedido(int pid_pedido)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_pedido FROM pedido WHERE Id_pedido LIKE :pid_pedido";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_PEDIDO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_pedido",pid_pedido);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_pedido", pid_pedido);
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

        public static bool ActualizarPedido(Pedido ped)// LISTO------------
        {
            int pid_pedido = 0;
            DateTime pfecha = DateTime.Now;
            int ptotal = 0;
            int pmesa_nro_mesa = 0;
            int pgarzon_id_garzon = 0;

            pid_pedido = ped.Id_pedido;
            pfecha = ped.Fecha;
            ptotal = ped.Total;
            pmesa_nro_mesa = ped.Mesa_nro_mesa;
            pgarzon_id_garzon = ped.Garzon_id_garzon;

            var respuesta = false;
            DaoPedido dao = new DaoPedido();
            var existe = dao.existePedido(pid_pedido);
            if (existe == true)
            {
                if (pid_pedido != 0 && pfecha != DateTime.Now && ptotal != 0 && pmesa_nro_mesa != 0 && pgarzon_id_garzon != 0)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_PEDIDO", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_pedido", pid_pedido);
                    cmd.Parameters.Add("@pfecha", pfecha);
                    cmd.Parameters.Add("@ptotal", ptotal);
                    cmd.Parameters.Add("@pmesa_nro_mesa", pmesa_nro_mesa);
                    cmd.Parameters.Add("@pgarzon_id_garzon", pgarzon_id_garzon);

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