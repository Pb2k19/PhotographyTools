namespace Photography_Tools.Helpers;

// Source: SunCalcNet, https://github.com/kostebudinoski/SunCalcNet, access date: 30.04.2024
public static class AstroHelper
{
    private const double JulianAndOADateDif = 2415018.5;

    public static double ToJulianDate(this DateTime date) =>
        date.ToUniversalTime().ToOADate() + JulianAndOADateDif;
}