namespace Photography_Tools.Models;

public class AstroTimeCalcUserInput : UserInput
{
    public required Lens Lens { get; set; }
    public required string SelectedSensorName { get; set; }
    public double Decilination { get; set; }
    public int FocalLengthUnitIndex { get; set; } = 0;

    public override bool Validate()
    {
        return Lens is not null && !string.IsNullOrWhiteSpace(SelectedSensorName);
    }
}