using Microsoft.AspNetCore.Mvc;
using WireguardWeb.Core.Entities;

namespace WireguardWeb.Api.Controllers;

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