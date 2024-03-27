using System.Collections.Immutable;

namespace Photography_Tools.Components.Controls.UnitEntry;

public class TimeUnitConverter : IUnitConverter
{
    public string BaseUnitName => TimeUnitsConst.TimeS;

    public ImmutableArray<string> Units => TimeUnitsConst.TimeUnits;

    public double ConvertBaseToSelectedUnit(double inputValue, string outputUnit)
    {
        return outputUnit switch
        {
            TimeUnitsConst.TimeMs => inputValue * 1000,
            TimeUnitsConst.TimeS => inputValue,
            TimeUnitsConst.TimeMin => inputValue / 60,
            TimeUnitsConst.TimeH => inputValue / 3600,
            _ => throw new ArgumentException("Unsupported unit", nameof(outputUnit))
        };
    }

    public double ConvertToBaseUnit(double inputValue, string inputUnit)
    {
        return inputUnit switch
        {
            TimeUnitsConst.TimeMs => inputValue / 1000,
            TimeUnitsConst.TimeS => inputValue,
            TimeUnitsConst.TimeMin => inputValue * 60,
            TimeUnitsConst.TimeH => inputValue * 3600,
            _ => throw new ArgumentException("Unsupported unit", nameof(inputUnit))
        };
    }
}
