namespace Core.DTOs
{
    public class ProductDto
    {
        public int Id { get; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
