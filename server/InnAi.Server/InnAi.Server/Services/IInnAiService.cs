namespace InnAiServer.Services;

public interface IInnAiService
{
    public Task<double[]> PredictCurrentAsync();
    public Task<double[]> PredictAsync(DateTime timestamp);
    public Task<double[]> GetActualAsync(DateTime dateTime);
}