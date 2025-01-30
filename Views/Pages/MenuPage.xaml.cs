using DineSync.ViewModels;

namespace DineSync.Views.Pages;

public partial class MenuPage : ContentPage
{
	public MenuPage(MenuPageViewModel menuPageViewModel)
	{
		InitializeComponent();
		BindingContext = menuPageViewModel;
	}
}