using System.Buffers;
using System.Globalization;

namespace Photography_Tools.Helpers;

public static class AstroHelper
{
    private const char
        NorthUpper = 'N',
        NorthLower = 'n',
        SouthUpper = 'S',
        SouthLower = 's',
        WestUpper = 'W',
        WestLower = 'w',
        EastUpper = 'E',
        EastLower = 'e';

    private static readonly SearchValues<char>
        directionsSearchValues = SearchValues.Create([NorthUpper, SouthUpper, WestUpper, EastUpper, NorthLower, SouthLower, WestLower, EastLower]),
        latitudeDirectionsSearchValues = SearchValues.Create([NorthUpper, SouthUpper, NorthLower, SouthLower]),
        separatorsSearchValues = SearchValues.Create([',', ';']),
        degreeSearchValues = SearchValues.Create(['°', '*']),
        primeSearchValues = SearchValues.Create(['′', '\'', '‘', '´', '`']),
        quotationSearchValues = SearchValues.Create(['″', '"', '〃']),
        dmsCharactersSearchValues = SearchValues.Create(['°', '*', '′', '\'', '‘', '´', '`', '″', '"', '〃']);

    private static readonly SearchValues<string>
        altSeparatorsSearchValues = SearchValues.Create([" ", ", ", ". "], StringComparison.Ordinal);

    public static bool IsJulianDate(this DateTime date)
    {
        if (date.Year < 1582 || (date.Year == 1582 && date.Month < 10) || (date.Year == 1582 && date.Month == 10 && date.Day < 5))
            return true;

        if (date.Year > 1582 || (date.Year == 1582 && date.Month > 10) || (date.Year == 1582 && date.Month == 10 && date.Day > 14))
            return false;

        throw new ArgumentOutOfRangeException(nameof(date), "Dates between 5-10-1582 and 14-10-1582 are incorrect");
    }

    // Source: ASTRONOMICAL ALGORITHMS SECOND EDITION, Jean Meeus, Published by Willmann-Bell, Inc. Page: 60-61
    public static double ToJulianDate(this DateTime date)
    {
        int y, m;
        double d = date.Day + date.Hour / 24.0 + date.Minute / 1440.0 + (date.Second + date.Millisecond / 1000.0) / 86400.0;

        if (date.Month > 2)
        {
            y = date.Year;
            m = date.Month;
        }
        else
        {
            y = date.Year - 1;
            m = date.Month + 12;
        }

        int b = IsJulianDate(date) ? 0 : 2 - y / 100 + y / 400;

        return (int)(365.25 * (y + 4716)) + (int)(30.6001 * (m + 1)) + d + b - 1524.5;
    }

    // Source: ASTRONOMICAL ALGORITHMS SECOND EDITION, Jean Meeus, Published by Willmann-Bell, Inc. Page: 63
    public static DateTime FromJulianDate(double julianDate)
    {
        int z = (int)(julianDate + 0.5);
        double f = julianDate + 0.5 - z;

        int a;
        if (z < 2299161)
            a = z;
        else
        {
            int alpha = (int)((z - 1867216.25) / 36524.25);
            a = z + 1 + alpha - alpha / 4;
        }

        int b = a + 1524;
        int c = (int)((b - 122.1) / 365.25);
        int d = (int)(365.25 * c);
        int e = (int)((b - d) / 30.6001);

        int day = b - d - (int)(30.6001 * e);
        int month = e < 14 ? e - 1 : e - 13;
        int year = month is 1 or 2 ? c - 4715 : c - 4716;

        TimeSpan time = TimeSpan.FromMilliseconds(Math.Round(86_400_000 * f / 100.0, 0) * 100);

        return new(year, month, day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds, DateTimeKind.Utc);
    }

    public static GeographicalCoordinates ConvertDdStringToDd(ReadOnlySpan<char> input)
    {
        input = input.Trim();
        int indexOfSecondPart = input.IndexOfAny(separatorsSearchValues);

        if (indexOfSecondPart == -1 || input.LastIndexOfAny(separatorsSearchValues) != indexOfSecondPart)
        {
            indexOfSecondPart = input.IndexOfAny(altSeparatorsSearchValues);
            if (indexOfSecondPart == -1)
                return new(double.NaN, double.NaN);
        }

        ReadOnlySpan<char> latitudePart = input[..indexOfSecondPart].Trim();
        indexOfSecondPart++;
        ReadOnlySpan<char> longitudePart = input[indexOfSecondPart..].Trim();

        return new(ParseHelper.ParseDifferentCulture<double>(latitudePart), ParseHelper.ParseDifferentCulture<double>(longitudePart));
    }

    public static GeographicalCoordinates ConvertDmsStringToDd(ReadOnlySpan<char> input)
    {
        input = input.Trim();
        int indexOfSecondPart = input.IndexOfAny(separatorsSearchValues);

        if (indexOfSecondPart == -1)
        {
            indexOfSecondPart = input.IndexOfAny(latitudeDirectionsSearchValues);
            if (indexOfSecondPart == -1)
                return new(double.NaN, double.NaN);
        }

        indexOfSecondPart++;
        ReadOnlySpan<char> latitudePart = input[..indexOfSecondPart].Trim();
        ReadOnlySpan<char> longitudePart = input[indexOfSecondPart..].Trim();

        return new(ConvertDmsPartToDd(latitudePart), ConvertDmsPartToDd(longitudePart));
    }

    public static double ConvertDmsPartToDd(ReadOnlySpan<char> chars)
    {
        int degIndex = chars.IndexOfAny(degreeSearchValues);
        int primeIndex = chars.IndexOfAny(primeSearchValues);
        int quotationIndex = chars.IndexOfAny(quotationSearchValues);
        int direction = chars.IndexOfAny(directionsSearchValues);
        bool containsDmsCharacters = chars.ContainsAny(dmsCharactersSearchValues);

        if (direction == -1 || !containsDmsCharacters)
            return double.NaN;

        double degrees = 0, minutes = 0, seconds = 0;

        if (degIndex != -1)
        {
            degrees = double.Parse(chars[..degIndex].Trim(), CultureInfo.InvariantCulture);
            degIndex++;
        }
        else
        {
            degIndex = 0;
        }

        if (primeIndex != -1)
        {
            minutes = double.Parse(chars[degIndex..primeIndex].Trim(), CultureInfo.InvariantCulture);
            primeIndex++;
        }
        else
        {
            primeIndex = degIndex;
        }

        if (quotationIndex != -1)
        {
            seconds = double.Parse(chars[primeIndex..quotationIndex].Trim(), CultureInfo.InvariantCulture);
        }

        double result = degrees + minutes / 60 + seconds / 3600;

        return chars[direction] switch
        {
            NorthUpper or NorthLower or EastUpper or EastLower => result,
            _ => result * -1
        };
    }

    public static string ConvertDdToDmsString(GeographicalCoordinates coordinates, int secondsPrecision = 4)
        => ConvertDdToDmsString(coordinates.Latitude, coordinates.Longitude, secondsPrecision);

    public static string ConvertDdToDmsString(double latitude, double longitude, int secondsPrecision = 4)
    {
        double latitudeDeg, longitudeDeg, latitudeMin, longitudeMin, latitudeS, longitudeS;

        latitudeDeg = Math.Abs((int)latitude);
        latitudeMin = (Math.Abs(latitude) - latitudeDeg) * 60;
        latitudeS = Math.Round((latitudeMin - ((int)latitudeMin)) * 60, secondsPrecision);
        latitudeMin = (int)latitudeMin;

        longitudeDeg = Math.Abs((int)longitude);
        longitudeMin = (Math.Abs(longitude) - longitudeDeg) * 60;
        longitudeS = Math.Round((longitudeMin - ((int)longitudeMin)) * 60, secondsPrecision);
        longitudeMin = (int)longitudeMin;

        return $"""{latitudeDeg}° {latitudeMin}' {latitudeS.ToString(CultureInfo.InvariantCulture)}" {(latitude >= 0 ? NorthUpper : SouthUpper)} {longitudeDeg}° {longitudeMin}' {longitudeS.ToString(CultureInfo.InvariantCulture)}" {(longitude >= 0 ? EastUpper : WestUpper)}""";
    }

    public static bool Validate(this GeographicalCoordinates coordinates) =>
        ValidateGeographicalCoordinates(coordinates.Latitude, coordinates.Longitude);

    public static bool ValidateGeographicalCoordinates(double latitude, double longitude) =>
        latitude >= AstroConst.LatitudeMinValue && latitude <= AstroConst.LatitudeMaxValue &&
        longitude >= AstroConst.LongitudeMinValue && longitude <= AstroConst.LongitudeMaxValue;
}