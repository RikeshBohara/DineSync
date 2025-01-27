using SQLite;

namespace DineSync.Models
{
    public class Menu
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { set; get; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    };
}
