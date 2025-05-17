namespace KM.Common.Infrastructure;
using System;

public class IsraelTimeProvider : TimeProvider
{
    private static readonly TimeZoneInfo IsraelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");

    public override TimeZoneInfo LocalTimeZone => IsraelTimeZone;
}
