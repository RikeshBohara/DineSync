using SQLite;

namespace DineSync.Models
{
    public class OrderItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Icon { get; set; }
        public decimal Price { get; set; }
        public string SpecialInstructions { get; set; }

        [Ignore]
        public decimal Amount => Quantity * Price;
    };
}
