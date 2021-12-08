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
    public class DaoRegistroPedProv
    {
        OracleConnection conexion = BD.getConexion();
        public List<RegistroPedProv> BuscarRegistroPedProv(int pid_reg_prov)
        {
            List<RegistroPedProv> lista = new List<RegistroPedProv>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_REGIS_PED_PROV", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_reg_prov", pid_reg_prov);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_reg_prov = int.Parse(dr.GetInt32(0).ToString());
                var producto_id_producto = int.Parse(dr.GetInt32(1).ToString());
                var precio_compra = int.Parse(dr.GetInt32(2).ToString());
                var cantidad = int.Parse(dr.GetInt32(3).ToString());
                var fecha_ingreso = DateTime.Parse(dr.GetDateTime(4).ToString());
                var fecha_vencimiento = DateTime.Parse(dr.GetDateTime(5).ToString());
                var pedido_prov_id_ped_prov = int.Parse(dr.GetInt32(6).ToString());


                RegistroPedProv regpedprov = new RegistroPedProv() { Id_reg_prov = id_reg_prov, Producto_id_producto = producto_id_producto, Precio_compra = precio_compra, Cantidad = cantidad, Fecha_ingreso = fecha_ingreso, Fecha_Vencimiento = fecha_vencimiento, Pedido_prov_id_ped_prov = pedido_prov_id_ped_prov };
                lista.Add(regpedprov);
            }
            conexion.Close();
            return lista;
        }

        public List<RegistroPedProv> listarRegistroPedProv() // LISTO-------------
        {
            List<RegistroPedProv> lista = new List<RegistroPedProv>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_REGIS_PED_PROV", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_reg_prov = int.Parse(dr.GetInt32(0).ToString());
                var producto_id_producto = int.Parse(dr.GetInt32(1).ToString());
                var precio_compra = int.Parse(dr.GetInt32(2).ToString());
                var cantidad = int.Parse(dr.GetInt32(3).ToString());
                var fecha_ingreso = DateTime.Parse(dr.GetDateTime(4).ToString());
                var fecha_vencimiento = DateTime.Parse(dr.GetDateTime(5).ToString());
                var pedido_prov_id_ped_prov = int.Parse(dr.GetInt32(6).ToString());


                RegistroPedProv regpedprov = new RegistroPedProv() { Id_reg_prov = id_reg_prov, Producto_id_producto = producto_id_producto, Precio_compra = precio_compra, Cantidad = cantidad, Fecha_ingreso = fecha_ingreso, Fecha_Vencimiento = fecha_vencimiento, Pedido_prov_id_ped_prov = pedido_prov_id_ped_prov };
                lista.Add(regpedprov);
            }
            conexion.Close();
            return lista;
        }

        public int agregarRegistroPedProv(RegistroPedProv regpedprov)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_REGIS_PED_PROV", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_reg_prov", regpedprov.Id_reg_prov);
            cmd.Parameters.Add(":pproducto_id_producto", regpedprov.Producto_id_producto);
            cmd.Parameters.Add(":pprecio_compra", regpedprov.Precio_compra);
            cmd.Parameters.Add(":pcantidad", regpedprov.Cantidad);
            cmd.Parameters.Add(":pfecha_ingreso", regpedprov.Fecha_ingreso);
            cmd.Parameters.Add(":pfecha_vencimiento", regpedprov.Fecha_Vencimiento);
            cmd.Parameters.Add(":ppedido_prov_id_ped_prov", regpedprov.Pedido_prov_id_ped_prov);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarRegistroPedProv(int pid_reg_prov)// LISTO-------------
        {
            var respuesta = false;
            DaoRegistroPedProv dao = new DaoRegistroPedProv();
            var existe = dao.existeRegistroPedProv(pid_reg_prov);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_REGIS_PED_PROV", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_reg_prov", pid_reg_prov);
                cmd.Parameters["@pid_reg_prov"].Value = (pid_reg_prov);
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

        public bool existeRegistroPedProv(int pid_reg_prov)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_reg_prov FROM registro_ped_prov WHERE Id_reg_prov LIKE :pid_reg_prov";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_REGIS_PED_PROV", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_reg_prov",pid_reg_prov);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_reg_prov", pid_reg_prov);
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

        public static bool ActualizarRegistroPedProv(RegistroPedProv regpedprov)// LISTO------------
        {
            int pid_reg_prov = 0;
            int pproducto_id_producto = 0;
            int pprecio_compra = 0;
            int pcantidad = 0;
            DateTime pfecha_ingreso = DateTime.Now;
            DateTime pfecha_vencimiento = DateTime.Now;
            int ppedido_prov_id_ped_prov = 0;

            pid_reg_prov = regpedprov.Id_reg_prov;
            pproducto_id_producto = regpedprov.Producto_id_producto;
            pprecio_compra = regpedprov.Precio_compra;
            pcantidad = regpedprov.Cantidad;
            pfecha_ingreso = regpedprov.Fecha_ingreso;
            pfecha_vencimiento = regpedprov.Fecha_Vencimiento;
            ppedido_prov_id_ped_prov = regpedprov.Pedido_prov_id_ped_prov;

            var respuesta = false;
            DaoRegistroPedProv dao = new DaoRegistroPedProv();
            var existe = dao.existeRegistroPedProv(pid_reg_prov);
            if (existe == true)
            {
                if (pid_reg_prov != 0 && pproducto_id_producto != 0 && pprecio_compra != 0 && pcantidad != 0 && pfecha_ingreso != DateTime.Now && pfecha_vencimiento != DateTime.Now && ppedido_prov_id_ped_prov != 0)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_REGIS_PED_PROV", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_reg_prov", pid_reg_prov);
                    cmd.Parameters.Add("@pproducto_id_producto", pproducto_id_producto);
                    cmd.Parameters.Add("@pprecio_compra", pprecio_compra);
                    cmd.Parameters.Add("@pcantidad", pcantidad);
                    cmd.Parameters.Add("@pfecha_ingreso", pfecha_ingreso);
                    cmd.Parameters.Add("@pfecha_vencimiento", pfecha_vencimiento);
                    cmd.Parameters.Add("@ppedido_prov_id_ped_prov", ppedido_prov_id_ped_prov);

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