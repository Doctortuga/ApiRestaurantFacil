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
    public class DaoIngredientesReceta
    {
        OracleConnection conexion = BD.getConexion();
        public List<IngredientesReceta> BuscarIngredientesReceta(int pid_ingredientes)
        {
            List<IngredientesReceta> lista = new List<IngredientesReceta>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_INGRE_RECETA", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_ingredientes", pid_ingredientes);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_ingredientes = int.Parse(dr.GetInt32(0).ToString());
                var receta_id_receta = int.Parse(dr.GetInt32(1).ToString());
                var cantidad = int.Parse(dr.GetInt32(2).ToString());
                var producto_id_producto = int.Parse(dr.GetInt32(3).ToString());

                IngredientesReceta ingrec = new IngredientesReceta() { Id_ingredientes = id_ingredientes, Receta_id_receta = receta_id_receta, Cantidad = cantidad, Producto_id_producto = producto_id_producto };
                lista.Add(ingrec);
            }
            conexion.Close();
            return lista;
        }

        public List<IngredientesReceta> listarIngredientesReceta() // LISTO-------------
        {
            List<IngredientesReceta> lista = new List<IngredientesReceta>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_INGRE_RECETA", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_ingredientes = int.Parse(dr.GetInt32(0).ToString());
                var receta_id_receta = int.Parse(dr.GetInt32(1).ToString());
                var cantidad = int.Parse(dr.GetInt32(2).ToString());
                var producto_id_producto = int.Parse(dr.GetInt32(3).ToString());

                IngredientesReceta ingrec = new IngredientesReceta() { Id_ingredientes = id_ingredientes, Receta_id_receta = receta_id_receta, Cantidad = cantidad, Producto_id_producto = producto_id_producto };
                lista.Add(ingrec);
            }
            conexion.Close();
            return lista;
        }

        public int agregarIngredientesReceta(IngredientesReceta ingrec)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_INGRE_RECETA", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_ingredintes", ingrec.Id_ingredientes);
            cmd.Parameters.Add(":preceta_id_receta", ingrec.Receta_id_receta);
            cmd.Parameters.Add(":pcantidad", ingrec.Cantidad);
            cmd.Parameters.Add(":pproducto_id_producto", ingrec.Producto_id_producto);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarIngredientesReceta(int pid_ingredientes)// LISTO-------------
        {
            var respuesta = false;
            DaoIngredientesReceta dao = new DaoIngredientesReceta();
            var existe = dao.existeIngredientesReceta(pid_ingredientes);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_INGRE_RECETA", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_ingredientes", pid_ingredientes);
                cmd.Parameters["@pid_ingredientes"].Value = (pid_ingredientes);
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

        public bool existeIngredientesReceta(int pid_ingredientes)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_ingredientes FROM ingrdientes_receta WHERE Id_ingredientes LIKE :pid_ingredientes";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_GARZON", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_ingredientes",pid_ingredientes);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_ingredientes", pid_ingredientes);
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

        public static bool ActualizarIngredientesReceta(IngredientesReceta ingrec)// LISTO------------
        {
            int pid_ingredientes = 0;
            int preceta_id_receta = 0;
            int pcantidad = 0;
            int pproducto_id_producto = 0;
            pid_ingredientes = ingrec.Id_ingredientes;
            preceta_id_receta = ingrec.Receta_id_receta;
            pcantidad = ingrec.Cantidad;
            pproducto_id_producto = ingrec.Producto_id_producto;

            var respuesta = false;
            DaoIngredientesReceta dao = new DaoIngredientesReceta();
            var existe = dao.existeIngredientesReceta(pid_ingredientes);
            if (existe == true)
            {
                if (pid_ingredientes != 0 && preceta_id_receta != 0 && pcantidad != 0 && pproducto_id_producto != 0)
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_GARZON", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_ingredientes", pid_ingredientes);
                    cmd.Parameters.Add("@preceta_id_receta", preceta_id_receta);
                    cmd.Parameters.Add("@pcantidad", pcantidad);
                    cmd.Parameters.Add("@pproducto_id_producto", pproducto_id_producto);
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