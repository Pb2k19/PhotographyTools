namespace Photography_Tools.Services.AstroDataService;

public interface IAstroDataService
{
    MoonDataResult GetMoonData(DateTime date, double latitude, double longitude);

    // Get Sun Data
}