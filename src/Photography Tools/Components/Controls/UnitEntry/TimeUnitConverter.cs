using System.Collections.Immutable;

namespace Photography_Tools.Components.Controls.UnitEntry;

public class TimeUnitConverter : IUnitConverter
{
    public string BaseUnitName => UnitConst.TimeS;

    public ImmutableArray<string> Units => UnitConst.TimeUnits;

    public double ConvertBaseToSelectedUnit(double inputValue, string outputUnit)
    {
        return outputUnit switch
        {
            UnitConst.TimeMs => inputValue * 1000,
            UnitConst.TimeS => inputValue,
            UnitConst.TimeMin => inputValue / 60,
            UnitConst.TimeH => inputValue / 3600,
            _ => throw new ArgumentException("Unsupported unit", nameof(outputUnit))
        };
    }

    public double ConvertToBaseUnit(double inputValue, string inputUnit)
    {
        return inputUnit switch
        {
            UnitConst.TimeMs => inputValue / 1000,
            UnitConst.TimeS => inputValue,
            UnitConst.TimeMin => inputValue * 60,
            UnitConst.TimeH => inputValue * 3600,
            _ => throw new ArgumentException("Unsupported unit", nameof(inputUnit))
        };
    }
}
