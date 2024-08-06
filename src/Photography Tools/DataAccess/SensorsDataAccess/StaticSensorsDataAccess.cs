using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Photography_Tools.DataAccess.SensorsDataAccess;

public class StaticSensorsDataAccess : ISensorsDataAccess
{
    private readonly static FrozenDictionary<string, Sensor> sensors;
    private readonly static ImmutableArray<string> keys;

    static StaticSensorsDataAccess()
    {
        sensors = new Dictionary<string, Sensor>()
        {
            { "Full Frame 24 Mpix", new Sensor(24, 36, 24) },
            { "Nikon D1, D1H", new Sensor(15.5, 23.7, 2.6) },
            { "Nikon D1x", new Sensor(15.5, 23.7, 5.3) },
            { "Nikon D2H", new Sensor(15.5, 23.7, 4) },
            { "Nikon D2Hs", new Sensor(15.4, 23.2, 4.1) },
            { "Nikon D2x", new Sensor(15.7, 23.7, 12.2) },
            { "Nikon D2xs", new Sensor(15.7, 23.7, 12.4) },
            { "Nikon D3", new Sensor(23.9, 36, 12.1) },
            { "Nikon D4, D4s", new Sensor(23.9, 36, 16.2) },
            { "Nikon D5", new Sensor(23.9, 35.8, 20.8) },
            { "Nikon D6", new Sensor(23.9, 35.9, 20.8) },
            { "Nikon D600, D610", new Sensor(24, 35.9, 24.3) },
            { "Nikon D700", new Sensor(23.9, 36, 12.1) },
            { "Nikon D750", new Sensor(24, 35.9, 24.5) },
            { "Nikon D780", new Sensor(23.9, 35.9, 24.5) },
            { "Nikon D800, D800E, D810", new Sensor(24, 35.9, 36.3) },
            { "Nikon D850", new Sensor(23.9, 35.9, 45.7) },
            { "Nikon Z30, Z50", new Sensor(15.7, 23.5, 20.90) },
            { "Nikon Z5, Z6, Z6 II, Z6 III", new Sensor(23.9, 35.9, 24.5) },
            { "Nikon Z7, Z7 II, Z8, Z9", new Sensor(23.9, 35.9, 45.7 ) },
            { "Nikon CX", new Sensor(8.8, 13.2, 20) },
            { "Nikon DX", new Sensor(15.7, 23.5, 24) },
        }.ToFrozenDictionary();

        keys = [.. sensors.Keys.Order()];
    }

    public ImmutableArray<string> GetSensorNames() => keys;

    public ImmutableArray<Sensor> GetSensors() => sensors.Values;

    public Sensor? GetSensor(string sensorName)
    {
        if (sensorName is null)
            return null;

        return sensors.TryGetValue(sensorName, out Sensor? sensor) ? sensor : null;
    }
}
