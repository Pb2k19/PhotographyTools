using Photography_Tools.Models;
using System.Collections.Frozen;

namespace Photography_Tools.Const;
public static class SensorConst
{
    public const double FullFrameDiagonal = 43.2666153055679;

    public static FrozenDictionary<string, Sensor> Sensors { get; }

    static SensorConst()
    {
        Sensors = new Dictionary<string, Sensor>()
        {
            { "Full Frame", new Sensor(36, 24, 24.3) },
            { "Nikon CX", new Sensor(8.8, 13.2, 24.3) },
            { "Nikon DX", new Sensor(23.5, 15.7, 24.3) },
            { "Nikon FX", new Sensor(35.9, 23.9, 24.3) },
        }.ToFrozenDictionary();
    }
}
