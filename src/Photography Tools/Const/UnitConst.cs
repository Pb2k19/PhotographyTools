using System.Collections.Immutable;

namespace Photography_Tools.Const;

public static class UnitConst
{
    public const string
        LengthMM = "mm",
        LengthCM = "cm",
        LengthM = "m",
        LengthIN = "in",
        LengthFT = "ft";

    public const string
        TimeMs = "ms",
        TimeS = "s",
        TimeMin = "min",
        TimeH = "h";

    public static readonly ImmutableArray<string> LengthUnits = [LengthMM, LengthCM, LengthM, LengthIN, LengthFT];

    public static readonly ImmutableArray<string> TimeUnits = [TimeMs, TimeS, TimeMin, TimeH];
}
