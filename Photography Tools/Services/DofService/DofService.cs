using Photography_Tools.Const;
using Photography_Tools.Models;

namespace Photography_Tools.Services.DofService;

public class DofService : IDofService
{
    private static double SqrtOf2 { get; } = Math.Sqrt(2);

    public DofCalcResult CalculateDofValues(DofCalcInput dofInfo)
    {
        double focalRatio = CalculateFullApertureValue(dofInfo.LensInfo.Aperture);

        double diagonalPrintMM = Math.Sqrt(Math.Pow(dofInfo.PrintWidthMM, 2) + Math.Pow(dofInfo.PrintHeighthMM, 2));
        double diagonalSensorMM = Math.Sqrt(Math.Pow(dofInfo.CameraInfo.SensorWidthMM, 2) + Math.Pow(dofInfo.CameraInfo.SensorHeightMM, 2));
        double enlargmentFactor = diagonalPrintMM / diagonalSensorMM;

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