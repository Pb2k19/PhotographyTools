using System.Collections.ObjectModel;

namespace Photography_Tools.Models
{
    public class NDFilterCalcUserInput
    {
        public required string TimeText { get; set; }
        public required ObservableCollection<NDFilter> NdFilters { get; set; }
    }
}