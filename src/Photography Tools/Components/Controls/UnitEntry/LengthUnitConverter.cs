using System.Collections.Immutable;

namespace Photography_Tools.Components.Controls.UnitEntry;

public class LengthUnitConverter : IUnitConverter
{
    public string BaseUnitName => UnitConst.LengthMM;

    public ImmutableArray<string> Units => UnitConst.LengthUnits;

    public double ConvertToBaseUnit(double inputValue, string inputUnit)
    {
        if (inputValue == 0)
            return 0;

        return inputUnit switch
        {
            UnitConst.LengthMM => inputValue,
            UnitConst.LengthCM => inputValue * 10,
            UnitConst.LengthM => inputValue * 1000,
            UnitConst.LengthIN => inputValue * 25.4,
            UnitConst.LengthFT => UnitConverters.InternationalFeetToMeters(inputValue) * 1000,
            _ => throw new ArgumentException("Unsupported unit", nameof(inputUnit)),
        };
    }

    public double ConvertBaseToSelectedUnit(double inputValue, string outputUnit)
    {
        if (inputValue == 0)
            return 0;

        return outputUnit switch
        {
            UnitConst.LengthMM => inputValue,
            UnitConst.LengthCM => inputValue / 10,
            UnitConst.LengthM => inputValue / 1000,
            UnitConst.LengthIN => inputValue / 25.4,
            UnitConst.LengthFT => UnitConverters.MetersToInternationalFeet(inputValue / 1000),
            _ => throw new ArgumentException("Unsupported unit", nameof(outputUnit)),
        };
    }
}