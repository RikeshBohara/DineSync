using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class SignupRepository : ISignupRepository
    {
        private readonly SQLiteAsyncConnection _Connection;

        public SignupRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }
        public async Task<int> AddUserAsync(User user)
        {
            if(user.Id!=0)
                return await _Connection.UpdateAsync(user);
            return await _Connection.InsertAsync(user);
        }
        public async Task<User> CheckIfEmailExistsAsync(string email)
        {
            return await _Connection.Table<User>().FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
