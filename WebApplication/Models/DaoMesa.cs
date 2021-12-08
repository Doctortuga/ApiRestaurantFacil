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
    public class DaoMesa
    {
        OracleConnection conexion = BD.getConexion();
        public List<Mesa> listarMesa() // LISTO-------------
        {
            List<Mesa> lista = new List<Mesa>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_MESAS", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                int nro_mesa = dr.GetInt32(0);
                string disponibilidad = dr.GetString(1);
                int id_reserva = dr.GetInt32(2);
                int capacidad = dr.GetInt32(3);


                Mesa mes = new Mesa() { Nro_mesa = nro_mesa, Disponibilidad = disponibilidad, Id_reserva = id_reserva , Capacidad = capacidad };
                lista.Add(mes);
            }
            conexion.Close();
            return lista;
        }
        public int agregarMesa(Mesa mes)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_MESA", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pnro_mesa", mes.Nro_mesa);
            cmd.Parameters.Add(":pdisponibilidad", mes.Disponibilidad);
            cmd.Parameters.Add(":pid_reserva", mes.Id_reserva);
            cmd.Parameters.Add(":pcapacidad", mes.Capacidad);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }
        public static bool EliminarMesa(int pnro_mesa)// LISTO-------------
        {
            var respuesta = false;
            DaoMesa dao = new DaoMesa();
            var existe = dao.existeMesa(pnro_mesa);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_MESA", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pnro_mesa", pnro_mesa);
                cmd.Parameters["@pnro_mesa"].Value = (pnro_mesa);
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
        public bool existeMesa(int pnro_mesa)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Rut FROM usuario WHERE Rut LIKE :prut";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_MESA", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":prut",prut);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pnro_mesa", pnro_mesa);
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
        public static bool ActualizarMESA(Mesa mes)// LISTO------------
        {
            int pnro_mesa = 0;
            string pdisponibilidad = "";
            int pid_reserva = 0;
            int pcapacidad = 0;
            pnro_mesa = mes.Nro_mesa;
            pdisponibilidad = mes.Disponibilidad;
            pid_reserva = mes.Id_reserva;
            pcapacidad = mes.Capacidad;

            var respuesta = false;
            DaoMesa dao = new DaoMesa();
            var existe = dao.existeMesa(pnro_mesa);
            if (existe == true)
            {
                if (pnro_mesa != 0 && pdisponibilidad != "" && pid_reserva != 0  && pcapacidad != 0)
                {

                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_MESA", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(":pnro_mesa", pnro_mesa);
                    cmd.Parameters.Add(":pdisponibilidad", pdisponibilidad);
                    cmd.Parameters.Add(":pid_reserva", pid_reserva);
                    cmd.Parameters.Add(":pcapacidad", pcapacidad);
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
        public List<MesasTotales> MesasTotales()// LISTO-------------
        {
            List<MesasTotales> lista = new List<MesasTotales>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_MESAS_TOTALES", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                int disponible = dr.GetInt32(0) ;
                int ocupadas = dr.GetInt32(1);
                int totales = dr.GetInt32(2);

                MesasTotales mes = new MesasTotales() {  Mesas_Disponibles = disponible, Mesas_Ocupadas = ocupadas, Mesas_Totales = totales};
                lista.Add(mes);
            }
            conexion.Close();
            return lista;
        }
    }
}