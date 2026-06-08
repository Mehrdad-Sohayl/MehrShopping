using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;

namespace MehrShopping.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetByIdsAsync(IReadOnlyCollection<int> ids);
        Task<Product?> GetByNameAsync(string name);
        Task<bool> DecreaseStockIfAvailable(int productId, int quantity, List<DomainError> errors);
    }
}
