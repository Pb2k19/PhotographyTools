using System.Collections.Immutable;

namespace Photography_Tools.Components.Controls.UnitEntry;

public class LengthUnitConverter : IUnitConverter
{
    public string BaseUnitName => LengthUnitsConst.LengthMM;

    public ImmutableArray<string> Units => LengthUnitsConst.LengthUnits;

    public double ConvertToBaseUnit(double inputValue, string inputUnit)
    {
        if (inputValue == 0)
            return 0;

        return inputUnit switch
        {
            LengthUnitsConst.LengthMM => inputValue,
            LengthUnitsConst.LengthCM => inputValue * 10,
            LengthUnitsConst.LengthM => inputValue * 1000,
            LengthUnitsConst.LengthIN => inputValue * 25.4,
            LengthUnitsConst.LengthFT => UnitConverters.InternationalFeetToMeters(inputValue) * 1000,
            _ => throw new ArgumentException("Unsupported unit", nameof(inputUnit)),
        };
    }

    public double ConvertBaseToSelectedUnit(double inputValue, string outputUnit)
    {
        if (inputValue == 0)
            return 0;

        return outputUnit switch
        {
            LengthUnitsConst.LengthMM => inputValue,
            LengthUnitsConst.LengthCM => inputValue / 10,
            LengthUnitsConst.LengthM => inputValue / 1000,
            LengthUnitsConst.LengthIN => inputValue / 25.4,
            LengthUnitsConst.LengthFT => UnitConverters.MetersToInternationalFeet(inputValue / 1000),
            _ => throw new ArgumentException("Unsupported unit", nameof(outputUnit)),
        };
    }
}