namespace Photography_Tools.Models;

public record AstroData(SunData SunData, MoonData MoonData);

public record MoonData(DateTime Rise, DateTime UpperTransit, DateTime Set, double Illumination, double MoonAge, string Phase);

public readonly record struct MoonPhase(double Fraction, double Phase, double Angle);

public readonly record struct MoonPosition(double Azimuth, double Altitude, double Distance, double ParallacticAngle);

public record SunData(DateTime Rise, DateTime CivilTwilightStart, DateTime CivilTwilightEnd, DateTime UpperTransit, DateTime Set);

public readonly record struct SunPhaseStart(string Name, DateTime Date);

public readonly record struct Phase(DateTime StartDate, DateTime EndDate);

public record SunPhasesResult(Phase? Day, Phase? MorningGoldenHour, Phase? EveningGoldenHour, Phase? MorningBlueHour, Phase? EveningBlueHour, Phase? MorningCivilTwilight, Phase? EveningCivilTwilight, DateTime? UpperTransit);