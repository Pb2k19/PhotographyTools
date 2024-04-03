namespace Photography_Tools.Pages;

public partial class TimeLapseCalcPage : ContentPage
{
    public TimeLapseCalcPage(TimeLapseCalculatorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}