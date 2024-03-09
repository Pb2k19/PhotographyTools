namespace Photography_Tools.Helpers;

public static class UnitHelper
{
    public static double ConvertUnitsToMM(double inputValue, string inputUnit)
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

    public static double ConvertMMToUnits(double inputValue, string outputUnit)
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