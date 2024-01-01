using Asp.Versioning;
using InnAi.Server.Dtos;
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
    public async Task<ActionResult<double[]>> PredictCurrentAsync(Guid? modelId)
    {
        try
        {
            var result = await _innAiService.PredictCurrentAsync(modelId);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, string.Empty);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("predict/history")]
    public async Task<ActionResult<HistoryResultDto>> PredictHistoryAsync(int year, int month, int day, int hour, Guid? modelId)
    {
        var dateTime = new DateTime(year, month, day, hour, 0, 0);

        try
        {
            var result = await _innAiService.PredictHistoryAsync(dateTime, modelId);

            var resultDto = new HistoryResultDto()
            {
                PredictionValues = result.PredictionValues,
                ActualValues = result.ActualValues,
                AverageDeviation = result.AverageDeviation,
                PercentageDeviation = result.PercentageDeviation
            };

            return Ok(resultDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, string.Empty);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}