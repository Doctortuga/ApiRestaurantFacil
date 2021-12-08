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
    public class DaoStockPendiente
    {
        OracleConnection conexion = BD.getConexion();
        public List<StockPendiente> BuscarStockPendiente(int pid_stock_pend)
        {
            List<StockPendiente> lista = new List<StockPendiente>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_STOCK_PENDIENTE", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_stock_pend", pid_stock_pend);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                

                var id_stock_pend = int.Parse(dr.GetInt32(0).ToString());
                var disponibilidad = dr.GetString(1);
                var producto_id_producto = int.Parse(dr.GetInt32(2).ToString());
                var precio = int.Parse(dr.GetInt32(3).ToString());
                var cantidad = int.Parse(dr.GetInt32(4).ToString());
                var fecha_ingreso = DateTime.Parse(dr.GetDateTime(5).ToString());
                var fecha_vencimiento = DateTime.Parse(dr.GetDateTime(6).ToString());
                var stock_id_reg_prov = int.Parse(dr.GetInt32(7).ToString());


                StockPendiente stopen = new StockPendiente() { Id_stock_pend = id_stock_pend, Disponibilidad = disponibilidad, Producto_id_producto = producto_id_producto, Precio = precio, Cantidad = cantidad, Fecha_ingreso = fecha_ingreso, Fecha_vencimiento = fecha_vencimiento, Stock_id_reg_prov = stock_id_reg_prov };
                lista.Add(stopen);
            }
            conexion.Close();
            return lista;
        }

        public List<StockPendiente> listarStockPendiente() // LISTO-------------
        {
            List<StockPendiente> lista = new List<StockPendiente>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_STOCK_PENDIENTE", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_stock_pend = int.Parse(dr.GetInt32(0).ToString());
                var disponibilidad = dr.GetString(1);
                var producto_id_producto = int.Parse(dr.GetInt32(2).ToString());
                var precio = int.Parse(dr.GetInt32(3).ToString());
                var cantidad = int.Parse(dr.GetInt32(4).ToString());
                var fecha_ingreso = DateTime.Parse(dr.GetDateTime(5).ToString());
                var fecha_vencimiento = DateTime.Parse(dr.GetDateTime(6).ToString());
                var stock_id_reg_prov = int.Parse(dr.GetInt32(7).ToString());


                StockPendiente stopen = new StockPendiente() { Id_stock_pend = id_stock_pend, Disponibilidad = disponibilidad, Producto_id_producto = producto_id_producto, Precio = precio, Cantidad = cantidad, Fecha_ingreso = fecha_ingreso, Fecha_vencimiento = fecha_vencimiento, Stock_id_reg_prov = stock_id_reg_prov };
                lista.Add(stopen);
            }
            conexion.Close();
            return lista;
        }

        public int agregarStockPendiente(StockPendiente stopen)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_STOCK_PENDIENTE", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_stock_pend", stopen.Id_stock_pend);
            cmd.Parameters.Add(":pdisponibilidad", stopen.Disponibilidad);
            cmd.Parameters.Add(":pproducto_id_producto", stopen.Producto_id_producto);
            cmd.Parameters.Add(":pprecio", stopen.Precio);
            cmd.Parameters.Add(":pcantidad", stopen.Cantidad);
            cmd.Parameters.Add(":pfecha_ingreso", stopen.Fecha_ingreso);
            cmd.Parameters.Add(":pfecha_vencimiento", stopen.Fecha_vencimiento);
            cmd.Parameters.Add(":pstock_id_reg_prov", stopen.Stock_id_reg_prov);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarStockPendiente(int pid_stock_pend)// LISTO-------------
        {
            var respuesta = false;
            DaoStockPendiente dao = new DaoStockPendiente();
            var existe = dao.existeStockPendiente(pid_stock_pend);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_STOCK_PENDIENTE", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_stock_pend", pid_stock_pend);
                cmd.Parameters["@pid_stock_pend"].Value = (pid_stock_pend);
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

        public bool existeStockPendiente(int pid_stock_pend)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT id_stock_pend FROM stock_pendiente WHERE id_stock_pend LIKE :pid_stock_pend";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_STOCK_PENDIENTE", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_stock_pend",pid_stock_pend);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_stock_pend", pid_stock_pend);
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

        public static bool ActualizarStockPendiente(StockPendiente stopen)// LISTO------------
        {
            int pid_stock_pend = 0;
            string pdisponibilidad = "";
            int pproducto_id_producto = 0;
            int pprecio = 0;
            int pcantidad = 0;
            DateTime pfecha_ingreso = DateTime.Now;
            DateTime pfecha_vencimiento = DateTime.Now;
            int pstock_id_reg_prov = 0;

            pid_stock_pend = stopen.Id_stock_pend;
            pdisponibilidad = stopen.Disponibilidad;
            pproducto_id_producto = stopen.Producto_id_producto;
            pprecio = stopen.Precio;
            pcantidad = stopen.Cantidad;
            pfecha_ingreso = stopen.Fecha_ingreso;
            pfecha_vencimiento = stopen.Fecha_vencimiento;
            pstock_id_reg_prov = stopen.Stock_id_reg_prov;

            var respuesta = false;
            DaoStockPendiente dao = new DaoStockPendiente();
            var existe = dao.existeStockPendiente(pid_stock_pend);
            if (existe == true)
            {
                if (pid_stock_pend != 0 && pdisponibilidad != "" && pproducto_id_producto != 0 && pprecio != 0 && pcantidad != 0 && pfecha_ingreso != DateTime.Now && pfecha_vencimiento != DateTime.Now && pstock_id_reg_prov != 0)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_STOCK_PENDIENTE", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_stock_pend", pid_stock_pend);
                    cmd.Parameters.Add("@pdisponibilidad", pdisponibilidad);
                    cmd.Parameters.Add("@pproducto_id_producto", pproducto_id_producto);
                    cmd.Parameters.Add("@pprecio", pprecio);
                    cmd.Parameters.Add("@pcantidad", pcantidad);
                    cmd.Parameters.Add("@pfecha_ingreso", pfecha_ingreso);
                    cmd.Parameters.Add("@pfecha_vencimiento", pfecha_vencimiento);
                    cmd.Parameters.Add("@pstock_id_reg_prov", pstock_id_reg_prov);

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