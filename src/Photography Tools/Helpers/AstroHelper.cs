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
        separatorsSearchValues = SearchValues.Create([',', ';']),
        degreeSearchValues = SearchValues.Create(['°', '*']),
        primeSearchValues = SearchValues.Create(['′', '\'', '‘', '´', '`']),
        quotationSearchValues = SearchValues.Create(['″', '"', '〃']);

    public static double ToJulianDate(this DateTime date) =>
        date.ToUniversalTime().ToOADate() + AstroConst.JulianAndOADateDif;

    public static (double latitude, double longitude) ConvertDdStringToDd(ReadOnlySpan<char> input)
    {
        int indexOfSecondPart = input.IndexOfAny(separatorsSearchValues);

        if (indexOfSecondPart == -1)
        {
            indexOfSecondPart = input.IndexOf(' ');
            if (indexOfSecondPart == -1)
                return (double.NaN, double.NaN);
        }

        ReadOnlySpan<char> latitudePart = input[..indexOfSecondPart].Trim();
        indexOfSecondPart++;
        ReadOnlySpan<char> longitudePart = input[indexOfSecondPart..].Trim();

        return (double.Parse(latitudePart, CultureInfo.InvariantCulture), double.Parse(longitudePart, CultureInfo.InvariantCulture));
    }

    public static (double latitude, double longitude) ConvertDmsStringToDd(ReadOnlySpan<char> input)
    {
        int indexOfSecondPart = input.IndexOfAny(separatorsSearchValues);

        if (indexOfSecondPart == -1)
        {
            indexOfSecondPart = input.IndexOf(' ');
            if (indexOfSecondPart == -1)
                return (double.NaN, double.NaN);
        }

        ReadOnlySpan<char> latitudePart = input[..indexOfSecondPart].Trim();
        indexOfSecondPart++;
        ReadOnlySpan<char> longitudePart = input[indexOfSecondPart..].Trim();

        return (ConvertDmsPartToDd(latitudePart), ConvertDmsPartToDd(longitudePart));
    }

    public static double ConvertDmsPartToDd(ReadOnlySpan<char> chars)
    {
        int degIndex = chars.IndexOfAny(degreeSearchValues);
        int primeIndex = chars.IndexOfAny(primeSearchValues);
        int quotationIndex = chars.IndexOfAny(quotationSearchValues);
        int direction = chars.IndexOfAny(directionsSearchValues);

        if (direction == -1)
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
}