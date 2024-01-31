namespace Photography_Tools.Models;

public class DofInfo
{
    public required CameraInfo CameraInfo { get; set; }
    public required LensInfo LensInfo { get; set; }
    public int FocusingDistanceMM { get; set; }
    public int PrintWidthMM { get; set; } = 270;
    public int PrintHeighthMM { get; set; } = 180;
    public int VisualAcuityLpPerMM { get; set; } = 5;
    public int StandardViewingDistanceMM { get; set; } = 250;
    public int ActualViewingDistanceMM { get; set; } = 250;

    // TO CALC
    public double CircleOfConfusion { get; set; }
    public double HyperfocalDistanceMM { get; set; }
    public double DofFarLimitMM { get; set; }
    public double DofNearLimitMM { get; set; }
    public double DofMM { get; set; }
    public double DofInFrontOfSubject { get; set; }
    public double DofInBackOfSubject { get; set; }
}