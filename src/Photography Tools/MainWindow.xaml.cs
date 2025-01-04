namespace Photography_Tools;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Page = new AppShell();

        AppTitleBar.Subtitle = AppInfo.Current.VersionString;
#if DEBUG
        AppTitleBar.BackgroundColor = Color.FromRgb(144, 105, 42);
        AppTitleBar.ForegroundColor = Color.FromRgb(0, 0, 0);
#endif
    }
}