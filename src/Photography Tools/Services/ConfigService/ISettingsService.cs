namespace Photography_Tools.Services.ConfigService;

public interface ISettingsService
{
    public static readonly Settings DefaultSettings = new(false);

    public Settings GetSettings();
    public int UpdateSettings(Settings settings);
}
