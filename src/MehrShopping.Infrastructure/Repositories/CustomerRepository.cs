using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MehrShopping.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MehrShoppingDbContext _dbContext;

        public CustomerRepository(MehrShoppingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
        }

        public void Delete(Customer customer)
        {
            _dbContext.Customers.Remove(customer);
        }

        public async Task<Customer?> FindByNationalCodeAsync(string nationalCode)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.NationalCode.Value == nationalCode);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _dbContext.Customers.FindAsync(id);
        }

        public void Update(Customer customer)
        {
            _dbContext.Customers.Update(customer);
        }
    }
}
