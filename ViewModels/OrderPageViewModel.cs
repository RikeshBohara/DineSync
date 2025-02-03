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
            if (order == null) return;

            SelectedOrder = order;
            OrderTable = await _TableRepository.GetTableByIdAsync(order.TableNumber);
            var items = await _OrderItemRepository.GetOrderItemsByOrderIdAsync(order.Id);
            OrderItems = new ObservableCollection<OrderItem>(items);

            OrderNumber = order.OrderNumber;
            TableNumber = order.TableNumber;
            GuestCount = order.GuestCount;
            OrderStatus = order.Status;
        }

        [RelayCommand]
        public async Task UpdateOrderStatus(string status)
        {
            if (SelectedOrder == null) return;

            SelectedOrder.Status = status;
            await _OrderRepository.UpdateOrderAsync(SelectedOrder);
            LoadOrders();
        }
    }
}
