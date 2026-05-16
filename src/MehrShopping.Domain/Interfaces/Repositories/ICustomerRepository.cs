using MehrShopping.Domain.Entities;

namespace MehrShopping.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> FindByNationalCodeAsync(string nationalCode);
    }
}
