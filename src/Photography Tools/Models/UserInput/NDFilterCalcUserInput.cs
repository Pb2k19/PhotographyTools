using System.Collections.ObjectModel;

namespace Photography_Tools.Models.UserInput;

public class NDFilterCalcUserInput : UserInput
{
    public required string TimeText { get; set; }
    public required ObservableCollection<NDFilter> NdFilters { get; set; }

    public override bool Validate()
    {
        return !string.IsNullOrWhiteSpace(TimeText) && NdFilters is not null;
    }
}