using Photography_Tools.Helpers;
using Photography_Tools.Models;

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
    [InlineData(2024, 5, 10, 12, 0, 0, 0, 2460441)]
    [InlineData(2020, 1, 1, 0, 0, 0, 0, 2458849.5)]
    [InlineData(1000, 10, 21, 10, 20, 0, 0, 2086601.930556)]
    [InlineData(1582, 10, 15, 11, 40, 0, 0, 2299160.986111)]
    [InlineData(1582, 10, 4, 11, 40, 22, 0, 2299159.986366)]
    [InlineData(2222, 1, 1, 23, 20, 10, 0, 2532629.472338)]
    [InlineData(1957, 10, 4, 19, 26, 24, 0, 2436116.31)]
    [InlineData(2024, 7, 8, 18, 57, 24, 300, 2460500.289865)]
    [InlineData(1034, 10, 1, 12, 00, 0, 900, 2099000.00001)]
    [InlineData(2024, 2, 29, 9, 46, 56, 600, 2460369.9076)]
    [InlineData(2024, 2, 12, 11, 46, 56, 600, 2460352.990933)]
    public void FromJulianDate_ShouldReturnDateAsJulianDateDouble(int year, int month, int day, int hour, int minute, int seconds, int milliseconds, double input)
    {
        DateTime expected = new(year, month, day, hour, minute, seconds, milliseconds, DateTimeKind.Utc);

        DateTime result = AstroHelper.FromJulianDate(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0'n, 0.0002*e", 0, 0.0002)]
    [InlineData("47° 13′ 11″ N, 14° 45′ 53″ E", 47.219722, 14.764722)]
    [InlineData("  38° 58′ 4.44″ N, 106° 2′ 42.61″ W", 38.9679, -106.04517)]
    [InlineData("23° 42′ 13″ S; 46° 41′ 59″ W", -23.703611, -46.699722)]
    [InlineData(""" 37* 50' 59" S, 144° 58′ 6″ E  test""", -37.849722, 144.968333)]
    [InlineData("""37° 51' 1.134" S 144° 58' 11.5356" E""", -37.850315, 144.969871)]
    [InlineData("""37° 51' 1.134" N 144° 58' 11.5356" E""", 37.850315, 144.969871)]
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
    [InlineData("-23.703611,-46.699722", -23.703611, -46.699722)]
    [InlineData(" -37.849722;144.968333  ", -37.849722, 144.968333)]
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

    [Theory]
    [InlineData(36.583602, -121.755501, """36° 35' 0.9672" N 121° 45' 19.8036" W""")]
    [InlineData(-23.704627, -46.699262, """23° 42' 16.6572" S 46° 41' 57.3432" W""")]
    [InlineData(35.887979, 76.512641, """35° 53' 16.7244" N 76° 30' 45.5076" E""")]
    [InlineData(-37.850315, 144.969871, """37° 51' 1.134" S 144° 58' 11.5356" E""")]
    [InlineData(0, 0, """0° 0' 0" N 0° 0' 0" E""")]
    public void ConvertDdToDmsString_ShouldReturnCorrectDmsString(double inputLat, double inputLong, string expected)
    {
        string actual = AstroHelper.ConvertDdToDmsString(inputLat, inputLong);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(36.583602, -121.755501, """36° 35' 0.9672" N 121° 45' 19.8036" W""")]
    [InlineData(-23.704627, -46.699262, """23° 42' 16.6572" S 46° 41' 57.3432" W""")]
    [InlineData(35.887979, 76.512641, """35° 53' 16.7244" N 76° 30' 45.5076" E""")]
    [InlineData(-37.850315, 144.969871, """37° 51' 1.134" S 144° 58' 11.5356" E""")]
    [InlineData(0, 0, """0° 0' 0" N 0° 0' 0" E""")]
    public void ConvertDdToDmsString_ShouldReturnCorrectDmsStringFromGeographicalCoordinates(double inputLat, double inputLong, string expected)
    {
        string actual = AstroHelper.ConvertDdToDmsString(new GeographicalCoordinates(inputLat, inputLong));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(36.583602, -121.755501, 4, """36° 35' 0.9672" N 121° 45' 19.8036" W""")]
    [InlineData(-23.704627, -46.699262, 3, """23° 42' 16.657" S 46° 41' 57.343" W""")]
    [InlineData(35.887979, 76.512641, 2, """35° 53' 16.72" N 76° 30' 45.51" E""")]
    [InlineData(-37.850315, 144.969871, 1, """37° 51' 1.1" S 144° 58' 11.5" E""")]
    [InlineData(-37.850315, 144.969871, 0, """37° 51' 1" S 144° 58' 12" E""")]
    public void ConvertDdToDmsString_ShouldReturnCorrectDmsStringWithSelectedAccuracy(double inputLat, double inputLong, int accuracy, string expected)
    {
        string actual = AstroHelper.ConvertDdToDmsString(inputLat, inputLong, accuracy);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(36.583602, -121.755501, 4, """36° 35' 0.9672" N 121° 45' 19.8036" W""")]
    [InlineData(-23.704627, -46.699262, 3, """23° 42' 16.657" S 46° 41' 57.343" W""")]
    [InlineData(35.887979, 76.512641, 2, """35° 53' 16.72" N 76° 30' 45.51" E""")]
    [InlineData(-37.850315, 144.969871, 1, """37° 51' 1.1" S 144° 58' 11.5" E""")]
    [InlineData(-37.850315, 144.969871, 0, """37° 51' 1" S 144° 58' 12" E""")]
    public void ConvertDdToDmsString_ShouldReturnCorrectDmsStringFromGeographicalCoordinatesWithSelectedAccuracy(double inputLat, double inputLong, int accuracy, string expected)
    {
        string actual = AstroHelper.ConvertDdToDmsString(new GeographicalCoordinates(inputLat, inputLong), accuracy);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(20, 20)]
    [InlineData(90, 180)]
    [InlineData(-20, -20)]
    [InlineData(-90, -180)]
    public void Validate_ShouldReturnTrueIfCordsAreValid(double inputLat, double inputLong)
    {
        GeographicalCoordinates coordinates = new(inputLat, inputLong);

        bool actual = coordinates.Validate();

        Assert.True(actual);
    }

    [Theory]
    [InlineData(90, -180.0001)]
    [InlineData(-90.001, 0)]
    [InlineData(90, 180.0001)]
    [InlineData(90.001, 0)]
    [InlineData(0, 180.0001)]
    [InlineData(90.001, 180.0001)]
    [InlineData(-90.001, -180.0001)]
    [InlineData(double.NaN, 20)]
    [InlineData(20, double.NaN)]
    [InlineData(double.NaN, double.NaN)]
    [InlineData(200, 200)]
    public void Validate_ShouldReturnFalseIfCordsAreInvalid(double inputLat, double inputLong)
    {
        GeographicalCoordinates coordinates = new(inputLat, inputLong);

        bool actual = coordinates.Validate();

        Assert.False(actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(20, 20)]
    [InlineData(90, 180)]
    [InlineData(-20, -20)]
    [InlineData(-90, -180)]
    public void ValidateGeographicalCoordinates_ShouldReturnTrueIfCordsAreValid(double inputLat, double inputLong)
    {
        bool actual = AstroHelper.ValidateGeographicalCoordinates(inputLat, inputLong);

        Assert.True(actual);
    }

    [Theory]
    [InlineData(90, -180.0001)]
    [InlineData(-90.001, 0)]
    [InlineData(90, 180.0001)]
    [InlineData(90.001, 0)]
    [InlineData(0, 180.0001)]
    [InlineData(90.001, 180.0001)]
    [InlineData(-90.001, -180.0001)]
    [InlineData(double.NaN, 20)]
    [InlineData(20, double.NaN)]
    [InlineData(double.NaN, double.NaN)]
    [InlineData(200, 200)]
    public void ValidateGeographicalCoordinates_ShouldReturnFalseIfCordsAreInvalid(double inputLat, double inputLong)
    {
        bool actual = AstroHelper.ValidateGeographicalCoordinates(inputLat, inputLong);

        Assert.False(actual);
    }
}