namespace Photography_Tools.Models;

public readonly record struct GeocentricCoordinates(double RightAscension, double Declination, double Distance);

public readonly record struct EquatorialCoordinates(double RightAscension, double Declination);