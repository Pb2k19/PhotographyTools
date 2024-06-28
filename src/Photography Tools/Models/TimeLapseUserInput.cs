namespace Photography_Tools.Models;

public class TimeLapseUserInput : UserInput
{
    public required TimeLapseCalcValues TimeLapseCalcValues { get; set; }
    public double EnteredClipFrameRateFPS { get; set; } = 30;
    public int EnteredShootsCount { get; set; } = 150;
    public int ShootingIntervalSelectedUnitIndex { get; set; } = 1;
    public int ShootingLengthSelectedUnitIndex { get; set; } = 2;
    public int ClipLengthSelectedUnitIndex { get; set; } = 1;
    public int PhotoSizeSelectedUnitIndex { get; set; } = 2;
    public int TotalStorageSelectedUnitIndex { get; set; } = 3;

    public override bool Validate()
    {
        return TimeLapseCalcValues is not null;
    }
}