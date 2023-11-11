using Microsoft.AspNetCore.Mvc;
using ProtectVpnWeb.Core.Dto;
using ProtectVpnWeb.Core.Dto.Connection;

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
    public IActionResult AddConnection([FromQuery] CreateConnectionDto dto)
    {
        
        return Ok(dto);
    }
    
}