using System.Diagnostics.CodeAnalysis;

namespace Photography_Tools.Helpers;

public static class DateTimeHelper
{
    public const string DefaultValue = " --- ", DefaultFormat = "d MMM HH:mm", ShortDefaultFormat = "HH:mm";

    public static string ToStringLocalTime(this DateTime? date, int referenceLocalDateDay, [ConstantExpected] string nullValue = DefaultValue, IFormatProvider? formatProvider = null)
    {
        if (date is null)
            return nullValue;

        DateTime localDate = date.Value.ToLocalTime();

        if (localDate.Day == referenceLocalDateDay)
            return localDate.ToString(ShortDefaultFormat, formatProvider);
        else
            return localDate.ToString(DefaultFormat, formatProvider);
    }

    public static string ToStringLocalTime(this DateTime? date, [ConstantExpected] string nullValue = DefaultValue, [ConstantExpected][StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string format = DefaultFormat, IFormatProvider? formatProvider = null) =>
        date?.ToLocalTime().ToString(format, formatProvider) ?? nullValue;

    public static string ToStringUtcTime(this DateTime? date, [ConstantExpected] string nullValue = DefaultValue, [ConstantExpected][StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string format = DefaultFormat, IFormatProvider? formatProvider = null) =>
        date?.ToUniversalTime().ToString(format, formatProvider) ?? nullValue;
}
