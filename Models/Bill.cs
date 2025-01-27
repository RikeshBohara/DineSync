using SQLite;

namespace DineSync.Models
{
    public class Bill
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime? PaymentTime { get; set; }
    };
}
