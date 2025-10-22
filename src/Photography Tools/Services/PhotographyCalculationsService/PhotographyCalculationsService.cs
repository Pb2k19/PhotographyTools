namespace Photography_Tools.Services.PhotographyCalculationsService;

public class PhotographyCalculationsService : IPhotographyCalculationsService
{
    private static double SqrtOf2 { get; } = Math.Sqrt(2);

    // Source: Depth of Field in Depth, Jeff Conrad, Large Format Page, https://www.largeformatphotography.info/articles/DoFinDepth.pdf access date: 19.02.2024
    public DofCalcResult CalculateDofValues(DofCalcInput dofInfo)
    {
        double focalRatio = CalculateFullApertureValue(dofInfo.LensInfo.Aperture);

        double diagonalPrintMM = Math.Sqrt(dofInfo.PrintWidthMM * dofInfo.PrintWidthMM + dofInfo.PrintHeighthMM * dofInfo.PrintHeighthMM);
        double enlargmentFactor = diagonalPrintMM / dofInfo.CameraInfo.Diagonal;

        double circleOfConfusion = dofInfo.ActualViewingDistanceMM / (double)(dofInfo.StandardViewingDistanceMM * dofInfo.VisualAcuityLpPerMM) / enlargmentFactor;
        if (circleOfConfusion < 0.005)
            circleOfConfusion = 0.005;
        else if (circleOfConfusion > 0.3)
            circleOfConfusion = 0.3;

        DofCalcResult result = new()
        {
            CircleOfConfusion = circleOfConfusion,
            HyperfocalDistanceMM = dofInfo.LensInfo.FocalLengthMM + dofInfo.LensInfo.FocalLengthMM * dofInfo.LensInfo.FocalLengthMM / (focalRatio * circleOfConfusion)
        };

        if (dofInfo.FocusingDistanceMM < dofInfo.LensInfo.FocalLengthMM)
        {
            result.DofFarLimitMM = result.DofNearLimitMM = result.DofInFrontOfSubject = result.DofInBackOfSubject = double.NaN;
            result.DofMM = 0;

            return result;
        }

        double farLimit = result.HyperfocalDistanceMM * dofInfo.FocusingDistanceMM / (result.HyperfocalDistanceMM - (dofInfo.FocusingDistanceMM - dofInfo.LensInfo.FocalLengthMM));
        result.DofFarLimitMM = farLimit >= 0 ? farLimit : double.PositiveInfinity;
        result.DofNearLimitMM = result.HyperfocalDistanceMM * dofInfo.FocusingDistanceMM / (result.HyperfocalDistanceMM + (dofInfo.FocusingDistanceMM - dofInfo.LensInfo.FocalLengthMM));

        result.DofMM = result.DofFarLimitMM - result.DofNearLimitMM;
        result.DofInFrontOfSubject = dofInfo.FocusingDistanceMM - result.DofNearLimitMM;
        result.DofInBackOfSubject = result.DofFarLimitMM - dofInfo.FocusingDistanceMM;

        return result;
    }

    // Source: Frederic Michaud from Le Havre Astronomical Society, https://sahavre.fr/wp/regle-npf-rule/ access date: 19.02.2024
    public double CalculateTimeForAstroWithNPFRule(Sensor sensor, Lens lens, int accuracy, double declinationDegrees = 0)
    {
        return accuracy * (16.856 * CalculateFullApertureValue(lens.Aperture) + 0.0997 *
            lens.FocalLengthMM + 13.713 * sensor.PixelPitch) / (lens.FocalLengthMM *
            Math.Cos(MathHelper.DegreesToRadians(declinationDegrees)));
    }

    public (double rule200, double rule300, double rule500) CalculateTimeForAstro(double sensorCrop, double focalLength)
    {
        double x = sensorCrop * focalLength;
        return (200 / x, 300 / x, 500 / x);
    }

    public TimeSpan CalculateTimeWithNDFilters(TimeSpan baseTime, IEnumerable<NDFilter> filters)
    {
        if (baseTime == TimeSpan.Zero)
            return baseTime;

        ulong multiplier = 1;
        checked
        {
            foreach (NDFilter filter in filters)
            {
                if (filter.Factor > 0)
                    multiplier *= filter.Factor;
            }
        }

        return baseTime * multiplier;
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
        else if (ApertureConst.OneHalfStops.Contains(apertureValue))
            return ApertureConst.SecondStopsMultiplier;
        else if (ApertureConst.OneThirdStops.Contains(apertureValue))
            return ApertureConst.ThirdStopsMultiplier;
        else
            return ApertureConst.Undefined;
    }
}