using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class MoonPhaseViewModel : ObservableObject
{
    private static readonly ImmutableArray<string> MoonImages = ["🌑", "🌒", "🌓", "🌔", "🌕", "🌖", "🌗", "🌘", "🌑"];

    private static readonly ImmutableArray<Models.Phase> AllMoonPhases;

    private TimeSpan lastSelectedTime = TimeSpan.Zero;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan selectedTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private string moonPhaseName = string.Empty, moonImage = string.Empty;

    [ObservableProperty]
    private int illuminationPerc = 0;

    [ObservableProperty]
    private double moonAge = 0.0;

    [ObservableProperty]
    private bool isNorthernHemisphere = true;

    static MoonPhaseViewModel()
    {
        int numberOfPhases = AstroConst.AllMoonPhases.Length;
        double phaseLength = AstroConst.SynodicMonthLength / numberOfPhases;
        Models.Phase[] phases = new Models.Phase[numberOfPhases];

        for (int i = 0; i < numberOfPhases; i++)
        {
            phases[i] = new Models.Phase { Name = AstroConst.AllMoonPhases[i], Start = phaseLength * i, End = phaseLength * (i + 1) };
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
        (double fraction, double phase, _) = AstroHelper.CalculateMoonPhase(SelectedDate.Date.Add(SelectedTime).ToUniversalTime());

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
        IlluminationPerc = (int)Math.Round(fraction * 100, 0);
        MoonAge = Math.Round(phase, 2);
    }
}