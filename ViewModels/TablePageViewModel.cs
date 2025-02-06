using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using DineSync.Views.Popups;
using SQLitePCL;

namespace DineSync.ViewModels
{
    public partial class TablePageViewModel : ObservableObject
    {
        #region Fields
        private readonly ITableRepository _TableRepository;
        private readonly IOrderRepository _OrderRepository;
        private readonly IOrderItemRepository _OrderItemRepository;
        #endregion

        #region Properties
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

        [ObservableProperty]
        private ObservableCollection<Order> _TableOrders;

        [ObservableProperty]
        private bool _IsWaiter;

        [ObservableProperty]
        private bool _IsOccupied;

        [ObservableProperty]
        private decimal _TotalTableAmount;

        [ObservableProperty]
        private ObservableCollection<OrderItem> _OrderItems;

        [ObservableProperty]
        private Order _SelectedOrder;

        [ObservableProperty]
        private bool _IsPaymentEnabled;

        [ObservableProperty]
        private Table _OrderTable;

        [ObservableProperty]
        private string _OrderNumber;

        [ObservableProperty]
        private int _TableNumber;

        [ObservableProperty]
        private int _GuestCount;

        [ObservableProperty]
        private string _OrderDate;

        [ObservableProperty]
        private string _OrderTime;

        [ObservableProperty]
        private string _OrderStatus;

        private Popup _AddTablePopup;
        private Popup _OrderDetailsPopup;
        #endregion

        #region Constructor
        public TablePageViewModel(ITableRepository tableRepository, IOrderItemRepository orderItemRepository, IOrderRepository orderRepository)
        {
            _TableRepository = tableRepository;
            _OrderRepository = orderRepository;
            _OrderItemRepository = orderItemRepository;
            Tables = new ObservableCollection<Table>();
            TableOrders = new ObservableCollection<Order>();
            LoadTables();
        }
        #endregion

        #region Methods
        public async void LoadTables()
        {
            IsOccupied = false;
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
            IsOccupied = false;
            IsWaiter = true;
            try
            {
                if (selectedTable.Status == "Occupied")
                {
                    IsOccupied = true;
                }
                _SelectedTable = selectedTable;
                OnPropertyChanged(nameof(SelectedTable));
                await GetOrders(selectedTable);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"{ex}", "OK");
            }
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
                        IsOccupied = true;

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
                    IsOccupied = true;
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
            if (count > 0)
            {
                SelectedTable.GuestCount = count;
                SelectedTable.Status = "Occupied";
                await _TableRepository.UpdateTableAsync(SelectedTable);
                LoadTables();
            }
        }

        [RelayCommand]
        public async Task EditGuestCount(int count)
        {
            SelectedTable.GuestCount = count;
            SelectedTable.Status = "Occupied";
            await _TableRepository.UpdateTableAsync(SelectedTable);
            LoadTables();
        }

        [RelayCommand]
        public async Task GetOrders(Table selectedTable)
        {
            TotalTableAmount = 0;
            var orders = await _OrderRepository.GetOrdersByTableAsync(selectedTable.Id);
            TableOrders = new ObservableCollection<Order>(orders);
            foreach (var order in orders)
            {
                TotalTableAmount += order.TotalAmount;
            }
        }

        [RelayCommand]
        public async Task ChangeTableStatus()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                   "Delete Category",
                   $"Are you sure you want to make Table '{SelectedTable.TableNumber}' Available?",
                   "Yes", "No");

            if (!confirm)
                return;

            SelectedTable.Status = "Available";
            SelectedTable.GuestCount = 0;
            GuestsCount = null;
            TotalTableAmount = 0;
            await SaveGuestCount(SelectedTable.GuestCount);
            await _TableRepository.UpdateTableAsync(SelectedTable);
            await ClearOrders(SelectedTable);
            LoadTables();
        }

        [RelayCommand]
        public async Task ClearOrders(Table selectedTable)
        {
            var orders = await _OrderRepository.ClearOrdersByTableAsync(selectedTable.TableNumber);
            TableOrders = new ObservableCollection<Order>(orders);
        }

        [RelayCommand]
        public async Task OrderTapped(Order selectedOrder)
        {
            var popup = new OrderDetailsPopup
            {
                BindingContext = this
            };
            _OrderDetailsPopup = popup;
            Shell.Current.ShowPopup(popup);

            IsPaymentEnabled = true;
            if (selectedOrder == null)
            {
                IsPaymentEnabled = false;
                return;
            }
            if (selectedOrder.PaymentStatus == "Paid")
            {
                IsPaymentEnabled = false;
            }

            SelectedOrder = selectedOrder;
            OrderTable = await _TableRepository.GetTableByIdAsync(selectedOrder.TableNumber);
            var items = await _OrderItemRepository.GetOrderItemsByOrderIdAsync(selectedOrder.Id);
            OrderItems = new ObservableCollection<OrderItem>(items);

            string date = SelectedOrder.OrderDate.ToString("yyyy-MM-dd");
            string time = SelectedOrder.OrderDate.ToString("hh:mm tt");

            OrderDate = date;
            OrderTime = time;
            OrderNumber = selectedOrder.OrderNumber;
            TableNumber = selectedOrder.TableNumber;
            GuestCount = selectedOrder.GuestCount;
            OrderStatus = selectedOrder.Status;
        }

        [RelayCommand]
        public async Task UpdatePaymentStatus()
        {
            if (SelectedOrder == null) return;
            else if (SelectedOrder.PaymentStatus == "Paid")
            {
                IsPaymentEnabled = false;
            }
            SelectedOrder.PaymentStatus = "Paid";
            await _OrderRepository.UpdateOrderAsync(SelectedOrder);
            await GetOrders(SelectedTable);
            LoadTables();
            IsPaymentEnabled = false;
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
    }
    #endregion
}
