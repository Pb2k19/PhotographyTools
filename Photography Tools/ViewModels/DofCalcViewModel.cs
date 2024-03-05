using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class DofCalcViewModel : ObservableObject
{
    private const int VisualAcuityMin = 1, VisualAcuityMax = 100;

    private readonly IPhotographyCalculationsService photographyCalcService;
    private readonly IPreferencesService preferencesService;
    private readonly ISensorsDataAccess sensorsDataAccess;

    [ObservableProperty]
    private string toggleText = string.Empty;

    [ObservableProperty]
    private DofCalcResult dofCalcResult;

    [ObservableProperty]
    private DofCalcUserInput dofCalcUserInput;

    public ImmutableArray<double> Apertures { get; }

    public ImmutableArray<string> SensorNames { get; }

    public DofCalcViewModel(IPhotographyCalculationsService photographyCalcService, IPreferencesService preferencesService, ISensorsDataAccess sensorsDataAccess)
    {
        this.photographyCalcService = photographyCalcService;
        this.preferencesService = preferencesService;
        this.sensorsDataAccess = sensorsDataAccess;

        Apertures = ApertureConst.AllStops;
        SensorNames = sensorsDataAccess.GetSensorNames();
        DofCalcResult = new();

        DofCalcUserInput? userInput = preferencesService.GetDeserailizedPreference<DofCalcUserInput>(PreferencesKeys.DofCalcUserInputPreferencesKey);
        DofCalcUserInput = userInput is not null ? userInput : new()
        {
            SelectedSensorName = SensorNames[0],
            DofCalcInput = new()
            {
                FocusingDistanceMM = 500,
                CameraInfo = sensorsDataAccess.GetSensor(SensorNames[0]),
                LensInfo = new() { Aperture = Apertures[8], FocalLengthMM = 50 }
            },
            VisualAcuityLpPerMM = 5
        };

        SetToggleText();
        CalculateValues();
    }

    [RelayCommand]
    private void OnDisappearing()
    {
        preferencesService.SerializedAndSetPreference(PreferencesKeys.DofCalcUserInputPreferencesKey, DofCalcUserInput);
    }

    [RelayCommand]
    private void CalculateValues()
    {
        DofCalcUserInput.DofCalcInput.CameraInfo = sensorsDataAccess.GetSensor(DofCalcUserInput.SelectedSensorName);
        DofCalcResult = photographyCalcService.CalculateDofValues(DofCalcUserInput.DofCalcInput);
    }

    [RelayCommand]
    private void OnVisualAcuityLpPerMMChanged()
    {
        if (DofCalcUserInput.VisualAcuityLpPerMM < VisualAcuityMin)
        {
            DofCalcUserInput.VisualAcuityLpPerMM = VisualAcuityMin;
            OnPropertyChanged(nameof(DofCalcUserInput));
        }
        else if (DofCalcUserInput.VisualAcuityLpPerMM > VisualAcuityMax)
        {
            DofCalcUserInput.VisualAcuityLpPerMM = VisualAcuityMax;
            OnPropertyChanged(nameof(DofCalcUserInput));
        }

        if (DofCalcUserInput.DofCalcInput.VisualAcuityLpPerMM != DofCalcUserInput.VisualAcuityLpPerMM)
        {
            DofCalcUserInput.DofCalcInput.VisualAcuityLpPerMM = DofCalcUserInput.VisualAcuityLpPerMM;
            CalculateValues();
        }
    }

    [RelayCommand]
    private void ChangeMode()
    {
        DofCalcUserInput.IsAdvancedModeEnabled = !DofCalcUserInput.IsAdvancedModeEnabled;
        OnPropertyChanged(nameof(DofCalcUserInput));
        SetToggleText();
    }

    private void SetToggleText()
    {
        ToggleText = DofCalcUserInput.IsAdvancedModeEnabled ? "Simple mode" : "Advanced mode";
    }
}