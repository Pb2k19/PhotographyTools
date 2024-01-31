using Photography_Tools.Models;

namespace Photography_Tools.Services.DofService;

public interface IDofService
{
    void CalculateDofValues(DofInfo dofInfo);
}