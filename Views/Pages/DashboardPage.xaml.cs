using DineSync.Models;
using DineSync.ViewModels;

namespace DineSync.Views.Pages;

[QueryProperty(nameof(User), "User")]
public partial class DashboardPage : ContentPage
{
	private User _User;
	public User User
	{
		get => _User;
		set
		{
			_User = value;
			OnUserReceived(value);
		}
	}
	public DashboardPage(DashboardPageViewModel dashboardPageViewModel)
	{
		InitializeComponent();
		BindingContext = dashboardPageViewModel;

    }

	public void OnUserReceived(User user)
	{
		if(BindingContext is DashboardPageViewModel dashboardPageViewModel)
		{
			dashboardPageViewModel.User = user;
		}
	}
}