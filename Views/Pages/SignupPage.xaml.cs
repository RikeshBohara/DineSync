using DineSync.ViewModels;

namespace DineSync.Views.Pages;

public partial class SignupPage : ContentPage
{
	public SignupPage(SignupPageViewModel signupPageViewModel)
	{
		InitializeComponent();
		BindingContext = signupPageViewModel;
	}
}