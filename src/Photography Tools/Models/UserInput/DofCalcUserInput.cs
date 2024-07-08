namespace Photography_Tools.Models.UserInput;

public class DofCalcUserInput : UserInput
{
    public required DofCalcInput DofCalcInput { get; set; }
    public required string SelectedSensorName { get; set; }
    public int VisualAcuityLpPerMM { get; set; } = 5;
    public bool IsAdvancedModeEnabled { get; set; } = false;
    public int FocalLengthUnitIndex { get; set; } = 0;
    public int FocusingDistanceUnitIndex { get; set; } = 2;
    public int PrintHeighthUnitIndex { get; set; } = 1;
    public int PrintWidthUnitIndex { get; set; } = 1;
    public int StandardViewingDistanceUnitIndex { get; set; } = 1;
    public int ActualViewingDistanceUnitIndex { get; set; } = 1;
    public int DofUnitIndex { get; set; } = 2;
    public int HyperfocalDistanceUnitIndex { get; set; } = 2;
    public int DofNearLimitUnitIndex { get; set; } = 2;
    public int DofFarLimitUnitIndex { get; set; } = 2;
    public int DofInFrontOfSubjectUnitIndex { get; set; } = 1;
    public int DofInBackOfSubjectUnitIndex { get; set; } = 1;

    public override bool Validate()
    {
        return !string.IsNullOrWhiteSpace(SelectedSensorName) && DofCalcInput is not null && DofCalcInput.Validate();
    }
}