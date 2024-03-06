namespace Photography_Tools.ViewModels;

public abstract partial class SaveableViewModel : ObservableObject
{
    protected readonly IPreferencesService preferencesService;

    public SaveableViewModel(IPreferencesService preferencesService)
    {
        this.preferencesService = preferencesService;
    }

    [RelayCommand]
    protected virtual void OnDisappearing()
    {
        SaveUserInput();
    }

    protected abstract void SaveUserInput();
}