using Photography_Tools.Components.Controls.UnitEntry;
using Photography_Tools.Const;

namespace PhotographyToolsTests.ComponentsTests.UnitEntryTests;

public class LengthUnitConverterTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(-15, -15)]
    [InlineData(23.706, 23.706)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromMMToMM(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, LengthUnitsConst.Millimeter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 100)]
    [InlineData(-150, -1_500)]
    [InlineData(23.706, 237.06)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromCMToMM(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, LengthUnitsConst.Centimeter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10000)]
    [InlineData(-150, -150000)]
    [InlineData(23.706, 23706)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromMToMM(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, LengthUnitsConst.Meter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 254)]
    [InlineData(-150, -3810)]
    [InlineData(23.706, 602.1324)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromINToMM(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, LengthUnitsConst.Inch);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 3048)]
    [InlineData(-150, -45_720)]
    [InlineData(23.706, 7225.5888)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromFTToMM(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, LengthUnitsConst.Foot);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(-150, -150)]
    [InlineData(23.706, 23.706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMMToMM(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, LengthUnitsConst.Millimeter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 1)]
    [InlineData(-150, -15)]
    [InlineData(23.706, 2.3706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMMToCM(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, LengthUnitsConst.Centimeter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.01)]
    [InlineData(-150, -0.15)]
    [InlineData(23.706, 0.023706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMMToM(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, LengthUnitsConst.Meter);

        Assert.Equal(expected, actual, 15);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.393700787)]
    [InlineData(-150, -5.90551181)]
    [InlineData(23.706, 0.933307087)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMMToIN(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, LengthUnitsConst.Inch);

        Assert.Equal(expected, actual, 8);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.032808399)]
    [InlineData(-150, -0.492125984)]
    [InlineData(23.706, 0.0777755906)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMMToFT(double inputValue, double expected)
    {
        LengthUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, LengthUnitsConst.Foot);

        Assert.Equal(expected, actual, 8);
    }

    [Fact]
    public void ConvertBaseToSelectedUnit_ShouldThrowArgumentException()
    {
        LengthUnitConverter converter = new();

        ArgumentException ex = Assert.Throws<ArgumentException>(() => converter.ConvertBaseToSelectedUnit(20, "Unsupported"));

        Assert.Equal("Unsupported unit (Parameter 'outputUnit')", ex.Message);
    }

    [Fact]
    public void ConvertToBaseUnit_ShouldThrowArgumentException()
    {
        LengthUnitConverter converter = new();

        ArgumentException ex = Assert.Throws<ArgumentException>(() => converter.ConvertToBaseUnit(20, "Unsupported"));

        Assert.Equal("Unsupported unit (Parameter 'inputUnit')", ex.Message);
    }
}