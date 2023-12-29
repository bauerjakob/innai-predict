namespace InnAi.Server.Clients;

public interface IPrecipitationMapClient
{
    public Task<byte[]> GetAsync(DateTime dateTime);
}