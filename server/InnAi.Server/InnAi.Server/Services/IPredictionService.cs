namespace InnAiServer.Services;

public interface IPredictionService
{
    public Task<double[]> PredictCurrentAsync(Guid modelId);
    public Task<double[]> PredictAsync(Guid modelId, DateTime timestamp);
    public Task<double[]> GetActualAsync(DateTime dateTime);
}