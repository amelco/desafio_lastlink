using Core.DTOs;

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

        public Product() { }

        public Product(string name, string category, decimal unitCost)
        {
            Name = name;
            Category = category;
            UnitCost = unitCost;
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        //public ProductDto ToDto()
        //{
        //    return new ProductDto
        //    {
        //        Id = this.Id,
        //        Name = this.Name,
        //        Category = this.Category,
        //        UnitCost = this.UnitCost,
        //        CreatedAt = this.CreatedAt,
        //    };
        //}
    }
}
