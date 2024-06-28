using Photography_Tools.Models.Interfaces;

namespace Photography_Tools.Models;

public class DofCalcInput : IValidatable
{
    public required Sensor CameraInfo { get; set; }
    public required Lens LensInfo { get; set; }
    public double FocusingDistanceMM { get; set; }
    public int PrintWidthMM { get; set; } = 270;
    public int PrintHeighthMM { get; set; } = 180;
    public int VisualAcuityLpPerMM { get; set; } = 5;
    public int StandardViewingDistanceMM { get; set; } = 250;
    public int ActualViewingDistanceMM { get; set; } = 250;

    public bool Validate()
    {
        return CameraInfo is not null && LensInfo is not null;
    }
}