using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetById(int id);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<ProductDto> Add(Product product);
        Task<ProductDto> Update(int id, UpdatePoductDto product);
        Task Delete(int id);
    }
}
