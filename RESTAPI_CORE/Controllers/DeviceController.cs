using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RESTAPI_CORE.Modelos;
using System.Data;
using Microsoft.Data.SqlClient;


namespace RESTAPI_CORE.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly string cadenaSQL;
        public DeviceController(IConfiguration config) {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        //REFERENCIAS
        //MODELO
        //SQL

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista() { 
            
            List<Device> lista = new List<Device>();

            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_device", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {

                            lista.Add(new Device
                            {
                                IdDevice = Convert.ToInt32(rd["Id"]),
                                DeviceTypeId = Convert.ToInt32(rd["DeviceTypeId"]),
                                DisplayName = rd["DisplayName"].ToString(),
                                ControllerId = Convert.ToInt32(rd["ControllerId"]),
                                Enabled = Convert.ToBoolean(rd["Enabled"])
                            });
                        }

                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" , response = lista });
            }
            catch(Exception error) {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message,  response = lista });

            }
        }

        [HttpGet]
        //[Route("Obtener")] // => Obtener?idProducto=13 | ampersand
        [Route("Obtener/{idDevice:int}")]
        public IActionResult Obtener(int idDevice)
        {
            List<Device> lista = new List<Device>();
            Device odevice = new Device();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_device", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Device
                            {
                                IdDevice = Convert.ToInt32(rd["Id"]),
                                DeviceTypeId = Convert.ToInt32(rd["DeviceTypeId"]),
                                DisplayName = rd["DisplayName"].ToString(),
                                ControllerId = Convert.ToInt32(rd["ControllerId"]),
                                Enabled = Convert.ToBoolean(rd["Enabled"])
                            });
                        }

                    }
                }

                odevice = lista.Where(item => item.IdDevice == idDevice).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = odevice });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = odevice });

            }
        }



        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Device objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_device", conexion);
                    cmd.Parameters.AddWithValue("devicetypeid", objeto.DeviceTypeId);
                    cmd.Parameters.AddWithValue("displayname", objeto.DisplayName);
                    cmd.Parameters.AddWithValue("controllerid", objeto.ControllerId);
                    cmd.Parameters.AddWithValue("enabled", objeto.Enabled);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "agregado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message});

            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Device objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_device", conexion);
                    cmd.Parameters.AddWithValue("deviceid", objeto.IdDevice == 0 ? DBNull.Value : objeto.IdDevice);
                    cmd.Parameters.AddWithValue("devicetypeid", objeto.DeviceTypeId == 0 ? DBNull.Value : objeto.DeviceTypeId);
                    cmd.Parameters.AddWithValue("displayname", objeto.DisplayName is null ? DBNull.Value : objeto.DisplayName);
                    cmd.Parameters.AddWithValue("controllerid", objeto.ControllerId == 0 ? DBNull.Value : objeto.ControllerId);
                    cmd.Parameters.AddWithValue("enabled", objeto.Enabled);
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
        [Route("Eliminar/{idDevice:int}")]
        public IActionResult Eliminar(int idDevice)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_device", conexion);
                    cmd.Parameters.AddWithValue("idDevice", idDevice);
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
