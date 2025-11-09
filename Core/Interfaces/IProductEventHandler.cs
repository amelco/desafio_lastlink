using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductEventHandler
    {
        public string Create(Product product);
    }
}
