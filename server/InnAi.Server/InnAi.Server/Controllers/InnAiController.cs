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
    public async Task<ActionResult<HistoryResultDto>> PredictHistoryAsync(int year, int month, int day, int hour,
        Guid? modelId)
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
    
    [HttpGet("models")]
    public async Task<ActionResult<AiModelDto[]>> GetAiModelsAsync()
    {
        try
        {
            var aiModels = await _innAiService.GetAiModelsAsync();

            var result = aiModels.Select(x => new AiModelDto {Id = x.ExternalId, Name = x.Name, Default = x.Default});
            
            return Ok(result.ToArray());
        }
        catch (Exception e)
        {
            _logger.LogError(e, string.Empty);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    // [HttpPost("model")]
    // public async Task<IActionResult> CretaeModelAsync(CreateModelRequestDto dto)
    // {
    //     try
    //     {
    //         await _innAiService.CreateAiModelAsync(dto);
    //         return Created();
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e, string.Empty);
    //         return StatusCode(StatusCodes.Status500InternalServerError);
    //     }
    //     
    // }

    [HttpGet("evaluate/month/{modelId}")]
    public async Task<ActionResult<EvaluateMonthDto>> EvaluateMonthAsync(Guid modelId, int year, int month)
    {
        var dateTime = new DateTime(year, month, 1);

        var days = DateTime.DaysInMonth(year, month);

        List<EvaluateDayDto> daysEvaluated = new();

        for (var i = 0; i < days; i++)
        {
            var predictionResult = await _innAiService.PredictHistoryAsync(dateTime, modelId);

            var evaluation = new EvaluateDayDto(dateTime, predictionResult.AverageDeviation);
            daysEvaluated.Add(evaluation);
            
            dateTime += TimeSpan.FromDays(1);
        }

        var sumDeviation = daysEvaluated.Sum(x => x.averageDeviation);
        var averageDeviation = sumDeviation / daysEvaluated.Count;

        return Ok(new EvaluateMonthDto(daysEvaluated.ToArray(), sumDeviation, averageDeviation));

    }
    
    
}