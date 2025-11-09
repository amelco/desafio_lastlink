using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductEvent
    {
        public int Id { get; set;  }
        public string Type { get; set; } = "";
        public Product? Product { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
