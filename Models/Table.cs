using SQLite;

namespace DineSync.Models
{
    public class Table
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public int? AssignedWaiterId { get; set; }
    };
}
