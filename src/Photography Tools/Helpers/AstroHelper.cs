namespace Photography_Tools.Helpers;

public static class AstroHelper
{
    public static double ToJulianDate(this DateTime date) =>
        date.ToUniversalTime().ToOADate() + AstroConst.JulianAndOADateDif;
}