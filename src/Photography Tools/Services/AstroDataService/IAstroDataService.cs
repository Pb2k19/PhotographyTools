namespace Photography_Tools.Services.AstroDataService;

public interface IAstroDataService
{
    public static readonly ServiceResponse<MoonData?>
        ErrorMoonResult = new(null, false, Message: "Something went wrong"),
        IncorrectInputMoonResult = ErrorMoonResult with { Message = "Incorrect input" };

    Task<ServiceResponse<MoonData?>> GetMoonDataAsync(DateTime date, double latitude, double longitude);

    // Get Sun Data
}