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

    public Task<ServiceResponse<SunPhasesResult?>> GetSunDataAsync(DateTime date, double latitude, double longitude, double heigth = 0)
    {
        Dictionary<string, DateTime> result = [];

        foreach (var item in AstroCalculations.CalculateSunPhases(date, latitude, longitude, heigth))
        {
            result[item.Name] = item.Date;
        }

        Period? day, morningGoldenHour, eveningGoldenHour, morningBlueHour, eveningBlueHour, morningCivilTwilight, eveningCivilTwilight;
        day = morningGoldenHour = eveningGoldenHour = morningBlueHour = eveningBlueHour = morningCivilTwilight = eveningCivilTwilight = null;

        if (result.TryGetValue(AstroConst.Sunrise, out DateTime start) && result.TryGetValue(AstroConst.Sunrise, out DateTime end))
            day = new(start, end);

        if (result.TryGetValue(AstroConst.MorningGoldenHourStart, out start) && result.TryGetValue(AstroConst.MorningGoldenHourEnd, out end))
            morningGoldenHour = new(start, end);

        if (result.TryGetValue(AstroConst.EveningGoldenHourStart, out start) && result.TryGetValue(AstroConst.EveningGoldenHourEnd, out end))
            eveningGoldenHour = new(start, end);

        if (result.TryGetValue(AstroConst.Dawn, out start) && result.TryGetValue(AstroConst.MorningGoldenHourStart, out end))
            morningBlueHour = new(start, end);

        if (result.TryGetValue(AstroConst.EveningGoldenHourEnd, out start) && result.TryGetValue(AstroConst.Dusk, out end))
            eveningBlueHour = new(start, end);

        if (result.TryGetValue(AstroConst.Dawn, out start) && result.TryGetValue(AstroConst.Sunrise, out end))
            morningCivilTwilight = new(start, end);

        if (result.TryGetValue(AstroConst.Sunset, out start) && result.TryGetValue(AstroConst.Dusk, out end))
            eveningCivilTwilight = new(start, end);

        return Task.FromResult(new ServiceResponse<SunPhasesResult?>(
            new(day, morningGoldenHour, eveningGoldenHour, morningBlueHour, eveningBlueHour, morningCivilTwilight, eveningCivilTwilight, result[AstroConst.SolarNoon]),
            true, 1));
    }
}