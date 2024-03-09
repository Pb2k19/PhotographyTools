using System.Collections.Immutable;

namespace Photography_Tools.DataAccess.NDFiltersData;

public interface INDFiltersDataAccess
{
    ImmutableArray<string> GetFilterNames();
    ImmutableArray<NDFilter> GetFilters();
    NDFilter GetFilter(string name);
}