using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace QuickStaff.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ODataController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}