﻿using System.Collections.Frozen;
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
            { "Full Frame 24 Mpix", new Sensor(24, 36, 24.0) },
            { "Nikon 1 AW1, J3, S2, V2", new Sensor(8.8, 13.2, 14.2) },
            { "Nikon 1 J1, J2, S1, V1", new Sensor(8.8, 13.2, 10.1) },
            { "Nikon 1 J4, V3", new Sensor(8.8, 13.2, 18.4) },
            { "Nikon D1, D1H", new Sensor(15.5, 23.7, 2.6) },
            { "Nikon D100", new Sensor(15.5, 23.7, 6.0) },
            { "Nikon D1x", new Sensor(15.5, 23.7, 5.3) },
            { "Nikon D200", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D2H", new Sensor(15.5, 23.7, 4.0) },
            { "Nikon D2Hs", new Sensor(15.4, 23.2, 4.1) },
            { "Nikon D2x", new Sensor(15.7, 23.7, 12.2) },
            { "Nikon D2xs", new Sensor(15.7, 23.7, 12.4) },
            { "Nikon D3, D3s", new Sensor(23.9, 36, 12.1) },
            { "Nikon D300, D300s", new Sensor(15.8, 23.6, 12.3) },
            { "Nikon D3000", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D3100", new Sensor(15.4, 23.1, 14.2) },
            { "Nikon D3200", new Sensor(15.4, 23.2, 24.2) },
            { "Nikon D3300, D3400, D3500", new Sensor(15.6, 23.5, 24.2) },
            { "Nikon D3X", new Sensor(24, 35.9, 24.5) },
            { "Nikon D4, D4s", new Sensor(23.9, 36, 16.2) },
            { "Nikon D40", new Sensor(15.6, 23.7, 6.1) },
            { "Nikon D40x", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D5", new Sensor(23.9, 35.8, 20.8) },
            { "Nikon D50", new Sensor(15.6, 23.7, 6.1) },
            { "Nikon D5000", new Sensor(15.8, 23.6, 12.3) },
            { "Nikon D5100", new Sensor(15.6, 23.6, 16.2) },
            { "Nikon D5200", new Sensor(15.6, 23.5, 24.1) },
            { "Nikon D5300, D5500, D5600", new Sensor(15.6, 23.5, 24.2) },
            { "Nikon D6", new Sensor(23.9, 35.9, 20.8) },
            { "Nikon D60", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D600, D610", new Sensor(24, 35.9, 24.3) },
            { "Nikon D70, D70s", new Sensor(15.6, 23.7, 6.1) },
            { "Nikon D700", new Sensor(23.9, 36, 12.1) },
            { "Nikon D7000", new Sensor(15.6, 23.6, 16.2) },
            { "Nikon D7100", new Sensor(15.6, 23.5, 24.1) },
            { "Nikon D7200", new Sensor(15.6, 23.5, 24.2) },
            { "Nikon D750", new Sensor(24, 35.9, 24.3) },
            { "Nikon D7500", new Sensor(15.7, 23.5, 20.9) },
            { "Nikon D780", new Sensor(23.9, 35.9, 24.5) },
            { "Nikon D80", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D800, D800E, D810", new Sensor(24, 35.9, 36.3) },
            { "Nikon D850", new Sensor(23.9, 35.9, 45.7) },
            { "Nikon D90", new Sensor(15.8, 23.6, 12.3) },
            { "Nikon Z30, Z50, Z fc", new Sensor(15.7, 23.5, 20.90) },
            { "Nikon Z5", new Sensor(23.9, 35.9, 24.3) },
            { "Nikon Z6, Z6 II, Z6 III, Z f", new Sensor(23.9, 35.9, 24.5) },
            { "Nikon Z7, Z7 II, Z8, Z9", new Sensor(23.9, 35.9, 45.7 ) },

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
