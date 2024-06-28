
namespace Photography_Tools.Services.UiMessageService;

public interface IUiMessageService
{
    void ShowMessageAndForget(string title, string message, string cancel = "Cancel");
    Task ShowMessageAsync(string title, string message, string cancel = "Cancel");
    Task<bool> ShowMessageAsync(string title, string message, string accept = "Ok", string cancel = "Cancel");
}
