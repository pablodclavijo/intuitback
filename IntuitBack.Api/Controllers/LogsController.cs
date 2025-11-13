using Microsoft.AspNetCore.Mvc;
using IntuitBack.Application.Interfaces;
using Mapster;
using IntuitBack.IntuitBack.Application.DTOs.Log;

namespace IntuitBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly ILogService _logService;

    public LogsController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int ultimos = 100)
    {
        var logs = await _logService.ObtenerUltimosAsync(ultimos);
        var logsDto = logs.Adapt<IEnumerable<GetLogDto>>();
        return Ok(logsDto);
    }
}
