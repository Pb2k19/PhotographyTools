using System.Collections.Immutable;

namespace Photography_Tools.DataAccess.SensorsDataAccess;

public interface ISensorsDataAccess
{
    Sensor? GetSensor(string sensorName);
    ImmutableArray<string> GetSensorNames();
    ImmutableArray<Sensor> GetSensors();
}