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
    public class DaoGarzon
    {
        OracleConnection conexion = BD.getConexion();
        public List<Garzon> BuscarGarzon(int pid_garzon)
        {
            List<Garzon> lista = new List<Garzon>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_GARZON", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_garzon", pid_garzon);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_garzon = int.Parse(dr.GetInt32(0).ToString());
                var nombre = dr.GetString(1);
                var usuario_rut = dr.GetString(2);

                Garzon gar = new Garzon() { Id_Garzon = id_garzon, Nombre = nombre, Usuario_Rut = usuario_rut };
                lista.Add(gar);
            }
            conexion.Close();
            return lista;
        }

        public List<Garzon> listarGarzon() // LISTO-------------
        {
            List<Garzon> lista = new List<Garzon>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_GARZON", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_garzon = int.Parse(dr.GetInt32(0).ToString());
                var nombre = dr.GetString(1);
                var usuario_rut = dr.GetString(2);

                Garzon gar = new Garzon() { Id_Garzon = id_garzon, Nombre = nombre, Usuario_Rut = usuario_rut };
                lista.Add(gar);
            }
            conexion.Close();
            return lista;
        }

        public int agregarGarzon(Garzon gar)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_GARZON", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_gar", gar.Id_Garzon);
            cmd.Parameters.Add(":pnombre", gar.Nombre);
            cmd.Parameters.Add(":pusuario_rut", gar.Usuario_Rut);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarGarzon(int pid_garzon)// LISTO-------------
        {
            var respuesta = false;
            DaoGarzon dao = new DaoGarzon();
            var existe = dao.existeGarzon(pid_garzon);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_GARZON", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_garzon", pid_garzon);
                cmd.Parameters["@pid_garzon"].Value = (pid_garzon);
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

        public bool existeGarzon(int pid_garzon)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_garzon FROM garzon WHERE Id_garzon LIKE :pid_garzon";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_GARZON", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_garzon",pid_garzon);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_garzon", pid_garzon);
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

        public static bool ActualizarGarzon(Garzon gar)// LISTO------------
        {
            int pid_garzon = 0;
            string pnombre = "";
            string pusuario_rut = "";
            pid_garzon = gar.Id_Garzon;
            pnombre = gar.Nombre;
            pusuario_rut = gar.Usuario_Rut;

            var respuesta = false;
            DaoGarzon dao = new DaoGarzon();
            var existe = dao.existeGarzon(pid_garzon);
            if (existe == true)
            {
                if (pid_garzon != 0 && pnombre != "" && pusuario_rut != "")
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_GARZON", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_garzon", pid_garzon);
                    cmd.Parameters.Add("@pnombre", pnombre);
                    cmd.Parameters.Add("@pusuario_rut", pusuario_rut);
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