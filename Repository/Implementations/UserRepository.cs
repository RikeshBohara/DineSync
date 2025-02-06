using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        #region Fields
        private readonly SQLiteAsyncConnection _Connection;
        #endregion

        #region Constructor
        public UserRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }
        #endregion

        #region Methods
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

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            var query = @"select * from User where Role=?";
            var users = await _Connection.QueryAsync<User>(query, role);
            return users.ToList();
        }

        public async Task<List<User>> GetSerachedUserAsync(string user)
        {
            var query = @"select * from User where Name = ?";
            var users = await _Connection.QueryAsync<User>(query, user);
            return users.ToList();
        }
        #endregion
    }
}
