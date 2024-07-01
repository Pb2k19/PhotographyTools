using Photography_Tools.Models;

namespace PhotographyToolsTests.ModelsTests;

public class SensorTests
{
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
            expectedResolutionVertical = 4000,
            expectedPixelPitch = 6;

        Sensor actual = new(expectedSensorHeightMM, expectedSensorWidthMM, expectedMegapixels);

        Assert.Equal(expectedSensorWidthMM, actual.SensorWidthMM);
        Assert.Equal(expectedSensorHeightMM, actual.SensorHeigthMM);
        Assert.Equal(expectedMegapixels, actual.Megapixels);
        Assert.Equal(expectedRatio, actual.Ratio);
        Assert.Equal(expectedDiagonal, actual.Diagonal, 13);
        Assert.Equal(expectedCropFactor, actual.CropFactor, 13);
        Assert.Equal(expectedResolutionHorizontal, actual.ResolutionHorizontal);
        Assert.Equal(expectedResolutionVertical, actual.ResolutionVertical);
        Assert.Equal(expectedPixelPitch, actual.PixelPitch, 13);
    }

    [Fact]
    public void SetSensorWidthMM_ShouldSetSensorWidthAndOtherCorrelatedData()
    {
        const double
            expectedSensorWidthMM = 36,
            expectedSensorHeightMM = 24,
            expectedMegapixels = 24,
            expectedRatio = 1.5,
            expectedDiagonal = 43.2666153055679,
            expectedCropFactor = 1,
            expectedResolutionHorizontal = 6000,
            expectedResolutionVertical = 4000,
            expectedPixelPitch = 6;

        Sensor actual = new(24, 30, 24);
        actual.SetSensorWidthMM(expectedSensorWidthMM);

        Assert.Equal(expectedSensorWidthMM, actual.SensorWidthMM);
        Assert.Equal(expectedSensorHeightMM, actual.SensorHeigthMM);
        Assert.Equal(expectedMegapixels, actual.Megapixels);
        Assert.Equal(expectedRatio, actual.Ratio);
        Assert.Equal(expectedDiagonal, actual.Diagonal, 13);
        Assert.Equal(expectedCropFactor, actual.CropFactor, 13);
        Assert.Equal(expectedResolutionHorizontal, actual.ResolutionHorizontal);
        Assert.Equal(expectedResolutionVertical, actual.ResolutionVertical);
        Assert.Equal(expectedPixelPitch, actual.PixelPitch, 13);
    }

    [Fact]
    public void SetSensorHeigthMM_ShouldSetSensorHeigthAndOtherCorrelatedData()
    {
        const double
            expectedSensorWidthMM = 36,
            expectedSensorHeightMM = 24,
            expectedMegapixels = 24,
            expectedRatio = 1.5,
            expectedDiagonal = 43.2666153055679,
            expectedCropFactor = 1,
            expectedResolutionHorizontal = 6000,
            expectedResolutionVertical = 4000,
            expectedPixelPitch = 6;

        Sensor actual = new(20, 36, 24);
        actual.SetSensorHeigthMM(expectedSensorHeightMM);

        Assert.Equal(expectedSensorWidthMM, actual.SensorWidthMM);
        Assert.Equal(expectedSensorHeightMM, actual.SensorHeigthMM);
        Assert.Equal(expectedMegapixels, actual.Megapixels);
        Assert.Equal(expectedRatio, actual.Ratio);
        Assert.Equal(expectedDiagonal, actual.Diagonal, 13);
        Assert.Equal(expectedCropFactor, actual.CropFactor, 13);
        Assert.Equal(expectedResolutionHorizontal, actual.ResolutionHorizontal);
        Assert.Equal(expectedResolutionVertical, actual.ResolutionVertical);
        Assert.Equal(expectedPixelPitch, actual.PixelPitch, 13);
    }

    [Fact]
    public void SetMegapixels_ShouldSetMegapixelsAndOtherCorrelatedData()
    {
        const double
            expectedSensorWidthMM = 36,
            expectedSensorHeightMM = 24,
            expectedMegapixels = 24,
            expectedRatio = 1.5,
            expectedDiagonal = 43.2666153055679,
            expectedCropFactor = 1,
            expectedResolutionHorizontal = 6000,
            expectedResolutionVertical = 4000,
            expectedPixelPitch = 6;

        Sensor actual = new(24, 36, 30);
        actual.SetMegapixels(24);


        Assert.Equal(expectedSensorWidthMM, actual.SensorWidthMM);
        Assert.Equal(expectedSensorHeightMM, actual.SensorHeigthMM);
        Assert.Equal(expectedMegapixels, actual.Megapixels);
        Assert.Equal(expectedRatio, actual.Ratio);
        Assert.Equal(expectedDiagonal, actual.Diagonal, 13);
        Assert.Equal(expectedCropFactor, actual.CropFactor, 13);
        Assert.Equal(expectedResolutionHorizontal, actual.ResolutionHorizontal);
        Assert.Equal(expectedResolutionVertical, actual.ResolutionVertical);
        Assert.Equal(expectedPixelPitch, actual.PixelPitch, 13);
    }
}

public class SensorCalculationsTests
{
    [Theory]
    [InlineData(24, 36, 43.2666153055679)]
    [InlineData(14.8, 22.2, 26.6810794384335)]
    public void CalculateDiagonal_ShouldReturnCalculatedDiagonalValue(double sensorHeigth, double sensorWidth, double expected)
    {
        double actual = SensorCalculations.CalculateDiagonal(sensorHeigth, sensorWidth);

        Assert.Equal(expected, actual, 13);
    }

    [Fact]
    public void CalculateSensorResolution_ShouldReturnSensorResolution()
    {
        const int expectedHorizontal = 6038, expectedVertical = 4025;

        (int actualHorizontal, int actualVertical) = SensorCalculations.CalculateSensorResolution(1.5, 24.3);

        Assert.Equal(expectedVertical, actualVertical);
        Assert.Equal(expectedHorizontal, actualHorizontal);
    }

    [Fact]
    public void CalculateRatio_ShouldReturnCalculatedRatio()
    {
        const double expected = 1.5;

        double actual = SensorCalculations.CalculateRatio(6000, 4000);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculatePixelPitch_ShouldReturnPixelPitch()
    {
        const double expected = 6;

        double actual = SensorCalculations.CalculatePixelPitch(36, 6000);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateCropFactor_ShouldReturnCropFactor()
    {
        const double expected = 1.53;

        double actual = SensorCalculations.CalculateCropFactor(28.20655952079232);

        Assert.Equal(expected, actual, 2);
    }
}