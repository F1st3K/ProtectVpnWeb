using Microsoft.AspNetCore.Mvc;
using WireguardWeb.Core.Entities;

namespace WireguardWeb.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    [HttpGet]
    public IActionResult AddNewConnection([FromQuery] Connection connection)
    {

        return Ok();
    }
}