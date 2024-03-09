namespace Photography_Tools;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

#if WINDOWS
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        window.MinimumHeight = 600;
        window.MinimumWidth = 800;

        return window;
    }
#endif
}
