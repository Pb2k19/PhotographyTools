using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.Activity;

namespace Photography_Tools.Platforms.Android;

internal sealed class BackPress : OnBackPressedCallback
{
    private const int delay = 3000;
    private readonly Activity activity;
    private long backPressed;

    public BackPress(Activity activity) : base(true)
    {
        this.activity = activity;
    }

    public override void HandleOnBackPressed()
    {
        INavigation? navigation = Microsoft.Maui.Controls.Application.Current?.Windows[0]?.Navigation;

        if (navigation is not null && navigation.NavigationStack.Count <= 1 && navigation.ModalStack.Count <= 0)
        {
            if (backPressed + delay > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                activity.FinishAndRemoveTask();
                Process.KillProcess(Process.MyPid());
            }
            else
            {
                Toast.MakeText(activity, "Press back again to exit", ToastLength.Short)?.Show();
                backPressed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
    }
}
