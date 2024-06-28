using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class AstroTimeCalcViewModel : SaveableViewModel
{
    private readonly IPhotographyCalculationsService photographyCalcService;
    private readonly ISensorsDataAccess sensorsDataAccess;

    [ObservableProperty]
    private AstroTimeCalcUserInput userInput;

    [ObservableProperty]
    private double rule200, rule300, rule500, npfRuleH, npfRuleM, npfRuleS;

    public ImmutableArray<double> Apertures { get; } = ApertureConst.AllStops;

    public ImmutableArray<string> SensorNames { get; }

    public AstroTimeCalcViewModel(IPhotographyCalculationsService photographyCalculationsService, IPreferencesService preferencesService, ISensorsDataAccess sensorsDataAccess) : base(preferencesService)
    {
        photographyCalcService = photographyCalculationsService;
        this.sensorsDataAccess = sensorsDataAccess;

        SensorNames = sensorsDataAccess.GetSensorNames();

        AstroTimeCalcUserInput? input = preferencesService?.GetDeserailizedPreference<AstroTimeCalcUserInput>(PreferencesKeys.AstroTimeCalcUserInputPreferencesKey);

        userInput = input is not null && input.Validate() ? input : new()
        {
            SelectedSensorName = SensorNames[0],
            Lens = new() { Aperture = Apertures[8], FocalLengthMM = 20 }
        };

        CalculateAllValues();
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
        if (UserInput.Decilination > 90)
            UserInput.Decilination = 90;
        else if (UserInput.Decilination < -90)
            UserInput.Decilination = -90;

        CalculateTimeForAstroWithNpfRule();
    }

    [RelayCommand]
    private void CalculateTimeForAstroWithNpfRule()
    {
        NpfRuleH = Math.Round(photographyCalcService.CalculateTimeForAstroWithNPFRule(sensorsDataAccess.GetSensor(UserInput.SelectedSensorName), UserInput.Lens, 1, UserInput.Decilination), 3);
        NpfRuleM = Math.Round(photographyCalcService.CalculateTimeForAstroWithNPFRule(sensorsDataAccess.GetSensor(UserInput.SelectedSensorName), UserInput.Lens, 2, UserInput.Decilination), 3);
        NpfRuleS = Math.Round(photographyCalcService.CalculateTimeForAstroWithNPFRule(sensorsDataAccess.GetSensor(UserInput.SelectedSensorName), UserInput.Lens, 3, UserInput.Decilination), 3);
    }

    private void CalculateTimeForAstro()
    {
        (double r200, double r300, double r500) = photographyCalcService.CalculateTimeForAstro(sensorsDataAccess.GetSensor(UserInput.SelectedSensorName).CropFactor, UserInput.Lens.FocalLengthMM);
        Rule200 = Math.Round(r200, 3);
        Rule300 = Math.Round(r300, 3);
        Rule500 = Math.Round(r500, 3);
    }

    protected override void SaveUserInput()
    {
        if (UserInput.Validate())
            preferencesService?.SerializedAndSetPreference(PreferencesKeys.AstroTimeCalcUserInputPreferencesKey, UserInput);
    }
}