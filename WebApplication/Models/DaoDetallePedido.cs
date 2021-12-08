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
    public class DaoDetallePedido
    {
        OracleConnection conexion = BD.getConexion();
        public List<DetallePedido> BuscarDetallePedido(int pid_detalle)// LISTO-------------
        {
            List<DetallePedido> lista = new List<DetallePedido>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_DETALLE_PEDIDO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_detalle", pid_detalle);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_detalle_pedido = int.Parse(dr.GetInt32(0).ToString());
                var menu_id_menu = int.Parse(dr.GetInt32(1).ToString());
                var realizado = dr.GetString(2).ToString();
                var tiempo_inicio = DateTime.Parse(dr.GetDateTime(3).ToString());
                var tiempo_termino = DateTime.Parse(dr.GetDateTime(4).ToString());
                var pedido_id_pedido = int.Parse(dr.GetInt32(5).ToString());


                DetallePedido dp = new DetallePedido() { Id_detalle_pedido = id_detalle_pedido, Menu_id_menu = menu_id_menu, Realizado = realizado, Tiempo_inicio = tiempo_inicio, Tiempo_termino = tiempo_termino, Pedido_id_pedido = pedido_id_pedido };
                lista.Add(dp);
            }
            conexion.Close();
            return lista;
        }

        public List<DetallePedido> listarDetallePedido() // LISTO-------------
        {
            List<DetallePedido> lista = new List<DetallePedido>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_DETALLE_PEDIDO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_detalle_pedido = int.Parse(dr.GetInt32(0).ToString());
                var menu_id_menu = int.Parse(dr.GetInt32(1).ToString());
                var realizado = dr.GetString(2).ToString();
                var tiempo_inicio = DateTime.Parse(dr.GetDateTime(3).ToString());
                var tiempo_termino = DateTime.Parse(dr.GetDateTime(4).ToString());
                var pedido_id_pedido = int.Parse(dr.GetInt32(5).ToString());


                DetallePedido dp = new DetallePedido() { Id_detalle_pedido = id_detalle_pedido, Menu_id_menu = menu_id_menu, Realizado = realizado, Tiempo_inicio = tiempo_inicio, Tiempo_termino = tiempo_termino, Pedido_id_pedido = pedido_id_pedido };
                lista.Add(dp);
            }
            conexion.Close();
            return lista;
        }

        public int agregarDetallePedido(DetallePedido dp)
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_DETALLE_PEDIDO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_detalle", dp.Id_detalle_pedido);
            cmd.Parameters.Add(":pmenu_id_menu", dp.Menu_id_menu);
            cmd.Parameters.Add(":prealizado", dp.Realizado);
            cmd.Parameters.Add(":ptiempo_inicio", dp.Tiempo_inicio);
            cmd.Parameters.Add(":ptiempo_termino", dp.Tiempo_termino);
            cmd.Parameters.Add(":ppedido_id_pedido", dp.Pedido_id_pedido);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarDetallePedido(int pid_detalle)// LISTO-------------
        {
            var respuesta = false;
            DaoDetallePedido dao = new DaoDetallePedido();
            var existe = dao.existeDetallePedido(pid_detalle);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_DETALLE_PEDIDO", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_detalle", pid_detalle);
                cmd.Parameters["@pid_detalle"].Value = (pid_detalle);
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

        public bool existeDetallePedido(int pid_detalle)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_detalle_pedido FROM detalle_pedido WHERE Id_detalle_pedido LIKE :pid_detalle";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_DETALLE_PEDIDO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_detalle",pid_detalle);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_detalle", pid_detalle);
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

        public static bool ActualizarDetallePedido(DetallePedido dp)// LISTO------------
        {
            int pid_detalle = 0;
            int pmenu_id_menu = 0;
            string prealizado = "";
            DateTime ptiempo_inicio = DateTime.Now;
            DateTime ptiempo_termino = DateTime.Now;
            int ppedido_id_pedido = 0;
            pid_detalle = dp.Id_detalle_pedido;
            pmenu_id_menu = dp.Menu_id_menu;
            prealizado = dp.Realizado;
            ptiempo_inicio = dp.Tiempo_inicio;
            ptiempo_termino = dp.Tiempo_termino;
            ppedido_id_pedido = dp.Pedido_id_pedido;


            var respuesta = false;
            DaoDetallePedido dao = new DaoDetallePedido();
            var existe = dao.existeDetallePedido(dp.Id_detalle_pedido);
            if (existe == true)
            {
                if (pid_detalle != 0 && pmenu_id_menu != 0 && prealizado != "" && ptiempo_inicio != DateTime.Now && ptiempo_termino != DateTime.Now && ppedido_id_pedido != 0)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_DETALLE_PEDIDO", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(":pid_detalle", dp.Id_detalle_pedido);
                    cmd.Parameters.Add(":pmenu_id_menu", dp.Menu_id_menu);
                    cmd.Parameters.Add(":prealizado", dp.Realizado);
                    cmd.Parameters.Add(":ptiempo_inicio", dp.Tiempo_inicio);
                    cmd.Parameters.Add(":ptiempo_termino", dp.Tiempo_termino);
                    cmd.Parameters.Add(":ppedido_id_pedido", dp.Pedido_id_pedido);
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