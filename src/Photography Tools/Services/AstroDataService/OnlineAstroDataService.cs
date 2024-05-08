using Photography_Tools.DataAccess.AstroDataAccess;

namespace Photography_Tools.Services.AstroDataService;

public class OnlineAstroDataService : IAstroDataService
{
    private readonly IAstroDataAccess astroDataAccess;

    public OnlineAstroDataService(IAstroDataAccess astroDataAccess)
    {
        this.astroDataAccess = astroDataAccess;
    }

    public async Task<ServiceResponse<MoonData?>> GetMoonDataAsync(DateTime date, double latitude, double longitude)
    {
        latitude = Math.Round(latitude, 2);
        longitude = Math.Round(longitude, 2);

        if (latitude < AstroConst.LatitudeMinValue || latitude > AstroConst.LatitudeMaxValue ||
            longitude < AstroConst.LongitudeMinValue || longitude > AstroConst.LongitudeMaxValue)
            return IAstroDataService.IncorrectInputMoonResult;

        ServiceResponse<AstroData?> response = await astroDataAccess.GetAstroDataAsync(date, latitude, longitude);

        if (!response.IsSuccess || response.Data is null)
            return IAstroDataService.ErrorMoonResult with { Message = response.Message, Code = response.Code };

        return new ServiceResponse<MoonData?>(response.Data.MoonData, response.IsSuccess, response.Code, response.Message);
    }
}