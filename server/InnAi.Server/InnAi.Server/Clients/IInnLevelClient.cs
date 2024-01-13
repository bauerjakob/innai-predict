namespace InnAi.Server.Clients;

public interface IInnLevelClient
{
    public Task<double[]> GetAsync(DateTime dateTime, int stationCount);
    public Task<double[]> GetNextHoursAsync(DateTime dateTime, int hourCount);
}