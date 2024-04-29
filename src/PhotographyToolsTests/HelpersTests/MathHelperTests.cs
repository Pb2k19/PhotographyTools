using Photography_Tools.Helpers;

namespace PhotographyToolsTests.HelpersTests;

public class MathHelperTests
{
    [Theory]
    [InlineData(1, 0.0174532925)]
    [InlineData(15, 0.261799388)]
    [InlineData(-15, -0.261799388)]
    [InlineData(90, 1.57079633)]
    [InlineData(180, 3.14159265)]
    [InlineData(270, 4.71238898)]
    [InlineData(360, 6.28318531)]
    [InlineData(370, 6.45771823)]
    public void DegreesToRadians_ShouldReturnValueInRadians(double input, double expected)
    {
        double actaul = MathHelper.DegreesToRadians(input);

        Assert.Equal(expected, actaul, 8);
    }
}