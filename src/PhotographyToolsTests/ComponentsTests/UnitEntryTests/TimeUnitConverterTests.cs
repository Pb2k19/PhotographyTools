using Photography_Tools.Components.Controls.UnitEntry;
using Photography_Tools.Const;

namespace PhotographyToolsTests.ComponentsTests.UnitEntryTests;

public class TimeUnitConverterTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(10000, 10)]
    [InlineData(-15000, -15)]
    [InlineData(23706, 23.706)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromMsToS(double inputValue, double expected)
    {
        TimeUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, TimeUnitsConst.TimeMs);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(-150, -150)]
    [InlineData(23.706, 23.706)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromSToS(double inputValue, double expected)
    {
        TimeUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, TimeUnitsConst.TimeS);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 600)]
    [InlineData(-150, -9000)]
    [InlineData(23.706, 1422.36)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromMinToS(double inputValue, double expected)
    {
        TimeUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, TimeUnitsConst.TimeMin);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 36000)]
    [InlineData(-150, -540_000)]
    [InlineData(23.706, 85_341.6)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromHToS(double inputValue, double expected)
    {
        TimeUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, TimeUnitsConst.TimeH);

        Assert.Equal(expected, actual, 10);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10_000)]
    [InlineData(-150, -150_000)]
    [InlineData(23.706, 23706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromSToMs(double inputValue, double expected)
    {
        TimeUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, TimeUnitsConst.TimeMs);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(-150, -150)]
    [InlineData(23.706, 23.706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromSToS(double inputValue, double expected)
    {
        TimeUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, TimeUnitsConst.TimeS);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.166666666666666)]
    [InlineData(-150, -2.5)]
    [InlineData(23.706, 0.3951)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromSToMin(double inputValue, double expected)
    {
        TimeUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, TimeUnitsConst.TimeMin);

        Assert.Equal(expected, actual, 10);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.0027777777777778)]
    [InlineData(-150, -0.0416666666666667)]
    [InlineData(23.706, 0.006585)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMMToFT(double inputValue, double expected)
    {
        TimeUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, TimeUnitsConst.TimeH);

        Assert.Equal(expected, actual, 10);
    }

    [Fact]
    public void ConvertBaseToSelectedUnit_ShouldThrowArgumentException()
    {
        TimeUnitConverter converter = new();

        ArgumentException ex = Assert.Throws<ArgumentException>(() => converter.ConvertBaseToSelectedUnit(20, "Unsupported"));

        Assert.Equal("Unsupported unit (Parameter 'outputUnit')", ex.Message);
    }

    [Fact]
    public void ConvertToBaseUnit_ShouldThrowArgumentException()
    {
        TimeUnitConverter converter = new();

        ArgumentException ex = Assert.Throws<ArgumentException>(() => converter.ConvertToBaseUnit(20, "Unsupported"));

        Assert.Equal("Unsupported unit (Parameter 'inputUnit')", ex.Message);
    }
}