namespace Photography_Tools;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell())
        {
            MinimumHeight = 600,
            MinimumWidth = 800
        };
    }
}
