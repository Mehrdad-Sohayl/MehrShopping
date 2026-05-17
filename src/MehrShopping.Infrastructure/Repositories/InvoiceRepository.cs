using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Infrastructure.Data;

namespace MehrShopping.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly MehrShoppingDbContext _context;

        public InvoiceRepository(MehrShoppingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }

        public Task DeleteAsync(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Invoice>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Invoice>> GetByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Invoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}
