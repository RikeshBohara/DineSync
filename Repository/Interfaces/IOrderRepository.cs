using System.Data.Common;
using DineSync.Models;
namespace DineSync.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<int> SaveOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task<List<Order>> GetPaidPaymentOrdersAsync();
        Task<List<Order>> GetUnpaidPaymentOrdersAsync();
        Task<List<Order>> GetPendingOrdersAsync();
        Task<List<Order>> GetCompletedOrdersAsync();
        Task<List<Order>> GetOrdersByTableAsync(int tableId);
        Task<List<Order>> ClearOrdersByTable(int tableNumber);
    }
}