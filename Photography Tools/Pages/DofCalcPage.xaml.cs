using Photography_Tools.ViewModels;

namespace Photography_Tools.Pages;

public partial class DofCalcPage : ContentPage
{
	public DofCalcPage(DofCalcViewModel dofCalcViewModel)
	{
		InitializeComponent();
		BindingContext = dofCalcViewModel;
	}
}