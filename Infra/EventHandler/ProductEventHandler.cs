using Core.Entities;
using Core.Interfaces;
using Newtonsoft.Json;

namespace Infra.EventHandler
{
    public class ProductEventHandler : IProductEventHandler
    {
        public string Create(Product product)
        {
            var productEvent = new ProductEvent
            {
                Type = "product.created",
                ProductId = product.Id,
                CreatedAt = DateTime.Now,
            };
            return JsonConvert.SerializeObject(productEvent);
        }
    }
}
