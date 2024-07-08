namespace Photography_Tools.Models;

public readonly record struct EquatorialCoordinates(double RightAscension, double Declination);

public readonly record struct GeocentricCoordinates(double RightAscension, double Declination, double Distance);

public record struct GeographicalCoordinates(double Latitude, double Longitude);