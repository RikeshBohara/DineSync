using DineSync.ViewModels;

namespace DineSync.Views.Pages;

public partial class EditMenuPage : ContentPage
{
	public EditMenuPage(MenuPageViewModel menuPageViewModel)
	{
		InitializeComponent();
		BindingContext = menuPageViewModel;
	}
}