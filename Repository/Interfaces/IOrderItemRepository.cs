using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetAllOrderItemsAsync();
        Task<int> SaveOrderItemAsync(OrderItem orderItem);
        Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
    }
}
