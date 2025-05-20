namespace Photography_Tools;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        AppVersionLabel.Text = $"{AppInfo.Current.VersionString} BETA";

#if DEBUG
        AppVersionLabel.Text += " DEBUG";
#endif
    }
}
