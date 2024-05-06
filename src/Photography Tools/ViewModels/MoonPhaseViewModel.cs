using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class MoonPhaseViewModel : ObservableObject
{
    private static readonly ImmutableArray<string> MoonImages = ["🌑", "🌒", "🌓", "🌔", "🌕", "🌖", "🌗", "🌘", "🌑"];

    private static readonly ImmutableArray<AstroPhase> AllMoonPhases;

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
        (double fraction, double phase, _) = AstroHelper.CalculateMoonPhase(SelectedDate.Date.Add(SelectedTime));

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

        MoonRiseAndSet = AstroHelper.CalculateMoonRiseAndDown(DateTime.Now, 52.232222, 21.008333); //tmp location
    }

    private void SetIlluminationPerc<T>(T value) => IlluminationPerc = $"{value}%";

    private void SetMoonAge(double value) => MoonAge = value < 2 ? $"{value} day" : $"{value} days";
}