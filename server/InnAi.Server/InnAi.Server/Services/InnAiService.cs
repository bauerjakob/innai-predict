using InnAi.Server.Data.Entities;
using InnAi.Server.Data.Repositories;
using InnAi.Server.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace InnAiServer.Services;

public class InnAiService(
    ILogger<InnAiService> logger,
    IPredictionService predictionService,
    IMainDbRepository dbRepository,
    IMemoryCache memoryCache)
    : IInnAiService
{
    public async Task<double[]> PredictCurrentAsync(Guid? modelId = null)
    {
        var now = DateTime.Now;
        var modelIdNotNull = modelId ?? (await dbRepository.GetDefaultAiModelAsync()).ExternalId;
        var cacheKey =
            $"{nameof(InnAiService)}.{nameof(PredictCurrentAsync)}.{modelIdNotNull}.{now.Year}.{now.Month}.{now.Day}.{now.Hour}";

        return await memoryCache.GetOrCreateAsync(cacheKey, async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            
            return await predictionService.PredictCurrentAsync(modelIdNotNull);
        }) ?? throw new Exception();
    }

    public async Task<(double[] PredictionValues, double[] ActualValues, double AverageDeviation, double PercentageDeviation)> PredictHistoryAsync(DateTime dateTime, Guid? modelId = null)
    {
        var now = DateTime.UtcNow;

        if (!(now - TimeSpan.FromDays(1) > dateTime))
        {
            throw new Exception();
        }
        
        var aiModel = modelId.HasValue ? await dbRepository.GetAiModelAsync(modelId.Value) : await dbRepository.GetDefaultAiModelAsync();
        var aiModelId = aiModel.ExternalId;

        var dbModelResult = await dbRepository.GetAiModelResultOrDefaultAsync(aiModelId, dateTime);
        if (dbModelResult is not null)
        {
            return (dbModelResult.PredictionValues, dbModelResult.ActualValues, dbModelResult.AverageDeviation, dbModelResult.PercentageDeviation);
        }

        var predictedValues = await predictionService.PredictAsync(aiModelId, dateTime);
        var actualValues = await predictionService.GetActualAsync(dateTime);

        var averageDeviation = CalculateAverageDeviation(predictedValues, actualValues);
        var percentageDeviation = CalculatePercentageDeviation(predictedValues, actualValues);

        await dbRepository.AddNewAiModelResultAsync(aiModelId, dateTime, predictedValues, actualValues, averageDeviation, percentageDeviation);

        return (predictedValues, actualValues, averageDeviation, percentageDeviation);
    }

    public Task<IEnumerable<AiModel>> GetAiModelsAsync()
    {
        return dbRepository.GetAiModelsAsync();
    }

    public Task CreateAiModelAsync(CreateModelRequestDto model)
    {
        return dbRepository.CreateAiModelAsync(model);
    }

    private static double CalculateAverageDeviation(IReadOnlyCollection<double> values1, IReadOnlyList<double> values2)
    {
        if (values1.Count != values2.Count)
        {
            throw new Exception();
        }

        var sum = values1
            .Select((t, i) => Math.Abs(t - values2[i]))
            .Sum();

        return sum / values1.Count;
    }
    
    private static double CalculatePercentageDeviation(IReadOnlyCollection<double> actualValues, IReadOnlyList<double> predictedValues)
    {
        if (actualValues.Count != predictedValues.Count)
        {
            throw new Exception();
        }

        var sumActual = actualValues.Sum();
        var diff = Math.Abs(sumActual - predictedValues.Sum());

        return diff / sumActual;
    }
}