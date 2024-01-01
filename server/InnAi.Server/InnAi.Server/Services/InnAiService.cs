using InnAi.Server.Data.Repositories;
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

    public async Task<(double[] PredictionValues, double[] ActualValues, double AverageDeviation)> PredictHistoryAsync(DateTime dateTime, Guid? modelId = null)
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
            return (dbModelResult.PredictionValues, dbModelResult.ActualValues, dbModelResult.AverageDeviation);
        }

        var predictedValues = await predictionService.PredictAsync(aiModelId, dateTime);
        var actualValues = await predictionService.GetActualAsync(dateTime);

        var averageDeviation = CalculateAverageDeviation(predictedValues, actualValues);

        await dbRepository.AddNewAiModelResultAsync(aiModelId, dateTime, predictedValues, actualValues, averageDeviation);

        return (predictedValues, actualValues, averageDeviation);
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
}