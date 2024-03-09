using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Photography_Tools.Const;

public static class ApertureConst
{
    public const int FullStopsMultiplier = 2, SecondStopsMultiplier = 4, ThirdStopsMultiplier = 6, Undefined = -1;

    public static FrozenSet<double> FullStops { get; }

    public static FrozenSet<double> SecondStops { get; }

    public static FrozenSet<double> ThirdStops { get; }

    public static FrozenSet<double> OtherStops { get; }

    public static ImmutableArray<double> AllStops { get; }

    static ApertureConst()
    {
        FullStops = new double[]
        { 1.0, 1.4, 2.0, 2.8, 4.0, 5.6, 8.0, 11.0, 16.0, 22.0, 32.0 }.ToFrozenSet();
        SecondStops = new double[]
        {
            1.0, 1.2, 1.4, 1.7, 2.0, 2.4, 2.8, 3.3, 4.0, 4.8, 5.6,
            6.7, 8.0, 9.5, 11.0, 13.0, 16.0, 19.0, 22.0, 27.0, 32.0
        }.ToFrozenSet();
        ThirdStops = new double[]
        {
            1.0, 1.1, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.5, 2.8, 3.2, 3.5, 4.0,
            4.5, 5.0, 5.6, 6.3, 7.1, 8.0, 9.0, 10.0, 11.0, 13.0, 14.0, 16.0,
            18.0, 20.0, 22.0, 25.0, 29.0, 32.0, 36.0
        }.ToFrozenSet();
        OtherStops = new double[]
        { 0.95, 1.05 }.ToFrozenSet();

        int arraySize = FullStops.Count + SecondStops.Count + ThirdStops.Count;
        Span<double> allStops = arraySize * sizeof(double) <= 512 ? stackalloc double[arraySize] : new double[arraySize];
        allStops.Clear();

        int insertedValues = ThirdStops.Count;
        ThirdStops.CopyTo(allStops);

        AddStops(allStops, SecondStops, ref insertedValues);
        AddStops(allStops, FullStops, ref insertedValues);
        AddStops(allStops, OtherStops, ref insertedValues);

        allStops = allStops[..insertedValues];
        allStops.Sort();

        AllStops = allStops.ToImmutableArray();
    }

    private static void AddStops(Span<double> destination, FrozenSet<double> source, ref int insertedValue)
    {
        foreach (double value in source)
        {
            if (destination[..insertedValue].Contains(value))
                continue;

            destination[insertedValue] = value;
            insertedValue++;
        }
    }
}