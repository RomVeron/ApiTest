using Api.Test.Domain.Application.Interfaces;
using Api.Test.Domain.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Test.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TestController : ControllerBase
{
    private readonly ITestCommandHandler _testHandler;
    private readonly ILogger<TestController> _logger;

    public TestController(ITestCommandHandler testHandler, ILogger<TestController> logger)
    {
        _testHandler = testHandler;
        _logger = logger;
    }
    
    [HttpGet, Route("users")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            _logger.LogInformation("Iniciar proceso de obtener test");
            var result = await _testHandler.Handle();

            if (result.Any())
            {
                return Ok(result);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio un error al obtener las solicitudes");

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet, Route("users/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            _logger.LogInformation("Iniciar proceso de obtener test");
            var result = await _testHandler.GetUser(id);

            if (result.Id != 0)
            {
                return Ok(result);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio un error al obtener las solicitudes");

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPost, Route("users")]
    public async Task<IActionResult> CreateUser(UserDto userDto)
    {
        try
        {
            _logger.LogInformation("Iniciar proceso de obtener test");
            var result = await _testHandler.CreateUser(userDto);

            if (result.Id != 0)
            {
                return Created("",result);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio un error al obtener las solicitudes");

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}