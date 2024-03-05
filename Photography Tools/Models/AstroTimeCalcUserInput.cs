namespace Photography_Tools.Models;

public class AstroTimeCalcUserInput
{
    public required Lens Lens { get; set; }
    public required string SelectedSensorName { get; set; }
    public double Decilination { get; set; }
    public int FocalLengthUnitIndex { get; set; } = 0;
}