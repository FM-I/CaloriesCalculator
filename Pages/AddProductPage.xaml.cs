using CaloriesCalculator.ViewModel;

namespace CaloriesCalculator.Pages;

public partial class AddProductPage : ContentPage
{
	private readonly AddProductViewModel _viewModel;
	private Frame? _previousSelectedItem;

	public AddProductPage(AddProductViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_viewModel = vm;
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
		var vm = BindingContext as AddProductViewModel;

		if (string.IsNullOrWhiteSpace(e.NewTextValue))
		{
			vm.Weight = null;
		}
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
		if (string.IsNullOrWhiteSpace(e.NewTextValue))
		{
			ProductList.ItemsSource = _viewModel.Products;
		}
		else
		{
			ProductList.ItemsSource = _viewModel.Products.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()));
		}
    }

}