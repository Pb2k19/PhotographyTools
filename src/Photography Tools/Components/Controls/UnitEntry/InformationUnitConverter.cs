using System.Collections.Immutable;

namespace Photography_Tools.Components.Controls.UnitEntry;

public class InformationUnitConverter : IUnitConverter
{
    public string BaseUnitName => InformationUnitsConst.Megabyte;

    public ImmutableArray<string> Units => InformationUnitsConst.Units;

    public double ConvertBaseToSelectedUnit(double inputValue, string outputUnit)
    {
        if (inputValue == 0)
            return 0;

        checked
        {
            return outputUnit switch
            {
                InformationUnitsConst.Byte => inputValue * 1_000_000,
                InformationUnitsConst.Kilobyte => inputValue * 1_000,
                InformationUnitsConst.Megabyte => inputValue,
                InformationUnitsConst.Gigabyte => inputValue / 1_000,
                InformationUnitsConst.Terabyte => inputValue / 1_000_000,
                _ => throw new ArgumentException("Unsupported unit", nameof(outputUnit))
            };
        }
    }

    public double ConvertToBaseUnit(double inputValue, string inputUnit)
    {
        if (inputValue == 0)
            return 0;

        checked
        {
            return inputUnit switch
            {
                InformationUnitsConst.Byte => inputValue / 1_000_000,
                InformationUnitsConst.Kilobyte => inputValue / 1_000,
                InformationUnitsConst.Megabyte => inputValue,
                InformationUnitsConst.Gigabyte => inputValue * 1_000,
                InformationUnitsConst.Terabyte => inputValue * 1_000_000,
                _ => throw new ArgumentException("Unsupported unit", nameof(inputUnit))
            };
        }
    }
}