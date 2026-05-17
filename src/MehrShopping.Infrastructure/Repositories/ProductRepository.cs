using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MehrShopping.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MehrShoppingDbContext _context;

        public ProductRepository(MehrShoppingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public Task<List<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name.Value == name);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
