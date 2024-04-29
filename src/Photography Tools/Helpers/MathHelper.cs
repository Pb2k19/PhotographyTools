namespace Photography_Tools.Helpers;

public static class MathHelper
{
    public const double ToRadianMultiplier = Math.PI / 180.0;

    public static double DegreesToRadians(double degrees) => degrees * ToRadianMultiplier;
}