using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProductEventRepository : IProductEventRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(ProductEvent productEvent)
        {
            _context.ProductEvents.Add(productEvent);
            await _context.SaveChangesAsync();
        }


        public IEnumerable<ProductEvent> GetAll()
        {
            return _context.ProductEvents.Include(ev => ev.Product).AsEnumerable();

        }

        public async Task<ProductEvent?> GetById(int id)
        {
            return await _context.ProductEvents.FindAsync(id);
        }
    }
}
