namespace Photography_Tools.Models;

public record AstroData(SunData SunData, MoonData MoonData);

public record MoonData(DateTime? Rise, DateTime? UpperTransit, DateTime? Set, double Illumination, double MoonAge, string Phase);

public readonly record struct MoonPhase(double Fraction, double Phase, double Angle);

public readonly record struct MoonPosition(double Azimuth, double Altitude, double Distance, double ParallacticAngle);

public record SunData(DateTime? Rise, DateTime? CivilTwilightStart, DateTime? CivilTwilightEnd, DateTime? UpperTransit, DateTime? Set);

public readonly record struct SunPhaseStart(string Name, DateTime Date);

public record Period(DateTime? StartDate, DateTime? EndDate);

public record SunPhasesResult(Period? Day, Period? MorningGoldenHour, Period? EveningGoldenHour, Period? MorningBlueHour, Period? EveningBlueHour, Period? MorningCivilTwilight, Period? EveningCivilTwilight, DateTime? UpperTransit);