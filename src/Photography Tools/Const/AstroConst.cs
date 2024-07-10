using System.Collections.Immutable;

namespace Photography_Tools.Const;

public class AstroConst
{
    public const int
        EarthToSunDistanceKM = 149598000,
        LatitudeMaxValue = 90,
        LatitudeMinValue = -90,
        LongitudeMaxValue = 180,
        LongitudeMinValue = -180;

    public const double
        EarthObliquity = MathHelper.ToRadianMultiplier * 23.4397,
        EarthPerihelion = MathHelper.ToRadianMultiplier * 102.9372,
        SynodicMonthLength = 29.53058770576,
        JulianDay01_01_2000_Noon = 2451545.0,
        J0 = 0.0009;

    public const string
        NewMoon = "New Moon",
        WaxingCrescent = "Waxing Crescent",
        FirstQuarter = "First Quarter",
        WaxingGibbous = "Waxing Gibbous",
        FullMoon = "Full Moon",
        WaningGibbous = "Waning Gibbous",
        ThirdQuarter = "Third Quarter",
        WaningCrescent = "Waning Crescent";

    public const string
        Dawn = "Dawn",
        Dusk = "Dusk",
        EveningGoldenHourStart = "Golden Hour",
        EveningGoldenHourEnd = "Golden Hour End",
        NauticalDawn = "Nautical Dawn",
        NauticalDusk = "Nautical Dusk",
        Nadir = "Nadir",
        Night = "Night",
        NightEnd = "Night End",
        MorningGoldenHourStart = "Morning Golden Hour",
        MorningGoldenHourEnd = "Morning Golden Hour End",
        SolarNoon = "Solar Noon",
        Sunrise = "Sunrise",
        SunriseEnd = "Sunrise End",
        Sunset = "Sunset",
        SunsetStart = "Sunset Start";


    public static readonly ImmutableArray<string> AllMoonPhases = [NewMoon, WaxingCrescent, FirstQuarter, WaxingGibbous, FullMoon, WaningGibbous, ThirdQuarter, WaningCrescent];
}