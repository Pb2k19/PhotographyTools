using CommunityToolkit.Maui.Views;
using Photography_Tools.Services.KeyValueStoreService;
using System.Collections.ObjectModel;

namespace Photography_Tools.Components.Popups;

public sealed partial class LocationPopup : Popup<Place?>, IQueryAttributable, IDisposable
{
    private readonly IKeyValueStore<Place> locationsStore;
    private readonly IUiMessageService messageService;

    bool isFirstOpen = true, isDisposed = false;

    public ObservableCollection<string> Places { get; private set; } = [];

    public string Name { get; set; } = string.Empty;

    public string SelectedName { get; private set; } = string.Empty;

    public string Coordinates { get; set; } = string.Empty;

    public string? InitSelected { get; private set; } = null;

    public LocationPopup(IKeyValueStore<Place> locationsStore, IUiMessageService messageService, string? initSelected = null)
    {
        BindingContext = this;
        InitializeComponent();
        Opened += OnOpened;
        this.locationsStore = locationsStore;
        this.messageService = messageService;
        this.InitSelected = initSelected;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue(nameof(InitSelected), out object? o);
        InitSelected = o as string;
    }

    private async void OnOpened(object? sender, EventArgs e)
    {
        ObjectDisposedException.ThrowIf(isDisposed, this);

        if (isFirstOpen)
        {
            isFirstOpen = false;
            Places = [.. (await locationsStore.GetAllKeys()).Order()];
            OnPropertyChanged(nameof(Places));
        }

        if (!string.IsNullOrWhiteSpace(InitSelected) && Places.Contains(InitSelected))
        {
            Place? place = await locationsStore.GetValueAsync(InitSelected);

            if (place is not null)
            {
                Name = place.Name;
                Coordinates = AstroHelper.ConvertDdToDmsString(place.Coordinates, 3);
                SelectedName = place.Name;

                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Coordinates));
                OnPropertyChanged(nameof(SelectedName));
            }
        }
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is null || ((CollectionView)sender).SelectedItem is not string name || string.IsNullOrWhiteSpace(name))
            return;

        Place? place = await locationsStore.GetValueAsync(name);

        if (place is null)
            return;

        Name = place.Name;
        Coordinates = AstroHelper.ConvertDdToDmsString(place.Coordinates, 3);
        SelectedName = place.Name;

        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Coordinates));
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        await SaveChangesAsync();
    }

    private async void SelectButton_Clicked(object sender, EventArgs e)
    {
        Place? place = await locationsStore.GetValueAsync(Name);

        if (place is null)
        {
            if (!await SaveChangesAsync())
                return;

            place = await locationsStore.GetValueAsync(Name);
        }

        await CloseAsync(place);
    }

    private async Task<bool> SaveChangesAsync()
    {
        if (string.IsNullOrEmpty(Name))
        {
            await messageService.ShowMessageAsync("Enter location name", "Locations must have a name", "Ok");
            return false;
        }

        GeographicalCoordinates coordinates;
        try
        {
            coordinates = AstroHelper.ConvertDmsStringToDd(Coordinates);

            if (!coordinates.Validate())
            {
                coordinates = AstroHelper.ConvertDdStringToDd(Coordinates);

                if (!coordinates.Validate())
                {
                    await messageService.ShowMessageAsync("Incorrect coordinate format", """
                        Coordinates must be in one of the following formats:
                        - Degrees Minutes Seconds - 47° 13' 11" N 14° 45' 53E
                        - Decimal Degrees - 47.22, 14.765
                        """, "Ok");
                    return false;
                }
            }
        }
        catch (FormatException)
        {
            await messageService.ShowMessageAsync("Incorrect coordinate format", """
                 Coordinates must be in one of the following formats:
                 - Degrees Minutes Seconds - 47° 13' 11" N 14° 45' 53" E
                 - Decimal Degrees - 47.22, 14.765
                 """, "Ok");

            return false;
        }

        if (await locationsStore.AddOrUpdateAsync(Name, new(Name, coordinates)))
        {
            if (!Places.Contains(Name))
            {
                Places.Add(Name);
                Places = new(Places.Order());
                OnPropertyChanged(nameof(Places));
            }

            Coordinates = AstroHelper.ConvertDdToDmsString(coordinates, 3);
            SelectedName = Name;

            OnPropertyChanged(nameof(Coordinates));
            OnPropertyChanged(nameof(SelectedName));

            return true;
        }

        return false;
    }

    private async void RemoveButton_Clicked(object sender, EventArgs e)
    {
        string? name = ((Button)sender).CommandParameter as string;

        if (!string.IsNullOrEmpty(name))
            await RemoveAsync(name);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        string? name = e.Parameter as string;

        if (!string.IsNullOrEmpty(name))
            await RemoveAsync(name);
    }

    private async Task RemoveAsync(string name)
    {
        Place? place = await locationsStore.GetValueAsync(name);

        if (place is not null)
        {
            if (await locationsStore.RemoveAsync(name))
            {
                Places.Remove(name);
                Name = string.Empty;
                Coordinates = string.Empty;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Coordinates));
            }
            else
                await messageService.ShowMessageAsync("Something went wrong", "Location has not been removed", "Ok");
        }
    }

    public void Dispose()
    {
        if (isDisposed)
            return;

        Opened -= OnOpened;
        isDisposed = true;
    }
}