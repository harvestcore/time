using Microsoft.AspNetCore.Mvc;

namespace RestApp.Controllers;

public record Status(bool Ok);

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new Status(true));
    }
}
