using Photography_Tools.Models;
using System.Collections.Frozen;

namespace Photography_Tools.Const;
public static class SensorConst
{
    public static FrozenDictionary<string, Sensor> Sensors { get; }

    static SensorConst()
    {
        Sensors = new Dictionary<string, Sensor>()
        {
            { "Full Frame", new Sensor { SensorHeightMM = 36, SensorWidthMM = 24 } },
            { "Nikon CX", new Sensor { SensorHeightMM = 8.8, SensorWidthMM = 13.2 } },
            { "Nikon DX", new Sensor { SensorHeightMM = 15.7, SensorWidthMM = 23.5 } },
            { "Nikon FX", new Sensor { SensorHeightMM = 35.9, SensorWidthMM = 23.9 } },
        }.ToFrozenDictionary();
    }
}
