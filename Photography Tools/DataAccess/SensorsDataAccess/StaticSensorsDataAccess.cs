using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Photography_Tools.DataAccess.SensorsDataAccess;

public class StaticSensorsDataAccess : ISensorsDataAccess
{
    private readonly static FrozenDictionary<string, Sensor> sensors;

    static StaticSensorsDataAccess()
    {
        sensors = new Dictionary<string, Sensor>()
        {
            { "Full Frame", new Sensor(24, 36, 24) },
            { "Nikon CX", new Sensor(8.8, 13.2, 20) },
            { "Nikon DX", new Sensor(15.7, 23.5, 24) },
            { "Nikon FX", new Sensor(23.9, 35.9, 24) },
        }.ToFrozenDictionary();
    }

    public ImmutableArray<string> GetSensorNames() => sensors.Keys;

    public ImmutableArray<Sensor> GetSensors() => sensors.Values;

    public Sensor GetSensor(string sensorName) => sensors[sensorName];
}
