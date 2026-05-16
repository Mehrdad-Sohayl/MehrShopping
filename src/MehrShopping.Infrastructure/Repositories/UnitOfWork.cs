using MehrShopping.Application.Interfaces;
using MehrShopping.Infrastructure.Data;

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
            return await _context.SaveChangesAsync();
        }
    }
}
