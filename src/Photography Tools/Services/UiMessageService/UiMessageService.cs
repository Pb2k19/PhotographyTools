namespace Photography_Tools.Services.UiMessageService;

public class UiMessageService : IUiMessageService
{
    public async Task ShowMessageAsync(string title, string message, string cancel = "Cancel")
    {
        if (Application.Current?.MainPage is null)
            return;

        if (MainThread.IsMainThread)
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        else
            MainThread.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert(title, message, cancel); });
    }

    public async Task<bool> ShowMessageAsync(string title, string message, string accept = "Ok", string cancel = "Cancel")
    {
        if (Application.Current?.MainPage is null)
            return false;

        if (MainThread.IsMainThread)
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);

        bool result = false;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            result = await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        });

        return result;
    }

    public void ShowMessageAndForget(string title, string message, string cancel = "Cancel")
    {
        if (Application.Current?.MainPage is null)
            return;

        Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            await ShowMessageAsync(title, message, cancel)
        );
    }
}
