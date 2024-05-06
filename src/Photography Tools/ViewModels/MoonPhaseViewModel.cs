using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class MoonPhaseViewModel : ObservableObject
{
    private static readonly ImmutableArray<string> MoonImages = ["🌑", "🌒", "🌓", "🌔", "🌕", "🌖", "🌗", "🌘", "🌑"];

    private static readonly ImmutableArray<AstroPhase> AllMoonPhases;

    private readonly IAstroDataService astroDataService;

    private TimeSpan lastSelectedTime = TimeSpan.Zero;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan selectedTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMoonCyclicallyVisible))]
    private RiseAndSetResult moonRiseAndSet;

    [ObservableProperty]
    private string moonPhaseName = string.Empty, moonImage = string.Empty, illuminationPerc = string.Empty, moonAge = string.Empty;

    [ObservableProperty]
    private bool isNorthernHemisphere = true;

    public bool IsMoonCyclicallyVisible => !(MoonRiseAndSet.AlwaysUp || MoonRiseAndSet.AlwaysDown);

    static MoonPhaseViewModel()
    {
        int numberOfPhases = AstroConst.AllMoonPhases.Length;
        double phaseLength = AstroConst.SynodicMonthLength / numberOfPhases;
        AstroPhase[] phases = new AstroPhase[numberOfPhases];

        for (int i = 0; i < numberOfPhases; i++)
        {
            phases[i] = new AstroPhase { Name = AstroConst.AllMoonPhases[i], Start = phaseLength * i, End = phaseLength * (i + 1) };
        }

        AllMoonPhases = [.. phases];
    }

    public MoonPhaseViewModel(IAstroDataService astroDataService)
    {
        this.astroDataService = astroDataService;
    }

    [RelayCommand]
    private void OnSelectedTimeChanged()
    {
        if (lastSelectedTime != SelectedTime)
        {
            lastSelectedTime = SelectedTime;
            Calculate();
        }
    }

    [RelayCommand]
    private void Calculate()
    {
        MoonDataResult result = astroDataService.GetMoonData(SelectedDate.Date.Add(SelectedTime), 52.23, 21.01); //tmp location

        if (!result.IsSuccess || result.RiseAndSet is null || result.Phase is null)
            return;

        (double fraction, double phase, _) = result.Phase.Value;
        int index = -1;
        for (int i = 0; i < AllMoonPhases.Length; i++)
        {
            if (phase >= AllMoonPhases[i].Start && phase <= AllMoonPhases[i].End)
            {
                index = i;
                break;
            }
        }

        MoonPhaseName = AllMoonPhases[index].Name;
        MoonImage = MoonImages[IsNorthernHemisphere ? index : ^(index + 1)];
        SetIlluminationPerc(Math.Round(fraction * 100, 0));
        SetMoonAge(Math.Round(phase, 2));

        MoonRiseAndSet = result.RiseAndSet.Value;
    }

    private void SetIlluminationPerc<T>(T value) => IlluminationPerc = $"{value}%";

    private void SetMoonAge(double value) => MoonAge = value < 2 ? $"{value} day" : $"{value} days";
}