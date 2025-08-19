using Photography_Tools.Helpers;
using System.Globalization;

namespace PhotographyToolsTests.HelpersTests;

public class ParseHelperTests
{
    [Theory]
    [InlineData("0.0", 0)]
    [InlineData("0,0", 0)]
    [InlineData("0.01", 0.01)]
    [InlineData("0,01", 0.01)]
    [InlineData("-0.01", -0.01)]
    [InlineData("-0,01", -0.01)]
    [InlineData("0.00001", 0.00001)]
    [InlineData("45468.379684", 45468.379684)]
    [InlineData("45468,379684", 45468.379684)]
    [InlineData("-45468.379684", -45468.379684)]
    [InlineData("-45468,379684", -45468.379684)]
    [InlineData("45 468,379684", 45468.379684)]
    [InlineData("45 468.379684", 45468.379684)]
    [InlineData("45,468,379684", 45468.379684)]
    [InlineData("45,468.379684", 45468.379684)]
    [InlineData("45.468,379684", 45468.379684)]
    [InlineData("-45 468,379684", -45468.379684)]
    [InlineData("-45 468.379684", -45468.379684)]
    [InlineData("-45,468,379684", -45468.379684)]
    [InlineData("-45,468.379684", -45468.379684)]
    [InlineData("-45.468,379684", -45468.379684)]
    [InlineData("+45.468,379684", 45468.379684)]
    [InlineData("1234.56", 1234.56)]
    [InlineData("+1234.56", 1234.56)]
    [InlineData("1234,56", 1234.56)]
    [InlineData("+1234,56", 1234.56)]
    [InlineData("1.234,56", 1234.56)]
    [InlineData("1 234,56", 1234.56)]
    [InlineData("1'234.56", 1234.56)]
    [InlineData("1 234,56", 1234.56)]
    [InlineData("1,23,456.78", 123456.78)]
    [InlineData("1,234.56", 1234.56)]
    [InlineData("1234.567", 1234.567)]
    [InlineData("-12,445.68", -12445.68)]
    [InlineData("-12 445,68", -12445.68)]
    [InlineData("123,456,789.0", 123456789)]
    [InlineData("0.00000000000000000001", 0.00000000000000000001)]
    [InlineData("0,00000000000000000001", 0.00000000000000000001)]
    public void TryParseDifferentCulture_ShouldReturnTrueAndParsedValueAsDecimal(string input, decimal expected)
    {
        bool tryParseActualUs = ParseHelper.TryParseDifferentCulture(input, out decimal actualUs, new CultureInfo("en-US"));
        bool tryParseActualPl = ParseHelper.TryParseDifferentCulture(input, out decimal actualPl, new CultureInfo("pl-PL"));
        bool tryParseActualDe = ParseHelper.TryParseDifferentCulture(input, out decimal actualDe, new CultureInfo("de-DE"));
        bool tryParseActualDe_CH = ParseHelper.TryParseDifferentCulture(input, out decimal actualDe_CH, new CultureInfo("de-CH"));
        bool tryParseActualAf = ParseHelper.TryParseDifferentCulture(input, out decimal actualAf, new CultureInfo("af"));
        bool tryParseActualAr = ParseHelper.TryParseDifferentCulture(input, out decimal actualAr, new CultureInfo("ar"));
        bool tryParseActualAr_DZ = ParseHelper.TryParseDifferentCulture(input, out decimal actualAr_DZ, new CultureInfo("ar-DZ"));
        bool tryParseActualFr = ParseHelper.TryParseDifferentCulture(input, out decimal actualFr, new CultureInfo("fr"));
        bool tryParseActualNqo = ParseHelper.TryParseDifferentCulture(input, out decimal actualNqo, new CultureInfo("nqo"));

        Assert.Equal(expected, actualUs, 20);
        Assert.Equal(expected, actualPl, 20);
        Assert.Equal(expected, actualDe, 20);
        Assert.Equal(expected, actualDe_CH, 20);
        Assert.Equal(expected, actualAf, 20);
        Assert.Equal(expected, actualAr, 20);
        Assert.Equal(expected, actualAr_DZ, 20);
        Assert.Equal(expected, actualFr, 20);
        Assert.Equal(expected, actualNqo, 20);
        Assert.True(tryParseActualUs);
        Assert.True(tryParseActualPl);
        Assert.True(tryParseActualDe);
        Assert.True(tryParseActualDe_CH);
        Assert.True(tryParseActualAf);
        Assert.True(tryParseActualAr);
        Assert.True(tryParseActualAr_DZ);
        Assert.True(tryParseActualFr);
        Assert.True(tryParseActualNqo);
    }

    [Theory]
    [InlineData("0.0", 0)]
    [InlineData("0,0", 0)]
    [InlineData("0.01", 0.01)]
    [InlineData("0,01", 0.01)]
    [InlineData("-0.01", -0.01)]
    [InlineData("-0,01", -0.01)]
    [InlineData("0.00001", 0.00001)]
    [InlineData("45468.379684", 45468.379684)]
    [InlineData("45468,379684", 45468.379684)]
    [InlineData("-45468.379684", -45468.379684)]
    [InlineData("-45468,379684", -45468.379684)]
    [InlineData("45 468,379684", 45468.379684)]
    [InlineData("45 468.379684", 45468.379684)]
    [InlineData("45,468,379684", 45468.379684)]
    [InlineData("45,468.379684", 45468.379684)]
    [InlineData("45.468,379684", 45468.379684)]
    [InlineData("-45 468,379684", -45468.379684)]
    [InlineData("-45 468.379684", -45468.379684)]
    [InlineData("-45,468,379684", -45468.379684)]
    [InlineData("-45,468.379684", -45468.379684)]
    [InlineData("-45.468,379684", -45468.379684)]
    [InlineData("+45.468,379684", 45468.379684)]
    [InlineData("1234.56", 1234.56)]
    [InlineData("+1234.56", 1234.56)]
    [InlineData("1234,56", 1234.56)]
    [InlineData("+1234,56", 1234.56)]
    [InlineData("1.234,56", 1234.56)]
    [InlineData("1 234,56", 1234.56)]
    [InlineData("1'234.56", 1234.56)]
    [InlineData("1 234,56", 1234.56)]
    [InlineData("1,23,456.78", 123456.78)]
    [InlineData("1,234.56", 1234.56)]
    [InlineData("1234.567", 1234.567)]
    [InlineData("-12,445.68", -12445.68)]
    [InlineData("-12 445,68", -12445.68)]
    [InlineData("123,456,789.0", 123456789)]
    [InlineData("0.00000000000000000001", 0.00000000000000000001)]
    [InlineData("0,00000000000000000001", 0.00000000000000000001)]
    public void ParseDifferentCulture_ShouldReturnParsedValueAsDecimal(string input, decimal expected)
    {
        decimal actualUs = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("en-US"));
        decimal actualPl = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("pl-PL"));
        decimal actualDe = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("de-DE"));
        decimal actualDe_CH = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("de-CH"));
        decimal actualAf = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("af"));
        decimal actualAr = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("ar"));
        decimal actualAr_DZ = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("ar-DZ"));
        decimal actualFr = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("fr"));
        decimal actualNqo = ParseHelper.ParseDifferentCulture<decimal>(input, new CultureInfo("nqo"));

        Assert.Equal(expected, actualUs, 20);
        Assert.Equal(expected, actualPl, 20);
        Assert.Equal(expected, actualDe, 20);
        Assert.Equal(expected, actualDe_CH, 20);
        Assert.Equal(expected, actualAf, 20);
        Assert.Equal(expected, actualAr, 20);
        Assert.Equal(expected, actualAr_DZ, 20);
        Assert.Equal(expected, actualFr, 20);
        Assert.Equal(expected, actualNqo, 20);
    }

    [Theory]
    [InlineData("0.0", 0)]
    [InlineData("0,0", 0)]
    [InlineData("0.01", 0.01)]
    [InlineData("0,01", 0.01)]
    [InlineData("-0.01", -0.01)]
    [InlineData("-0,01", -0.01)]
    [InlineData("0.00001", 0.00001)]
    [InlineData("45468.379684", 45468.379684)]
    [InlineData("45468,379684", 45468.379684)]
    [InlineData("-45468.379684", -45468.379684)]
    [InlineData("-45468,379684", -45468.379684)]
    [InlineData("45 468,379684", 45468.379684)]
    [InlineData("45 468.379684", 45468.379684)]
    [InlineData("45,468,379684", 45468.379684)]
    [InlineData("45,468.379684", 45468.379684)]
    [InlineData("45.468,379684", 45468.379684)]
    [InlineData("-45 468,379684", -45468.379684)]
    [InlineData("-45 468.379684", -45468.379684)]
    [InlineData("-45,468,379684", -45468.379684)]
    [InlineData("-45,468.379684", -45468.379684)]
    [InlineData("-45.468,379684", -45468.379684)]
    [InlineData("+45.468,379684", 45468.379684)]
    [InlineData("1234.56", 1234.56)]
    [InlineData("+1234.56", 1234.56)]
    [InlineData("1234,56", 1234.56)]
    [InlineData("+1234,56", 1234.56)]
    [InlineData("1.234,56", 1234.56)]
    [InlineData("1 234,56", 1234.56)]
    [InlineData("1'234.56", 1234.56)]
    [InlineData("1 234,56", 1234.56)]
    [InlineData("1,23,456.78", 123456.78)]
    [InlineData("1,234.56", 1234.56)]
    [InlineData("1234.567", 1234.567)]
    [InlineData("-12,445.68", -12445.68)]
    [InlineData("-12 445,68", -12445.68)]
    [InlineData("123,456,789.0", 123456789)]
    [InlineData("0.00000000000000000001", 0.00000000000000000001)]
    [InlineData("0,00000000000000000001", 0.00000000000000000001)]
    public void TryParseDifferentCulture_ShouldReturnTrueAndParsedValueAsDouble(string input, double expected)
    {
        bool tryParseActualUs = ParseHelper.TryParseDifferentCulture(input, out double actualUs, new CultureInfo("en-US"));
        bool tryParseActualPl = ParseHelper.TryParseDifferentCulture(input, out double actualPl, new CultureInfo("pl-PL"));
        bool tryParseActualDe = ParseHelper.TryParseDifferentCulture(input, out double actualDe, new CultureInfo("de-DE"));
        bool tryParseActualDe_CH = ParseHelper.TryParseDifferentCulture(input, out double actualDe_CH, new CultureInfo("de-CH"));
        bool tryParseActualAf = ParseHelper.TryParseDifferentCulture(input, out double actualAf, new CultureInfo("af"));
        bool tryParseActualAr = ParseHelper.TryParseDifferentCulture(input, out double actualAr, new CultureInfo("ar"));
        bool tryParseActualAr_DZ = ParseHelper.TryParseDifferentCulture(input, out double actualAr_DZ, new CultureInfo("ar-DZ"));
        bool tryParseActualFr = ParseHelper.TryParseDifferentCulture(input, out double actualFr, new CultureInfo("fr"));
        bool tryParseActualNqo = ParseHelper.TryParseDifferentCulture(input, out double actualNqo, new CultureInfo("nqo"));

        Assert.Equal(expected, actualUs, 15);
        Assert.Equal(expected, actualPl, 15);
        Assert.Equal(expected, actualDe, 15);
        Assert.Equal(expected, actualDe_CH, 15);
        Assert.Equal(expected, actualAf, 15);
        Assert.Equal(expected, actualAr, 15);
        Assert.Equal(expected, actualAr_DZ, 15);
        Assert.Equal(expected, actualFr, 15);
        Assert.Equal(expected, actualNqo, 15);
        Assert.True(tryParseActualUs);
        Assert.True(tryParseActualPl);
        Assert.True(tryParseActualDe);
        Assert.True(tryParseActualDe_CH);
        Assert.True(tryParseActualAf);
        Assert.True(tryParseActualAr);
        Assert.True(tryParseActualAr_DZ);
        Assert.True(tryParseActualFr);
        Assert.True(tryParseActualNqo);
    }

    [Theory]
    [InlineData("0.0", 0)]
    [InlineData("0,0", 0)]
    [InlineData("0.01", 0.01)]
    [InlineData("0,01", 0.01)]
    [InlineData("-0.01", -0.01)]
    [InlineData("-0,01", -0.01)]
    [InlineData("0.00001", 0.00001)]
    [InlineData("45468.379684", 45468.379684)]
    [InlineData("45468,379684", 45468.379684)]
    [InlineData("-45468.379684", -45468.379684)]
    [InlineData("-45468,379684", -45468.379684)]
    [InlineData("45 468,379684", 45468.379684)]
    [InlineData("45 468.379684", 45468.379684)]
    [InlineData("45,468,379684", 45468.379684)]
    [InlineData("45,468.379684", 45468.379684)]
    [InlineData("45.468,379684", 45468.379684)]
    [InlineData("-45 468,379684", -45468.379684)]
    [InlineData("-45 468.379684", -45468.379684)]
    [InlineData("-45,468,379684", -45468.379684)]
    [InlineData("-45,468.379684", -45468.379684)]
    [InlineData("-45.468,379684", -45468.379684)]
    [InlineData("+45.468,379684", 45468.379684)]
    [InlineData("1234.56", 1234.56)]
    [InlineData("+1234.56", 1234.56)]
    [InlineData("1234,56", 1234.56)]
    [InlineData("+1234,56", 1234.56)]
    [InlineData("1.234,56", 1234.56)]
    [InlineData("1 234,56", 1234.56)]
    [InlineData("1'234.56", 1234.56)]
    [InlineData("1 234,56", 1234.56)]
    [InlineData("1,23,456.78", 123456.78)]
    [InlineData("1,234.56", 1234.56)]
    [InlineData("1234.567", 1234.567)]
    [InlineData("-12,445.68", -12445.68)]
    [InlineData("-12 445,68", -12445.68)]
    [InlineData("123,456,789.0", 123456789)]
    [InlineData("0.00000000000000000001", 0.00000000000000000001)]
    [InlineData("0,00000000000000000001", 0.00000000000000000001)]
    public void ParseDifferentCulture_ShouldReturnParsedValueAsDouble(string input, double expected)
    {
        double actualUs = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("en-US"));
        double actualPl = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("pl-PL"));
        double actualDe = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("de-DE"));
        double actualDe_CH = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("de-CH"));
        double actualAf = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("af"));
        double actualAr = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("ar"));
        double actualAr_DZ = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("ar-DZ"));
        double actualFr = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("fr"));
        double actualNqo = ParseHelper.ParseDifferentCulture<double>(input, new CultureInfo("nqo"));

        Assert.Equal(expected, actualUs, 15);
        Assert.Equal(expected, actualPl, 15);
        Assert.Equal(expected, actualDe, 15);
        Assert.Equal(expected, actualDe_CH, 15);
        Assert.Equal(expected, actualAf, 15);
        Assert.Equal(expected, actualAr, 15);
        Assert.Equal(expected, actualAr_DZ, 15);
        Assert.Equal(expected, actualFr, 15);
        Assert.Equal(expected, actualNqo, 15);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("-1", -1)]
    [InlineData("45468", 45468)]
    [InlineData("-45468", -45468)]
    [InlineData("45 468", 45468)]
    [InlineData("-45 468", -45468)]
    [InlineData("45.468", 45468)]
    [InlineData("123456", 123456)]
    [InlineData("+123456", 123456)]
    [InlineData("1 234", 1234)]
    [InlineData("1'234", 1234)]
    [InlineData("12,345,678", 12345678)]
    [InlineData("123,456", 123456)]
    [InlineData("1.234.567", 1234567)]
    [InlineData("-1,244,568", -1244568)]
    [InlineData("123,456,789", 123456789)]
    [InlineData("+1,068,379,684", 1068379684)]
    [InlineData("+1.068.379.684", 1068379684)]
    [InlineData("-1,068,379,684", -1068379684)]
    [InlineData("-1.068.379.684", -1068379684)]
    public void TryParseDifferentCulture_ShouldReturnTrueAndParsedValueAsInt(string input, int expected)
    {
        bool tryParseActualUs = ParseHelper.TryParseDifferentCulture(input, out int actualUs, new CultureInfo("en-US"));
        bool tryParseActualPl = ParseHelper.TryParseDifferentCulture(input, out int actualPl, new CultureInfo("pl-PL"));
        bool tryParseActualDe = ParseHelper.TryParseDifferentCulture(input, out int actualDe, new CultureInfo("de-DE"));
        bool tryParseActualDe_CH = ParseHelper.TryParseDifferentCulture(input, out int actualDe_CH, new CultureInfo("de-CH"));
        bool tryParseActualAf = ParseHelper.TryParseDifferentCulture(input, out int actualAf, new CultureInfo("af"));
        bool tryParseActualAr = ParseHelper.TryParseDifferentCulture(input, out int actualAr, new CultureInfo("ar"));
        bool tryParseActualAr_DZ = ParseHelper.TryParseDifferentCulture(input, out int actualAr_DZ, new CultureInfo("ar-DZ"));
        bool tryParseActualFr = ParseHelper.TryParseDifferentCulture(input, out int actualFr, new CultureInfo("fr"));
        bool tryParseActualNqo = ParseHelper.TryParseDifferentCulture(input, out int actualNqo, new CultureInfo("nqo"));

        Assert.Equal(expected, actualUs);
        Assert.Equal(expected, actualPl);
        Assert.Equal(expected, actualDe);
        Assert.Equal(expected, actualDe_CH);
        Assert.Equal(expected, actualAf);
        Assert.Equal(expected, actualAr);
        Assert.Equal(expected, actualAr_DZ);
        Assert.Equal(expected, actualFr);
        Assert.Equal(expected, actualNqo);
        Assert.True(tryParseActualUs);
        Assert.True(tryParseActualPl);
        Assert.True(tryParseActualDe);
        Assert.True(tryParseActualDe_CH);
        Assert.True(tryParseActualAf);
        Assert.True(tryParseActualAr);
        Assert.True(tryParseActualAr_DZ);
        Assert.True(tryParseActualFr);
        Assert.True(tryParseActualNqo);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("-1", -1)]
    [InlineData("45468", 45468)]
    [InlineData("-45468", -45468)]
    [InlineData("45 468", 45468)]
    [InlineData("-45 468", -45468)]
    [InlineData("45.468", 45468)]
    [InlineData("123456", 123456)]
    [InlineData("+123456", 123456)]
    [InlineData("1 234", 1234)]
    [InlineData("1'234", 1234)]
    [InlineData("12,345,678", 12345678)]
    [InlineData("123,456", 123456)]
    [InlineData("1.234.567", 1234567)]
    [InlineData("-1,244,568", -1244568)]
    [InlineData("123,456,789", 123456789)]
    [InlineData("+1,068,379,684", 1068379684)]
    [InlineData("+1.068.379.684", 1068379684)]
    [InlineData("-1,068,379,684", -1068379684)]
    [InlineData("-1.068.379.684", -1068379684)]
    public void ParseDifferentCulture_ShouldReturnParsedValueAsInt(string input, int expected)
    {
        int actualUs = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("en-US"));
        int actualPl = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("pl-PL"));
        int actualDe = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("de-DE"));
        int actualDe_CH = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("de-CH"));
        int actualAf = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("af"));
        int actualAr = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("ar"));
        int actualAr_DZ = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("ar-DZ"));
        int actualFr = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("fr"));
        int actualNqo = ParseHelper.ParseDifferentCulture<int>(input, new CultureInfo("nqo"));

        Assert.Equal(expected, actualUs);
        Assert.Equal(expected, actualPl);
        Assert.Equal(expected, actualDe);
        Assert.Equal(expected, actualDe_CH);
        Assert.Equal(expected, actualAf);
        Assert.Equal(expected, actualAr);
        Assert.Equal(expected, actualAr_DZ);
        Assert.Equal(expected, actualFr);
        Assert.Equal(expected, actualNqo);
    }
}