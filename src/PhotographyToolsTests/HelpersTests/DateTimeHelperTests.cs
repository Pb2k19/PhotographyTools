using Photography_Tools.Helpers;
using System.Globalization;

namespace PhotographyToolsTests.HelpersTests;

public class DateTimeHelperTests
{
    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithShortTimeWhenDateIsEqualToReferenceDate()
    {
        const int referanceDateDay = 10;
        DateTime? date = new DateTime(2134, 9, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"{date.Value.ToLocalTime().Hour}:00";

        string acutal = date.ToStringLocalTime(referanceDateDay, formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithShortTimeWhenDateIsEqualToReferenceDate_January()
    {
        const int referanceDateDay = 10;
        DateTime? date = new DateTime(2134, 1, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"{date.Value.ToLocalTime().Hour}:00";

        string acutal = date.ToStringLocalTime(referanceDateDay, formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithDateAndTimeWhenDateIsNotEqualToReferenceDate()
    {
        const int referanceDateDay = 9;
        DateTime? date = new DateTime(2134, 9, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"10 Sep {date.Value.ToLocalTime().Hour}:00";

        string acutal = date.ToStringLocalTime(referanceDateDay, formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithDateAndTimeWhenDateIsNotEqualToReferenceDate_January()
    {
        const int referanceDateDay = 9;
        DateTime? date = new DateTime(2134, 1, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"10 Jan {date.Value.ToLocalTime().Hour}:00";

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
        DateTime? date = new DateTime(2134, 9, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"10 Sep {date.Value.ToLocalTime().Hour}:00";

        string acutal = date.ToStringLocalTime(formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringLocalTime_ShouldReturnStringWithTime_January()
    {
        DateTime? date = new DateTime(2134, 1, 10, 12, 00, 0, DateTimeKind.Utc);
        string expected = $"10 Jan {date.Value.ToLocalTime().Hour}:00";

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
        DateTime? date = new DateTime(2134, 9, 10, 12, 00, 0, DateTimeKind.Local);
        string expected = $"10 Sep {date.Value.ToUniversalTime().Hour}:00";

        string acutal = date.ToStringUtcTime(formatProvider: CultureInfo.InvariantCulture);

        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void ToStringUtcTime_ShouldReturnStringWithTime_January()
    {
        DateTime? date = new DateTime(2134, 1, 10, 12, 00, 0, DateTimeKind.Local);
        string expected = $"10 Jan {date.Value.ToUniversalTime().Hour}:00";

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