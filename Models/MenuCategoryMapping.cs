using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DineSync.Models
{
    public class MenuCategoryMapping
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MenuCategoryId {  get; set; }
        public int MenuId {  get; set; }
    }
}
