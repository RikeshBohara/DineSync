using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem[]> GetAllOrderItemsAsync();
        Task<int> SaveOrderItemAsync(OrderItem orderItem);
        Task<OrderItem[]> GetOrderItemsByOrderIdAsync(int orderId);
    }
}
