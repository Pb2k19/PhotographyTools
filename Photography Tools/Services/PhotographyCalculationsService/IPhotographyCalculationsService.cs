using Photography_Tools.Models;

namespace Photography_Tools.Services.PhotographyCalculationsService;

public interface IPhotographyCalculationsService
{
    DofCalcResult CalculateDofValues(DofCalcInput dofInfo);
    (double rule200, double rule300, double rule500) CalculateTimeForAstro(double sensorCrop, double focalLength);
    double CalculateTimeForAstroWithNPFRule(Sensor sensor, Lens lens, int accuracy, double declinationDegrees = 0);
    TimeSpan CalculateTimeWithNDFilters(TimeSpan inputTime, IEnumerable<NDFilter> filters);
}