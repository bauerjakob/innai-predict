namespace InnAi.Server.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo GermanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin"); 
    
    public static DateTime ToGermanTime(this DateTime utcTime)
    {
        var time = TimeZoneInfo.ConvertTimeFromUtc(utcTime, GermanTimeZone);
        return time;
    }

    public static int GetUtcOffset(this DateTime dateTime)
    {
        return GermanTimeZone.GetUtcOffset(dateTime).Hours;
    }

    public static long ToUnixTimeStamp(this DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTime((DateTimeOffset)dateTime, TimeZoneInfo.Utc)
            .ToUnixTimeSeconds();
    }
}