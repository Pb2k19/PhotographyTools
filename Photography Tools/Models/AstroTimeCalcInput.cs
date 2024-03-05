namespace Photography_Tools.Models;

public class AstroTimeCalcInput
{
    public required Lens Lens { get; set; }
    public required string SelectedSensorName { get; set; }
    public double Decilination { get; set; }
}