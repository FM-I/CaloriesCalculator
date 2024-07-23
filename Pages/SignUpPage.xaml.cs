using CaloriesCalculator.ViewModel;

namespace CaloriesCalculator.Pages;

public partial class SignUpPage : ContentPage
{
	public SignUpPage(SignUpViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}