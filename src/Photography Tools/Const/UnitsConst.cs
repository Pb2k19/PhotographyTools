using System.Collections.Immutable;

namespace Photography_Tools.Const;

public static class LengthUnitsConst
{
    public const string
        Millimeter = "mm",
        Centimeter = "cm",
        Meter = "m",
        Inch = "in",
        Foot = "ft";

    public static readonly ImmutableArray<string> Units = [Millimeter, Centimeter, Meter, Inch, Foot];
}

public static class TimeUnitsConst
{
    public const string
        Millisecond = "ms",
        Second = "s",
        Minute = "min",
        Hour = "h";

    public static readonly ImmutableArray<string> Units = [Millisecond, Second, Minute, Hour];
}

public static class InformationUnitsConst
{
    public const string
        Byte = "B",
        Kilobyte = "kB",
        Megabyte = "MB",
        Gigabyte = "GB",
        Terabyte = "TB";

    public static readonly ImmutableArray<string> Units = [Byte, Kilobyte, Megabyte, Gigabyte, Terabyte];
}