using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Photography_Tools.Const;
using Photography_Tools.Models;
using Photography_Tools.Services.DofService;
using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class DofCalcViewModel : ObservableObject
{
    private readonly IDofService dofService;

    [ObservableProperty]
    private bool isAdvancedModeEnabled = false;

    [ObservableProperty]
    private string selectedSensorName;

    [ObservableProperty]
    private DofCalcResult dofCalcResult = new();

    [ObservableProperty]
    private DofCalcInput dofCalcInput = new() { CameraInfo = new(), LensInfo = new() };

    public ImmutableArray<double> Apertures { get; } = ApertureConst.AllStops;

    public ImmutableArray<string> SensorNames { get; } = SensorConst.Sensors.Keys;

    public DofCalcViewModel(IDofService dofService)
    {
        this.dofService = dofService;

        SelectedSensorName = SensorNames.Length > 0 ? SensorNames[0] : string.Empty;
        DofCalcInput.LensInfo.Aperture = Apertures.Length > 8 ? Apertures[8] : 2.0;
        DofCalcInput.LensInfo.FocalLengthMM = 50;
        DofCalcInput.FocusingDistanceMM = 50;

        CalculateValues();
    }

    [RelayCommand]
    private void CalculateValues()
    {
        DofCalcInput.CameraInfo.SensorWidthMM = SensorConst.Sensors[SelectedSensorName].SensorWidthMM;
        DofCalcInput.CameraInfo.SensorHeightMM = SensorConst.Sensors[SelectedSensorName].SensorHeightMM;

        DofCalcResult = dofService.CalculateDofValues(DofCalcInput);
    }

    [RelayCommand]
    private void OnUnitChanged()
    {

    }
}