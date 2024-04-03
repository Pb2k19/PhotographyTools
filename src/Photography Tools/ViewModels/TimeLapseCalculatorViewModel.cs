namespace Photography_Tools.ViewModels;

public partial class TimeLapseCalculatorViewModel : SaveableViewModel
{
    private const int ShootsCountMin = 0, ShootsCountMax = 2_000_000_000;
    private const double ClipFrameRateFPSMin = 0, ClipFrameRateFPSMax = 10_000_000;

    [ObservableProperty]
    private TimeLapseUserInput userInput;

    public TimeLapseCalculatorViewModel(IPreferencesService preferencesService) : base(preferencesService)
    {
        UserInput = preferencesService?.GetDeserailizedPreference<TimeLapseUserInput>(PreferencesKeys.TimeLapseCalcUserInputPreferencesKey) ?? new() { TimeLapseCalcValues = new() };
    }

    [RelayCommand]
    public void OnShootingIntervalChanged()
    {
        UserInput.TimeLapseCalcValues.CalculateShotsCount();
        UserInput.TimeLapseCalcValues.CalculateClipLength();
        UserInput.TimeLapseCalcValues.CalculateStorageSize();
        UserInput.EnteredShootsCount = UserInput.TimeLapseCalcValues.ShootsCount;
        OnPropertyChanged(nameof(UserInput));
    }

    [RelayCommand]
    public void OnShootingLengthChaneged()
    {
        UserInput.TimeLapseCalcValues.CalculateShotsCount();
        UserInput.TimeLapseCalcValues.CalculateClipLength();
        UserInput.TimeLapseCalcValues.CalculateStorageSize();
        UserInput.EnteredShootsCount = UserInput.TimeLapseCalcValues.ShootsCount;
        OnPropertyChanged(nameof(UserInput));
    }

    [RelayCommand]
    private void OnShootsCountChanged()
    {
        if (UserInput.EnteredShootsCount < ShootsCountMin)
        {
            UserInput.EnteredShootsCount = ShootsCountMin;
            OnPropertyChanged(nameof(UserInput));
        }
        else if (UserInput.EnteredShootsCount > ShootsCountMax)
        {
            UserInput.EnteredShootsCount = ShootsCountMax;
            OnPropertyChanged(nameof(UserInput));
        }

        if (UserInput.TimeLapseCalcValues.ShootsCount != UserInput.EnteredShootsCount)
        {
            UserInput.TimeLapseCalcValues.ShootsCount = UserInput.EnteredShootsCount;
            UserInput.TimeLapseCalcValues.CalculateLength();
            UserInput.TimeLapseCalcValues.CalculateClipLength();
            UserInput.TimeLapseCalcValues.CalculateStorageSize();
            OnPropertyChanged(nameof(UserInput));
        }
    }

    [RelayCommand]
    private void OnClipFrameRateFPSChanged()
    {
        if (UserInput.EnteredClipFrameRateFPS < ShootsCountMin)
        {
            UserInput.EnteredClipFrameRateFPS = ClipFrameRateFPSMin;
            OnPropertyChanged(nameof(UserInput));
        }
        else if (UserInput.EnteredClipFrameRateFPS > ShootsCountMax)
        {
            UserInput.EnteredClipFrameRateFPS = ClipFrameRateFPSMax;
            OnPropertyChanged(nameof(UserInput));
        }

        if (UserInput.TimeLapseCalcValues.ClipFrameRateFPS != UserInput.EnteredClipFrameRateFPS)
        {
            UserInput.TimeLapseCalcValues.ClipFrameRateFPS = UserInput.EnteredClipFrameRateFPS;
            UserInput.TimeLapseCalcValues.CalculateClipLength();
            OnPropertyChanged(nameof(UserInput));
        }
    }

    [RelayCommand]
    private void OnClipLengthChanged()
    {
        UserInput.TimeLapseCalcValues.CalculateShotsCountFromResultClip();
        UserInput.TimeLapseCalcValues.CalculateLength();
        UserInput.TimeLapseCalcValues.CalculateStorageSize();
        UserInput.EnteredShootsCount = UserInput.TimeLapseCalcValues.ShootsCount;
        OnPropertyChanged(nameof(UserInput));
    }

    [RelayCommand]
    private void OnPhotoSizeChanged()
    {
        UserInput.TimeLapseCalcValues.CalculateStorageSize();
        OnPropertyChanged(nameof(UserInput));
    }

    [RelayCommand]
    private void OnTotalStorageSizeChanged()
    {
        UserInput.TimeLapseCalcValues.CalculateShotsCountFromStorage();
        UserInput.TimeLapseCalcValues.CalculateLength();
        UserInput.TimeLapseCalcValues.CalculateClipLength();
        UserInput.EnteredShootsCount = UserInput.TimeLapseCalcValues.ShootsCount;
        OnPropertyChanged(nameof(UserInput));
    }

    protected override void SaveUserInput()
    {
        preferencesService?.SerializedAndSetPreference(PreferencesKeys.TimeLapseCalcUserInputPreferencesKey, UserInput);
    }
}
