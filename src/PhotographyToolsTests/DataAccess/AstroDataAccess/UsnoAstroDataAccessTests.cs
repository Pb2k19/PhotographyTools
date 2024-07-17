using Photography_Tools.DataAccess.AstroDataAccess;

namespace PhotographyToolsTests.DataAccess.AstroDataAccess;

public class UsnoAstroDataAccessTests
{
    [Theory]
    [InlineData(638577708000000000, "02:51", "Last Quarter", 28, 7, 2024, 22.60)] // 2024-07-28 13:40:00
    [InlineData(638576676000000000, "02:51", "Last Quarter", 28, 7, 2024, 21.40)] // 2024-07-27 09:00:00
    [InlineData(638555172000000000, "22:57", "New Moon", 5, 7, 2024, 26.06)] // 2024-07-02 11:40:00
    [InlineData(638559492000000000, "22:57", "New Moon", 5, 7, 2024, 1.53)] // 2024-07-07 11:40:00
    [InlineData(638573157000000000, "10:17", "Full Moon", 21, 7, 2024, 16.64)] // 2024-07-23 07:15:00
    [InlineData(638570565000000000, "10:17", "Full Moon", 21, 7, 2024, 13.64)] // 2024-07-20 07:15:00
    [InlineData(638564544000000000, "22:49", "First Quarter", 13, 7, 2024, 6.77)] // 2024-07-13 08:00:00
    [InlineData(638565408000000000, "22:49", "First Quarter", 13, 7, 2024, 7.77)] // 2024-07-14 08:00:00
    public void GetMoonAge_ShouldReturnCorrectMoonAge(long dateTicks, string phaseTime, string phaseName, int day, int month, int year, double expected)
    {
        DateTime dateTime = new(dateTicks);
        UsnoAstroDataAccess.PhaseData phaseData = new() { Name = phaseName, TimeString = phaseTime, Day = day, Month = month, Year = year };

        double actual = UsnoAstroDataAccess.GetMoonAge(dateTime, phaseData);

        Assert.Equal(expected, actual, 2);
    }
}