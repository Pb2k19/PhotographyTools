namespace Photography_Tools.Models;

public class DofCalcResult
{
    public double CircleOfConfusion { get; set; }
    public double HyperfocalDistanceMM { get; set; }
    public double DofFarLimitMM { get; set; }
    public double DofNearLimitMM { get; set; }
    public double DofMM { get; set; }
    public double DofInFrontOfSubject { get; set; }
    public double DofInBackOfSubject { get; set; }
}