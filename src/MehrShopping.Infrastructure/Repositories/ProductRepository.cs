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

        public Task AddAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name.Value == name);
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
