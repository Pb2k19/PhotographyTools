using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class DofCalcViewModel : SaveableViewModel
{
    private const int VisualAcuityMin = 1, VisualAcuityMax = 100;

    private readonly IPhotographyCalculationsService photographyCalcService;
    private readonly ISensorsDataAccess sensorsDataAccess;
    private readonly IUiMessageService messageService;

    [ObservableProperty]
    private string toggleText = string.Empty;

    [ObservableProperty]
    private DofCalcResult dofCalcResult;

    [ObservableProperty]
    private DofCalcUserInput userInput;

    public ImmutableArray<double> Apertures { get; }

    public ImmutableArray<string> SensorNames { get; }

    public DofCalcViewModel(IPhotographyCalculationsService photographyCalcService, IPreferencesService preferencesService, ISensorsDataAccess sensorsDataAccess, IUiMessageService messageService) : base(preferencesService)
    {
        this.photographyCalcService = photographyCalcService;
        this.sensorsDataAccess = sensorsDataAccess;
        this.messageService = messageService;

        Apertures = ApertureConst.AllStops;
        SensorNames = sensorsDataAccess.GetSensorNames();
        DofCalcResult = new();

        DofCalcUserInput? userInput = preferencesService.GetDeserailizedPreference<DofCalcUserInput>(PreferencesKeys.DofCalcUserInputPreferencesKey);
        UserInput = userInput is not null && userInput.Validate() ? userInput : new()
        {
            SelectedSensorName = SensorNames[0],
            DofCalcInput = new()
            {
                FocusingDistanceMM = 500,
                CameraInfo = sensorsDataAccess.GetSensor(SensorNames[0]) ?? new Sensor(24, 36, 24),
                LensInfo = new() { Aperture = Apertures[8], FocalLengthMM = 50 }
            },
            VisualAcuityLpPerMM = 5
        };

        SetToggleText();
        CalculateValues();
    }

    [RelayCommand]
    private void CalculateValues()
    {
        Sensor? sensor = sensorsDataAccess.GetSensor(UserInput.SelectedSensorName);

        if (sensor is null)
        {
            messageService.ShowMessageAndForget("Select sensor", "Select correct sensor and try again", "Ok");
            return;
        }

        UserInput.DofCalcInput.CameraInfo = sensor;
        DofCalcResult = photographyCalcService.CalculateDofValues(UserInput.DofCalcInput);
    }

    [RelayCommand]
    private void OnVisualAcuityLpPerMMChanged()
    {
        if (UserInput.VisualAcuityLpPerMM < VisualAcuityMin)
        {
            UserInput.VisualAcuityLpPerMM = VisualAcuityMin;
            OnPropertyChanged(nameof(UserInput));
        }
        else if (UserInput.VisualAcuityLpPerMM > VisualAcuityMax)
        {
            UserInput.VisualAcuityLpPerMM = VisualAcuityMax;
            OnPropertyChanged(nameof(UserInput));
        }

        if (UserInput.DofCalcInput.VisualAcuityLpPerMM != UserInput.VisualAcuityLpPerMM)
        {
            UserInput.DofCalcInput.VisualAcuityLpPerMM = UserInput.VisualAcuityLpPerMM;
            CalculateValues();
        }
    }

    [RelayCommand]
    private void ChangeMode()
    {
        UserInput.IsAdvancedModeEnabled = !UserInput.IsAdvancedModeEnabled;
        OnPropertyChanged(nameof(UserInput));
        SetToggleText();
    }

    private void SetToggleText()
    {
        ToggleText = UserInput.IsAdvancedModeEnabled ? "Simple mode" : "Advanced mode";
    }

    protected override void SaveUserInput()
    {
        if (UserInput.Validate())
            preferencesService.SerializeAndSetPreference(PreferencesKeys.DofCalcUserInputPreferencesKey, UserInput);
    }
}