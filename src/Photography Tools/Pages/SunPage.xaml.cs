namespace Photography_Tools.Pages;

public partial class SunPage : ContentPage
{
    public SunPage(SunViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}