using Photography_Tools.Helpers;
using System.Globalization;

namespace PhotographyToolsTests.HelpersTests;

public class DateTimeHelperTests
{
    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithShortTimeWhenDateIsEqualToReferenceDate()
    {
        const int referanceDateDay = 10;
        int timeDiff = (int)Math.Round((DateTime.Now - DateTime.UtcNow).TotalHours, 0);
        DateTime? date = new DateTime(2134, 9, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"{12 + timeDiff}:00";

        string acutal = date.ToStringLocalTime(referanceDateDay, formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithDateAndTimeWhenDateIsNotEqualToReferenceDate()
    {
        const int referanceDateDay = 9;
        int timeDiff = (int)Math.Round((DateTime.Now - DateTime.UtcNow).TotalHours, 0);
        DateTime? date = new DateTime(2134, 9, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"10 Sep {12 + timeDiff}:00";

        string acutal = date.ToStringLocalTime(referanceDateDay, formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_WithReferenceDate_ShouldReturnStringWithTime_HU()
    {
        const int referanceDateDay = 9;
        const string expected = "10 szept. 16:49";
        DateTime? date = new DateTime(2134, 9, 10, 16, 49, 0, DateTimeKind.Local);

        string acutal = date.ToStringLocalTime(referanceDateDay, formatProvider: CultureInfo.GetCultureInfo("hu-hu"));

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_WithReferenceDate_ShouldReturnCustomDefaultValue()
    {
        const string expected = "DateTimeHelper.DefaultValue";
        DateTime? date = null;

        string acutal = date.ToStringLocalTime(0, expected);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithTime()
    {
        int timeDiff = (int)Math.Round((DateTime.Now - DateTime.UtcNow).TotalHours, 0);
        DateTime? date = new DateTime(2134, 9, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"10 Sep {12 + timeDiff}:00";

        string acutal = date.ToStringLocalTime(formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithTime_InvariantCulture()
    {
        const string expected = "10 Sep 23:49";
        DateTime? date = new DateTime(2134, 9, 10, 23, 49, 0, DateTimeKind.Local);

        string acutal = date.ToStringLocalTime(formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithTime_PL()
    {
        const string expected = "10 wrz 18:49";
        DateTime? date = new DateTime(2134, 9, 10, 18, 49, 0, DateTimeKind.Local);

        string acutal = date.ToStringLocalTime(formatProvider: CultureInfo.GetCultureInfo("pl-pl"));

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithTime_HU()
    {
        const string expected = "10 szept. 16:49";
        DateTime? date = new DateTime(2134, 9, 10, 16, 49, 0, DateTimeKind.Local);

        string acutal = date.ToStringLocalTime(formatProvider: CultureInfo.GetCultureInfo("hu-hu"));

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnCustomStringWithTime_InvariantCulture()
    {
        const string expected = "09/10/2134";
        DateTime? date = new DateTime(2134, 9, 10, 16, 49, 0, DateTimeKind.Local);

        string acutal = date.ToStringLocalTime(format: "d", formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnCustomStringWithTime_PL()
    {
        const string expected = "10.09.2134";
        DateTime? date = new DateTime(2134, 9, 10, 16, 49, 0, DateTimeKind.Local);

        string acutal = date.ToStringLocalTime(format: "d", formatProvider: CultureInfo.GetCultureInfo("pl-pl"));

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnDefaultValue()
    {
        const string expected = DateTimeHelper.DefaultValue;
        DateTime? date = null;

        string acutal = date.ToStringLocalTime();

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnCustomDefaultValue()
    {
        const string expected = " value ";
        DateTime? date = null;

        string acutal = date.ToStringLocalTime(nullValue: expected);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnStringWithTime()
    {
        int timeDiff = (int)Math.Round((DateTime.Now - DateTime.UtcNow).TotalHours, 0);
        DateTime? date = new DateTime(2134, 9, 10, 12, 00, 0, DateTimeKind.Local);
        string expected = $"10 Sep {12 - timeDiff}:00";

        string acutal = date.ToStringUtcTime(formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnStringWithTime_InvariantCulture()
    {
        const string expected = "10 Sep 23:49";
        DateTime? date = new DateTime(2134, 9, 10, 23, 49, 0, DateTimeKind.Utc);

        string acutal = date.ToStringUtcTime(formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnStringWithTime_PL()
    {
        const string expected = "10 wrz 18:49";
        DateTime? date = new DateTime(2134, 9, 10, 18, 49, 0, DateTimeKind.Utc);

        string acutal = date.ToStringUtcTime(formatProvider: CultureInfo.GetCultureInfo("pl-pl"));

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnStringWithTime_HU()
    {
        const string expected = "10 szept. 16:49";
        DateTime? date = new DateTime(2134, 9, 10, 16, 49, 0, DateTimeKind.Utc);

        string acutal = date.ToStringUtcTime(formatProvider: CultureInfo.GetCultureInfo("hu-hu"));

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnCustomStringWithTime_InvariantCulture()
    {
        const string expected = "09/10/2134";
        DateTime? date = new DateTime(2134, 9, 10, 16, 49, 0, DateTimeKind.Utc);

        string acutal = date.ToStringUtcTime(format: "d", formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnCustomStringWithTime_PL()
    {
        const string expected = "10.09.2134";
        DateTime? date = new DateTime(2134, 9, 10, 16, 49, 0, DateTimeKind.Utc);

        string acutal = date.ToStringUtcTime(format: "d", formatProvider: CultureInfo.GetCultureInfo("pl-pl"));

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnDefaultValue()
    {
        const string expected = DateTimeHelper.DefaultValue;
        DateTime? date = null;

        string acutal = date.ToStringUtcTime();

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnCustomDefaultValue()
    {
        const string expected = " value ";
        DateTime? date = null;

        string acutal = date.ToStringUtcTime(nullValue: expected);

        Assert.Equal(expected, acutal);
    }
}