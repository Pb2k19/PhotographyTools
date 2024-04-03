namespace Photography_Tools.Models;

public class TimeLapseCalcValues
{
    public double ShootingIntervalSeconds { get; set; } = 2;
    public double ShootingLengthSeconds { get; set; } = 300;
    public int ShootsCount { get; set; } = 150;
    public double ClipFrameRateFPS { get; set; } = 30;
    public double ClipLengthSeconds { get; set; } = 5;
    public double PhotoSizeMB { get; set; } = 24;
    public double TotalStorageSizeMB { get; set; } = 3600;

    public void CalculateInterval()
        => ShootingIntervalSeconds = ShootsCount is not 0 ? ShootingLengthSeconds / ShootsCount : 0;

    public void CalculateLength()
        => ShootingLengthSeconds = ShootsCount * ShootingIntervalSeconds;

    public void CalculateShotsCount()
        => ShootsCount = ShootingIntervalSeconds is not 0 ? (int)(ShootingLengthSeconds / ShootingIntervalSeconds) : 0;

    public void CalculateShotsCountFromResultClip()
        => ShootsCount = (int)(ClipFrameRateFPS * ClipLengthSeconds);

    public void CalculateShotsCountFromStorage()
        => ShootsCount = PhotoSizeMB is not 0 ? (int)(TotalStorageSizeMB / PhotoSizeMB) : 0;

    public void CalculateClipLength()
        => ClipLengthSeconds = ClipFrameRateFPS is not 0 ? ShootsCount / ClipFrameRateFPS : 0;

    public void CalculateFrameRatio()
        => ClipFrameRateFPS = ClipLengthSeconds is not 0 ? ShootsCount / ClipLengthSeconds : 0;

    public void CalculatePhotoSize()
        => PhotoSizeMB = ShootsCount is not 0 ? TotalStorageSizeMB / ShootsCount : 0;

    public void CalculateStorageSize()
        => TotalStorageSizeMB = PhotoSizeMB * ShootsCount;
}

public class TimeLapseUserInput
{
    public required TimeLapseCalcValues TimeLapseCalcValues { get; set; }
    public double EnteredClipFrameRateFPS { get; set; } = 30;
    public int EnteredShootsCount { get; set; } = 150;
    public int ShootingIntervalSelectedUnitIndex { get; set; } = 1;
    public int ShootingLengthSelectedUnitIndex { get; set; } = 2;
    public int ClipLengthSelectedUnitIndex { get; set; } = 1;
    public int PhotoSizeSelectedUnitIndex { get; set; } = 2;
    public int TotalStorageSelectedUnitIndex { get; set; } = 3;
}