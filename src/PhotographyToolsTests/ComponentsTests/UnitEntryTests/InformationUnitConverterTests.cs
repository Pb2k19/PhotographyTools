using Photography_Tools.Components.Controls.UnitEntry;
using Photography_Tools.Const;

namespace PhotographyToolsTests.ComponentsTests.UnitEntryTests;

public class InformationUnitConverterTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.00001)]
    [InlineData(-15, -0.000015)]
    [InlineData(23.706, 0.000023706)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromBToMB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, InformationUnitsConst.Byte);

        Assert.Equal(expected, actual, 15);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.01)]
    [InlineData(-150, -0.15)]
    [InlineData(23.706, 0.023706)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromkBToMB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, InformationUnitsConst.Kilobyte);

        Assert.Equal(expected, actual, 15);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(-150, -150)]
    [InlineData(23.706, 23.706)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromMBToMB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, InformationUnitsConst.Megabyte);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10.0005, 10_000.5)]
    [InlineData(-150, -150_000)]
    [InlineData(23.706, 23706)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromGBToMB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, InformationUnitsConst.Gigabyte);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10.0000005, 10000000.5)]
    [InlineData(-150, -150000000)]
    [InlineData(23.706, 23706000)]
    public void ConvertToBaseUnit_ShouldReturnConvertedValueFromTBToMB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertToBaseUnit(inputValue, InformationUnitsConst.Terabyte);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10000000)]
    [InlineData(-150, -150000000)]
    [InlineData(23.706, 23706000)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMBToB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, InformationUnitsConst.Byte);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10000)]
    [InlineData(-150, -150000)]
    [InlineData(23.706, 23706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMBTokB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, InformationUnitsConst.Kilobyte);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(-150, -150)]
    [InlineData(23.706, 23.706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMBToMB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, InformationUnitsConst.Megabyte);

        Assert.Equal(expected, actual, 15);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.01)]
    [InlineData(-150, -0.15)]
    [InlineData(23.706, 0.023706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMBToGB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, InformationUnitsConst.Gigabyte);

        Assert.Equal(expected, actual, 15);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.00001)]
    [InlineData(-150, -0.00015)]
    [InlineData(23.706, 0.000023706)]
    public void ConvertBaseToSelectedUnit_ShouldReturnConvertedValueFromMBToTB(double inputValue, double expected)
    {
        InformationUnitConverter converter = new();

        double actual = converter.ConvertBaseToSelectedUnit(inputValue, InformationUnitsConst.Terabyte);

        Assert.Equal(expected, actual, 15);
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