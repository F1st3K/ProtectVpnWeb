using Microsoft.AspNetCore.Mvc;
using Wireguard.ServiceLibrary.Domains;
using Wireguard.ServiceLibrary.Entities;

namespace WireguardWeb.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    [HttpGet]
    public IActionResult AddNewConnection([FromQuery] WGConnectionEntity connectionEntity)
    {
        var businessLogic = new WGConnection();
        businessLogic.Save(connectionEntity);

        return Ok();
    }
}