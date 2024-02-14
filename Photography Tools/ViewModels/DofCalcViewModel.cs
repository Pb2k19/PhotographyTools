using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Photography_Tools.Const;
using Photography_Tools.Models;
using Photography_Tools.Services.PhotographyCalculationsService;
using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class DofCalcViewModel : ObservableObject
{
    private readonly IPhotographyCalculationsService photographyCalcService;

    [ObservableProperty]
    private int visualAcuityLpPerMM;

    [ObservableProperty]
    private bool isAdvancedModeEnabled;

    [ObservableProperty]
    private string selectedSensorName, toggleText = string.Empty;

    [ObservableProperty]
    private DofCalcResult dofCalcResult = new();

    [ObservableProperty]
    private DofCalcInput dofCalcInput = new() { CameraInfo = new(36, 24, 24.3), LensInfo = new() };

    public ImmutableArray<double> Apertures { get; } = ApertureConst.AllStops;

    public ImmutableArray<string> SensorNames { get; } = SensorConst.Sensors.Keys;

    public DofCalcViewModel(IPhotographyCalculationsService photographyCalcService)
    {
        this.photographyCalcService = photographyCalcService;

        SelectedSensorName = SensorNames.Length > 0 ? SensorNames[0] : string.Empty;
        DofCalcInput.LensInfo.Aperture = Apertures.Length > 8 ? Apertures[8] : 2.0;
        DofCalcInput.LensInfo.FocalLengthMM = 50;
        DofCalcInput.FocusingDistanceMM = 50;

        SetToggleText();

        CalculateValues();
    }

    [RelayCommand]
    private void CalculateValues()
    {
        DofCalcInput.CameraInfo = SensorConst.Sensors[SelectedSensorName];

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