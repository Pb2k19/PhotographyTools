using Photography_Tools.Models;

namespace PhotographyToolsTests.ModelsTests;

public class TimeLapseCalcValuesTests
{
    [Theory]
    [InlineData(10, 5, 2)]
    [InlineData(10.5, 3, 3.5)]
    [InlineData(20, 0, 0)]
    [InlineData(0, 2, 0)]
    [InlineData(0, 0, 0)]
    public void CalculateInterval_ShouldCalculateCorrectly(double sls, int sc, double expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            ShootingLengthSeconds = sls,
            ShootsCount = sc
        };

        calculator.CalculateInterval();

        Assert.Equal(expected, calculator.ShootingIntervalSeconds);
    }

    [Theory]
    [InlineData(2, 5, 10)]
    [InlineData(40.5, 2, 81)]
    [InlineData(40.5, 3, 121.5)]
    [InlineData(0, 3, 0)]
    [InlineData(40.5, 0, 0)]
    [InlineData(0, 0, 0)]
    public void CalculateLength_ShouldCalculateCorrectly(double sis, int sc, double expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            ShootingIntervalSeconds = sis,
            ShootsCount = sc
        };

        calculator.CalculateLength();

        Assert.Equal(expected, calculator.ShootingLengthSeconds);
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(70, 7, 10)]
    [InlineData(72, 7, 10)]
    [InlineData(91, 7, 13)]
    [InlineData(93, 7, 13)]
    [InlineData(96.1, 7, 13)]
    [InlineData(0, 7, 0)]
    [InlineData(96, 0, 0)]
    [InlineData(0, 0, 0)]
    public void CalculateShotsCount_ShouldCalculateCorrectly(double sls, double sis, int expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            ShootingLengthSeconds = sls,
            ShootingIntervalSeconds = sis
        };

        calculator.CalculateShotsCount();

        Assert.Equal(expected, calculator.ShootsCount);
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(70, 7, 10)]
    [InlineData(72, 7, 10)]
    [InlineData(91, 7, 13)]
    [InlineData(93, 7, 13)]
    [InlineData(96.3, 7, 13)]
    [InlineData(0, 7, 0)]
    [InlineData(96, 0, 0)]
    [InlineData(0, 0, 0)]
    public void CalculateShotsCountFromStorage_ShouldCalculateCorrectly(double ts, double ps, int expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            PhotoSizeMB = ps,
            TotalStorageSizeMB = ts
        };

        calculator.CalculateShotsCountFromStorage();

        Assert.Equal(expected, calculator.ShootsCount);
    }

    [Theory]
    [InlineData(10, 2, 20)]
    [InlineData(70, 7, 490)]
    [InlineData(72, 7, 504)]
    [InlineData(91.5, 7, 640)]
    [InlineData(0, 7, 0)]
    [InlineData(96, 0, 0)]
    [InlineData(0, 0, 0)]
    public void CalculateShotsCountFromResultClip_ShouldCalculateCorrectly(double cls, double cfr, int expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            ClipLengthSeconds = cls,
            ClipFrameRateFPS = cfr
        };

        calculator.CalculateShotsCountFromResultClip();

        Assert.Equal(expected, calculator.ShootsCount);
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(7, 7, 1)]
    [InlineData(13, 4, 3.25)]
    [InlineData(14, 3.5, 4)]
    [InlineData(0, 4, 0)]
    [InlineData(13, 0, 0)]
    [InlineData(0, 0, 0)]
    public void CalculateClipLength_ShouldCalculateCorrectly(int sc, double cfr, double expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            ShootsCount = sc,
            ClipFrameRateFPS = cfr
        };

        calculator.CalculateClipLength();

        Assert.Equal(expected, calculator.ClipLengthSeconds);
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(7, 7, 1)]
    [InlineData(13, 4, 3.25)]
    [InlineData(14, 3.5, 4)]
    [InlineData(0, 4, 0)]
    [InlineData(13, 0, 0)]
    [InlineData(0, 0, 0)]
    public void CalculateFrameRatio_ShouldCalculateCorrectly(int sc, double cls, double expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            ShootsCount = sc,
            ClipLengthSeconds = cls
        };

        calculator.CalculateFrameRatio();

        Assert.Equal(expected, calculator.ClipFrameRateFPS);
    }

    [Theory]
    [InlineData(200, 20, 10)]
    [InlineData(2, 20, 0.1)]
    [InlineData(0, 4, 0)]
    [InlineData(13, 0, 0)]
    [InlineData(0, 0, 0)]
    public void CalculatePhotoSize_ShouldCalculateCorrectly(double tss, int sc, double expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            TotalStorageSizeMB = tss,
            ShootsCount = sc
        };

        calculator.CalculatePhotoSize();

        Assert.Equal(expected, calculator.PhotoSizeMB);
    }

    [Theory]
    [InlineData(10, 20, 200)]
    [InlineData(0.1, 20, 2)]
    [InlineData(0, 4, 0)]
    [InlineData(13, 0, 0)]
    [InlineData(0, 0, 0)]
    public void CalculateStorage_ShouldCalculateCorrectly(double ps, int sc, double expected)
    {
        TimeLapseCalcValues calculator = new()
        {
            PhotoSizeMB = ps,
            ShootsCount = sc
        };

        calculator.CalculateStorageSize();

        Assert.Equal(expected, calculator.TotalStorageSizeMB);
    }
}