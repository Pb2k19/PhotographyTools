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
    [InlineData("0.00000000000000000001", 0.00000000000000000001)]
    [InlineData("0,00000000000000000001", 0.00000000000000000001)]
    public void TryParseDecimalDifferentCulture_ShouldReturnTrueAndParsedValueAsDouble(string input, decimal expected)
    {
        bool tryParseActual = ParseHelper.TryParseDecimalDifferentCulture(input, out decimal actual);

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
    [InlineData("0.00000000000000000001", 0.00000000000000000001)]
    [InlineData("0,00000000000000000001", 0.00000000000000000001)]
    public void ParseDecimalDifferentCulture_ShouldReturnParsedValueAsDouble(string input, decimal expected)
    {
        decimal actual = ParseHelper.ParseDecimalDifferentCulture(input);

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
    [InlineData("0.000000000000001", 0.000000000000001)]
    [InlineData("0,000000000000001", 0.000000000000001)]
    public void TryParseDoubleDifferentCulture_ShouldReturnTrueAndParsedValueAsDouble(string input, double expected)
    {
        bool tryParseActual = ParseHelper.TryParseDoubleDifferentCulture(input, out double actual);

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
    [InlineData("0.000000000000001", 0.000000000000001)]
    [InlineData("0,000000000000001", 0.000000000000001)]
    public void ParseDoubleDifferentCulture_ShouldReturnParsedValueAsDouble(string input, double expected)
    {
        double actual = ParseHelper.ParseDoubleDifferentCulture(input);

        Assert.Equal(expected, actual, 15);
    }

    [Fact]
    public void ParseDoubleDifferentCulture_ShouldThrowFormatException()
    {
        Assert.Throws<FormatException>(() => ParseHelper.ParseDoubleDifferentCulture("12.34a"));
    }

    [Fact]
    public void ParseDecimalDifferentCulture_ShouldThrowFormatException()
    {
        Assert.Throws<FormatException>(() => ParseHelper.ParseDecimalDifferentCulture("12.34a"));
    }

    [Fact]
    public void TryParseDoubleDifferentCulture_ShouldReturnFalse()
    {
        const double expectedValue = 0;
        bool actual = ParseHelper.TryParseDoubleDifferentCulture("12.34a", out double actualValue);

        Assert.False(actual);
        Assert.Equal(expectedValue, actualValue, 5);
    }

    [Fact]
    public void ParseDecimalDifferentCulture_ShouldReturnFalse()
    {
        const decimal expectedValue = 0;
        bool actual = ParseHelper.TryParseDecimalDifferentCulture("12.34a", out decimal actualValue);

        Assert.False(actual);
        Assert.Equal(expectedValue, actualValue, 5);
    }
}