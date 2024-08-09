namespace Photography_Tools.Services.AstroDataService;

public interface IAstroDataService
{
    public static readonly ServiceResponse<MoonData?> IncorrectInputMoonResult = new(null, false, -1, "Incorrect input");

    public static readonly ServiceResponse<SunPhasesResult?> IncorrectInputSunResult = new(null, false, -1, "Incorrect input");

    public string DataSourceInfo { get; }

    Task<ServiceResponse<MoonData?>> GetMoonDataAsync(DateTime date, double latitude, double longitude);

    Task<ServiceResponse<SunPhasesResult?>> GetSunDataAsync(DateTime date, double latitude, double longitude, double height = 0);
}