using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace DineSync.Models
{
    public partial class OrderItem : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public decimal Price { get; set; }

        [ObservableProperty, NotifyPropertyChangedFor(nameof(Amount))]
        private int _Quantity;

        [Ignore]
        public decimal Amount => Quantity * Price;
    };
}
