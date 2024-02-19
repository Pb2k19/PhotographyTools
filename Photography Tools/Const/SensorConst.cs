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
            { "Full Frame", new Sensor(24, 36, 24) },
            { "Nikon CX", new Sensor(8.8, 13.2, 20) },
            { "Nikon DX", new Sensor(15.7, 23.5, 24) },
            { "Nikon FX", new Sensor(23.9, 35.9, 24) },
        }.ToFrozenDictionary();
    }
}
