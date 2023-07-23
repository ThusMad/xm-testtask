namespace XmPriceAgg.API.Helpers;

public static class TimeHelper
{
    public static long GetHourPrecisionTimestamp(long timestamp)
    {
        var numDigits = timestamp.ToString().Length;

        DateTime dateTime;
        if (numDigits == 13)
        {
            dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
        }
        else if (numDigits == 10)
        {
            dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        }
        else
        {
            throw new ArgumentException("Invalid timestamp format. Timestamp should be in seconds or milliseconds.");
        }

        dateTime = dateTime.AddMinutes(-dateTime.Minute).AddSeconds(-dateTime.Second).AddMilliseconds(-dateTime.Millisecond);
        var hourPrecisionTimestamp = ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();

        return hourPrecisionTimestamp;
    }
}