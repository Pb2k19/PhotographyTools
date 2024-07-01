using Photography_Tools.Models;

namespace PhotographyToolsTests.ModelsTests;

public class GeographicalCoordinatesTests
{
    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(20, 20, true)]
    [InlineData(90, 180, true)]
    [InlineData(-20, -20, true)]
    [InlineData(-90, -180, true)]
    [InlineData(90, -180.0001, false)]
    [InlineData(-90.001, 0, false)]
    [InlineData(90, 180.0001, false)]
    [InlineData(90.001, 0, false)]
    [InlineData(0, 180.0001, false)]
    [InlineData(90.001, 180.0001, false)]
    [InlineData(-90.001, -180.0001, false)]
    [InlineData(double.NaN, 20, false)]
    [InlineData(20, double.NaN, false)]
    [InlineData(double.NaN, double.NaN, false)]
    public void Validate_ShouldReturnTrueIfCoordinatesAreValid(double latitude, double longitude, bool expected)
    {
        GeographicalCoordinates coordinates = new(latitude, longitude);

        bool actual = coordinates.Validate();

        Assert.Equal(expected, actual);
    }
}
