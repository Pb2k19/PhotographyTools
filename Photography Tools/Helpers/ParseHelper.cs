using System.Globalization;

namespace Photography_Tools.Helpers;

public class ParseHelper
{
    private const char Comma = ',', Dot = '.';

    public static bool TryParseDecimalDifferentCulture(ReadOnlySpan<char> readonlySpan, out decimal value)
    {
        if (readonlySpan.Contains(Comma))
        {
            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];
            readonlySpan.CopyTo(span);
            span.Replace(Comma, Dot);

            return decimal.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }
        else
        {
            if (decimal.TryParse(readonlySpan, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return true;
        }
        return false;
    }

    public static decimal ParseDecimalDifferentCulture(ReadOnlySpan<char> readonlySpan)
    {
        if (readonlySpan.Contains(Comma))
        {
            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];
            readonlySpan.CopyTo(span);
            span.Replace(Comma, Dot);

            return decimal.Parse(span, NumberStyles.Float, CultureInfo.InvariantCulture);
        }
        else
            return decimal.Parse(readonlySpan, NumberStyles.Float, CultureInfo.InvariantCulture);
    }

    public static bool TryParseDoubleDifferentCulture(ReadOnlySpan<char> readonlySpan, out double value)
    {
        if (readonlySpan.Contains(Comma))
        {
            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];
            readonlySpan.CopyTo(span);
            span.Replace(Comma, Dot);

            return double.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }
        else
        {
            if (double.TryParse(readonlySpan, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return true;
        }
        return false;
    }

    public static double ParseDoubleDifferentCulture(ReadOnlySpan<char> readonlySpan)
    {
        if (readonlySpan.Contains(Comma))
        {
            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];
            readonlySpan.CopyTo(span);
            span.Replace(Comma, Dot);

            return double.Parse(span, NumberStyles.Float, CultureInfo.InvariantCulture);
        }
        else
            return double.Parse(readonlySpan, NumberStyles.Float, CultureInfo.InvariantCulture);
    }
}
