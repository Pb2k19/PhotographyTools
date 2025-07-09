namespace Photography_Tools;

public partial class MainWindow : Window
{
#if WINDOWS
    //tmp
    public static MainWindow Current = new();

    private ITitleBar? defaultTitleBar;
#endif

    public MainWindow()
    {
        InitializeComponent();
        Page = new AppShell();

        AppTitleBar.Subtitle = AppInfo.Current.VersionString;
#if DEBUG
        AppTitleBar.BackgroundColor = Color.FromRgb(144, 105, 42);
        AppTitleBar.ForegroundColor = Color.FromRgb(0, 0, 0);
#endif

#if WINDOWS
        //tmp
        defaultTitleBar = AppTitleBar;
#endif
    }

#if WINDOWS
    //tmp
    public void SetTitleBarNull()
    {
        if (TitleBar is null)
            return;

        TitleBar = null;
    }

    //tmp
    public void ShowTitleBar()
    {
        TitleBar = defaultTitleBar;
    }
#endif
}