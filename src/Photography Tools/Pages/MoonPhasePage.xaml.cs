namespace Photography_Tools.Pages;

public partial class MoonPhasePage : ContentPage
{
    public MoonPhasePage(MoonPhaseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}