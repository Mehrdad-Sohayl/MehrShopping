using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Domain.ValueObjects;
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

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetByIdsAsync(IReadOnlyCollection<int> ids)
        {
            return await _context.Products
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name.Value == name);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<bool> DecreaseStockIfAvailable(int productId, int quantity, List<DomainError> errors)
        {
            var rows = await _context.Products
                .Where(p => p.Id == productId && p.StockQuantity.Value >= quantity)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.StockQuantity,
                        p => Quantity.Create(p.StockQuantity.Value - quantity, errors)));

            return rows > 0;
        }
    }
}
