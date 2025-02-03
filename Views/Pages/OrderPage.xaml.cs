using DineSync.ViewModels;

namespace DineSync.Views.Pages;

public partial class OrderPage : ContentPage
{
	public OrderPage(OrderPageViewModel orderPageViewModel)
	{
		InitializeComponent();
		BindingContext = orderPageViewModel;
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