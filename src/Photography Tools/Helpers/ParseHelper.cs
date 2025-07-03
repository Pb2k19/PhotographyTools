using System.Buffers;
using System.Globalization;

namespace Photography_Tools.Helpers;

public static class ParseHelper
{
    public const char Comma = ',', Dot = '.', Space = ' ', ThinSpace = ' ';

    private static readonly SearchValues<char>
        StandardCharacterSearchValues,
        DecimalSeparatorSearchValues,
        NegativeSignSearchValues,
        AllArabicNumeralsSearchValues;

    static ParseHelper()
    {
        HashSet<char>
            decimalSeparators = [Comma, Dot,],
            groupSeparators = [Comma, Dot, Space, ThinSpace, '\'',],
            negativeSigns = ['-'];

        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            if (char.TryParse(culture.NumberFormat.NumberDecimalSeparator, out char c))
                decimalSeparators.Add(c);

            if (char.TryParse(culture.NumberFormat.CurrencyDecimalSeparator, out c))
                decimalSeparators.Add(c);

            if (char.TryParse(culture.NumberFormat.NegativeSign, out c))
                negativeSigns.Add(c);
        }

        NegativeSignSearchValues = SearchValues.Create(negativeSigns.ToArray());
        DecimalSeparatorSearchValues = SearchValues.Create(decimalSeparators.ToArray());
        AllArabicNumeralsSearchValues = SearchValues.Create('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
        StandardCharacterSearchValues = SearchValues.Create('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '-', '+');
    }

    public static bool TryParseDecimalDifferentCulture(ReadOnlySpan<char> readonlySpan, out decimal value)
    {
        if (readonlySpan.ContainsAnyExcept(StandardCharacterSearchValues))
        {
            if (decimal.TryParse(readonlySpan, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                return true;

            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];

            GetNormalizedSpan(readonlySpan, span, out int charsWritten);

            return decimal.TryParse(span[..charsWritten], NumberStyles.Float, CultureInfo.InvariantCulture, out value);
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
        if (readonlySpan.ContainsAnyExcept(StandardCharacterSearchValues))
        {
            if (decimal.TryParse(readonlySpan, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                return value;

            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];

            GetNormalizedSpan(readonlySpan, span, out int charsWritten);

            return decimal.Parse(span, NumberStyles.Float, CultureInfo.InvariantCulture);
        }
        else
            return decimal.Parse(readonlySpan, NumberStyles.Float, CultureInfo.InvariantCulture);
    }

    public static bool TryParseDoubleDifferentCulture(ReadOnlySpan<char> readonlySpan, out double value)
    {
        if (readonlySpan.ContainsAnyExcept(StandardCharacterSearchValues))
        {
            if (double.TryParse(readonlySpan, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                return true;

            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];

            GetNormalizedSpan(readonlySpan, span, out int charsWritten);

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
        if (readonlySpan.ContainsAnyExcept(StandardCharacterSearchValues))
        {
            if (double.TryParse(readonlySpan, NumberStyles.Any, CultureInfo.CurrentCulture, out double value))
                return value;

            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];

            GetNormalizedSpan(readonlySpan, span, out int charsWritten);

            return double.Parse(span, NumberStyles.Float, CultureInfo.InvariantCulture);
        }
        else
            return double.Parse(readonlySpan, NumberStyles.Float, CultureInfo.InvariantCulture);
    }

    private static void GetNormalizedSpan(ReadOnlySpan<char> source, Span<char> destiantion, out int charsWritten)
    {
        int indexOfSeparator = source.LastIndexOfAny(DecimalSeparatorSearchValues);
        charsWritten = 0;

        if (NegativeSignSearchValues.Contains(source.TrimStart()[0]))
        {
            destiantion[charsWritten++] = '-';
        }

        for (int i = charsWritten; i < source.Length; i++)
        {
            if (i == indexOfSeparator)
            {
                destiantion[charsWritten++] = Dot;
            }
            else if (AllArabicNumeralsSearchValues.Contains(source[i]))
            {
                destiantion[charsWritten++] = source[i];
            }
        }
    }
}
