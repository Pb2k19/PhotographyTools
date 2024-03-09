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

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (lastWidth != width)
        {
            lastWidth = width;

            if (!isOneColumn && width < height) //vertical
            {
                isOneColumn = true;
                FiltersCollectionView.ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem;
                GridItemsLayout.Span = 1;
            }
            else if (isOneColumn && width > height) //horizontal
            {
                isOneColumn = false;
                FiltersCollectionView.ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems;
                GridItemsLayout.Span = 2;
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