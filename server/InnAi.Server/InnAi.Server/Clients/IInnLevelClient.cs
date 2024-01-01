namespace InnAi.Server.Clients;

public interface IInnLevelClient
{
    public Task<double[]> GetAsync(DateTime dateTime, int count);
}