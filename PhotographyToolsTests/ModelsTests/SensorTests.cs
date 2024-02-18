using Photography_Tools.Models;

namespace PhotographyToolsTests.ModelsTests;

public class SensorTests
{
    [Theory]
    [InlineData(24, 36, 43.2666153055679)]
    [InlineData(14.8, 22.2, 26.6810794384335)]
    public void CalculateDiagonal_ShouldReturnCalculatedDiagonalValueForFullFrame(double sensorHeigth, double sensorWidth, double expected)
    {
        double actual = Sensor.CalculateDiagonal(sensorHeigth, sensorWidth);

        Assert.Equal(expected, actual, 13);
    }

    [Theory]
    [InlineData(24, -1, "Argument cannot be less than zero (Parameter 'sensorWidth')")]
    [InlineData(-0.0001, 22.2, "Argument cannot be less than zero (Parameter 'sensorHeigth')")]
    public void CalculateDiagonal_ShouldThrowArgumentException(double sensorHeigth, double sensorWidth, string expectedMessage)
    {
        ArgumentException ex = Assert.Throws<ArgumentException>(() => Sensor.CalculateDiagonal(sensorHeigth, sensorWidth));
        Assert.Equal(expectedMessage, ex.Message);
    }

    [Fact]
    public void CalculateSensorResolution_ShouldReturnSensorResolution()
    {
        const int expectedHorizontal = 6038, expectedVertical = 4025;

        (int actualHorizontal, int actualVertical) = Sensor.CalculateSensorResolution(1.5, 24.3);

        Assert.Equal(expectedVertical, actualVertical);
        Assert.Equal(expectedHorizontal, actualHorizontal);
    }


    [Theory]
    [InlineData(-20, 24.6, "Argument cannot be less than zero (Parameter 'ratio')")]
    [InlineData(1.5, -22.2, "Argument cannot be less than zero (Parameter 'megapixels')")]
    public void CalculateSensorResolution_ShouldThrowArgumentException(double ratio, double megapixels, string expectedMessage)
    {
        ArgumentException ex = Assert.Throws<ArgumentException>(() => Sensor.CalculateSensorResolution(ratio, megapixels));
        Assert.Equal(expectedMessage, ex.Message);
    }

    [Fact]
    public void Sensor_ShouldCreateObjectWithCorrectValues()
    {
        const double 
            expectedSensorWidthMM = 36, 
            expectedSensorHeightMM = 24, 
            expectedMegapixels = 24, 
            expectedRatio = 1.5, 
            expectedDiagonal = 43.2666153055679, 
            expectedCropFactor = 1,
            expectedResolutionHorizontal = 6000,
            expectedResoultionVertical = 4000,
            expectedPixelPitch = 6;

        Sensor actual = new(expectedSensorWidthMM, expectedSensorHeightMM, expectedMegapixels);

        Assert.Equal(expectedSensorWidthMM, actual.SensorWidthMM);
        Assert.Equal(expectedSensorHeightMM, actual.SensorHeightMM);
        Assert.Equal(expectedMegapixels, actual.Megapixels);
        Assert.Equal(expectedRatio, actual.Ratio);
        Assert.Equal(expectedDiagonal, actual.Diagonal, 13);
        Assert.Equal(expectedCropFactor, actual.CropFactor, 13);
        Assert.Equal(expectedResolutionHorizontal, actual.ResolutionHorizontal);
        Assert.Equal(expectedResoultionVertical, actual.ResolutionVertical);
        Assert.Equal(expectedPixelPitch, actual.PixelPitch, 13);
    }
}