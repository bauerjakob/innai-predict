using Asp.Versioning;
using InnAiServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace InnAi.Server.Controllers;

[ApiController()]
[Route("api/v1/innAi")]
[ApiVersion("1.0")]
public class InnAiController : ControllerBase
{
    private readonly ILogger<InnAiController> _logger;
    private readonly IInnAiService _innAiService;

    public InnAiController(ILogger<InnAiController> logger, IInnAiService innAiService)
    {
        _logger = logger;
        _innAiService = innAiService;
    }

    [HttpGet("predict/current")]
    public async Task<ActionResult<double[]>> PredictCurrentAsync()
    {

        try
        {
            var result = await _innAiService.PredictCurrentAsync();
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, string.Empty);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("predict")]
    public async Task<ActionResult<double[]>> PredictCurrentAsync(int year, int month, int day, int hour)
    {
        var dateTime = new DateTime(year, month, day, hour, 0, 0);

        try
        {
            var result = await _innAiService.PredictAsync(dateTime);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, string.Empty);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("actual")]
    public async Task<ActionResult<double[]>> GetActualAsync(int year, int month, int day, int hour)
    {
        var dateTime = new DateTime(year, month, day, hour, 0, 0);

        try
        {
            var actualValues = await _innAiService.GetActualAsync(dateTime);
            return Ok(actualValues);
        }
        catch (Exception e)
        {
            _logger.LogError(e, string.Empty);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}