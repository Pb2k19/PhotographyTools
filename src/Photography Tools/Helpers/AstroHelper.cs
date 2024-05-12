using System.Buffers;
using System.Globalization;

namespace Photography_Tools.Helpers;

public static class AstroHelper
{
    private static readonly SearchValues<char>
        directionsChars = SearchValues.Create(['N', 'S', 'W', 'E', 'n', 's', 'w', 'e']),
        separators = SearchValues.Create([',', ';']);

    public static double ToJulianDate(this DateTime date) =>
        date.ToUniversalTime().ToOADate() + AstroConst.JulianAndOADateDif;

    public static (double latitude, double longitude) ConvertDmsToDd(ReadOnlySpan<char> input)
    {
        int indexOfSecondPart = input.IndexOfAny(separators);

        if (indexOfSecondPart == -1)
            indexOfSecondPart = input.IndexOf(' ');

        ReadOnlySpan<char> latitudePart = input[..indexOfSecondPart].Trim();
        indexOfSecondPart++;
        ReadOnlySpan<char> longitudePart = input[indexOfSecondPart..].Trim();

        return (ConvertDmsPartToDd(latitudePart), ConvertDmsPartToDd(longitudePart));
    }

    public static double ConvertDmsPartToDd(ReadOnlySpan<char> chars)
    {
        int degIndex = chars.IndexOf('°');
        int primeIndex = chars.IndexOf('\'');
        int quotationIndex = chars.IndexOf('"');
        int direction = chars.IndexOfAny(directionsChars);

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
            'N' or 'n' or 'E' or 'e' => result,
            'S' or 's' or 'W' or 'w' => result * -1,
            _ => double.NaN
        };
    }
}