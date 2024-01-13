using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using InnAi.Server.Dtos;
using InnAi.Server.Extensions;
using InnAiServer.Options;
using Microsoft.Extensions.Options;

namespace InnAi.Server.Clients;

public class InnLevelClient : IInnLevelClient
{
    private readonly InnLevelClientOptions _options;
    private readonly HttpClient _client;

    public InnLevelClient(IOptions<ApiClients> options)
    {
        _options = options.Value.InnLevelClient;
        
        _client = new HttpClient
        {
            BaseAddress = new Uri(_options.BaseUrl?.TrimEnd('/') ?? throw new Exception())
        };
    }
    
    public async Task<double[]> GetAsync(DateTime dateTime, int stationCount)
    {
        if (stationCount > _options.Stations.Length)
        {
            throw new Exception();
        }
        
        var timestamp = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
        var dateStringFrom = ToDateString(timestamp - TimeSpan.FromHours(6));
        var dateStringEnd = ToDateString(timestamp + TimeSpan.FromHours(1));
        List<double> ret = new();

        for (var i = 0; i < stationCount; i++)
        {
            var station = _options.Stations[i];
            
            var payload = await MakeCallAsync(station, dateStringEnd, dateStringFrom);
            var nearestMatch = GetNearestItem(payload, timestamp);
            ret.Add(nearestMatch.Value);
        }

        return ret.ToArray();
    }

    public async Task<double[]> GetNextHoursAsync(DateTime dateTime, int hourCount)
    {
        var timestamp = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
        var dateStringFrom = ToDateString(timestamp);
        var dateStringEnd = ToDateString(timestamp + TimeSpan.FromHours(12));
        
        var station = _options.Stations[0];

        var result = await MakeCallAsync(station, dateStringEnd, dateStringFrom);
        
        return result.History.Select(x => x.Value).Take(3).ToArray();
    }
    
    private async Task<PegelAlarmDtoPayload> MakeCallAsync(string station, string dateStringEnd, string dateStringFrom)
    {
        var response = await _client.GetAsync($"/api/station/1.0/height/{station}/history?granularity=hour&loadEndDate={dateStringEnd}&loadStartDate={dateStringFrom}");
            
        var content = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(content);
            
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    
        var result = json.Deserialize<PegelAlarmDto>(serializerOptions);

        var payload = result?.Payload ?? throw new Exception();
        
        return payload;
    }

    private string ToDateString(DateTime dateTime)
    {
        var utcOffset = dateTime.GetUtcOffset();
        return dateTime.ToString("dd.MM.yyyyTHH:mm:ss")+ $"%2B0{utcOffset}00";
    }

    private PegelAlarmDtoPayloadItem GetNearestItem(PegelAlarmDtoPayload payload, DateTime dateTime)
    {
        PegelAlarmDtoPayloadItem closestMatch = null;
        long closestDiff = long.MaxValue;
        
        foreach (var item in payload.History)
        {
            var time = UrlTimeToUtcDateTime(item.SourceDate);

            var diff = Math.Abs((dateTime - time).Ticks);

            if (diff < closestDiff)
            {
                closestMatch = item;
                closestDiff = diff;
            }
        }

        return closestMatch ?? throw new Exception();
    }
    
    private static DateTime UrlTimeToUtcDateTime(string dateTime)
    {
        // var utcOffset = DateTimeExtensions.GetUtcOffset();
        var utcOffset = Regex.Match(dateTime, "(?<=\\+0)\\d").Value;
        var dateTimeOffset = DateTimeOffset.ParseExact(dateTime, $"dd.MM.yyyyTHH:mm:ss+0{utcOffset}00", CultureInfo.InvariantCulture);
        return dateTimeOffset.UtcDateTime;
    }
}