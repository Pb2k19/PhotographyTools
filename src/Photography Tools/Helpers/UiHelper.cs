namespace Photography_Tools.Helpers;

public static class UiHelper
{
    public static Page? GetMainPage()
    {
        if (Application.Current is null || Application.Current.Windows.Count < 1)
            return null;

        return Application.Current.Windows[0].Page;
    }
}
