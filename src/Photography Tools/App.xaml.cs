namespace Photography_Tools;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
#if WINDOWS
        //tmp
        return MainWindow.Current;
#else
        return new MainWindow();
#endif
    }
}
