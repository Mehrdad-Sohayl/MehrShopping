using MehrShopping.Domain.Entities;

namespace MehrShopping.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
        Task DeleteByIdAsync(int id);
        Task<Customer?> GetByIdAsync(int id);
        Task<List<Customer>> GetAllAsync();
    }
}
