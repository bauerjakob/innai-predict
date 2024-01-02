using InnAi.Server.Data.Entities;
using InnAi.Server.Dtos;

namespace InnAiServer.Services;

public interface IInnAiService
{
    public Task<double[]> PredictCurrentAsync(Guid? modelId = null);
    public Task<(double[] PredictionValues, double[] ActualValues, double AverageDeviation, double PercentageDeviation)> PredictHistoryAsync(DateTime dateTime, Guid? modelId = null);
    public Task<IEnumerable<AiModel>> GetAiModelsAsync();
    public Task CreateAiModelAsync(CreateModelRequestDto model);
}