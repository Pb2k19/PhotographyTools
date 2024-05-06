namespace Photography_Tools.Services.AstroDataService;

public class OfflineAstroDataService : IAstroDataService
{
    private const double
        Hc = 0.133 * MathHelper.ToRadianMultiplier;

    public MoonDataResult GetMoonData(DateTime date, double latitude, double longitude)
    {
        MoonPhaseResult moonPhase = CalculateMoonPhase(date);
        RiseAndSetResult moonRiseAndDown = CalculateMoonRiseAndDown(date, latitude, longitude);

        return new MoonDataResult(moonRiseAndDown, moonPhase, true);
    }

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

    public static double CalculateSiderealTime(double d, double lw) =>
        MathHelper.DegreesToRadians((280.16 + 360.9856235 * d)) - lw;

    public static double CalculateAltitude(double h, double phi, double dec) =>
        Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(h));

    public static double CalculateAzimuth(double h, double phi, double dec) =>
        Math.Atan2(Math.Sin(h), Math.Cos(h) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi));

    public static double CalculateAstroRefraction(double h)
    {
        if (h < 0)
            h = 0;

        // Formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
        return 0.0002967 / Math.Tan(h + 0.00312536 / (h + 0.08901179));
    }

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

    public static MoonPhaseResult CalculateMoonPhase(DateTime date)
    {
        double julianDateDiff = date.ToJulianDate() - AstroConst.JulianDay01_01_2000_Noon;

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

    public static MoonPosition CalculateMoonPosition(DateTime date, double latitude, double longitude)
    {
        double lw = MathHelper.DegreesToRadians(-longitude);
        double phi = MathHelper.DegreesToRadians(latitude);
        double julianDate = date.ToJulianDate() - AstroConst.JulianDay01_01_2000_Noon;

        GeocentricCoordinates moonGeocentricCoords = CalculateMoonGeocentricCoords(julianDate);
        double siderealTime = CalculateSiderealTime(julianDate, lw) - moonGeocentricCoords.RightAscension;
        double hAltitude = CalculateAltitude(siderealTime, phi, moonGeocentricCoords.Declination);

        // Formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
        double pa = Math.Atan2(Math.Sin(siderealTime), Math.Tan(phi) * Math.Cos(moonGeocentricCoords.Declination) - Math.Sin(moonGeocentricCoords.Declination) * Math.Cos(siderealTime));

        hAltitude += CalculateAstroRefraction(hAltitude);

        double azimuth = CalculateAzimuth(siderealTime, phi, moonGeocentricCoords.Declination);

        return new MoonPosition(azimuth, hAltitude, moonGeocentricCoords.Distance, pa);
    }

    // Calculations for moon rise/set times are based on http://www.stargazing.net/kepler/moonrise.html article
    public static RiseAndSetResult CalculateMoonRiseAndDown(DateTime date, double latitude, double longitude)
    {
        date = date.Date;

        double ye = 0;
        double? rise = null, set = null;
        double moonPosition = CalculateMoonPosition(date, latitude, longitude).Altitude - Hc;

        for (int i = 1; i <= 24; i += 2)
        {
            double moonPosition1 = CalculateMoonPosition(date.AddHours(i), latitude, longitude).Altitude - Hc;
            double moonPosition2 = CalculateMoonPosition(date.AddHours(i + 1), latitude, longitude).Altitude - Hc;

            double a = (moonPosition + moonPosition2) / 2 - moonPosition1;
            double b = (moonPosition2 - moonPosition) / 2;
            double xe = -b / (2 * a);
            ye = (a * xe + b) * xe + moonPosition1;
            double d = b * b - 4 * a * moonPosition1;
            double roots = 0, x1 = 0, x2 = 0;

            if (d >= 0)
            {
                double dx = Math.Sqrt(d) / (Math.Abs(a) * 2);
                x1 = xe - dx;
                x2 = xe + dx;
                if (Math.Abs(x1) <= 1)
                    roots++;

                if (Math.Abs(x2) <= 1)
                    roots++;

                if (x1 < -1)
                    x1 = x2;
            }

            if (roots == 1)
            {
                if (moonPosition < 0)
                    rise = i + x1;
                else
                    set = i + x1;
            }
            else if (roots == 2)
            {
                rise = i + (ye < 0 ? x2 : x1);
                set = i + (ye < 0 ? x1 : x2);
            }

            if (rise.HasValue && set.HasValue)
                break;

            moonPosition = moonPosition2;
        }

        return new RiseAndSetResult(rise.HasValue ? date.AddHours(rise.Value) : null, set.HasValue ? date.AddHours(set.Value) : null, ye);
    }
}