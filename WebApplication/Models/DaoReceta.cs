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
    public class DaoReceta
    {
        OracleConnection conexion = BD.getConexion();
        public List<Receta> BuscarReceta(int pid_receta)
        {
            List<Receta> lista = new List<Receta>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_RECETA", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_receta", pid_receta);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_receta = int.Parse(dr.GetInt32(0).ToString());
                var tiempo_preparacion = DateTime.Parse(dr.GetDateTime(2).ToString());


                Receta rec = new Receta() { Id_receta = id_receta, Tiempo_preparacion = tiempo_preparacion };
                lista.Add(rec);
            }
            conexion.Close();
            return lista;
        }

        public List<Receta> listarReceta() // LISTO-------------
        {
            List<Receta> lista = new List<Receta>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_RECETA", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_receta = int.Parse(dr.GetInt32(0).ToString());
                var tiempo_preparacion = DateTime.Parse(dr.GetDateTime(2).ToString());


                Receta rec = new Receta() { Id_receta = id_receta, Tiempo_preparacion = tiempo_preparacion };
                lista.Add(rec);
            }
            conexion.Close();
            return lista;
        }

        public int agregarReceta(Receta rec)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_RECETA", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_receta", rec.Id_receta);
            cmd.Parameters.Add(":ptiempo_preparacion", rec.Tiempo_preparacion);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarReceta(int pid_receta)// LISTO-------------
        {
            var respuesta = false;
            DaoReceta dao = new DaoReceta();
            var existe = dao.existeReceta(pid_receta);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_RECETA", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_receta", pid_receta);
                cmd.Parameters["@pid_receta"].Value = (pid_receta);
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

        public bool existeReceta(int pid_receta)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_receta FROM receta WHERE Id_receta LIKE :pid_receta";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_RECETA", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_receta",pid_receta);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_receta", pid_receta);
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

        public static bool ActualizarReceta(Receta rec)// LISTO------------
        {
            int pid_receta = 0;
            DateTime ptiempo_preparacion = DateTime.Now;

            pid_receta = rec.Id_receta;
            ptiempo_preparacion = rec.Tiempo_preparacion;

            var respuesta = false;
            DaoReceta dao = new DaoReceta();
            var existe = dao.existeReceta(pid_receta);
            if (existe == true)
            {
                if (pid_receta != 0 && ptiempo_preparacion != DateTime.Now)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_RECETA", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_receta", pid_receta);
                    cmd.Parameters.Add("@ptiempo_preparacion", ptiempo_preparacion);

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