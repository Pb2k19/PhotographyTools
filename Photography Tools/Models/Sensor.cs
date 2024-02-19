using Photography_Tools.Const;

namespace Photography_Tools.Models;

public class Sensor
{
    public double SensorWidthMM { get; private set; }
    public double SensorHeigthMM { get; private set; }
    public double Megapixels { get; private set; }
    public double Ratio { get; private set; }
    public int ResolutionHorizontal { get; private set; }
    public double Diagonal { get; private set; }
    public int ResolutionVertical { get; private set; }
    public double CropFactor { get; private set; }
    public double PixelPitch { get; private set; }

    public Sensor(double sensorHeigthMM, double sensorWidthMM, double megapixels)
    {
        SensorWidthMM = sensorWidthMM;
        SensorHeigthMM = sensorHeigthMM;
        Megapixels = megapixels;

        UpdateAll();
    }

    public void SetSensorWidthMM(double value)
    {
        if (SensorWidthMM != value)
        {
            SensorWidthMM = value;
            UpdateAll();
        }
    }

    public void SetSensorHeigthMM(double value)
    {
        if (SensorHeigthMM != value)
        {
            SensorHeigthMM = value;
            UpdateAll();
        }
    }

    public void SetMegapixels(double value)
    {
        if (Megapixels != value)
        {
            Megapixels = value;
            UpdateResolution();
            UpdatePixelPitch();
        }
    }

    private void UpdateAll()
    {
        UpdateRatio();
        UpdateDiagonal();
        UpdateCropFactor();
        UpdateResolution();
        UpdatePixelPitch();
    }

    private void UpdateRatio() => Ratio = SensorCalculations.CalculateRatio(SensorWidthMM, SensorHeigthMM);

    private void UpdateDiagonal() => Diagonal = SensorCalculations.CalculateDiagonal(SensorWidthMM, SensorHeigthMM);

    private void UpdateCropFactor() => CropFactor = SensorCalculations.CalculateCropFactor(Diagonal);

    private void UpdateResolution()
    {
        (int horizontal, int vertical) = SensorCalculations.CalculateSensorResolution(Ratio, Megapixels);
        ResolutionHorizontal = horizontal;
        ResolutionVertical = vertical;
    }

    private void UpdatePixelPitch() => PixelPitch = SensorCalculations.CalculatePixelPitch(SensorWidthMM, ResolutionHorizontal);
}

public static class SensorCalculations
{
    public static double CalculateCropFactor(double diagonal) => SensorConst.FullFrameDiagonal / diagonal;

    public static double CalculatePixelPitch(double sensorWidthMM, double resolutionHorizontal) => sensorWidthMM / resolutionHorizontal * 1000;

    public static double CalculateRatio(double sensorWidthMM, double sensorHeightMM) => sensorWidthMM / sensorHeightMM;

    public static double CalculateDiagonal(double sensorWidth, double sensorHeigth) => Math.Sqrt(sensorWidth * sensorWidth + sensorHeigth * sensorHeigth);

    /// <summary>
    /// Calculates sensor resolution
    /// </summary>
    /// <param name="ratio">Sensor width / Sensor height</param>
    /// <param name="megapixels">Effective megapixels</param>
    /// <returns>Sensor resolution</returns>
    public static (int horizontal, int vertical) CalculateSensorResolution(double ratio, double megapixels)
    {
        double vertical = Math.Round(Math.Sqrt(megapixels * 1000000 / ratio));
        return ((int)Math.Round(vertical * ratio, 0), (int)Math.Round(vertical, 0));
    }
}