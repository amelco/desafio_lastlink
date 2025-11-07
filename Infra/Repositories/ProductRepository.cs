using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Task<ProductDto> Add(Product product)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> Update(int id, UpdatePoductDto product)
        {
            throw new NotImplementedException();
        }
    }
}
