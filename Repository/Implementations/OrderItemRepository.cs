using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly SQLiteAsyncConnection _Connection;

        public OrderItemRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }

        public async Task<List<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _Connection.Table<OrderItem>().ToListAsync();
        }

        public async Task<int> SaveOrderItemAsync(OrderItem orderItem)
        {
            return await _Connection.InsertAsync(orderItem);
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _Connection.Table<OrderItem>().Where(orderItem => orderItem.OrderId == orderId).ToListAsync();
        }
    }
}
