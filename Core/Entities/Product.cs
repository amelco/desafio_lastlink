namespace Core.Entities
{
    public class Product
    {
        public int Id { get; set;  }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal UnitCost { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<ProductEvent>? Events { get; }
    }
}
