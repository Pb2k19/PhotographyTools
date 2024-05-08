namespace Photography_Tools.Models;

public record AstroData(SunData SunData, MoonData MoonData);

public record MoonData(TimeSpan Rise, TimeSpan UpperTransit, TimeSpan Set, double Illumination, double MoonAge, string Phase);

public record SunData(TimeSpan Rise, TimeSpan CivilTwilightStart, TimeSpan CivilTwilightEnd, TimeSpan UpperTransit, TimeSpan Set);