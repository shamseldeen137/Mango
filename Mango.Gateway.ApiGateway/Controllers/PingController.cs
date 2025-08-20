using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Gateway.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var instanceId = Environment.MachineName;
            return Ok($"Response from {instanceId}");
        }

    }
}
