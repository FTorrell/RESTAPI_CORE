using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RESTAPI_CORE.Modelos;
using System.Data;
using System.Data.SqlTypes;


namespace RESTAPI_CORE.Controllers
{
    [Route("api/[controller]")]
    [Authorize]

    public class DeviceTypeController : ControllerBase
    {
        private readonly string cadenaSQL;
        public DeviceTypeController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        //REFERENCIAS
        //MODELO
        //SQL

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<DeviceType> lista = new List<DeviceType>();

            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_devicetype", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {

                            lista.Add(new DeviceType
                            {
                                IdDevice = Convert.ToInt32(rd["Id"]),
                                Description = rd["Description"].ToString(),
                                DisplayName = rd["DisplayName"].ToString()
                            });
                        }

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
        [Route("Obtener/{idDeviceType:int}")]
        public IActionResult Obtener(int idDeviceType)
        {

            List<DeviceType> lista = new List<DeviceType>();
            DeviceType odevicetype = new DeviceType();

            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_devicetype", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {

                            lista.Add(new DeviceType
                            {
                                IdDevice = Convert.ToInt32(rd["Id"]),
                                Description = rd["Description"].ToString(),
                                DisplayName = rd["DisplayName"].ToString()
                            });
                        }

                    }
                }

                odevicetype = lista.Where(item => item.IdDevice== idDeviceType).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = odevicetype });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = odevicetype });

            }
        }



        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] DeviceType objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_devicetype", conexion);
                    cmd.Parameters.AddWithValue("description", objeto.Description);
                    cmd.Parameters.AddWithValue("displayname", objeto.DisplayName);
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
        [Route("Editar")]
        public IActionResult Editar([FromBody] DeviceType objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_devicetype", conexion);
                    cmd.Parameters.AddWithValue("deviceid", objeto.IdDevice == 0 ? DBNull.Value : objeto.IdDevice);
                    cmd.Parameters.AddWithValue("description", objeto.Description is null ? DBNull.Value : objeto.Description);
                    cmd.Parameters.AddWithValue("displayname", objeto.DisplayName is null ? DBNull.Value : objeto.DisplayName);
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
        [Route("Eliminar/{idDeviceType:int}")]
        public IActionResult Eliminar(int idDeviceType)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_devicetype", conexion);
                    cmd.Parameters.AddWithValue("iddevicetype", idDeviceType);
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
