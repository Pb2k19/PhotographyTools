namespace Photography_Tools.Services.AstroDataService;

public class OfflineAstroDataService : IAstroDataService
{
    public string DataSourceInfo { get; } = "SunCalcNet";

    public async Task<ServiceResponse<MoonData?>> GetMoonDataAsync(DateTime date, double latitude, double longitude)
    {
        MoonPhase? moonPhase = null;
        RiseAndSetResult? moonRiseAndSet = null;

        await Task.WhenAll(Task.Run(() => moonPhase = AstroCalculations.CalculateMoonPhase(date)), Task.Run(() => moonRiseAndSet = AstroCalculations.CalculateMoonRiseAndDown(date, latitude, longitude)));

        if (moonPhase is null || moonRiseAndSet is null)
            return new(null, false, -1, "Something went wrong");

        return new(new MoonData(moonRiseAndSet?.Rise ?? date, date, moonRiseAndSet?.Set ?? date, moonPhase.Value.Fraction * 100, moonPhase.Value.Phase, AstroCalculations.GetPhaseName(moonPhase.Value.Phase)), true, 1);
    }

    public Task<ServiceResponse<SunPhasesResult?>> GetSunDataAsync(DateTime date, double latitude, double longitude, double height = 0)
    {
        Dictionary<string, DateTime> result = [];

        foreach (var item in AstroCalculations.CalculateSunPhases(date, latitude, longitude, height))
        {
            result[item.Name] = item.Date;
        }

        Period day = new(result.GetValueOrNull(AstroConst.Sunrise), result.GetValueOrNull(AstroConst.Sunset));
        Period morningGoldenHour = new(result.GetValueOrNull(AstroConst.MorningGoldenHourStart), result.GetValueOrNull(AstroConst.MorningGoldenHourEnd));
        Period eveningGoldenHour = new(result.GetValueOrNull(AstroConst.EveningGoldenHourStart), result.GetValueOrNull(AstroConst.EveningGoldenHourEnd));
        Period morningBlueHour = new(result.GetValueOrNull(AstroConst.Dawn), result.GetValueOrNull(AstroConst.MorningGoldenHourStart));
        Period eveningBlueHour = new(result.GetValueOrNull(AstroConst.EveningGoldenHourEnd), result.GetValueOrNull(AstroConst.Dusk));
        Period morningCivilTwilight = new(result.GetValueOrNull(AstroConst.Dawn), result.GetValueOrNull(AstroConst.Sunrise));
        Period eveningCivilTwilight = new(result.GetValueOrNull(AstroConst.Sunset), result.GetValueOrNull(AstroConst.Dusk));

        return Task.FromResult(new ServiceResponse<SunPhasesResult?>(
            new(day, morningGoldenHour, eveningGoldenHour, morningBlueHour, eveningBlueHour, morningCivilTwilight, eveningCivilTwilight, result[AstroConst.SolarNoon]),
            true, 1));
    }
}