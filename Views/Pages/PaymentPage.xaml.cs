using DineSync.Models;
using DineSync.ViewModels;

namespace DineSync.Views.Pages;

[QueryProperty(nameof(User), "User")]
public partial class PaymentPage : ContentPage
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
	public PaymentPage(TablePageViewModel tablePageViewModel)
	{
		InitializeComponent();
		BindingContext = tablePageViewModel;

    }

	public void OnUserReceived(User user)
	{
		if(BindingContext is TablePageViewModel tablePageViewModel)
		{
            tablePageViewModel.User = user;
		}
	}
}