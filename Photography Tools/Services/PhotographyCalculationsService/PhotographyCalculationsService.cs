using Photography_Tools.Const;
using Photography_Tools.Models;

namespace Photography_Tools.Services.PhotographyCalculationsService;

public class PhotographyCalculationsService : IPhotographyCalculationsService
{
    private static double SqrtOf2 { get; } = Math.Sqrt(2);

    public DofCalcResult CalculateDofValues(DofCalcInput dofInfo)
    {
        double focalRatio = CalculateFullApertureValue(dofInfo.LensInfo.Aperture);

        double diagonalPrintMM = Math.Sqrt(dofInfo.PrintWidthMM * dofInfo.PrintWidthMM + dofInfo.PrintHeighthMM * dofInfo.PrintHeighthMM);
        double enlargmentFactor = diagonalPrintMM / dofInfo.CameraInfo.Diagonal;

        DofCalcResult result = new()
        {
            CircleOfConfusion = dofInfo.ActualViewingDistanceMM / (double)(dofInfo.StandardViewingDistanceMM * dofInfo.VisualAcuityLpPerMM) / enlargmentFactor
        };
        result.HyperfocalDistanceMM = dofInfo.LensInfo.FocalLengthMM + Math.Pow(dofInfo.LensInfo.FocalLengthMM, 2) / (focalRatio * result.CircleOfConfusion);
        result.DofFarLimitMM = result.HyperfocalDistanceMM * dofInfo.FocusingDistanceMM / (result.HyperfocalDistanceMM - (dofInfo.FocusingDistanceMM - dofInfo.LensInfo.FocalLengthMM));
        result.DofNearLimitMM = result.HyperfocalDistanceMM * dofInfo.FocusingDistanceMM / (result.HyperfocalDistanceMM + (dofInfo.FocusingDistanceMM - dofInfo.LensInfo.FocalLengthMM));

        result.DofMM = result.DofFarLimitMM - result.DofNearLimitMM;
        result.DofInFrontOfSubject = dofInfo.FocusingDistanceMM - result.DofNearLimitMM;
        result.DofInBackOfSubject = result.DofFarLimitMM - dofInfo.FocusingDistanceMM;

        return result;
    }

    public double CalculateTimeForAstroWithNPFRule(Sensor sensor, Lens lens, int accuracy, double declinationDegrees = 0)
    {
        return accuracy * (16.856 * CalculateFullApertureValue(lens.Aperture) + 0.1 * lens.FocalLengthMM + 13.713 * sensor.PixelPitch) / (lens.FocalLengthMM * Math.Cos(DegreesToRadians(declinationDegrees)));
    }

    public (double rule200, double rule300, double rule500) CalculateTimeForAstro(double sensorCrop, double focalLength)
    {
        double x = sensorCrop * focalLength;
        return (200 / x, 300 / x, 500 / x);
    }

    public static double CalculateFullApertureValue(double apertureValue)
    {
        int apertureMultiplier = GetApertureMultiplier(apertureValue);
        int x = FindXForFocalRatio(apertureValue, apertureMultiplier);

        return Math.Round(apertureMultiplier switch
        {
            ApertureConst.FullStopsMultiplier => Math.Pow(SqrtOf2, x),
            ApertureConst.SecondStopsMultiplier => Math.Pow(SqrtOf2, x / 2.0),
            ApertureConst.ThirdStopsMultiplier => Math.Pow(SqrtOf2, x / 3.0),
            _ => apertureValue
        }, 6);
    }

    public static int FindXForFocalRatio(double apertureValue, int apertureMultiplier)
    {
        double x = apertureMultiplier * Math.Log(apertureValue, 2);
        return (int)Math.Round(x, 0);
    }

    public static int GetApertureMultiplier(double apertureValue)
    {
        if (ApertureConst.FullStops.Contains(apertureValue))
            return ApertureConst.FullStopsMultiplier;
        else if (ApertureConst.SecondStops.Contains(apertureValue))
            return ApertureConst.SecondStopsMultiplier;
        else if (ApertureConst.ThirdStops.Contains(apertureValue))
            return ApertureConst.ThirdStopsMultiplier;
        else
            return ApertureConst.Undefined;
    }

    public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
}