namespace Photography_Tools.Pages;

public partial class AstroTimeCalcPage : ContentPage
{
	public AstroTimeCalcPage(AstroTimeCalcViewModel astroTimeCalcViewModel)
	{
		InitializeComponent();
		BindingContext = astroTimeCalcViewModel;
	}
}