using CommunityToolkit.Mvvm.ComponentModel;
using DineSync.Models;

namespace DineSync.ViewModels
{
    public partial class DashboardPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private User _User;

        public DashboardPageViewModel()
        {

        }
    }

}
