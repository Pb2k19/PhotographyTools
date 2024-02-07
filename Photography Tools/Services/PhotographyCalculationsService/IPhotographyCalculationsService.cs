using Photography_Tools.Models;

namespace Photography_Tools.Services.PhotographyCalculationsService;

public interface IPhotographyCalculationsService
{
    DofCalcResult CalculateDofValues(DofCalcInput dofInfo);
}