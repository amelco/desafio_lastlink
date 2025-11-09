namespace Core.Entities
{
    public class ProductEvent
    {
        public int Id { get; set; }
        public string Type { get; set; } = "";
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Product? Product { get; set; }
    }
}
