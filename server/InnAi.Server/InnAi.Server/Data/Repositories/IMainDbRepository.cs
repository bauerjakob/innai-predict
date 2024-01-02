using InnAi.Server.Data.Entities;
using InnAi.Server.Dtos;

namespace InnAi.Server.Data.Repositories;

public interface IMainDbRepository
{
    public Task<AiModelResult?> GetAiModelResultOrDefaultAsync(Guid modelId, DateTime dateTime);

    public Task AddNewAiModelResultAsync(Guid modelId, DateTime dateTime, double[] predictedValues,
        double[] actualValues, double averageDeviation, double percentageDeviation);

    public Task<AiModel> GetDefaultAiModelAsync();
    public Task<AiModel> GetAiModelAsync(Guid modelId);
    public Task<IEnumerable<AiModel>> GetAiModelsAsync();
    public Task CreateAiModelAsync(CreateModelRequestDto model);
}