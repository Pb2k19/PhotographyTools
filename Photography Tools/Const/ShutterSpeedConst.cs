using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Photography_Tools.Const;

public class ShutterSpeedConst
{
    public static FrozenDictionary<string, double> AllShutterSpeeds { get; }

    public static ImmutableArray<string> AllShutterSpeedsNamesSorted { get; }

    static ShutterSpeedConst()
    {
        AllShutterSpeeds = new Dictionary<string, double>()
        {
            { "1/32000s", 1.0 / 32000 },
            { "1/25000s", 1.0 / 25000 },
            { "1/20000s", 1.0 / 20000 },
            { "1/16000s", 1.0 / 16000 },
            { "1/12500s", 1.0 / 12500 },
            { "1/10000s", 1.0 / 10000 },
            { "1/8000s", 1.0 / 8000 },
            { "1/6400s", 1.0 / 6400 },
            { "1/5000s", 1.0 / 5000 },
            { "1/4000s", 1.0 / 4000 },
            { "1/3200s", 1.0 / 3200 },
            { "1/2500s", 1.0 / 2500 },
            { "1/2000s", 1.0 / 2000 },
            { "1/1600s", 1.0 / 1600 },
            { "1/1250s", 1.0 / 1250 },
            { "1/1000s", 1.0 / 1000 },
            { "1/800s", 1.0 / 800 },
            { "1/640s", 1.0 / 640 },
            { "1/500s", 1.0 / 500 },
            { "1/400s", 1.0 / 400 },
            { "1/320s", 1.0 / 320 },
            { "1/250s", 1.0 / 250 },
            { "1/200s", 1.0 / 200 },
            { "1/160s", 1.0 / 160 },
            { "1/125s", 1.0 / 125 },
            { "1/100s", 1.0 / 100 },
            { "1/80s", 1.0 / 80 },
            { "1/60s", 1.0 / 60 },
            { "1/50s", 1.0 / 50 },
            { "1/40s", 1.0 / 40 },
            { "1/30s", 1.0 / 30 },
            { "1/25s", 1.0 / 25 },
            { "1/20s", 1.0 / 20 },
            { "1/15s", 1.0 / 15 },
            { "1/13s", 1.0 / 13 },
            { "1/10s", 1.0 / 10 },
            { "1/8s", 1.0 / 8 },
            { "1/6s", 1.0 / 6 },
            { "1/5s", 1.0 / 5 },
            { "1/4s", 1.0 / 4 },
            { "1/3s", 1.0 / 3 },
            { "1/2.5s", 1.0 / 2.5 },
            { "1/2s", 1.0 / 2 },
            { "1/1.6s", 1.0 / 1.6 },
            { "1/1.3s", 1.0 / 1.3 },
            { "1s", 1.0 },
            { "1.3s", 1.3 },
            { "1.6s", 1.6 },
            { "2s", 2.0 },
            { "2.5s", 2.5 },
            { "3s", 3.0 },
            { "4s", 4.0 },
            { "5s", 5.0 },
            { "6s", 6.0 },
            { "8s", 8.0 },
            { "10s", 10.0 },
            { "13s", 13.0 },
            { "15s", 15.0 },
            { "20s", 20.0 },
            { "25s", 25.0 },
            { "30s", 30.0 },
            { "40s", 40.0 },
            { "50s", 50.0 },
            { "60s", 60.0 }
        }.ToFrozenDictionary();
        AllShutterSpeedsNamesSorted = AllShutterSpeeds.OrderBy(s => s.Value).Select(s => s.Key).ToImmutableArray();
    }
}