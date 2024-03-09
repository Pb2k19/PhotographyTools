using Photography_Tools.Const;
using Photography_Tools.Helpers;

namespace PhotographyToolsTests.HelpersTests;

public class UnitHelperTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(-15, -15)]
    [InlineData(23.706, 23.706)]
    public void ConvertUnitsToMM_ShouldReturnConvertedValueFromMMToMM(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertUnitsToMM(inputValue, UnitConst.LengthMM);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 100)]
    [InlineData(-150, -1_500)]
    [InlineData(23.706, 237.06)]
    public void ConvertUnitsToMM_ShouldReturnConvertedValueFromCMToMM(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertUnitsToMM(inputValue, UnitConst.LengthCM);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10000)]
    [InlineData(-150, -150000)]
    [InlineData(23.706, 23706)]
    public void ConvertUnitsToMM_ShouldReturnConvertedValueFromMToMM(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertUnitsToMM(inputValue, UnitConst.LengthM);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 254)]
    [InlineData(23.706, 602.1324)]
    public void ConvertUnitsToMM_ShouldReturnConvertedValueFromINToMM(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertUnitsToMM(inputValue, UnitConst.LengthIN);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 3048)]
    [InlineData(-150, -45_720)]
    [InlineData(23.706, 7225.5888)]
    public void ConvertUnitsToMM_ShouldReturnConvertedValueFromFTToMM(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertUnitsToMM(inputValue, UnitConst.LengthFT);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(-150, -150)]
    [InlineData(23.706, 23.706)]
    public void ConvertMMToUnits_ShouldReturnConvertedValueFromMMToMM(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertMMToUnits(inputValue, UnitConst.LengthMM);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 1)]
    [InlineData(-150, -15)]
    [InlineData(23.706, 2.3706)]
    public void ConvertMMToUnits_ShouldReturnConvertedValueFromMMToCM(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertMMToUnits(inputValue, UnitConst.LengthCM);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.01)]
    [InlineData(-150, -0.15)]
    [InlineData(23.706, 0.023706)]
    public void ConvertMMToUnits_ShouldReturnConvertedValueFromMMToM(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertMMToUnits(inputValue, UnitConst.LengthM);

        Assert.Equal(expected, actual, 15);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.393700787)]
    [InlineData(-150, -5.90551181)]
    [InlineData(23.706, 0.933307087)]
    public void ConvertMMToUnits_ShouldReturnConvertedValueFromMMToIN(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertMMToUnits(inputValue, UnitConst.LengthIN);

        Assert.Equal(expected, actual, 8);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0.032808399)]
    [InlineData(-150, -0.492125984)]
    [InlineData(23.706, 0.0777755906)]
    public void ConvertMMToUnits_ShouldReturnConvertedValueFromMMToFT(double inputValue, double expected)
    {
        double actual = UnitHelper.ConvertMMToUnits(inputValue, UnitConst.LengthFT);

        Assert.Equal(expected, actual, 8);
    }

    [Fact]
    public void ConvertMMToUnits_ShouldThrowArgumentException()
    {
        ArgumentException ex = Assert.Throws<ArgumentException>(() => UnitHelper.ConvertMMToUnits(20, "Unsupported"));
        Assert.Equal("Unsupported unit (Parameter 'outputUnit')", ex.Message);
    }

    [Fact]
    public void ConvertUnitsToMM_ShouldThrowArgumentException()
    {
        ArgumentException ex = Assert.Throws<ArgumentException>(() => UnitHelper.ConvertUnitsToMM(20, "Unsupported"));
        Assert.Equal("Unsupported unit (Parameter 'inputUnit')", ex.Message);
    }
}