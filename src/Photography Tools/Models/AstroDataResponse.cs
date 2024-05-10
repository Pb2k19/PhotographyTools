namespace Photography_Tools.Models;

public record AstroData(SunData SunData, MoonData MoonData);

public record MoonData(DateTime Rise, DateTime UpperTransit, DateTime Set, double Illumination, double MoonAge, string Phase);

public record SunData(DateTime Rise, DateTime CivilTwilightStart, DateTime CivilTwilightEnd, DateTime UpperTransit, DateTime Set);