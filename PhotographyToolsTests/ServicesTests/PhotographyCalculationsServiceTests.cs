using Photography_Tools.Const;
using Photography_Tools.Services.PhotographyCalculationsService;

namespace PhotographyToolsTests.ServicesTests;

public class PhotographyCalculationsServiceTests
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
        double actaul = PhotographyCalculationsService.DegreesToRadians(input);

        Assert.Equal(expected, actaul, 8);
    }

    [Theory]
    [InlineData(11, ApertureConst.FullStopsMultiplier)]
    [InlineData(9.5, ApertureConst.SecondStopsMultiplier)]
    [InlineData(25, ApertureConst.ThirdStopsMultiplier)]
    [InlineData(0.0001, ApertureConst.Undefined)]
    public void GetApertureMultiplier_ShouldReturnApertureMultiplier(double input, int expected)
    {
        int actual = PhotographyCalculationsService.GetApertureMultiplier(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(2, ApertureConst.FullStopsMultiplier, 2)]
    [InlineData(2.8, ApertureConst.SecondStopsMultiplier, 6)]
    [InlineData(25, ApertureConst.ThirdStopsMultiplier, 28)]
    public void FindXForFocalRatio_ShouldReturnXValue(double input, int apertureMultiplier, int expected)
    {
        int actual = PhotographyCalculationsService.FindXForFocalRatio(input, apertureMultiplier);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(1.05, 1.05)]
    [InlineData(11, 11.314)]
    [InlineData(2.8, 2.8284)]
    [InlineData(0.00001, 0.00001)]
    public void CalculateFullApertureValue_ShouldRetrunFullApertureValue(double input, double expected)
    {
        double actual = PhotographyCalculationsService.CalculateFullApertureValue(input);

        Assert.Equal(expected, actual, 3);
    }

    [Fact]
    public void CalculateTimeForAstro_ShouldReturnValueForRule200and300and500()
    {
        const double 
            expected200 = 5.55555555555556, 
            expected300 = 8.33333333333334,
            expected500 = 13.88888888888889;
        PhotographyCalculationsService service = new();

        (double actual200, double actual300, double actual500) = service.CalculateTimeForAstro(1.5, 24);

        Assert.Equal(expected200, actual200, 10);
        Assert.Equal(expected300, actual300, 10);
        Assert.Equal(expected500, actual500, 10);
    }
}