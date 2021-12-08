using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ClienteController : ApiController
    {
        //    api/cliente
        public bool ctrlRegistro(Cliente cli)
        {
            DaoCliente modelo = new DaoCliente();
            var respuesta = false;

            if (string.IsNullOrEmpty(cli.Rut) || string.IsNullOrEmpty(cli.Nombre) || string.IsNullOrEmpty(cli.Telefono) || string.IsNullOrEmpty(cli.Correo) || (0 == (cli.Id_rol)))
            {
                respuesta = false;
            }
            else
            {
                if (modelo.existeCliente(cli.Rut))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarCliente(cli);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public IEnumerable<Cliente> Get(string rut)
        {
            return new DaoCliente().BuscarCliente(rut);
        }

        public IEnumerable<Cliente> GetAll()
        {
            return new DaoCliente().listarCliente();
        }

        public bool Put(Cliente cli)
        {

            return DaoCliente.ActualizarCliente(cli);
        }

        public bool Delete(string Rut)
        {
            return DaoCliente.EliminarCliente(Rut);
        }

        //public IEnumerable<string> Get()
        //{

        //    return new string[] { "value1", "value2" };
        //}

    }
}