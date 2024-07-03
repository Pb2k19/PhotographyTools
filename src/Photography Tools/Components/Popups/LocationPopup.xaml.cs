using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Photography_Tools.Services.KeyValueStoreService;
using System.Collections.ObjectModel;

namespace Photography_Tools.Components.Popups;

public partial class LocationPopup : Popup
{
    private readonly IKeyValueStore<Place> locationsStore;
    private readonly IUiMessageService messageService;

    private string? initSelected = null;

    public ObservableCollection<string> Places { get; private set; } = [];

    public string Name { get; set; } = string.Empty;

    public string Coordinates { get; set; } = string.Empty;

    public LocationPopup(IKeyValueStore<Place> locationsStore, IUiMessageService messageService, string? initSelected = null)
    {
        BindingContext = this;
        InitializeComponent();
        Opened += OnOpened;
        Closed += OnClosed;
        this.locationsStore = locationsStore;
        this.messageService = messageService;
        this.initSelected = initSelected;
    }

    private async void OnOpened(object? sender, PopupOpenedEventArgs e)
    {
        Places = [.. (await locationsStore.GetAllKeys()).Order()];

        if (!string.IsNullOrWhiteSpace(initSelected) && Places.Contains(initSelected))
        {
            Place? place = await locationsStore.GetValueAsync(initSelected);

            if (place is not null)
            {
                Name = place.Name;
                Coordinates = AstroHelper.ConvertDdToDmsString(place.Coordinates, 3);
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Coordinates));
            }
        }

        OnPropertyChanged(nameof(Places));
    }

    private void OnClosed(object? sender, PopupClosedEventArgs e)
    {
        Closed -= OnClosed;
        Opened -= OnOpened;
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
                 - Degrees Minutes Seconds - 47° 13' 11" N 14° 45' 53E
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

            return true;
        }

        return false;
    }

    private async void RemoveButton_Clicked(object sender, EventArgs e)
    {
        string name = (string)((Button)sender).CommandParameter;

        Place? place = await locationsStore.GetValueAsync(name);

        if (place is not null)
        {
            if (await locationsStore.RemoveAsync(name))
                Places.Remove(name);
            else
                await messageService.ShowMessageAsync("Something went wrong", "Location has not been removed", "Ok");
        }
    }
}