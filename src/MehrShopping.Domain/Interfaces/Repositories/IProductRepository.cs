using MehrShopping.Domain.Entities;

namespace MehrShopping.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetByIdsAsync(IReadOnlyCollection<int> ids);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByNameAsync(string name);
    }
}
