using MehrShopping.Application.Interfaces;
using MehrShopping.Infrastructure.Data;
using MehrShopping.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MehrShopping.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MehrShoppingDbContext _context;

        public UnitOfWork(MehrShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException(ex.Message, ex);
            }
        }
    }
}
