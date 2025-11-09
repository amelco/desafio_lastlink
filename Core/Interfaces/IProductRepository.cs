using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetById(int id);
        IEnumerable<Product> GetAll();
        Task<Product> Add(Product product);
        Task<Product?> Update(int id, Product product);
        Task Delete(int id);
    }
}
