using Microsoft.AspNetCore.Mvc;
using WireguardWeb.Core.Domains;
using WireguardWeb.Core.Entities;

namespace WireguardWeb.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    [HttpGet]
    public IActionResult AddNewConnection([FromQuery] Connection connection)
    {
        var businessLogic = new WGConnection();
        businessLogic.Save(connection);

        return Ok();
    }
}