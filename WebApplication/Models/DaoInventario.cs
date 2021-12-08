using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Collections.Generic;
using System.Data;

namespace WebApplication.Models
{
    public class DaoInventario
    {
        OracleConnection conexion = BD.getConexion();
        public List<Inventario> BuscarInventario(int pid_inventario)
        {
            List<Inventario> lista = new List<Inventario>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_BUSCAR_INVENTARIO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            comando.Parameters.Add(":pid_inventario", pid_inventario);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();

            while (dr.Read())
            {
                var id_inventario = int.Parse(dr.GetInt32(0).ToString());
                var stock = int.Parse(dr.GetInt32(1).ToString());
                var registro_ped_prov_prod_id_prod = int.Parse(dr.GetInt32(2).ToString());
                var registro_ped_prov_id_reg_prov = int.Parse(dr.GetInt32(3).ToString());
                var usuario_rut = dr.GetString(4);
                var id_producto = dr.GetString(5);

                Inventario inv = new Inventario() { Id_Inventario = id_inventario, Stock = stock, Registro_Ped_Prov_Prod_Id_Prod = registro_ped_prov_prod_id_prod, Registro_Ped_Prov_Id_Reg_Prov = registro_ped_prov_id_reg_prov, Usuario_rut = usuario_rut, Id_Producto = id_producto };
                lista.Add(inv);
            }
            conexion.Close();
            return lista;
        }

        public List<Inventario> listarInventario() // LISTO-------------
        {
            List<Inventario> lista = new List<Inventario>();
            conexion.Open();
            OracleCommand comando = new OracleCommand("FN_LISTAR_INVENTARIO", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter output = comando.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            output.Direction = ParameterDirection.ReturnValue;
            comando.ExecuteNonQuery();
            OracleDataReader dr = ((OracleRefCursor)output.Value).GetDataReader();
            while (dr.Read())
            {
                var id_inventario = int.Parse(dr.GetInt32(0).ToString());
                var stock = int.Parse(dr.GetInt32(1).ToString());
                var registro_ped_prov_prod_id_prod = int.Parse(dr.GetInt32(2).ToString());
                var registro_ped_prov_id_reg_prov = int.Parse(dr.GetInt32(3).ToString());
                var usuario_rut = dr.GetString(4);
                var id_producto = dr.GetString(5);

                Inventario inv = new Inventario() { Id_Inventario = id_inventario, Stock = stock, Registro_Ped_Prov_Prod_Id_Prod = registro_ped_prov_prod_id_prod, Registro_Ped_Prov_Id_Reg_Prov = registro_ped_prov_id_reg_prov, Usuario_rut = usuario_rut, Id_Producto = id_producto };
                lista.Add(inv);
            }
            conexion.Close();
            return lista;
        }

        public int agregarInventario(Inventario inv)// LISTO-------------VER INDICE RESTRINCCION
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand("SP_AGREGAR_INVENTARIO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(":pid_inventario", inv.Id_Inventario);
            cmd.Parameters.Add(":pstock", inv.Stock);
            cmd.Parameters.Add(":preg_ped_prov_prod_id_prod", inv.Registro_Ped_Prov_Prod_Id_Prod);
            cmd.Parameters.Add(":pregistro_ped_prov_id_reg_prov", inv.Registro_Ped_Prov_Id_Reg_Prov);
            cmd.Parameters.Add(":pusuario_rut", inv.Usuario_rut);
            cmd.Parameters.Add(":pid_producto", inv.Id_Producto);
            int resultado = cmd.ExecuteNonQuery();
            return resultado;
        }

        public static bool EliminarInventario(int pid_inventario)// LISTO-------------
        {
            var respuesta = false;
            DaoInventario dao = new DaoInventario();
            var existe = dao.existeInventario(pid_inventario);
            if (existe == true)
            {
                var ora = BD.getConexion();
                OracleDataReader reader;
                OracleCommand cmd = new OracleCommand("SP_ELIMINAR_INVENTARIO", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pid_inventario", pid_inventario);
                cmd.Parameters["@pid_inventario"].Value = (pid_inventario);
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

        public bool existeInventario(int pid_inventario)// LISTO------------
        {
            OracleDataReader reader;
            OracleConnection conexion = BD.getConexion();
            conexion.Open();

            //string sql = "SELECT Id_Inventario FROM inventario WHERE Id_Inventario LIKE :pid_inventario";
            //OracleCommand comando = new OracleCommand(sql, conexion);
            OracleCommand cmd = new OracleCommand("FN_EXISTE_INVENTARIO", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(":pid_inventario",pid_inventario);

            //reader = cmd.ExecuteReader();
            OracleParameter output = cmd.Parameters.Add("l_cursor", OracleDbType.RefCursor);
            cmd.Parameters.Add(":pid_inventario", pid_inventario);
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

        public static bool ActualizarInventario(Inventario inv)// LISTO------------
        {
            int pid_inventario = 0;
            int pstock = 0;
            int preg_p_p_p_id_prod = 0;
            int preg_p_p_i_reg_prov = 0;
            string pusuario_rut = "";
            string pid_producto = "";

            pid_inventario = inv.Id_Inventario;
            pstock = inv.Stock;
            preg_p_p_p_id_prod = inv.Registro_Ped_Prov_Prod_Id_Prod;
            preg_p_p_i_reg_prov = inv.Registro_Ped_Prov_Id_Reg_Prov;
            pusuario_rut = inv.Usuario_rut;
            pid_producto = inv.Id_Producto;

            var respuesta = false;
            DaoInventario dao = new DaoInventario();
            var existe = dao.existeInventario(pid_inventario);
            if (existe == true)
            {
                if (pid_inventario != 0 && pstock != 0 && preg_p_p_p_id_prod != 0 && preg_p_p_i_reg_prov != 0 && pusuario_rut != "" && pid_producto != "")
                {
                    var ora = BD.getConexion();
                    OracleDataReader reader;
                    OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_INVENTARIO", ora);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pid_inventario", pid_inventario);
                    cmd.Parameters.Add("@pstock", pstock);
                    cmd.Parameters.Add("@preg_ped_prov_prod_id_prod", preg_p_p_p_id_prod);
                    cmd.Parameters.Add("@pregistro_ped_prov_id_reg_prov", preg_p_p_i_reg_prov);
                    cmd.Parameters.Add("@pusuario_rut", pusuario_rut);
                    cmd.Parameters.Add(":pid_producto", pid_producto);
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