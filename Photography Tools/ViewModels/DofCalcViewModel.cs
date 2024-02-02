using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Photography_Tools.Models;
using Photography_Tools.Services.DofService;

namespace Photography_Tools.ViewModels;

public partial class DofCalcViewModel : ObservableObject
{
    private readonly IDofService dofService;

    private readonly DofInfo dofInfo;

    [ObservableProperty]
    private double focalLength, aperture;

    [ObservableProperty]
    private bool isAdvancedModeEnabled = false;

    public DofCalcViewModel(IDofService dofService)
    {
        this.dofService = dofService;

        dofInfo = new()
        {
            CameraInfo = new()
            {
                SensorHeightMM = 36,
                SensorWidthMM = 24
            },
            LensInfo = new()
            {
                Aperture = 2.5,
                FocalLengthMM = 90
            }
        };
    }

    [RelayCommand]
    private void CalculateValues()
    {
        dofService.CalculateDofValues(dofInfo);
    }

    [RelayCommand]
    private void OnUnitChanged()
    {

    }
}