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
    public class DaoPedidoProv
    {
        OracleConnection conexion = BD.getConexion();
        //public List<PedidoProv> BuscarPedidoProv(int pid_ped_prov)
        //{
        //    List<PedidoProv> lista = new List<PedidoProv>();
        //    conexion.Open();
        //    OracleCommand comando = new OracleCommand("FN_BUSCAR_PEDIDO_PROV", conexion);
        //    comando.CommandType = System.Data.CommandType.StoredProcedure;
        //    OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
        //    comando.Parameters.Add(":pid_ped_prev", pid_ped_prov);
        //    output.Direction = ParameterDirection.ReturnValue;
        //    comando.ExecuteNonQuery();
        //    OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

        //    while (dr.Read())
        //    {
        //        var id_ped_prov = int.Parse(dr.GetInt32(0).ToString());
        //        var total = int.Parse(dr.GetInt32(1).ToString());
        //        var fecha = DateTime.Parse(dr.GetDateTime(2).ToString());
        //        var responsable = dr.GetString(3);
        //        var usuario_rut = dr.GetString(4);
        //        var proveedor_id_proveedor = int.Parse(dr.GetInt32(5).ToString());


        //        PedidoProv pedprov = new PedidoProv() { Id_ped_prov = id_ped_prov, Total = total, Fecha = fecha, Responsable = responsable, Usuario_rut = usuario_rut, Proveedor_id_proveedor = proveedor_id_proveedor };
        //        lista.Add(pedprov);
        //    }
        //    conexion.Close();
        //    return lista;
        //}
        public int GetIdPedido() // LISTO-------------
        {
            var id_ped_prov = 0;
            List<PedidoProv> lista = new List<PedidoProv>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_ID_PEDIDO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                id_ped_prov = int.Parse(dr.GetInt32(0).ToString());



                PedidoProv pedprov = new PedidoProv() { Id_ped_prov = id_ped_prov };
                lista.Add(pedprov);
            }
            conexion.Close();
            return id_ped_prov;
        }
        public List<PedidoProv> listarPedidoProv() // LISTO-------------
        {
            List<PedidoProv> lista = new List<PedidoProv>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_PEDIDO_PROV", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_ped_prov = int.Parse(dr.GetInt32(0).ToString());
                var total = int.Parse(dr.GetInt32(1).ToString());
                var fecha = dr.GetString(2);
                var responsable = dr.GetString(3);
                var usuario_rut = dr.GetString(4);
                var proveedor_id_proveedor = int.Parse(dr.GetInt32(5).ToString());


                PedidoProv pedprov = new PedidoProv() { Id_ped_prov = id_ped_prov, Total = total, Fecha = fecha, Responsable = responsable, Usuario_rut = usuario_rut, Proveedor_id_proveedor = proveedor_id_proveedor };
                lista.Add(pedprov);
            }
            conexion.Close();
            return lista;
        }

        public PedidoProv agregarPedidoProv(PedidoProv pedprov)// LISTO-------------VER INDICE RESTRINCCION
        {
            try
            {
                conexion.Open();
                OracleCommand cmd = new OracleCommand("SP_AGREGAR_PEDIDO_PROV", conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(":pid_ped_prov", pedprov.Id_ped_prov);
                cmd.Parameters.Add(":ptotal", pedprov.Total);
                cmd.Parameters.Add(":pfecha", pedprov.Fecha);
                cmd.Parameters.Add(":presponsable", pedprov.Responsable);
                cmd.Parameters.Add(":pusuario_rut", pedprov.Usuario_rut);
                cmd.Parameters.Add(":pproveedor_id_proveedor", pedprov.Proveedor_id_proveedor);
                int resultado = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                pedprov = null;
            }



            return pedprov;
        }

        public static bool EliminarPedidoProv(int pid_ped_prov)// LISTO-------------
        {
            var respuesta = false;
            DaoPedidoProv dao = new DaoPedidoProv();
            var existe = dao.existePedidoProv(pid_ped_prov);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_PEDIDO_PREV", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_ped_prov", pid_ped_prov);
                cmd.Parameters["@pid_ped_prov"].Value = (pid_ped_prov);
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

        public bool existePedidoProv(int pid_ped_prov)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_pedido FROM pedido WHERE Id_pedido LIKE :pid_pedido";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_PEDIDO_PROV", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_ped_prov",pid_ped_prov);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_ped_prov", pid_ped_prov);
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

        public static bool ActualizarPedidoProv(PedidoProv pedprov)// LISTO------------
        {
            int pid_ped_prov = 0;
            int ptotal = 0;
            string pfecha = "";
            string presponsable = "";
            string pusuario_rut = "";
            int pproveedor_id_proveedor = 0;

            pid_ped_prov = pedprov.Id_ped_prov;
            ptotal = pedprov.Total;
            pfecha = pedprov.Fecha;
            presponsable = pedprov.Responsable;
            pusuario_rut = pedprov.Usuario_rut;
            pproveedor_id_proveedor = pedprov.Proveedor_id_proveedor;

            var respuesta = false;
            DaoPedidoProv dao = new DaoPedidoProv();
            var existe = dao.existePedidoProv(pid_ped_prov);
            if (existe == true)
            {
                if (pid_ped_prov != 0 && ptotal != 0 && pfecha != "" && presponsable != "" && pusuario_rut != "" && pproveedor_id_proveedor != 0)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_PEDIDO_PROV", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_ped_prov", pid_ped_prov);
                    cmd.Parameters.Add("@ptotal", ptotal);
                    cmd.Parameters.Add("@pfecha", pfecha);
                    cmd.Parameters.Add("@presponsable", presponsable);
                    cmd.Parameters.Add("@pusuario_rut", pusuario_rut);
                    cmd.Parameters.Add("@pproveedor_id_proveedor", pproveedor_id_proveedor);

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