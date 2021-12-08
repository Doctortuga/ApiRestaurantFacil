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
    public class DaoTableroOrden
    {
        OracleConnection conexion = BD.getConexion();
        public List<TableroOrden> BuscarTableroOrden(int pid_tablero)
        {
            List<TableroOrden> lista = new List<TableroOrden>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_TABLERO_ORDEN", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_tablero", pid_tablero);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_tablero = int.Parse(dr.GetInt32(0).ToString());
                var reserva_id_reserva = int.Parse(dr.GetInt32(0).ToString());
                var detalle_pedido_id_det_ped = int.Parse(dr.GetInt32(0).ToString());

                TableroOrden tabor = new TableroOrden() { Id_tablero = id_tablero, Reserva_id_reserva = reserva_id_reserva, Detalle_pedido_id_det_ped = detalle_pedido_id_det_ped };
                lista.Add(tabor);
            }
            conexion.Close();
            return lista;
        }

        public List<TableroOrden> listarTableroOrden() // LISTO-------------
        {
            List<TableroOrden> lista = new List<TableroOrden>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_TABLERO_ORDEN", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_tablero = int.Parse(dr.GetInt32(0).ToString());
                var reserva_id_reserva = int.Parse(dr.GetInt32(0).ToString());
                var detalle_pedido_id_det_ped = int.Parse(dr.GetInt32(0).ToString());

                TableroOrden tabor = new TableroOrden() { Id_tablero = id_tablero, Reserva_id_reserva = reserva_id_reserva, Detalle_pedido_id_det_ped = detalle_pedido_id_det_ped };
                lista.Add(tabor);
            }
            conexion.Close();
            return lista;
        }

        public int agregarTableroOrden(TableroOrden tabor)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_TABLERO_ORDEN", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_tablero", tabor.Id_tablero);
            cmd.Parameters.Add(":preserva_id_reserva", tabor.Reserva_id_reserva);
            cmd.Parameters.Add(":pdetalle_pedido_id_det_ped", tabor.Detalle_pedido_id_det_ped);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarTableroOrden(int pid_tablero)// LISTO-------------
        {
            var respuesta = false;
            DaoTableroOrden dao = new DaoTableroOrden();
            var existe = dao.existeTableroOrden(pid_tablero);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_RESERVA", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_tablero", pid_tablero);
                cmd.Parameters["@pid_tablero"].Value = (pid_tablero);
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

        public bool existeTableroOrden(int pid_tablero)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_tablero FROM tablero_orden WHERE Id_tablero LIKE :pid_tablero";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_TABLERO_ORDEN", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_tablero",pid_tablero);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_tablero", pid_tablero);
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

        public static bool ActualizarTableroOrden(TableroOrden tabor)// LISTO------------
        {
            int pid_tablero = 0;
            int preserva_id_reserva = 0;
            int pdetalle_pedido_id_det_ped = 0;

            pid_tablero = tabor.Id_tablero;
            preserva_id_reserva = tabor.Reserva_id_reserva;
            pdetalle_pedido_id_det_ped = tabor.Detalle_pedido_id_det_ped;


            var respuesta = false;
            DaoTableroOrden dao = new DaoTableroOrden();
            var existe = dao.existeTableroOrden(pid_tablero);
            if (existe == true)
            {
                if (pid_tablero != 0 && preserva_id_reserva != 0 && pdetalle_pedido_id_det_ped != 0)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_TABLERO_ORDEN", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_tablero", pid_tablero);
                    cmd.Parameters.Add("@preserva_id_reserva", preserva_id_reserva);
                    cmd.Parameters.Add("@pdetalle_pedido_id_det_ped", pdetalle_pedido_id_det_ped);

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