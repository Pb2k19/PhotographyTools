using System.Buffers;
using System.Globalization;
using System.Numerics;

namespace Photography_Tools.Helpers;

public static class ParseHelper
{
    public const char Comma = ',', Dot = '.', Space = ' ', ThinSpace = ' ';

    public static readonly SearchValues<char>
        StandardFloatCharacterSearchValues,
        StandardIntCharacterSearchValues,
        DecimalSeparatorSearchValues,
        NegativeSignSearchValues,
        AllArabicNumeralsSearchValues;

    static ParseHelper()
    {
        HashSet<char>
            decimalSeparators = [Comma, Dot,],
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
        StandardFloatCharacterSearchValues = SearchValues.Create('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '-', '+');
        StandardIntCharacterSearchValues = SearchValues.Create('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '+');
    }

    public static bool TryParseDifferentCulture<T>(ReadOnlySpan<char> readonlySpan, out T? value, IFormatProvider? defaultFormat = null) where T : INumber<T>
    {
        bool isInteger = NumberTypeHelper<T>.IsInteger;
        NumberStyles numberStyles = isInteger ? NumberStyles.Integer : NumberStyles.Float;

        if (readonlySpan.ContainsAnyExcept(isInteger ? StandardIntCharacterSearchValues : StandardFloatCharacterSearchValues))
        {
            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];

            GetNormalizedSpan(readonlySpan, span, isInteger, out int charsWritten);

            if (T.TryParse(span[..charsWritten], numberStyles, CultureInfo.InvariantCulture, out value))
                return true;

            return T.TryParse(readonlySpan, NumberStyles.Any, defaultFormat is not null ? defaultFormat : CultureInfo.CurrentCulture, out value);
        }
        else
        {
            if (T.TryParse(readonlySpan, numberStyles, CultureInfo.InvariantCulture, out value))
                return true;
        }
        return false;
    }

    public static T ParseDifferentCulture<T>(ReadOnlySpan<char> readonlySpan, IFormatProvider? defaultFormat = null) where T : INumber<T>
    {
        bool isInteger = NumberTypeHelper<T>.IsInteger;
        NumberStyles numberStyles = isInteger ? NumberStyles.Integer : NumberStyles.Float;

        if (readonlySpan.ContainsAnyExcept(isInteger ? StandardIntCharacterSearchValues : StandardFloatCharacterSearchValues))
        {
            Span<char> span = readonlySpan.Length * sizeof(char) <= 512 ? stackalloc char[readonlySpan.Length] : new char[readonlySpan.Length];

            GetNormalizedSpan(readonlySpan, span, isInteger, out int charsWritten);

            if (T.TryParse(span, numberStyles, CultureInfo.InvariantCulture, out T? value))
                return value;

            return T.Parse(readonlySpan, NumberStyles.Any, defaultFormat is not null ? defaultFormat : CultureInfo.CurrentCulture);
        }
        else
            return T.Parse(readonlySpan, numberStyles, CultureInfo.InvariantCulture);
    }

    private static void GetNormalizedSpan(ReadOnlySpan<char> source, Span<char> destiantion, bool isInteger, out int charsWritten)
    {
        int indexOfSeparator = isInteger ? -1 : source.LastIndexOfAny(DecimalSeparatorSearchValues);
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