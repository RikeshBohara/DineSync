using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLiteAsyncConnection _Connection;

        public UserRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _Connection.Table<User>().ToListAsync();
        }

        public async Task<int> AddUserAsync(User user)
        {
            return await _Connection.InsertAsync(user);
        }

        public async Task<int> RemoveUserAsync(User user)
        {
            return await _Connection.DeleteAsync(user);
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            return await _Connection.UpdateAsync(user);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _Connection.Table<User>().FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}
