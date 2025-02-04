using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SQLiteAsyncConnection _Connection;

        public OrderRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }
        public async Task<Order[]> GetAllOrdersAsync()
        {
            return await _Connection.Table<Order>().ToArrayAsync();
        }
        public async Task<int> SaveOrderAsync(Order order)
        {
            await _Connection.InsertAsync(order);
            return order.Id;
        }
        public async Task UpdateOrderAsync(Order order)
        {
            await _Connection.UpdateAsync(order);
        }
        public async Task<Order[]> GetOrderByPaymentStatus(Order order)
        {
            throw new NotImplementedException();
        }
        public async Task<Order[]> GetOrderByStatus(Order order)
        {
            throw new NotImplementedException();
        }
    }
}