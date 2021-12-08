using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication.Models;
namespace WebApplication.Controllers
{
    public class ProductoController : ApiController
    {

        public IEnumerable<Producto> GetAll()
        {
            return new DaoProducto().listarProducto();
        }

        public bool AddProducto(Producto prod)
        {
            DaoProducto modelo = new DaoProducto();

            var respuesta = false;

            if (0 == prod.Id_Producto || string.IsNullOrEmpty(prod.Nombre) || string.IsNullOrEmpty(prod.Formato)
                || 0 == prod.Precio || string.IsNullOrEmpty(prod.Categoria) || string.IsNullOrEmpty(prod.Imagen))
            {
                respuesta = false;
            }
            else
            {

                if (modelo.existeProducto(prod.Id_Producto))
                {
                    respuesta = false;
                }
                else
                {

                    modelo.agregarProducto(prod);
                    respuesta = true;
                }

            }
            return respuesta;

        }

        public bool Put(Producto prod)
        {

            return DaoProducto.ActualizarProducto(prod);
        }

        public bool Delete(int id_producto)
        {
            return DaoProducto.EliminarProducto(id_producto);
        }

        [HttpPost]
        [Route("api/Producto/NombreImagen")]
        public async Task<string> NombreImagen()
        {
            var ctx = System.Web.HttpContext.Current;
            var root = ctx.Server.MapPath("~/Imagenes");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.FileData)
                {
                    var name = file.Headers.ContentDisposition.FileName;

                    name = name.Trim('"');

                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine(root, name);
                    File.Delete(filePath);
                    File.Move(localFileName, filePath);
                    

                }
            }
            catch (Exception e)
            {
                return $"Error:{e.Message}";
            }

            return "File uploaded!";
        }


    }
}