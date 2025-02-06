using SQLite;

namespace DineSync.Models
{
    public class MenuCategory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
