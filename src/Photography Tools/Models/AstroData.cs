namespace Photography_Tools.Models;

public record AstroData(SunData SunData, MoonData MoonData);

public record MoonData(DateTime Rise, DateTime UpperTransit, DateTime Set, double Illumination, double MoonAge, string Phase);

public record SunData(DateTime Rise, DateTime CivilTwilightStart, DateTime CivilTwilightEnd, DateTime UpperTransit, DateTime Set);

public readonly record struct MoonPhaseResult(double Fraction, double Phase, double Angle);

public readonly record struct MoonPosition(double Azimuth, double Altitude, double Distance, double ParallacticAngle);