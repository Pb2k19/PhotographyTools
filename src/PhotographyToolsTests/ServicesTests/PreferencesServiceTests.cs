using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Photography_Tools.Services.PreferencesService;

namespace PhotographyToolsTests.ServicesTests;

public class PreferencesServiceTests
{
    private readonly IPreferencesService preferencesService;
    private readonly IPreferences preferences = Substitute.For<IPreferences>();

    public PreferencesServiceTests()
    {
        preferencesService = new PreferencesService(preferences);
    }

    [Fact]
    public void GetPreference_ShouldReturnPreferenceStringValue_WhenKeyExists()
    {
        const string key = "test", expected = "Test Value String 123 @#$";
        preferences.Get(key, string.Empty).Returns(expected);

        string? actual = preferencesService.GetPreference(key, string.Empty);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetPreference_ShouldReturnNull_WhenKeyDoesNotExists()
    {
        const string key = "test";
        preferences.Get<string?>(key, null).ReturnsNull();

        string? actual = preferencesService.GetPreference<string?>(key);

        Assert.Null(actual);
    }

    [Fact]
    public void GetPreference_ShouldReturnCustomDefaultString_WhenKeyDoesNotExists()
    {
        const string key = "test", expected = "CustomDefault";
        preferences.Get<string?>(key, expected).Returns(expected);

        string? actual = preferencesService.GetPreference(key, expected);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetPreference_ShouldReturnPreferenceIntValue_WhenKeyExists()
    {
        const int expected = 55;
        const string key = "test";
        preferences.Get(key, 0).Returns(expected);

        int actual = preferencesService.GetPreference<int>(key);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetPreference_ShouldReturn0_WhenKeyDoesNotExists()
    {
        const int expected = default;
        const string key = "test";
        preferences.Get(key, expected).Returns(expected);

        int actual = preferencesService.GetPreference<int>(key);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SetPreferences_ShouldReturnTrue_WhenValueIsNotNull()
    {
        const string key = "test", value = "Test Value String 123 @#$";
        preferences.Get<string?>(key, null).ReturnsNull();

        bool actual = preferencesService.SetPreference(key, value);

        preferences.Received(1).Get<string?>(key, null);
        preferences.Received(1).Set(key, value);
        Assert.True(actual);
    }

    [Fact]
    public void SetPreferences_ShouldReturnTrue_WhenKeyHasSameValue()
    {
        const string key = "test", value = "Test Value String 123 @#$";
        preferences.Get<string?>(key, null).Returns(value);

        bool actual = preferencesService.SetPreference(key, value);

        preferences.Received(1).Get<string?>(key, null);
        preferences.DidNotReceiveWithAnyArgs().Set(key, value);
        Assert.True(actual);
    }

    [Fact]
    public void SetPreferences_ShouldReturnFalse_WhenValueIsNull()
    {
        const string key = "test";

        bool actual = preferencesService.SetPreference<string?>(key, null);

        Assert.False(actual);
    }

    [Fact]
    public void SerializedAndSetPreference_ShouldReturnTrue()
    {
        const string key = "test", expectedSerialized = """{"Name":"TestObject0","FocalLengthMM":55,"Aperture":2.2}""";
        TestObject obj = new() { Name = "TestObject0", Aperture = 2.2, FocalLengthMM = 55 };
        preferences.Get<string?>(key, null).ReturnsNull();

        bool actual = preferencesService.SerializeAndSetPreference(key, obj);

        preferences.Received(1).Get<string?>(key, null);
        preferences.Received(1).Set(key, expectedSerialized);
        Assert.True(actual);
    }

    [Fact]
    public void GetDeserailizedPreference_ShouldReturnDeserializedObject()
    {
        const string key = "test", serialized = """{"Name":"TestObject1","FocalLengthMM":55,"Aperture":2.2}""";

        TestObject expected = new() { Name = "TestObject1", Aperture = 2.2, FocalLengthMM = 55 };
        preferences.Get<string?>(key, string.Empty).Returns(serialized);

        TestObject? actual = preferencesService.GetDeserailizedPreference<TestObject>(key);

        preferences.Received(1).Get(key, string.Empty);
        Assert.NotNull(actual);
        Assert.Equal(expected.FocalLengthMM, actual.FocalLengthMM);
        Assert.Equal(expected.Aperture, actual.Aperture);
        Assert.Equal(expected.Name, actual.Name);
    }

    [Fact]
    public void GetDeserailizedPreference_ShouldReturnNull_WhenValueIsNotSet()
    {
        const string key = "test";
        preferences.Get<string?>(key, string.Empty).ReturnsNull();

        TestObject? actual = preferencesService.GetDeserailizedPreference<TestObject>(key);

        preferences.Received(1).Get(key, string.Empty);
        Assert.Null(actual);
    }

    [Fact]
    public void GetDeserailizedPreference_ShouldReturnNull_WhenValueIsNotJson()
    {
        const string key = "test";
        preferences.Get<string?>(key, string.Empty).Returns("NOT A JSON");

        TestObject? actual = preferencesService.GetDeserailizedPreference<TestObject>(key);

        preferences.Received(1).Get(key, string.Empty);
        Assert.Null(actual);
    }

    [Fact]
    public void GetDeserailizedPreference_ShouldReturnCustomDefaultValue_WhenValueIsNotSet()
    {
        const string key = "test";
        preferences.Get<string?>(key, string.Empty).ReturnsNull();
        TestObject expected = new() { Name = "TestObject2", Aperture = 2.8, FocalLengthMM = 75 };

        TestObject? actual = preferencesService.GetDeserailizedPreference(key, expected);

        preferences.Received(1).Get(key, string.Empty);
        Assert.NotNull(actual);
        Assert.Equal(expected.FocalLengthMM, actual.FocalLengthMM);
        Assert.Equal(expected.Aperture, actual.Aperture);
        Assert.Equal(expected.Name, actual.Name);
    }

    [Fact]
    public void GetDeserailizedPreference_ShouldReturnCustomDefaultValue_WhenValueIsNotJson()
    {
        const string key = "test";
        preferences.Get<string?>(key, string.Empty).Returns("NOT A JSON");
        TestObject expected = new() { Name = "TestObject3", Aperture = 2.8, FocalLengthMM = 75 };

        TestObject? actual = preferencesService.GetDeserailizedPreference(key, expected);

        preferences.Received(1).Get(key, string.Empty);
        Assert.NotNull(actual);
        Assert.Equal(expected.FocalLengthMM, actual.FocalLengthMM);
        Assert.Equal(expected.Aperture, actual.Aperture);
        Assert.Equal(expected.Name, actual.Name);
    }
}