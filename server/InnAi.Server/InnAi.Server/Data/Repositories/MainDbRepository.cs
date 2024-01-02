using InnAi.Server.Data.Entities;
using InnAi.Server.Dtos;
using Microsoft.EntityFrameworkCore;

namespace InnAi.Server.Data.Repositories;

public class MainDbRepository : IMainDbRepository
{
    private readonly ILogger<MainDbRepository> _logger;
    private readonly MainDbContext _dbContext;

    public MainDbRepository(ILogger<MainDbRepository> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<AiModelResult?> GetAiModelResultOrDefaultAsync(Guid modelId, DateTime dateTime)
    {
        var aiModel = await GetAiModelAsync(modelId);

        return await _dbContext.AiModelResults
            .SingleOrDefaultAsync(x => x.AiModelId == aiModel.Id && x.DateTime == dateTime);
    }

    public async Task AddNewAiModelResultAsync(Guid modelId, DateTime dateTime, double[] predictedValues,
        double[] actualValues, double averageDeviation, double percentageDeviation)
    {
        var aiModel = await GetAiModelAsync(modelId);

        var aiModelResult = new AiModelResult
        {
            AiModel = aiModel,
            DateTime = dateTime,
            PredictionValues = predictedValues,
            ActualValues = actualValues,
            AverageDeviation = averageDeviation,
            PercentageDeviation = percentageDeviation
        };

        await _dbContext.AiModelResults.AddAsync(aiModelResult);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<AiModel> GetDefaultAiModelAsync()
    {
        try
        {
            return await _dbContext.AiModels.FirstAsync(x => x.Default == true);
        }
        catch (Exception e)
        {
            return await _dbContext.AiModels.FirstAsync();
        }
    }

    public Task<AiModel> GetAiModelAsync(Guid modelId)
    {
        return _dbContext.AiModels.SingleAsync(x => x.ExternalId == modelId);
    }

    public async Task<IEnumerable<AiModel>> GetAiModelsAsync()
    {
        return await _dbContext.AiModels.ToListAsync();
    }

    public async Task CreateAiModelAsync(CreateModelRequestDto model)
    {
        var aiModel = new AiModel
        {
            ExternalId = model.ExternalId,
            Default = model.Default,
            Name = model.Name,
            NumberOfInnLevels = model.NumberOfInnLevels,
            PrecipitationMapSize = model.PrecipitationMapSize
        };

        await _dbContext.AiModels.AddAsync(aiModel);
        await _dbContext.SaveChangesAsync();
    }
}