using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductEventRepository
    {
        Task<ProductEvent?> GetById(int id);
        IEnumerable<ProductEvent> GetAll();
        Task Add(ProductEvent productEvent);
    }
}
