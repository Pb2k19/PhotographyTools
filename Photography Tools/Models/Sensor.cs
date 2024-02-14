using Photography_Tools.Const;

namespace Photography_Tools.Models;

public class Sensor
{
    public double SensorWidthMM { get; }
    public double SensorHeightMM { get; }
    public double Diagonal { get; }
    public double CropFactor { get; }
    public double Megapixels { get; }
    public double Ratio { get; }
    public double PixelPitch { get; }
    public int ResolutionHorizontal { get; }
    public int ResolutionVertical { get; }


    public Sensor(double sensorWidthMM, double sensorHeightMM, double megapixels)
    {
        SensorWidthMM = sensorWidthMM;
        SensorHeightMM = sensorHeightMM;
        Megapixels = megapixels;

        Ratio = sensorWidthMM / sensorHeightMM;
        Diagonal = CalculateDiagonal(sensorHeightMM, sensorWidthMM);
        CropFactor = SensorConst.FullFrameDiagonal / Diagonal;

        (int horizontal, int vertical) = CalculateSensorResolution(Ratio, megapixels);
        ResolutionHorizontal = horizontal;
        ResolutionVertical = vertical;

        PixelPitch = sensorWidthMM / ResolutionHorizontal * 1000;
    }

    public static double CalculateDiagonal(double sensorHeigth, double sensorWidth)
    {
        return Math.Sqrt(sensorWidth * sensorWidth + sensorHeigth * sensorHeigth);
    }

    /// <summary>
    /// Calculates sensor resolution
    /// </summary>
    /// <param name="ratio">Sensor width / Sensor height</param>
    /// <param name="megapixles">Effective megapixels</param>
    /// <returns>Sensor resolution</returns>
    public static (int horizontal, int vertical) CalculateSensorResolution(double ratio, double megapixles)
    {
        double vertical = Math.Round(Math.Sqrt(megapixles * 1000000 / ratio));
        return ((int)Math.Round(vertical * ratio, 0), (int)Math.Round(vertical, 0)); 
    }
}