using System.Diagnostics.CodeAnalysis;

namespace Photography_Tools.Helpers;

public static class DateTimeHelper
{
    public const string DefaultValue = " --- ", DefaultFormat = "d MMM HH:mm";

    public static string ToStringLocalTime(this DateTime? date, [ConstantExpected] string nullValue = DefaultValue, [ConstantExpected] string format = DefaultFormat, IFormatProvider? formatProvider = null) =>
        date?.ToLocalTime().ToString(format, formatProvider) ?? nullValue;

    public static string ToStringUtcTime(this DateTime? date, [ConstantExpected] string nullValue = DefaultValue, [ConstantExpected] string format = DefaultFormat, IFormatProvider? formatProvider = null) =>
        date?.ToUniversalTime().ToString(format, formatProvider) ?? nullValue;
}
