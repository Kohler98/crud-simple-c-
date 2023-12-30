using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Api_pruebas.Models;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Cors;
namespace Api_pruebas.Controllers
{
    [EnableCors("ReglasCors.")]
    [Route("api/[controller]")]
    [ApiController]
    public class productoController : ControllerBase
    {
        public readonly DbapiContext _dbcontext;

        public productoController(DbapiContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Lista")]

        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                lista = _dbcontext.Productos.Include(c => c.oCategoria).ToList();
            
                return StatusCode(StatusCodes.Status200OK, new {mensaje = "ok",response = lista});
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new {mensaje = ex.Message,response = lista});

            }
        }
           
        [HttpGet]
        [Route("Obtener/{idProducto:int}")]

        public IActionResult Obtener(int idProducto)
        {
        Producto oProducto = _dbcontext.Productos.Find(idProducto);

            if(oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto = _dbcontext.Productos.Include(c => c.oCategoria).Where(p => p.IdProducto == idProducto).FirstOrDefault();
            
                return StatusCode(StatusCodes.Status200OK, new {mensaje = "ok",response = oProducto });
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new {mensaje = ex.Message,response = oProducto });

            }
        }

        [HttpPost]
        [Route("Guardar")]

        public IActionResult Guardar([FromBody] Producto producto)
        {
            try
            {
                _dbcontext.Productos.Add(producto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new {mensaje = "ok"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new {mensaje = ex.Message});

            }
        }        
        [HttpPut]
        [Route("Editar/{id:int}")]

        public IActionResult Editar([FromBody] Producto producto, int id)
        {
          
            Producto oProducto = _dbcontext.Productos.Find(id);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }
            try
            {
                oProducto.CodigoBarra = producto.CodigoBarra is null ? oProducto.CodigoBarra : producto.CodigoBarra;
                oProducto.Descripcion = producto.Descripcion is null ? oProducto.Descripcion : producto.Descripcion;
                oProducto.Marca = producto.Marca is null ? oProducto.Marca : producto.Marca;
                oProducto.IdCategoria = producto.IdCategoria is null ? oProducto.IdCategoria : producto.IdCategoria;
                oProducto.Precio = producto.Precio is null ? oProducto.Precio : producto.Precio;
                _dbcontext.Productos.Update(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new {mensaje = "ok"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new {mensaje = ex.Message});

            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            Producto oProducto = _dbcontext.Productos.Find(id);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }
            try
            {
                _dbcontext.Productos.Remove(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });

            }
        }
    }
}
