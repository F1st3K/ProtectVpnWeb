using Microsoft.AspNetCore.Mvc;
using Wireguard.ServiceLibrary.Domains;
using WireguardWeb.Core.Entities;

namespace WireguardWeb.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    [HttpGet]
    public IActionResult AddNewConnection([FromQuery] ConnectionEntity connectionEntity)
    {
        var businessLogic = new WGConnection();
        businessLogic.Save(connectionEntity);

        return Ok();
    }
}