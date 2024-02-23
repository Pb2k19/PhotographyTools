using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Photography_Tools.DataAccess.NDFiltersData;

public class StaticNDFiltersDataAccess : INDFiltersDataAccess
{
    private static readonly FrozenDictionary<string, NDFilter> filters;

    static StaticNDFiltersDataAccess() 
    {
        filters = new Dictionary<string, NDFilter>
        {
            { "ND2", new NDFilter { Name="ND2", OpticalDensity = 0.3, Factor = 2, FStopReduction = 1 } },
            { "ND4", new NDFilter { Name="ND4", OpticalDensity = 0.6, Factor = 4, FStopReduction = 2 } },
            { "ND8", new NDFilter { Name="ND8", OpticalDensity = 0.9, Factor = 8, FStopReduction = 3 } },
            { "ND16", new NDFilter { Name="ND16", OpticalDensity = 1.2, Factor = 16, FStopReduction = 4 } },
            { "ND32", new NDFilter { Name="ND32", OpticalDensity = 1.5, Factor = 32, FStopReduction = 5 } },
            { "ND64", new NDFilter { Name="ND64", OpticalDensity = 1.8, Factor = 64, FStopReduction = 6 } },
            { "ND128", new NDFilter { Name="ND128", OpticalDensity = 2.1, Factor = 128, FStopReduction = 7 } },
            { "ND256", new NDFilter { Name="ND256", OpticalDensity = 2.4, Factor = 256, FStopReduction = 8 } },
            { "ND512", new NDFilter { Name="ND512", OpticalDensity = 2.7, Factor = 512, FStopReduction = 9 } },
            { "ND1024", new NDFilter { Name="ND1024", OpticalDensity = 3.0, Factor = 1024, FStopReduction = 10 } },
            { "ND2048", new NDFilter { Name="ND2048", OpticalDensity = 3.3, Factor = 2048, FStopReduction = 11 } },
            { "ND4096", new NDFilter { Name="ND4096", OpticalDensity = 3.6, Factor = 4096, FStopReduction = 12 } },
            { "ND8192", new NDFilter { Name="ND8192", OpticalDensity = 3.9, Factor = 8192, FStopReduction = 13 } },
            { "ND16384", new NDFilter { Name="ND16384", OpticalDensity = 4.2, Factor = 16384, FStopReduction = 14 } },
            { "ND32768", new NDFilter { Name="ND32768", OpticalDensity = 4.5, Factor = 32768, FStopReduction = 15 } },
            { "ND65536", new NDFilter { Name="ND65536", OpticalDensity = 4.8, Factor = 65536, FStopReduction = 16 } },
            { "ND131072", new NDFilter { Name="ND131072", OpticalDensity = 5.1, Factor = 131072, FStopReduction = 17 } },
            { "ND262144", new NDFilter { Name="ND262144", OpticalDensity = 5.4, Factor = 262144, FStopReduction = 18 } },
            { "ND524288", new NDFilter { Name="ND524288", OpticalDensity = 5.7, Factor = 524288, FStopReduction = 19 } },
            { "ND1048576", new NDFilter { Name="ND1048576", OpticalDensity = 6.0, Factor = 1048576, FStopReduction = 20 } },
            { "ND2097152", new NDFilter { Name="ND2097152", OpticalDensity = 6.3, Factor = 2097152, FStopReduction = 21 } },
            { "ND4194304", new NDFilter { Name="ND4194304", OpticalDensity = 6.6, Factor = 4194304, FStopReduction = 22 } },
            { "ND8388608", new NDFilter { Name="ND8388608", OpticalDensity = 6.9, Factor = 8388608, FStopReduction = 23 } },
            { "ND16777216", new NDFilter { Name="ND16777216", OpticalDensity = 7.2, Factor = 16777216, FStopReduction = 24 } }
        }.ToFrozenDictionary();
    }

    public ImmutableArray<string> GetFilterNames() => filters.Keys;

    public ImmutableArray<NDFilter> GetFilters() => filters.Values;

    public NDFilter GetFilter(string name) => filters[name];
}