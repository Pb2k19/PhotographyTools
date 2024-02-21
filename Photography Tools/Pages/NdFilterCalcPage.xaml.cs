namespace Photography_Tools.Pages;

public partial class NdFilterCalcPage : ContentPage
{
	public NdFilterCalcPage(NDFilterCalcViewModel ndFilterCalcViewModel)
	{
		InitializeComponent();
		BindingContext = ndFilterCalcViewModel;
	}
}