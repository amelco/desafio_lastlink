namespace Core.Entities
{
    public class Product
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime CreatedAt { get; set; }

        public Product(string name, string category, decimal unitCost)
        {
            Name = name;
            Category = category;
            UnitCost = unitCost;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
