﻿using Photography_Tools.DataAccess.AstroDataAccess;
using Photography_Tools.Services.KeyValueStoreService;
using System.Diagnostics;

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

        return new ServiceResponse<MoonData?>(astroData.Data is not null ? astroData.Data.MoonData with { MoonAge = CalculateMoonAge(astroData.Data.MoonData, date) } : null, astroData.IsSuccess, astroData.Code, astroData.Message);
    }

    public async Task<ServiceResponse<SunPhasesResult?>> GetSunDataAsync(DateTime date, double latitude, double longitude, double height = 0)
    {
        if (!AstroHelper.ValidateGeographicalCoordinates(latitude, longitude))
            return IAstroDataService.IncorrectInputSunResult;

        ServiceResponse<AstroData?> astroData = await GetAstroDataAsync(date, latitude, longitude);

        if (!astroData.IsSuccess || astroData.Data is null)
            return new(null, false, astroData.Code, astroData.Message);

        Period? day, morningGoldenHour, eveningGoldenHour, morningBlueHour, eveningBlueHour, morningCivilTwilight, eveningCivilTwilight;

        day = new(astroData.Data.SunData.Rise, astroData.Data.SunData.Set);
        morningCivilTwilight = new(astroData.Data.SunData.CivilTwilightStart, astroData.Data.SunData.Rise);
        eveningCivilTwilight = new(astroData.Data.SunData.Set, astroData.Data.SunData.CivilTwilightEnd);

        Period? goldenHour6deg = AstroCalculations.CalculateSunPhase(date, latitude, longitude, 6);
        Period? goldenHourMinus4deg = AstroCalculations.CalculateSunPhase(date, latitude, longitude, -4);

        morningGoldenHour = new(goldenHourMinus4deg?.StartDate, goldenHour6deg?.StartDate);
        eveningGoldenHour = new(goldenHour6deg?.EndDate, goldenHourMinus4deg?.EndDate);
        morningBlueHour = new(morningCivilTwilight.StartDate, morningGoldenHour.StartDate);
        eveningBlueHour = new(eveningGoldenHour.EndDate, eveningCivilTwilight.EndDate);

        return new(new(day, morningGoldenHour, eveningGoldenHour, morningBlueHour, eveningBlueHour, morningCivilTwilight, eveningCivilTwilight, astroData.Data.SunData.UpperTransit),
            true, 1);
    }

    private async Task<ServiceResponse<AstroData?>> GetAstroDataAsync(DateTime date, double latitude, double longitude)
    {
        date = date.ToUniversalTime();
        latitude = Math.Round(latitude, 2);
        longitude = Math.Round(longitude, 2);

        string cacheKey = $"{date:yyyy-MM-dd}-{latitude:0.00}-{longitude:0.00}";

        AstroData? astroData = await cacheStore.GetValueAsync(cacheKey);

        if (astroData is not null)
        {
#if DEBUG
            Debug.WriteLine("Astro data loaded from cache");
#endif
            return new(astroData, true, 1);
        }

        ServiceResponse<AstroData?> response = await astroDataAccess.GetAstroDataAsync(date.Date, latitude, longitude);

        if (!response.IsSuccess || response.Data is null)
            return new(null, false, response.Code, response.Message);

        await cacheStore.AddAsync(cacheKey, response.Data);

        return response;
    }

    private static double CalculateMoonAge(MoonData data, DateTime currentUtcTime)
    {
        double result = data.MoonAge + (currentUtcTime.Date - currentUtcTime).Duration().TotalDays;
        return result > AstroConst.SynodicMonthLength ? result - AstroConst.SynodicMonthLength : result;
    }
}