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
	public PaymentPage(OrderPageViewModel orderPageViewModel)
	{
		InitializeComponent();
		BindingContext = orderPageViewModel;

    }

	public void OnUserReceived(User user)
	{
		if(BindingContext is OrderPageViewModel orderPageViewModel)
		{
            orderPageViewModel.User = user;
		}
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is OrderPageViewModel orderPageViewModel)
        {
            orderPageViewModel.LoadOrders();
        }
    }
}