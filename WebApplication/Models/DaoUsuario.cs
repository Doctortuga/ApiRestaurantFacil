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
    public class DaoUsuario
    {
        OracleConnection conexion = BD.getConexion();
        public List<Usuario> BuscarUsuario(string prut)// LISTO-------------
        {
            List<Usuario> lista = new List<Usuario>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_USUARIOS", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor",OracleDbType.RefCursor);
            comando.Parameters.Add(":prut", prut);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                 var rut = dr.GetString(0).ToString();
                 var nombre = dr.GetString(1);
                 var contraseña = dr.GetString(2);
                 var id_rol = int.Parse(dr.GetInt32(3).ToString());

                Usuario usu = new Usuario() { Rut = rut, Nombre = nombre, Contraseña = contraseña ,Id_rol=id_rol };
                lista.Add(usu);
            }
            conexion.Close();
            return lista;
        }
        public List<Usuario> listarUsuario() // LISTO-------------
        {
            List<Usuario> lista = new List<Usuario>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_USUARIOS", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var rut = dr.GetString(0).ToString();
                var nombre = dr.GetString(1);
                var contraseña = dr.GetString(2);
                var id_rol = int.Parse(dr.GetInt32(3).ToString());

                Usuario usu = new Usuario() { Rut = rut, Nombre = nombre, Contraseña = contraseña, Id_rol = id_rol };
                lista.Add(usu);
            }
            conexion.Close();
            return lista;
        }

        public int agregarUsuario(Usuario usu)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_USUARIO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":prut", usu.Rut);
            cmd.Parameters.Add(":pnombre", usu.Nombre);
            cmd.Parameters.Add(":pclave", usu.Contraseña);
            cmd.Parameters.Add(":pid_rol", usu.Id_rol);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarUsuario(string prut)// LISTO-------------
        {
            var respuesta = false;
            DaoUsuario dao = new DaoUsuario();
            var existe = dao.existeUsuario(prut);
            if(existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_USUARIO", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@prut", prut);
                cmd.Parameters["@prut"].Value = (prut);
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
        public static bool ActualizarUsuario(Usuario usu)// LISTO------------
        {
            string prut = "";
            string pnombre =""; 
            string pclave=""; 
            int pid_rol=0;
            prut = usu.Rut;
            pnombre = usu.Nombre;
            pclave = usu.Contraseña;
            pid_rol = usu.Id_rol;

            var respuesta = false;
            DaoUsuario dao = new DaoUsuario();
            var existe = dao.existeUsuario(prut);
            if (existe == true)
            {
                if (prut != "" && pnombre != "" && pclave != "" && pid_rol != 0)
                {
                    var Sha1 = dao.generarSHA1(pclave);
                    pclave = Sha1;
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_USUARIO", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@prut", prut);
                    cmd.Parameters.Add("@pnombre", pnombre);
                    cmd.Parameters.Add("@pclave", pclave);
                    cmd.Parameters.Add("@pid_rol", pid_rol);
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



        public bool existeUsuario(string prut)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Rut FROM usuario WHERE Rut LIKE :prut";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_USUARIO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":prut",prut);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":prut", prut);
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
        public Usuario porUsuario(string prut)// LISTO------------
        {


            OracleConnection conexion = BD.getConexion();
            conexion.Open();
            OracleCommand cmd = new OracleCommand("FN_BUSCAR_USUARIOS", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":prut", prut);
            output.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            var flag = 1;
            Usuario usr = null;
            OracleDataReader reader = cmd.ExecuteReader(); // hacer consulta con todos los datos del anexo E
            if (reader.HasRows)
            {
                flag = 1;
                reader.Read();
                usr = new Usuario();
                usr.Rut = reader["Rut"].ToString();
                usr.Contraseña = reader["Contrasena"].ToString();
                usr.Nombre = reader["Nombre"].ToString();
                usr.Id_rol = Convert.ToInt32(reader["ROL_ID_ROL"]);
            }
            else
            {
                flag = 0;
            }

            return usr;
        }
        public string generarSHA1(string cadena)
        {
            UTF8Encoding enc = new UTF8Encoding();
            byte[] data = enc.GetBytes(cadena);
            byte[] result;

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();

            result = sha.ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {

                if (result[i] < 16)
                {
                    sb.Append("0");
                }
                sb.Append(result[i].ToString("x"));
            }

            return sb.ToString();
        }
    }
}