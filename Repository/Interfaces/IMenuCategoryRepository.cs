using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface IMenuCategoryRepository
    {
        Task<MenuCategory[]> GetMenuCategoriesAsync();
        Task<int> AddMenuCategoryAsync(MenuCategory menuCategory);
        Task<int> RemoveMenuCategoryAsync(MenuCategory menuCategory);
    }
}
