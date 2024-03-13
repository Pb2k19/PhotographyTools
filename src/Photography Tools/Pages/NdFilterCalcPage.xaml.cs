namespace Photography_Tools.Pages;

public partial class NdFilterCalcPage : ContentPage
{
    public NdFilterCalcPage(NDFilterCalcViewModel ndFilterCalcViewModel)
    {
        InitializeComponent();
        BindingContext = ndFilterCalcViewModel;
    }

#if ANDROID
    private double lastWidth = 0;
    private bool isOneColumn = true;
    private Thickness
        filterLabelMarginVertical = new(0, 20, 0, 10),
        filterLabelMarginHorizontal = new(0, 0, 0, 10),
        addGridMarginVertical = new(0, 0, 0, 5),
        addGridMarginHorizontal = new(0, 10, 0, 0);

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (lastWidth != width)
        {
            lastWidth = width;

            if (!isOneColumn && width < height) //vertical
            {
                isOneColumn = true;

                FilterLabel.Margin = filterLabelMarginVertical;
                AddGrid.Margin = addGridMarginVertical;

                Grid.SetColumn(FilterLabel, 0);
                Grid.SetRow(FilterLabel, 4);

                Grid.SetColumn(FiltersCollectionView, 0);
                Grid.SetRow(FiltersCollectionView, 6);
                Grid.SetRowSpan(FiltersCollectionView, 0);

            }
            else if (isOneColumn && width > height) //horizontal
            {
                isOneColumn = false;

                FilterLabel.Margin = filterLabelMarginHorizontal;
                AddGrid.Margin = addGridMarginHorizontal;

                Grid.SetColumn(FilterLabel, 1);
                Grid.SetRow(FilterLabel, 0);

                Grid.SetColumn(FiltersCollectionView, 1);
                Grid.SetRow(FiltersCollectionView, 1);
                Grid.SetRowSpan(FiltersCollectionView, 6);
            }
        }
    }
#elif WINDOWS
    private const int MinTwoLineWidth = 600;
    private bool isOneColumn = false;
    
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (isOneColumn && width > MinTwoLineWidth)
        {
            isOneColumn = false;
            GridItemsLayout.Span = 2;
        }
        else if (!isOneColumn && width < MinTwoLineWidth)
        {
            isOneColumn = true;
            GridItemsLayout.Span = 1;
        }
    }
#endif
}