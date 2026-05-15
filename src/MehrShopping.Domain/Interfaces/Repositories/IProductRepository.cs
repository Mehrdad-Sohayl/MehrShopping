using MehrShopping.Domain.Entities;

namespace MehrShopping.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task DeleteByIdAsync(int id);
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
    }
}
