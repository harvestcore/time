using Microsoft.AspNetCore.Mvc;

namespace RestApp.Controllers
{
    public class Status
    {
        public bool Ok { get; set; }
    }

    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Status()
            {
                Ok = true,
            });
        }
    }
}
