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
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _Connection.Table<Order>().ToListAsync();
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
        public async Task<List<Order>> GetPaidPaymentOrdersAsync()
        {
            var query = @"select * from [Order] where PaymentStatus='Paid'";
            var PaidOrder = await _Connection.QueryAsync<Order>(query);
            return PaidOrder.ToList();
        }
        public async Task<List<Order>> GetUnpaidPaymentOrdersAsync()
        {
            var query = @"select * from [Order] where PaymentStatus='Unpaid'";
            var UnpaidOrder = await _Connection.QueryAsync<Order>(query);
            return UnpaidOrder.ToList();
        }
        public async Task<List<Order>> GetPendingOrdersAsync()
        {
            var query = @"select * from [Order] where Status='Pending'";
            var PendingOrder = await _Connection.QueryAsync<Order>(query);
            return PendingOrder.ToList();
        }
        public async Task<List<Order>> GetCompletedOrdersAsync()
        {
            var query = @"select * from [Order] where Status='Completed'";
            var CompletedOrder = await _Connection.QueryAsync<Order>(query);
            return CompletedOrder.ToList();
        }
        public async Task<List<Order>> GetOrdersByTableAsync(int tableId)
        {
            var query = "SELECT * FROM [Order] WHERE TableId = ?";
            var orders = await _Connection.QueryAsync<Order>(query, tableId);
            return orders.ToList();
        }
    }
}