namespace Photography_Tools.Models;

public class NDFilter
{
    public required string Name { get; set; }
    public int FStopReduction { get; set; }
    public double OpticalDensity { get; set; }
    public uint Factor { get; set; }
}