using InnAi.Server.Data.Entities;

namespace InnAi.Server.Data.Repositories;

public interface IMainDbRepository
{
    public Task<AiModelResult?> GetAiModelResultOrDefaultAsync(Guid modelId, DateTime dateTime);

    public Task AddNewAiModelResultAsync(Guid modelId, DateTime dateTime, double[] predictedValues, double[] actualValues, double averageDeviation);

    public Task<AiModel> GetDefaultAiModelAsync();
    public Task<AiModel> GetAiModelAsync(Guid modelId);
}