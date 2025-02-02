using SQLite;

namespace DineSync.Models
{
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TableId { get; set; }
        public int EmployeeId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItemsCount { get; set; }
        public int GuestCount { get; set; }
        public string PaymentMode { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    };
}
