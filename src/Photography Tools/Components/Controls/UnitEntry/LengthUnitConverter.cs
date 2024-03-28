using System.Collections.Immutable;

namespace Photography_Tools.Components.Controls.UnitEntry;

public class LengthUnitConverter : IUnitConverter
{
    public string BaseUnitName => LengthUnitsConst.Millimeter;

    public ImmutableArray<string> Units => LengthUnitsConst.Units;

    public double ConvertToBaseUnit(double inputValue, string inputUnit)
    {
        if (inputValue == 0)
            return 0;

        return inputUnit switch
        {
            LengthUnitsConst.Millimeter => inputValue,
            LengthUnitsConst.Centimeter => inputValue * 10,
            LengthUnitsConst.Meter => inputValue * 1000,
            LengthUnitsConst.Inch => inputValue * 25.4,
            LengthUnitsConst.Foot => UnitConverters.InternationalFeetToMeters(inputValue) * 1000,
            _ => throw new ArgumentException("Unsupported unit", nameof(inputUnit)),
        };
    }

    public double ConvertBaseToSelectedUnit(double inputValue, string outputUnit)
    {
        if (inputValue == 0)
            return 0;

        return outputUnit switch
        {
            LengthUnitsConst.Millimeter => inputValue,
            LengthUnitsConst.Centimeter => inputValue / 10,
            LengthUnitsConst.Meter => inputValue / 1000,
            LengthUnitsConst.Inch => inputValue / 25.4,
            LengthUnitsConst.Foot => UnitConverters.MetersToInternationalFeet(inputValue / 1000),
            _ => throw new ArgumentException("Unsupported unit", nameof(outputUnit)),
        };
    }
}