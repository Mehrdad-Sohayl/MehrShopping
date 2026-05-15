using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;

namespace MehrShopping.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task AddAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
