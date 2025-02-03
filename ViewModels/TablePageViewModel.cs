using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using DineSync.Views.Popups;

namespace DineSync.ViewModels
{
    public partial class TablePageViewModel : ObservableObject
    {
        #region Fields
        private readonly ITableRepository _TableRepository;
        private readonly IUserRepository _UserRepository;

        [ObservableProperty]
        private User _User;

        [ObservableProperty]
        private ObservableCollection<Table> _Tables;

        [ObservableProperty]
        private string _NewTableCapacity;

        [ObservableProperty]
        private Table _SelectedTable;

        [ObservableProperty]
        private string _GuestsCount;

        private Popup _AddTablePopup;

        private Popup _SaveTableOccupantsPopup;
        #endregion

        #region Constructor
        public TablePageViewModel(ITableRepository tableRepository)
        {
            _TableRepository = tableRepository;
            Tables = new ObservableCollection<Table>();
            LoadTables();
        }
        #endregion

        #region Methods
        public async void LoadTables()
        {
            var tables = await _TableRepository.GetAllTablesAsync();
            Tables = new ObservableCollection<Table>(tables);
        }

        [RelayCommand]
        private async Task AddTable()
        {
            if (int.TryParse(NewTableCapacity, out int capacity) && capacity > 0)
            {
                await AddNewTable(capacity);
                NewTableCapacity = string.Empty;
                _AddTablePopup?.Close();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Capacity must be a valid number greater than 0", "OK");
            }
        }

        [RelayCommand]
        public async Task AddNewTable(int capacity)
        {
            var newTable = new Table
            {
                TableNumber = Tables.Count + 1,
                Capacity = capacity,
                Status = "Available"
            };
            await _TableRepository.AddTableAsync(newTable);
            LoadTables();
        }

        [RelayCommand]
        public async Task TableTapped(Table selectedTable)
        {
            _SelectedTable = selectedTable;
            OnPropertyChanged(nameof(SelectedTable));
        }

        [RelayCommand]
        private async Task RemoveTable()
        {
            if (SelectedTable != null)
            {
                await _TableRepository.RemoveTableAsync(SelectedTable);
                Tables.Remove(SelectedTable);
                SelectedTable = null;
                LoadTables();
            }
        }

        [RelayCommand]
        public async Task SaveGuests(object selectedTable)
        {
            try
            {
                if (SelectedTable.Status != "Occupied")
                {
                    if (int.TryParse(GuestsCount, out int count) && count > 0 && count <= SelectedTable.Capacity)
                    {
                        await SaveGuestCount(count);
                        GuestsCount = string.Empty;
                        _SaveTableOccupantsPopup?.Close();

                        var navigationParams = new Dictionary<string, object>
                        {
                            { "SelectedTable", SelectedTable }
                        };
                        await Shell.Current.GoToAsync("//MenuPage", navigationParams);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Occupants must be a valid number greater than 0", "OK");
                    }
                }
                else
                {
                    var navigationParams = new Dictionary<string, object>
                    {
                        { "SelectedTable", SelectedTable }
                    };
                    await Shell.Current.GoToAsync("//MenuPage", navigationParams);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"{ex}", "OK");
            }
        }

        [RelayCommand]
        public async Task SaveGuestCount(int count)
        {
            SelectedTable.GuestCount = count;
            SelectedTable.Status = "Occupied";
            SelectedTable.AssignedWaiterId = User.Id;
            await _TableRepository.UpdateTableAsync(SelectedTable);
            LoadTables();
        }

        [RelayCommand]
        public async Task EditGuestCount(int count)
        {
            SelectedTable.GuestCount = count;
            SelectedTable.Status = "Occupied";
            SelectedTable.AssignedWaiterId = User.Id;
            await _TableRepository.UpdateTableAsync(SelectedTable);
            LoadTables();
        }

        [RelayCommand]
        public void TablePopup(Table table)
        {
            var popup = new AddTablePopup
            {
                BindingContext = this
            };
            _AddTablePopup = popup;
            Shell.Current.ShowPopup(popup);
        }
        #endregion
    }
}
