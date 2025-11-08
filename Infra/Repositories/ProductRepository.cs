using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Add(Product product)
        {
            _context.Products.Add(product);
            // TODO: fix problem with isDeleted being NULL for some reason
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is not null)
            {
                product.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Where(prod => prod.IsDeleted == false);

        }

        public async Task<Product?> GetById(int id)
        {
            return await _context.Products
                .Where(prod => prod.IsDeleted == false && prod.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Product?> Update(int id, Product product)
        {
            var isUpgradable = _context.Products.Any(prod => prod.Id == id && prod.IsDeleted == false);
            if (isUpgradable)
            {
                product.Id = id;
                _context.Products.Update(product);
                // TODO: fix problem with CreateAt after update
                await _context.SaveChangesAsync();
                return product;
            }
            return null;
        }
    }
}
