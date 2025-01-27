using System;

namespace OpenTabletDriver.Plugin;

public static class Extensions
{
    public static TimeSpan ValidateTimespan(this TimeSpan timeSpan, TimeSpan defaultTimeSpan, string logGroup = "ValidateTimespan")
    {
        if (timeSpan != TimeSpan.Zero)
            return timeSpan;

        Log.WriteNotify(logGroup,
            $"Timespan cannot be 0 ms, defaulted to {defaultTimeSpan.TotalMilliseconds} ms", LogLevel.Error);
        return defaultTimeSpan;
    }
}
