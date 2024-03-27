using System.Collections.Immutable;

namespace Photography_Tools.Components.Controls.UnitEntry;

public interface IUnitConverter
{
    string BaseUnitName { get; }
    ImmutableArray<string> Units { get; }
    double ConvertToBaseUnit(double inputValue, string inputUnit);
    double ConvertBaseToSelectedUnit(double inputValue, string outputUnit);
}