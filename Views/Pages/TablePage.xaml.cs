using CommunityToolkit.Mvvm.Messaging;
using DineSync.ViewModels;

namespace DineSync.Views.Pages;

public partial class TablePage : ContentPage
{
	public TablePage(TablePageViewModel tablePageViewModel)
	{
		InitializeComponent();
		BindingContext = tablePageViewModel;
	}
}