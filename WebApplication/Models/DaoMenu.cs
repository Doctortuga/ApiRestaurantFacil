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
    public class DaoMenu
    {
        OracleConnection conexion = BD.getConexion();
        public List<Menu> BuscarMenu(int pid_menu)
        {
            List<Menu> lista = new List<Menu>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_MENU", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_menu", pid_menu);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_menu = int.Parse(dr.GetInt32(0).ToString());
                var nombre = dr.GetString(1);
                var precio = int.Parse(dr.GetInt32(2).ToString());
                var receta_id_receta = int.Parse(dr.GetInt32(3).ToString());
                var categoria = dr.GetString(4);


                Menu men = new Menu() { Id_menu = id_menu, Nombre = nombre, Precio = precio, Receta_id_receta = receta_id_receta, Categoria = categoria };
                lista.Add(men);
            }
            conexion.Close();
            return lista;
        }

        public List<Menu> listarMenu() // LISTO-------------
        {
            List<Menu> lista = new List<Menu>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_MENU", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_menu = int.Parse(dr.GetInt32(0).ToString());
                var nombre = dr.GetString(1);
                var precio = int.Parse(dr.GetInt32(2).ToString());
                var receta_id_receta = int.Parse(dr.GetInt32(3).ToString());
                var categoria = dr.GetString(4);


                Menu men = new Menu() { Id_menu = id_menu, Nombre = nombre, Precio = precio, Receta_id_receta = receta_id_receta, Categoria = categoria };
                lista.Add(men);
            }
            conexion.Close();
            return lista;
        }

        public int agregarMenu(Menu men)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_MENU", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_menu", men.Id_menu);
            cmd.Parameters.Add(":pnombre", men.Nombre);
            cmd.Parameters.Add(":pprecio", men.Precio);
            cmd.Parameters.Add(":preceta_id_receta", men.Receta_id_receta);
            cmd.Parameters.Add(":pcategoria", men.Categoria);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarMenu(int pid_menu)// LISTO-------------
        {
            var respuesta = false;
            DaoMenu dao = new DaoMenu();
            var existe = dao.existeMenu(pid_menu);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_MENU", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_menu", pid_menu);
                cmd.Parameters["@pid_menu"].Value = (pid_menu);
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

        public bool existeMenu(int pid_menu)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_menu FROM menu WHERE Id_menu LIKE :pid_menu";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_MENU", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_menu",pid_menu);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_menu", pid_menu);
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

        public static bool ActualizarMenu(Menu men)// LISTO------------
        {
            int pid_menu = 0;
            string pnombre = "";
            int pprecio = 0;
            int preceta_id_receta = 0;
            string pcategoria = "";

            pid_menu = men.Id_menu;
            pnombre = men.Nombre;
            pprecio = men.Precio;
            preceta_id_receta = men.Receta_id_receta;
            pcategoria = men.Categoria;

            var respuesta = false;
            DaoMenu dao = new DaoMenu();
            var existe = dao.existeMenu(pid_menu);
            if (existe == true)
            {
                if (pid_menu != 0 && pnombre != "" && pprecio != 0 && preceta_id_receta != 0 && pcategoria != "")
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_MENU", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_menu", pid_menu);
                    cmd.Parameters.Add("@pnombre", pnombre);
                    cmd.Parameters.Add("@pprecio", pprecio);
                    cmd.Parameters.Add("@preceta_id_receta", preceta_id_receta);
                    cmd.Parameters.Add("@pcategoria", pcategoria);
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