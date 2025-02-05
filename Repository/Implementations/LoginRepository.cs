using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class LoginRepository : ILoginRepository
    {
        #region Fields
        private readonly SQLiteAsyncConnection _Connection;
        #endregion

        #region Constructor
        public LoginRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }
        #endregion

        #region Methods
        public Task<User> LoginAsync(string email, string password)
        {
            return _Connection.Table<User>().FirstOrDefaultAsync(user => user.Email == email && user.Password == password);
        }
        #endregion
    }
}
