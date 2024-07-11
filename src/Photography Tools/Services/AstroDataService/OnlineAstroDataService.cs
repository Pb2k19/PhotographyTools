using Photography_Tools.DataAccess.AstroDataAccess;
using Photography_Tools.Services.KeyValueStoreService;

namespace Photography_Tools.Services.AstroDataService;

public class OnlineAstroDataService : IAstroDataService
{
    private readonly IAstroDataAccess astroDataAccess;
    private readonly IKeyValueStore<AstroData> cacheStore;

    public string DataSourceInfo { get; }

    public OnlineAstroDataService(IAstroDataAccess astroDataAccess, IKeyValueStore<AstroData> cacheStore)
    {
        this.astroDataAccess = astroDataAccess;
        this.cacheStore = cacheStore;
        DataSourceInfo = astroDataAccess.DataSourceInfo;
    }

    public async Task<ServiceResponse<MoonData?>> GetMoonDataAsync(DateTime date, double latitude, double longitude)
    {
        if (!AstroHelper.ValidateGeographicalCoordinates(latitude, longitude))
            return IAstroDataService.IncorrectInputMoonResult;

        ServiceResponse<AstroData?> astroData = await GetAstroDataAsync(date, latitude, longitude);

        return new ServiceResponse<MoonData?>(astroData.Data?.MoonData ?? null, astroData.IsSuccess, astroData.Code, astroData.Message);
    }

    public async Task<ServiceResponse<SunPhasesResult?>> GetSunDataAsync(DateTime date, double latitude, double longitude, double heigth = 0)
    {
        if (!AstroHelper.ValidateGeographicalCoordinates(latitude, longitude))
            return IAstroDataService.IncorrectInputSunResult;

        ServiceResponse<AstroData?> astroData = await GetAstroDataAsync(date, latitude, longitude);

        if (!astroData.IsSuccess || astroData.Data is null)
            return new(null, false, astroData.Code, astroData.Message);

        Models.Phase? day, morningGoldenHour, eveningGoldenHour, morningBlueHour, eveningBlueHour, morningCivilTwilight, eveningCivilTwilight;
        day = morningGoldenHour = eveningGoldenHour = morningBlueHour = eveningBlueHour = morningCivilTwilight = eveningCivilTwilight = null;

        day = new(astroData.Data.SunData.Rise, astroData.Data.SunData.Set);
        morningCivilTwilight = new(astroData.Data.SunData.CivilTwilightStart, astroData.Data.SunData.Rise);
        eveningCivilTwilight = new(astroData.Data.SunData.Set, astroData.Data.SunData.CivilTwilightEnd);

        Models.Phase? goldenHour6deg = AstroCalculations.CalculateSunPhase(date, latitude, longitude, 6);
        Models.Phase? goldenHourMinus4deg = AstroCalculations.CalculateSunPhase(date, latitude, longitude, -4);

        if (goldenHour6deg is not null && goldenHourMinus4deg is not null)
        {
            morningGoldenHour = new(goldenHourMinus4deg.Value.StartDate, goldenHour6deg.Value.StartDate);
            eveningGoldenHour = new(goldenHour6deg.Value.EndDate, goldenHourMinus4deg.Value.EndDate);
            morningBlueHour = new(astroData.Data.SunData.Set, morningGoldenHour.Value.StartDate);
            eveningBlueHour = new(eveningGoldenHour.Value.EndDate, astroData.Data.SunData.CivilTwilightEnd);
        }

        return new(new(day, morningGoldenHour, eveningGoldenHour, morningBlueHour, eveningBlueHour, morningCivilTwilight, eveningCivilTwilight, astroData.Data.SunData.UpperTransit),
            true, 1);
    }

    private async Task<ServiceResponse<AstroData?>> GetAstroDataAsync(DateTime date, double latitude, double longitude)
    {
        latitude = Math.Round(latitude, 2);
        longitude = Math.Round(longitude, 2);

        string cacheKey = $"{date:yyyy-MM-dd-HH}-{latitude:0.00}-{longitude:0.00}";

        AstroData? astroData = await cacheStore.GetValueAsync(cacheKey);

        if (astroData is not null)
            return new(astroData, true, 1);

        ServiceResponse<AstroData?> response = await astroDataAccess.GetAstroDataAsync(date, latitude, longitude);

        if (!response.IsSuccess || response.Data is null)
            return new(null, false, response.Code, response.Message);

        await cacheStore.AddAsync(cacheKey, response.Data);

        return response;
    }
}