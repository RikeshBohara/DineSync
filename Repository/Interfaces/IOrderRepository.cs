using System.Data.Common;
using DineSync.Models;
namespace DineSync.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order[]> GetAllOrdersAsync();
        Task<int> SaveOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task<Order[]> GetOrderByStatus(Order order);
        Task<Order[]> GetOrderByPaymentStatus(Order order);
    }
}