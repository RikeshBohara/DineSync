using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DineSync.Models;
using SQLite;

namespace DineSync.Data
{
    public class DbConfig
    {
        private readonly SQLiteAsyncConnection _Connection;

        public DbConfig()
        {
            var dbPath = GetDatabasePath();
            _Connection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
        }

        public string GetDatabasePath()
        {
#if WINDOWS
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DineSync.db3");
#elif ANDROID
            var downloadPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "DineSync.db3");
            return downloadPath;
#else
            throw new InvalidOperationException("Unsupported platform");
#endif
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return _Connection;
        }

        public async Task InitializeDatabaseAsync()
        {
            await _Connection.CreateTableAsync<Owner>();
            await _Connection.CreateTableAsync<User>();
            await _Connection.CreateTableAsync<Table>();
            await _Connection.CreateTableAsync<MenuCategory>();
            await _Connection.CreateTableAsync<Menu>();
            await _Connection.CreateTableAsync<MenuCategoryMapping>();
            await _Connection.CreateTableAsync<OrderItem>();
            await _Connection.CreateTableAsync<Order>();
            await _Connection.CreateTableAsync<Bill>();

            await SeedDataAsync();
        }

        private async Task SeedDataAsync()
        {
            var firstCategory = await _Connection.Table<MenuCategory>().FirstOrDefaultAsync();
            if (firstCategory != null) {
                return;
            }

            var categories = SeedData.SeedMenuCategories();
            var menuItems = SeedData.SeedMenuItems();
            var menuCategoryMapping = SeedData.SeedMenuCategoryMappings();
            var tables = SeedData.SeedTables();
            var users = SeedData.SeedUsers();
            var owners = SeedData.SeedOwners();

            await _Connection.InsertAllAsync(categories);
            await _Connection.InsertAllAsync(menuItems);
            await _Connection.InsertAllAsync(menuCategoryMapping);
            await _Connection.InsertAllAsync(tables);
            await _Connection.InsertAllAsync(users);
            await _Connection.InsertAllAsync(owners);
        }
    }
}
