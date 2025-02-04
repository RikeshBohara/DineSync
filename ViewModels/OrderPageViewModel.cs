using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Models;
using DineSync.Repository.Interfaces;

namespace DineSync.ViewModels
{
    public partial class OrderPageViewModel : ObservableObject
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IOrderItemRepository _OrderItemRepository;
        private readonly ITableRepository _TableRepository;

        [ObservableProperty]
        private User _User;

        [ObservableProperty]
        private ObservableCollection<Order> _Orders;

        [ObservableProperty]
        private ObservableCollection<OrderItem> _OrderItems;

        [ObservableProperty]
        private Order _SelectedOrder;

        [ObservableProperty]
        private Table _OrderTable;

        [ObservableProperty]
        private string _OrderNumber;

        [ObservableProperty]
        private int _TableNumber;

        [ObservableProperty]
        private int _GuestCount;

        [ObservableProperty]
        private string _OrderStatus;

        [ObservableProperty]
        private bool _IsEnabled;

        [ObservableProperty]
        private string _OrderDate;

        [ObservableProperty]
        private string _OrderTime;

        [ObservableProperty]
        private bool _IsPaymentEnabled;

        public OrderPageViewModel(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, ITableRepository tableRepository)
        {
            _OrderRepository = orderRepository;
            _OrderItemRepository = orderItemRepository;
            _TableRepository = tableRepository;
            LoadOrders();
        }

        public void SetOrderNumber(string orderNumber)
        {
            OrderNumber = orderNumber;
            OnPropertyChanged(nameof(OrderNumber));
        }

        public async void LoadOrders()
        {
            var orders = await _OrderRepository.GetAllOrdersAsync();
            Orders = new ObservableCollection<Order>(orders.OrderByDescending(order => order.OrderDate));
        }

        [RelayCommand]
        public async Task ShowOrderDetails(Order order)
        {
            IsEnabled = true;
            IsPaymentEnabled = true;
            if (order == null)
            {
                IsEnabled = false;
                IsPaymentEnabled = false;
                return;
            }
            if (order.Status == "Completed")
            {
                IsEnabled = false;
            }
            if (order.PaymentStatus == "Paid")
            {
                IsEnabled = false;
            }

            SelectedOrder = order;
            OrderTable = await _TableRepository.GetTableByIdAsync(order.TableNumber);
            var items = await _OrderItemRepository.GetOrderItemsByOrderIdAsync(order.Id);
            OrderItems = new ObservableCollection<OrderItem>(items);

            string date = SelectedOrder.OrderDate.ToString("yyyy-MM-dd");
            string time = SelectedOrder.OrderDate.ToString("hh:mm tt");

            OrderDate = date;
            OrderTime = time;
            OrderNumber = order.OrderNumber;
            TableNumber = order.TableNumber;
            GuestCount = order.GuestCount;
            OrderStatus = order.Status;
        }

        [RelayCommand]
        public async Task UpdateOrderStatus()
        {
            if (SelectedOrder == null) return;
            else if (SelectedOrder.Status == "Completed")
            {
                IsEnabled = false;
            }
            SelectedOrder.Status = "Completed";
            await _OrderRepository.UpdateOrderAsync(SelectedOrder);
            LoadOrders();
            IsEnabled = false;
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
            LoadOrders();
            IsPaymentEnabled = false;
        }
    }
}
