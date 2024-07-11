using System.Collections.Immutable;

namespace Photography_Tools.Services.AstroDataService;

public record struct AstroPhase(int Index, double MinValue, double MaxValue);

public record SunPhaseAngle(string StartName, string EndName, double Angle);

// Source: SunCalcNet, https://github.com/kostebudinoski/SunCalcNet, access date: 30.04.2024
public static class AstroCalculations
{
    private const double Hc = 0.133 * MathHelper.ToRadianMultiplier;

    public static readonly ImmutableArray<AstroPhase> AllMoonPhases;

    public static readonly ImmutableArray<SunPhaseAngle> AllSunPhaseAngles;

    static AstroCalculations()
    {
        AllSunPhaseAngles = [
            new(AstroConst.Sunrise, AstroConst.Sunset, -0.833),
            new(AstroConst.SunriseEnd, AstroConst.SunsetStart, -0.3),
            new(AstroConst.Dawn, AstroConst.Dusk, -6),
            new(AstroConst.NauticalDawn, AstroConst.NauticalDusk, -12),
            new(AstroConst.NightEnd, AstroConst.Night, -18),
            new(AstroConst.MorningGoldenHourEnd, AstroConst.EveningGoldenHourStart, 6),
            new(AstroConst.MorningGoldenHourStart, AstroConst.EveningGoldenHourEnd, -4)
        ];

        AllMoonPhases = [
            new AstroPhase(0, AstroConst.SynodicMonthLength * -0.02, AstroConst.SynodicMonthLength * 0.025),    // 0%
            new AstroPhase(1, AstroConst.SynodicMonthLength * 0.025, AstroConst.SynodicMonthLength * 0.23),
            new AstroPhase(2, AstroConst.SynodicMonthLength * 0.23, AstroConst.SynodicMonthLength * 0.275),     // 50%
            new AstroPhase(3, AstroConst.SynodicMonthLength * 0.275, AstroConst.SynodicMonthLength * 0.48),
            new AstroPhase(4, AstroConst.SynodicMonthLength * 0.48, AstroConst.SynodicMonthLength * 0.525),     // 100%
            new AstroPhase(5, AstroConst.SynodicMonthLength * 0.525, AstroConst.SynodicMonthLength * 0.73),
            new AstroPhase(6, AstroConst.SynodicMonthLength * 0.73, AstroConst.SynodicMonthLength * 0.775),     // 50%
            new AstroPhase(7, AstroConst.SynodicMonthLength * 0.775, AstroConst.SynodicMonthLength * 0.98)
        ];
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

    public static double CalculateJulianCycle(double d, double lw) =>
        Math.Round(d - AstroConst.J0 - lw / (2 * Math.PI));

    public static double CalculateApproxTransit(double ht, double lw, double n) =>
        AstroConst.J0 + (ht + lw) / (2 * Math.PI) + n;

    public static double CalculateSiderealTime(double d, double lw) =>
        MathHelper.DegreesToRadians((280.16 + 360.9856235 * d)) - lw;

    public static double CalculateAltitude(double h, double phi, double dec) =>
        Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(h));

    public static double CalculateAzimuth(double h, double phi, double dec) =>
        Math.Atan2(Math.Sin(h), Math.Cos(h) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi));

    public static double CalculateObserverAngle(double height) =>
        -2.076 * Math.Sqrt(height) / 60;

    public static double CalculateAstroRefraction(double h)
    {
        if (h < 0)
            h = 0;

        // Formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
        return 0.0002967 / Math.Tan(h + 0.00312536 / (h + 0.08901179));
    }

    public static double CalculateSolarTransitJ(double ds, double m, double l) =>
        AstroConst.JulianDay01_01_2000_Noon + ds + 0.0053 * Math.Sin(m) - 0.0069 * Math.Sin(2 * l);

    private static double CalculateHourAngle(double h, double phi, double d) =>
        Math.Acos((Math.Sin(h) - Math.Sin(phi) * Math.Sin(d)) / (Math.Cos(phi) * Math.Cos(d)));

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

    public static MoonPhase CalculateMoonPhase(DateTime date)
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

        return new MoonPhase(fraction, phase, angle);
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

    public static string GetPhaseName(double phase)
    {
        int index = -1;
        for (int i = 0; i < AllMoonPhases.Length; i++)
        {
            if (phase >= AllMoonPhases[i].MinValue && phase < AllMoonPhases[i].MaxValue)
            {
                index = i;
                break;
            }
        }

        if (index == -1 && phase <= AstroConst.SynodicMonthLength)
            index = 0;

        return AstroConst.AllMoonPhases[index];
    }

    public static double CalculateSunSetTime(double h, double lw, double phi, double dec, double n, double m, double l)
    {
        var w = CalculateHourAngle(h, phi, dec);
        var a = CalculateApproxTransit(w, lw, n);
        return CalculateSolarTransitJ(a, m, l);
    }

    public static IEnumerable<SunPhaseStart> CalculateSunPhases(DateTime date, double latitude, double longitude, double height = 0)
    {
        double lw = MathHelper.DegreesToRadians(-longitude);
        double phi = MathHelper.DegreesToRadians(latitude);

        double d = date.ToJulianDate() - AstroConst.JulianDay01_01_2000_Noon;

        double n = CalculateJulianCycle(d, lw);
        double ds = CalculateApproxTransit(0, lw, n);

        double m = CalculateMeanAnomaly(ds);
        double l = CalculateEclipticLongitude(m);
        double dec = CalculateDeclination(l, 0);
        double jnoon = CalculateSolarTransitJ(ds, m, l);

        yield return new SunPhaseStart(AstroConst.SolarNoon, AstroHelper.FromJulianDate(jnoon));
        yield return new SunPhaseStart(AstroConst.Nadir, AstroHelper.FromJulianDate(jnoon - 0.5));

        double dh = CalculateObserverAngle(height);
        foreach (var sunPhase in AllSunPhaseAngles)
        {
            double h0 = MathHelper.DegreesToRadians(sunPhase.Angle + dh);
            double jset = CalculateSunSetTime(h0, lw, phi, dec, n, m, l);

            if (double.IsNaN(jset) || double.IsInfinity(jset))
                continue;

            double jrise = jnoon - (jset - jnoon);
            yield return new SunPhaseStart(sunPhase.StartName, AstroHelper.FromJulianDate(jrise));
            yield return new SunPhaseStart(sunPhase.EndName, AstroHelper.FromJulianDate(jset));
        }
    }

    public static Models.Phase? CalculateSunPhase(DateTime date, double latitude, double longitude, double angle, double height = 0)
    {
        double lw = MathHelper.DegreesToRadians(-longitude);
        double phi = MathHelper.DegreesToRadians(latitude);

        double d = date.ToJulianDate() - AstroConst.JulianDay01_01_2000_Noon;

        double n = CalculateJulianCycle(d, lw);
        double ds = CalculateApproxTransit(0, lw, n);

        double m = CalculateMeanAnomaly(ds);
        double l = CalculateEclipticLongitude(m);
        double dec = CalculateDeclination(l, 0);
        double dh = CalculateObserverAngle(height);
        double jnoon = CalculateSolarTransitJ(ds, m, l);

        double h0 = MathHelper.DegreesToRadians(angle + dh);
        double jset = CalculateSunSetTime(h0, lw, phi, dec, n, m, l);

        if (double.IsNaN(jset) || double.IsInfinity(jset))
            return null;

        double jrise = jnoon - (jset - jnoon);

        return new(AstroHelper.FromJulianDate(jrise), AstroHelper.FromJulianDate(jset));
    }
}
