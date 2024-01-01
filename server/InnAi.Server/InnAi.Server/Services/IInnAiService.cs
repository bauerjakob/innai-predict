namespace InnAiServer.Services;

public interface IInnAiService
{
    public Task<double[]> PredictCurrentAsync(Guid? modelId = null);
    public Task<(double[] PredictionValues, double[] ActualValues, double AverageDeviation, double PercentageDeviation)> PredictHistoryAsync(DateTime dateTime, Guid? modelId = null);
}