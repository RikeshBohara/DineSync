using DineSync.ViewModels;

namespace DineSync.Views.Pages;

public partial class EmployeePage : ContentPage
{
	public EmployeePage(EmployeePageViewModel employeePageViewModel)
	{
		InitializeComponent();
		BindingContext = employeePageViewModel;
	}
}