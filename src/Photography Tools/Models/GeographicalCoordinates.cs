using Photography_Tools.Models.Interfaces;

namespace Photography_Tools.Models;

public record struct GeographicalCoordinates(double Latitude, double Longitude) : IValidatable
{
    public readonly bool Validate() => Latitude >= -90.0 && Latitude <= 90.0 && Longitude >= -180.0 && Longitude <= 180.0;
}