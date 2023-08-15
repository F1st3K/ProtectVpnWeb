using Microsoft.AspNetCore.Mvc;
using ProtectVpnWeb.Core.Entities;

namespace ProtectVpnWeb.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllConnection()
    {

        return Ok();
    }
    
    [HttpGet("{id}")]
    public IActionResult GetConnection(string id)
    {

        return Ok(id);
    }
    
    [HttpPost]
    public IActionResult AddConnection([FromQuery] string id)
    {
        return Ok(id);
    }
    
}