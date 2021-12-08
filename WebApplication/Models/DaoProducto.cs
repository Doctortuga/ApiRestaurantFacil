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
    public class DaoProducto
    {
        OracleConnection conexion = BD.getConexion();
        public List<Producto> listarProducto() // LISTO-------------
        {
            List<Producto> lista = new List<Producto>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_PRODUCTOS", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                int id = dr.GetInt32(0);
                string nombre = dr.GetString(1);
                int precio = dr.GetInt32(2);
                string categoria = dr.GetString(3).ToString();
                string formato = dr.GetString(4);
                string imagen = dr.GetString(5);

                Producto prod = new Producto()
                {
                    Id_Producto = id,
                    Nombre = nombre,
                    Categoria = categoria,
                    Precio = precio,
                    Formato = formato,
                    Imagen = imagen
                };
                lista.Add(prod);
            } 
            conexion.Close();
            return lista;
        }
        public int agregarProducto(Producto prod)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_PRODUCTO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_producto", prod.Id_Producto);
            cmd.Parameters.Add(":pnombre", prod.Nombre);
            cmd.Parameters.Add(":pprecio", prod.Precio);
            cmd.Parameters.Add(":pformato", prod.Formato);
            cmd.Parameters.Add(":pcategoria", prod.Categoria);
            cmd.Parameters.Add(":pimagen", prod.Imagen);
            
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }
        public bool existeProducto(int id_producto)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Rut FROM usuario WHERE Rut LIKE :prut";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_PRODUCTO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":prut",prut);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_producto", id_producto);
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
        public static bool ActualizarProducto(Producto prod)// LISTO------------
        {
            int pid_producto = 0;
            string pnombre = "";
            string pformato = "";
            string pcategoria = "";
            int pprecio = 0;
            string pimagen = "";
            pid_producto = prod.Id_Producto;
            pnombre = prod.Nombre;
            pformato = prod.Formato;
            pcategoria = prod.Categoria;
            pprecio = prod.Precio;
            pimagen = prod.Imagen;

            var respuesta = false;
            DaoProducto dao = new DaoProducto();
            var existe = dao.existeProducto(pid_producto);
            if (existe == true)
            {
                if (pid_producto != 0 && pnombre != "" && pformato != "" && pprecio != 0 && pcategoria != "" &&
                    pimagen != "")
                {

                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_PRODUCTO", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(":pid_producto", pid_producto);
                    cmd.Parameters.Add(":pnombre", pnombre);
                    cmd.Parameters.Add(":pprecio", pprecio);
                    cmd.Parameters.Add(":pformato", pformato);
                    cmd.Parameters.Add(":pcategoria", pcategoria);
                    cmd.Parameters.Add(":pimagen", pimagen);
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
        public static bool EliminarProducto(int pid_producto)// LISTO-------------
        {
            var respuesta = false;
            DaoProducto dao = new DaoProducto();
            var existe = dao.existeProducto(pid_producto);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_PRODUCTO", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_producto", pid_producto);
                cmd.Parameters["@pid_producto"].Value = (pid_producto);
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
    }
}