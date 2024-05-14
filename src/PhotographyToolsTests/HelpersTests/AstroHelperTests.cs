using Photography_Tools.Helpers;

namespace PhotographyToolsTests.HelpersTests;

public class AstroHelperTests
{
    [Theory]
    [InlineData(2024, 5, 10, 12, 0, 0, 2460441)]
    [InlineData(2020, 1, 1, 0, 0, 0, 2458849.5)]
    [InlineData(1000, 10, 21, 10, 20, 0, 2086601.930556)]
    [InlineData(1582, 10, 15, 11, 40, 0, 2299160.986111)]
    [InlineData(1582, 10, 4, 11, 40, 22, 2299159.986366)]
    [InlineData(2222, 1, 1, 23, 20, 10, 2532629.472338)]
    public void ToJulianDate_ShouldReturnDateAsJulianDateDouble(int year, int month, int day, int hour, int minute, int seconds, double expected)
    {
        DateTime date = new(year, month, day, hour, minute, seconds, DateTimeKind.Utc);

        double result = AstroHelper.ToJulianDate(date);

        Assert.Equal(expected, result, 6);
    }

    [Theory]
    [InlineData(2024, 5, 10, 12, 0, 0, false)]
    [InlineData(2020, 1, 1, 0, 0, 0, false)]
    [InlineData(1000, 10, 21, 10, 20, 0, true)]
    [InlineData(1582, 10, 15, 11, 40, 0, false)]
    [InlineData(1582, 10, 4, 11, 40, 22, true)]
    [InlineData(2222, 1, 1, 23, 20, 10, false)]
    public void IsJulianDate_ShouldReturnCorrectValue(int year, int month, int day, int hour, int minute, int seconds, bool expected)
    {
        DateTime date = new(year, month, day, hour, minute, seconds, DateTimeKind.Utc);

        bool result = AstroHelper.IsJulianDate(date);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1582, 10, 14, 11, 40, 0)]
    [InlineData(1582, 10, 10, 0, 0, 0)]
    [InlineData(1582, 10, 5, 11, 40, 22)]
    public void IsJulianDate_ShouldThrowException(int year, int month, int day, int hour, int minute, int seconds)
    {
        DateTime date = new(year, month, day, hour, minute, seconds, DateTimeKind.Utc);

        Assert.Throws<ArgumentOutOfRangeException>(() => AstroHelper.IsJulianDate(date));
    }

    [Theory]
    [InlineData("0'n, 0.0002*e", 0, 0.0002)]
    [InlineData("47° 13′ 11″ N, 14° 45′ 53″ E", 47.219722, 14.764722)]
    [InlineData("  38° 58′ 4.44″ N, 106° 2′ 42.61″ W", 38.9679, -106.04517)]
    [InlineData("23° 42′ 13″ S; 46° 41′ 59″ W", -23.703611, -46.699722)]
    [InlineData(""" 37* 50' 59" S, 144° 58′ 6″ E  test""", -37.849722, 144.968333)]
    public void ConvertDmsStringToDd_ShouldReturnCorrectLatitudeAndLongitude(string input, double expectedLat, double expectedLong)
    {
        (double latitude, double longitude) = AstroHelper.ConvertDmsStringToDd(input);

        Assert.Equal(expectedLat, latitude, 5);
        Assert.Equal(expectedLong, longitude, 5);
    }

    [Fact]
    public void ConvertDmsStringToDd_ShouldReturnNaNWhenInputIsIncorrect()
    {
        (double latitude, double longitude) = AstroHelper.ConvertDmsStringToDd("38.9679-106.04517");

        Assert.Equal(double.NaN, latitude);
        Assert.Equal(double.NaN, longitude);
    }

    [Theory]
    [InlineData("0, 0.0002", 0, 0.0002)]
    [InlineData("47.219722, 14.764722", 47.219722, 14.764722)]
    [InlineData("  38.9679 -106.04517", 38.9679, -106.04517)]
    [InlineData("-23.703611, -46.699722", -23.703611, -46.699722)]
    [InlineData(" -37.849722; 144.968333  ", -37.849722, 144.968333)]
    public void ConvertDdStringToDd_ShouldReturnCorrectLatitudeAndLongitude(string input, double expectedLat, double expectedLong)
    {
        (double latitude, double longitude) = AstroHelper.ConvertDdStringToDd(input);

        Assert.Equal(expectedLat, latitude);
        Assert.Equal(expectedLong, longitude);
    }

    [Fact]
    public void ConvertDdStringToDd_ShouldReturnNaNWhenInputIsIncorrect()
    {
        (double latitude, double longitude) = AstroHelper.ConvertDdStringToDd("38.9679-106.04517");

        Assert.Equal(double.NaN, latitude);
        Assert.Equal(double.NaN, longitude);
    }

    [Theory]
    [InlineData("0.0002*e", 0.0002)]
    [InlineData("47° 13′ 11″ N, ", 47.219722)]
    [InlineData("  106° 2′ 42.61″ W", -106.04517)]
    [InlineData("23° 42′ 13″ S; ", -23.703611)]
    [InlineData(""" 144° 58′ 6″ E  test""", 144.968333)]
    public void ConvertDmsPartToDd_ShouldReturnCorrectLatitudeAndLongitude(string input, double expected)
    {
        double actual = AstroHelper.ConvertDmsPartToDd(input);

        Assert.Equal(expected, actual, 5);
    }

    [Fact]
    public void ConvertDmsPartToDd_ShouldReturnNaNWhenInputIsIncorrect()
    {
        double actual = AstroHelper.ConvertDmsPartToDd("38.9679");

        Assert.Equal(double.NaN, actual);
    }
}