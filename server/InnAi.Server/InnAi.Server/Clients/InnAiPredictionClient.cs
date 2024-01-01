using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using InnAiServer.Options;
using Microsoft.Extensions.Options;

namespace InnAi.Server.Clients;

public class InnAiPredictionClient : IInnAiPredictionClient
{
    private readonly InnAiPredictionClientOptions _options;
    private readonly HttpClient _client;

    public InnAiPredictionClient(IOptions<ApiClients> options)
    {
        _options = options.Value.InnAiPredictionClient;

        _client = new HttpClient()
        {
            BaseAddress = new Uri(_options.BaseUrl ?? throw new Exception())
        };
    }
    
    public async Task<double[]> PredictAsync(Guid modelId, double[] input)
    {
        var json = JsonSerializer.Serialize(input);
        
        var buffer = System.Text.Encoding.UTF8.GetBytes(json);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        var result = await _client.PostAsync($"/predict/{modelId}", byteContent);
        var resultStream = await result.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<double[]>(resultStream) ?? throw new Exception();
    }
}