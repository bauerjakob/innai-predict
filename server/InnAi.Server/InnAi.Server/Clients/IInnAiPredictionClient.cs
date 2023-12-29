namespace InnAi.Server.Clients;

public interface IInnAiPredictionClient
{
    public Task<double[]> PredictAsync(double[] input);
}