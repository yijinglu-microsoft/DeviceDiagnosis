using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatGPTEdge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceResultController : ControllerBase
    {
        public DeviceResultController()
        {
        }

        // <summary>
        /// Get details from 30 min device dataset
        /// </summary>
        [HttpGet(Name = "DeviceDetails")]
        public DeviceResult Get()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DeviceResult.json");

            DeviceResult deviceResult = null;

            // Read the content of the file
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();

                // Deserialize the JSON content into a List<DeviceDetail>
                deviceResult = JsonConvert.DeserializeObject<DeviceResult>(json);
            }

            return deviceResult;
        }

        [HttpGet("swagger.json")]
        public IActionResult ServeOpenApiJson()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "swagger.json");
                if (System.IO.File.Exists(filePath))
                {
                    return PhysicalFile(filePath, "application/json");
                }
                else
                {
                    return NotFound("File not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}