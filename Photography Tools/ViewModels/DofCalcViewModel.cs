using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class DofCalcViewModel : ObservableObject
{
    private readonly IPhotographyCalculationsService photographyCalcService;
    private readonly ISensorsDataAccess sensorsDataAccess;

    [ObservableProperty]
    private int visualAcuityLpPerMM;

    [ObservableProperty]
    private bool isAdvancedModeEnabled;

    [ObservableProperty]
    private string selectedSensorName, toggleText = string.Empty;

    [ObservableProperty]
    private DofCalcResult dofCalcResult;

    [ObservableProperty]
    private DofCalcInput dofCalcInput;

    public ImmutableArray<double> Apertures { get; }

    public ImmutableArray<string> SensorNames { get; }

    public DofCalcViewModel(IPhotographyCalculationsService photographyCalcService, ISensorsDataAccess sensorsDataAccess)
    {
        this.photographyCalcService = photographyCalcService;
        this.sensorsDataAccess = sensorsDataAccess;

        Apertures = ApertureConst.AllStops;
        SensorNames = sensorsDataAccess.GetSensorNames();
        SelectedSensorName = SensorNames[0];

        DofCalcResult = new();
        DofCalcInput = new() { CameraInfo = sensorsDataAccess.GetSensor(SelectedSensorName), LensInfo = new() { Aperture = Apertures[8], FocalLengthMM = 50 }, FocusingDistanceMM = 500 };

        SetToggleText();

        CalculateValues();
    }

    [RelayCommand]
    private void CalculateValues()
    {
        DofCalcInput.CameraInfo = sensorsDataAccess.GetSensor(SelectedSensorName);
        DofCalcResult = photographyCalcService.CalculateDofValues(DofCalcInput);
    }

    [RelayCommand]
    private void OnVisualAcuityLpPerMMChanged()
    {
        if (DofCalcInput.VisualAcuityLpPerMM != VisualAcuityLpPerMM)
        {
            DofCalcInput.VisualAcuityLpPerMM = VisualAcuityLpPerMM;
            CalculateValues();
        }

        OnPropertyChanged(nameof(VisualAcuityLpPerMM));
    }

    [RelayCommand]
    private void ChangeMode()
    {
        IsAdvancedModeEnabled = !IsAdvancedModeEnabled;
        SetToggleText();
    }

    [RelayCommand]
    private void OnUnitChanged()
    {

    }

    private void SetToggleText()
    {
        ToggleText = IsAdvancedModeEnabled ? "Simple mode" : "Advanced mode";
    }
}