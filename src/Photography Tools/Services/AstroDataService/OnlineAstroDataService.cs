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
        latitude = Math.Round(latitude, 2);
        longitude = Math.Round(longitude, 2);

        if (latitude < AstroConst.LatitudeMinValue || latitude > AstroConst.LatitudeMaxValue ||
            longitude < AstroConst.LongitudeMinValue || longitude > AstroConst.LongitudeMaxValue)
            return IAstroDataService.IncorrectInputMoonResult;

        string cacheKey = $"{date:yyyy-MM-dd-HH}-{latitude:0.00}-{longitude:0.00}";

        AstroData? moonData = await cacheStore.GetValueAsync(cacheKey);

        if (moonData is not null)
            return new(moonData.MoonData, true, 1);

        ServiceResponse<AstroData?> response = await astroDataAccess.GetAstroDataAsync(date, latitude, longitude);

        if (!response.IsSuccess || response.Data is null)
            return IAstroDataService.ErrorMoonResult with { Message = response.Message, Code = response.Code };

        await cacheStore.AddAsync(cacheKey, response.Data);

        return new ServiceResponse<MoonData?>(response.Data.MoonData, response.IsSuccess, response.Code, response.Message);
    }
}