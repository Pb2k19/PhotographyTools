namespace Photography_Tools.Helpers;

// Source: SunCalcNet, https://github.com/kostebudinoski/SunCalcNet, access date: 30.04.2024
public static class AstroHelper
{
    private const double JulianAndOADateDif = 2415018.5;

    public static double ToJulianDate(this DateTime date) =>
        date.ToOADate() + JulianAndOADateDif;

    public static DateTime FromJulianDateTime(double julianDate) =>
        DateTime.FromOADate(julianDate - JulianAndOADateDif);

    public static double CalculateRightAscension(double longitude, double b) =>
        Math.Atan2(Math.Sin(longitude) * Math.Cos(AstroConst.EarthObliquity) -
            Math.Tan(b) * Math.Sin(AstroConst.EarthObliquity), Math.Cos(longitude));

    public static double CalculateDeclination(double longitude, double b) =>
        Math.Asin(Math.Sin(b) * Math.Cos(AstroConst.EarthObliquity) +
            Math.Cos(b) * Math.Sin(AstroConst.EarthObliquity) * Math.Sin(longitude));

    public static double CalculateEclipticLongitude(double m) =>
        m + CalculateEquationOfCenter(m) + AstroConst.EarthPerihelion + Math.PI;

    private static double CalculateEquationOfCenter(double m) =>
        MathHelper.DegreesToRadians(1.9148 * Math.Sin(m) + 0.02 * Math.Sin(2 * m) + 0.0003 * Math.Sin(3 * m));

    public static double CalculateMeanAnomaly(double days) =>
        MathHelper.DegreesToRadians(357.5291 + 0.98560028 * days);

    public static GeocentricCoordinates CalculateMoonGeocentricCoords(double days)
    {
        double eclipticLongitude = MathHelper.DegreesToRadians(218.316 + 13.176396 * days);
        double meanAnomaly = MathHelper.DegreesToRadians(134.963 + 13.064993 * days);
        double meanDistance = MathHelper.DegreesToRadians(93.272 + 13.229350 * days);

        double longitude = eclipticLongitude + MathHelper.DegreesToRadians(6.289) * Math.Sin(meanAnomaly);
        double latitude = MathHelper.DegreesToRadians(5.128) * Math.Sin(meanDistance);
        double distanceToMoonKM = 385001 - 20905 * Math.Cos(meanAnomaly);

        return new GeocentricCoordinates(CalculateRightAscension(longitude, latitude), CalculateDeclination(longitude, latitude), distanceToMoonKM);
    }

    public static EquatorialCoordinates CalculateSunEquatorialCoords(double days)
    {
        double meanAnomaly = CalculateMeanAnomaly(days);
        double eclipticLongitude = CalculateEclipticLongitude(meanAnomaly);

        return new EquatorialCoordinates(CalculateRightAscension(eclipticLongitude, 0), CalculateDeclination(eclipticLongitude, 0));
    }

    public static MoonPhaseResult CalculateMoonPhase(DateTime utcDateTime)
    {
        double julianDateDiff = utcDateTime.ToJulianDate() - AstroConst.JulianDay01_01_2000_Noon;

        EquatorialCoordinates sunCoords = CalculateSunEquatorialCoords(julianDateDiff);
        GeocentricCoordinates moonCoords = CalculateMoonGeocentricCoords(julianDateDiff);

        double phi = Math.Acos(Math.Sin(sunCoords.Declination) * Math.Sin(moonCoords.Declination) +
                            Math.Cos(sunCoords.Declination) * Math.Cos(moonCoords.Declination) *
                            Math.Cos(sunCoords.RightAscension - moonCoords.RightAscension));

        double inc = Math.Atan2(AstroConst.EarthToSunDistanceKM * Math.Sin(phi),
                             moonCoords.Distance - AstroConst.EarthToSunDistanceKM * Math.Cos(phi));

        double angle = Math.Atan2(Math.Cos(sunCoords.Declination) * Math.Sin(sunCoords.RightAscension - moonCoords.RightAscension),
                               Math.Sin(sunCoords.Declination) * Math.Cos(moonCoords.Declination) -
                               Math.Cos(sunCoords.Declination) * Math.Sin(moonCoords.Declination) *
                               Math.Cos(sunCoords.RightAscension - moonCoords.RightAscension));

        double fraction = (1 + Math.Cos(inc)) / 2;
        double phase = (0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI) * AstroConst.SynodicMonthLength;

        return new MoonPhaseResult(fraction, phase, angle);
    }
}