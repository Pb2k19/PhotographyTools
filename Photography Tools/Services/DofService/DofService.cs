using Photography_Tools.Const;
using Photography_Tools.Models;

namespace Photography_Tools.Services.DofService;

public class DofService : IDofService
{
    private static double SqrtOf2 { get; } = Math.Sqrt(2);

    public void CalculateDofValues(DofInfo dofInfo)
    {
        double focalRatio = CalculateFullApertureValue(dofInfo.LensInfo.Aperture);

        double diagonalPrintMM = Math.Sqrt(Math.Pow(dofInfo.PrintWidthMM, 2) + Math.Pow(dofInfo.PrintHeighthMM, 2));
        double diagonalSensorMM = Math.Sqrt(Math.Pow(dofInfo.CameraInfo.SensorWidthMM, 2) + Math.Pow(dofInfo.CameraInfo.SensorHeightMM, 2));
        double enlargmentFactor = diagonalPrintMM / diagonalSensorMM;

        dofInfo.CircleOfConfusion = dofInfo.ActualViewingDistanceMM / (double)(dofInfo.StandardViewingDistanceMM * dofInfo.VisualAcuityLpPerMM) / enlargmentFactor;
        dofInfo.HyperfocalDistanceMM = dofInfo.LensInfo.FocalLengthMM + Math.Pow(dofInfo.LensInfo.FocalLengthMM, 2) / (focalRatio * dofInfo.CircleOfConfusion);
        dofInfo.DofFarLimitMM = dofInfo.HyperfocalDistanceMM * dofInfo.FocusingDistanceMM / (dofInfo.HyperfocalDistanceMM - (dofInfo.FocusingDistanceMM - dofInfo.LensInfo.FocalLengthMM));
        dofInfo.DofNearLimitMM = dofInfo.HyperfocalDistanceMM * dofInfo.FocusingDistanceMM / (dofInfo.HyperfocalDistanceMM + (dofInfo.FocusingDistanceMM - dofInfo.LensInfo.FocalLengthMM));

        dofInfo.DofMM = dofInfo.DofFarLimitMM - dofInfo.DofNearLimitMM;
        dofInfo.DofInFrontOfSubject = dofInfo.FocusingDistanceMM - dofInfo.DofNearLimitMM;
        dofInfo.DofInBackOfSubject = dofInfo.DofFarLimitMM - dofInfo.FocusingDistanceMM;
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
}