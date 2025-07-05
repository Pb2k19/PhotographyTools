using Photography_Tools.Helpers;

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
        bool tryParseActual = ParseHelper.TryParseDifferentCulture(input, out decimal actual);

        Assert.Equal(expected, actual, 20);
        Assert.True(tryParseActual);
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
        decimal actual = ParseHelper.ParseDifferentCulture<decimal>(input);

        Assert.Equal(expected, actual, 20);
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
        bool tryParseActual = ParseHelper.TryParseDifferentCulture(input, out double actual);

        Assert.Equal(expected, actual, 15);
        Assert.True(tryParseActual);
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
        double actual = ParseHelper.ParseDifferentCulture<double>(input);

        Assert.Equal(expected, actual, 15);
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
        bool tryParseActual = ParseHelper.TryParseDifferentCulture(input, out int actual);

        Assert.Equal(expected, actual);
        Assert.True(tryParseActual);
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
        double actual = ParseHelper.ParseDifferentCulture<int>(input);

        Assert.Equal(expected, actual);
    }
}