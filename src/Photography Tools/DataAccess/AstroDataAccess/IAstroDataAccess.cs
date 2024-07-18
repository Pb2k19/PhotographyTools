
namespace Photography_Tools.DataAccess.AstroDataAccess;

public interface IAstroDataAccess
{
    public static readonly ServiceResponse<AstroData?>
        FailResultResponse = new(null, false, Message: "Something went wrong"),
        IncorrectInputResponse = new(null, false, Message: "Incorrect input");

    public string DataSourceInfo { get; }

    Task<ServiceResponse<AstroData?>> GetAstroDataAsync(DateTime dateTimeUtc, double latitude, double longitude);
}