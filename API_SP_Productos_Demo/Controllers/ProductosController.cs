using API_SP_Productos_Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace API_SP_Productos_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ProductosController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("cadenaSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_productos", conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    using var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = Convert.ToInt32(rd["IdProducto"]),
                            CodigoBarra = rd["CodigoBarra"].ToString(),
                            Nombre = rd["Nombre"].ToString(),
                            Marca = rd["Marca"].ToString(),
                            Categoria = rd["Categoria"].ToString(),
                            Precio = Convert.ToDecimal(rd["Precio"])
                        });
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });

            }

        }

        [HttpGet]
        [Route("Buscar/{idProducto:int}")]
        public IActionResult Buscar(int idProducto) 
        {
            List<Producto> lista = new();
            Producto producto = new();

            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_buscar_producto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {

                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });
                        }

                    }
                }

                producto = lista.Where(item => item.IdProducto == idProducto).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = producto });

            }
        }

        [HttpPost]
        [Route("Crear")]
        public IActionResult Crear([FromBody] Producto objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_insertar_producto", conexion);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "agregado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

        [HttpPut]
        [Route("Actualizar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_actualizar_producto", conexion);
                    cmd.Parameters.AddWithValue("idProducto", objeto.IdProducto == 0 ? DBNull.Value : objeto.IdProducto);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.CodigoBarra is null ? DBNull.Value : objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre is null ? DBNull.Value : objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca is null ? DBNull.Value : objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria is null ? DBNull.Value : objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio == 0 ? DBNull.Value : objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_producto", conexion);
                    cmd.Parameters.AddWithValue("idProducto", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }
    }
}
