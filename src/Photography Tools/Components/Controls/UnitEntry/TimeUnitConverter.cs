using System.Collections.Immutable;

namespace Photography_Tools.Components.Controls.UnitEntry;

public class TimeUnitConverter : IUnitConverter
{
    public string BaseUnitName => TimeUnitsConst.Second;

    public ImmutableArray<string> Units => TimeUnitsConst.Units;

    public double ConvertBaseToSelectedUnit(double inputValue, string outputUnit)
    {
        return outputUnit switch
        {
            TimeUnitsConst.Millisecond => inputValue * 1000,
            TimeUnitsConst.Second => inputValue,
            TimeUnitsConst.Minute => inputValue / 60,
            TimeUnitsConst.Hour => inputValue / 3600,
            _ => throw new ArgumentException("Unsupported unit", nameof(outputUnit))
        };
    }

    public double ConvertToBaseUnit(double inputValue, string inputUnit)
    {
        return inputUnit switch
        {
            TimeUnitsConst.Millisecond => inputValue / 1000,
            TimeUnitsConst.Second => inputValue,
            TimeUnitsConst.Minute => inputValue * 60,
            TimeUnitsConst.Hour => inputValue * 3600,
            _ => throw new ArgumentException("Unsupported unit", nameof(inputUnit))
        };
    }
}
