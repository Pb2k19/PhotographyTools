using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class AstroTimeCalcViewModel : ObservableObject
{
    private readonly IPhotographyCalculationsService photographyCalcService;
    private readonly ISensorsDataAccess sensorsDataAccess;

    [ObservableProperty]
    private Lens lens;

    [ObservableProperty]
    private double rule200, rule300, rule500, npfRuleH, npfRuleM, npfRuleS, decilination;

    [ObservableProperty]
    private string selectedSensorName;

    public ImmutableArray<double> Apertures { get; }

    public ImmutableArray<string> SensorNames { get; }

    public AstroTimeCalcViewModel(IPhotographyCalculationsService photographyCalculationsService, ISensorsDataAccess sensorsDataAccess)
    {
        photographyCalcService = photographyCalculationsService;
        this.sensorsDataAccess = sensorsDataAccess;
        Apertures = ApertureConst.AllStops;
        SensorNames = sensorsDataAccess.GetSensorNames();

        selectedSensorName = SensorNames[0];
        lens = new() { Aperture = Apertures[8], FocalLengthMM = 20 };
        CalculateAllValues();
    }

    [RelayCommand]
    private void OnUnitChanged()
    {

    }

    [RelayCommand]
    private void CalculateAllValues()
    {
        CalculateTimeForAstro();
        CalculateTimeForAstroWithNpfRule();
    }

    [RelayCommand]
    private void OnDecilinationTextChanged()
    {
        if (Decilination > 90)
            Decilination = 90;
        else if (Decilination < -90)
            Decilination = -90;

        CalculateTimeForAstroWithNpfRule();
    }

    [RelayCommand]
    private void CalculateTimeForAstroWithNpfRule()
    {
        NpfRuleH = Math.Round(photographyCalcService.CalculateTimeForAstroWithNPFRule(sensorsDataAccess.GetSensor(SelectedSensorName), Lens, 1, Decilination), 3);
        NpfRuleM = Math.Round(photographyCalcService.CalculateTimeForAstroWithNPFRule(sensorsDataAccess.GetSensor(SelectedSensorName), Lens, 2, Decilination), 3);
        NpfRuleS = Math.Round(photographyCalcService.CalculateTimeForAstroWithNPFRule(sensorsDataAccess.GetSensor(SelectedSensorName), Lens, 3, Decilination), 3);
    }

    private void CalculateTimeForAstro()
    {
        (double r200, double r300, double r500) = photographyCalcService.CalculateTimeForAstro(sensorsDataAccess.GetSensor(SelectedSensorName).CropFactor, Lens.FocalLengthMM);
        Rule200 = Math.Round(r200, 3);
        Rule300 = Math.Round(r300, 3);
        Rule500 = Math.Round(r500, 3);
    }
}